using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using CzuczenLand.EntityFramework;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Interfaces;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;
using ExcelDataReader;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner;

public static class DistrictClonerHelper
{
    public const string WardenNameField = "WardenName";
    public const string WardenPasswordField = "WardenPassword";
        
    
    private static SqlCommandContext GetSqlCommandContext(object entity, string tableName, List<PropertyInfo> entityProperties)
    {
        var ret = new SqlCommandContext
        {
            Query = $@"
                           SET IDENTITY_INSERT {tableName} ON

                           Insert into {tableName}("
        };
            
        var valuesFields = @")
                                Values(";
            
        var sqlParameters = new List<SqlParameter>();
        foreach (var prop in entityProperties)
        {
            var fieldName = "@" + prop.Name;
            var propIndex = entityProperties.FindIndex(item => item.Name == prop.Name) + 1;
            var itIsLast = propIndex == entityProperties.Count;
            var tempObjValue = prop.GetValue(entity);
            var objValue = tempObjValue ?? DBNull.Value;
                
            ret.Query += prop.Name + (itIsLast ? "" : ", ");
            valuesFields += fieldName + (itIsLast ? "" : ", ");
            sqlParameters.Add(new SqlParameter(fieldName, objValue));
        }
            
        ret.Query += $@"{valuesFields})

                            SET IDENTITY_INSERT {tableName} OFF";
            
        ret.SqlParameters = sqlParameters.Cast<object>().ToArray();

        return ret;
    }

    public static async Task<List<int>> CloneObjects(AbpDbContext ctx, List<object> objects, 
        List<Tuple<string, int, int>> entityNameOldIdNewIdList, Dictionary<string, int> entitiesMaxIdNumbers)
    {
        var ret = new List<int>();
        if (objects == null || objects.Count <= 0) return ret;

        var objectsType = objects.First().GetType();
        var entityName = objectsType.Name;
        var entityProperties = objectsType.GetProperties().ToList();
        var tableName = GetTableNameByEntityDbName(entityName);
            
        foreach (var obj in objects)
        {
            SetId(obj, entityName, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
            SetRelationIds(obj, objectsType, entityNameOldIdNewIdList);
            var command = GetSqlCommandContext(obj, tableName, entityProperties);
            var status = await ctx.Database.ExecuteSqlCommandAsync(command.Query, command.SqlParameters);
            if (status == 1)
                ret.Add(((IEntity<int>) obj).Id);
            else
                throw new Exception("Błąd. Status nieprawidłowy. Status: " + status);
        }

        return ret;
    }

    private static void SetRelationIds(object obj, Type objType, List<Tuple<string, int, int>> entityNameOldIdNewIdList)
    {
        switch (objType.Name)
        {
            case EntitiesDbNames.District:
                break;
            case EntitiesDbNames.Drop:
                var dropEntity = (Drop) obj;
                dropEntity.DistrictId = GetRelationId(entityNameOldIdNewIdList, EntitiesDbNames.District,
                    dropEntity.DistrictId);
                dropEntity.GeneratedTypeId = dropEntity.GeneratedTypeId != null
                    ? GetRelationId(entityNameOldIdNewIdList, EntitiesDbNames.GeneratedType,
                        (int) dropEntity.GeneratedTypeId)
                    : null;
                break;
            case EntitiesDbNames.Requirement:
                var requirementEntity = (Requirement) obj;
                requirementEntity.DistrictId = GetRelationId(entityNameOldIdNewIdList, EntitiesDbNames.District,
                    requirementEntity.DistrictId);
                requirementEntity.GeneratedTypeId = requirementEntity.GeneratedTypeId != null
                    ? GetRelationId(entityNameOldIdNewIdList, EntitiesDbNames.GeneratedType,
                        (int) requirementEntity.GeneratedTypeId)
                    : null;
                break;
            case EntitiesDbNames.GeneratedType:
                var districtEntity = (IDistrictEntity) obj;
                districtEntity.DistrictId = GetRelationId(entityNameOldIdNewIdList, EntitiesDbNames.District,
                    districtEntity.DistrictId);
                break;
            case EntitiesDbNames.DriedFruit:
            case EntitiesDbNames.Lamp:
            case EntitiesDbNames.Manure:
            case EntitiesDbNames.Pot:
            case EntitiesDbNames.Seed:
            case EntitiesDbNames.Soil:
            case EntitiesDbNames.Water:
            case EntitiesDbNames.Quest:
            case EntitiesDbNames.Bonus:
                var generatedEntity = (IGeneratedEntity) obj;
                generatedEntity.GeneratedTypeId = GetRelationId(entityNameOldIdNewIdList,
                    EntitiesDbNames.GeneratedType, generatedEntity.GeneratedTypeId);
                break;
            case EntitiesDbNames.QuestRequirementsProgress:
                var questRequirementsProgress = (QuestRequirementsProgress) obj;
                questRequirementsProgress.QuestId = GetRelationId(entityNameOldIdNewIdList,
                    EntitiesDbNames.Quest, questRequirementsProgress.QuestId);
                var requirementsProgress = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(questRequirementsProgress.RequirementsProgress);
                var newRequirementsProgress = new Dictionary<int, decimal>();
                foreach (var reqProgress in requirementsProgress)
                {
                    var newRelId = GetRelationId(entityNameOldIdNewIdList,
                        EntitiesDbNames.Requirement, reqProgress.Key);
                    newRequirementsProgress[newRelId] = reqProgress.Value;
                }

                questRequirementsProgress.RequirementsProgress = JsonConvert.SerializeObject(newRequirementsProgress);
                break;
            case EntitiesDbNames.DropQuest:
                var dropQuest = (DropQuest) obj;
                dropQuest.QuestId = GetRelationId(entityNameOldIdNewIdList,
                    EntitiesDbNames.Quest, dropQuest.QuestId);
                dropQuest.DropId = GetRelationId(entityNameOldIdNewIdList,
                    EntitiesDbNames.Drop, dropQuest.DropId);
                break;
            default:
                throw new ArgumentOutOfRangeException(objType.Name);
        }
    }

    private static int GetRelationId(List<Tuple<string, int, int>> entityNameOldIdNewIdList, string entityName, int oldId)
    {
        return entityNameOldIdNewIdList.Single(item => item.Item1 == entityName && item.Item2 == oldId).Item3;
    }
        
    private static void SetId(object obj, string entityName, List<Tuple<string, int, int>> entityNameOldIdNewIdList,  
        Dictionary<string, int> entitiesMaxIdNumbers)
    {
        var entity = (IEntity<int>) obj;
        var oldId = entity.Id;
        var maxEntityId = entitiesMaxIdNumbers[entityName];
            
        var anyEntityIdAlreadyExist = entityNameOldIdNewIdList.FirstOrDefault(item => item.Item1 == entityName);
        if (anyEntityIdAlreadyExist != null)
            entity.Id = entityNameOldIdNewIdList.Where(item => item.Item1 == entityName)
                .Max(item => item.Item3) + 1;
        else
            entity.Id = maxEntityId + 1;

        var entityNameToOldIdAndNewId = new Tuple<string, int, int>(entityName, oldId, entity.Id);
        entityNameOldIdNewIdList.Add(entityNameToOldIdAndNewId);
    }

    private static string GetTableNameByEntityDbName(string entityDbName)
    {
        const string startFulName = "System.Data.Entity.DbSet`1[[CzuczenLand.ExtendingModels.Models.";
        var tablesProperties = typeof(CzuczenLandDbContext).GetProperties().Where(item =>
            item.PropertyType.FullName != null && item.PropertyType.FullName.Contains(startFulName));

        return tablesProperties.Single(item =>
        {
            var currEntityTableName = item.PropertyType.FullName?.Split(new[] {",", "."}, StringSplitOptions.None)
                .SingleOrDefault(element => element == entityDbName);
            return !string.IsNullOrWhiteSpace(currEntityTableName);
        }).Name;
    }

    public static Dictionary<string, string> DownloadFilesAndGetIdsAndNames(string definitionsPath, List<string> filesIds)
    {
        var ret = new Dictionary<string, string>();
            
        const string googleDiskRootFolder = "https://drive.google.com/uc?export=download&id=";
        using var client = new WebClient();

        foreach (var fileId in filesIds)
        {
            var fullPathOnDisk = Path.Combine(definitionsPath, fileId + ".xls");
            var fullPathUrl = googleDiskRootFolder + fileId;
                
            client.DownloadFile(fullPathUrl, fullPathOnDisk);
            if (!string.IsNullOrEmpty(client.ResponseHeaders["Content-Disposition"]))
                ret[fileId] = StringUtils.CutString(client.ResponseHeaders["Content-Disposition"], "\"", ".xls\"");
            else
                throw new Exception("Brak nazwy pliku");

        }

        return ret;
    }

    public static Dictionary<string, List<Dictionary<string, object>>> GetXlsDocumentsAsEntityDictListObjects(
        string definitionsPath, Dictionary<string, string> filesIdsAndNames = null)
    {
        var ret = new Dictionary<string, List<Dictionary<string, object>>>();
            
        var xlsFilesFromDisk= Directory.EnumerateFiles(definitionsPath,"*.xls", SearchOption.AllDirectories).ToList();
        foreach (var xlsFileFullPath in xlsFilesFromDisk)
        {
            var fileName = Path.GetFileNameWithoutExtension(xlsFileFullPath);
            var entityName = filesIdsAndNames != null ? filesIdsAndNames[fileName] : fileName;
                
            var stream = File.Open(xlsFileFullPath, FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateReader(stream);
            var result = excelReader.AsDataSet(new ExcelDataSetConfiguration{ConfigureDataTable = _ => new ExcelDataTableConfiguration{UseHeaderRow = true}});
            var dt = result.Tables[0];

            var objectsList = new List<Dictionary<string, object>>();
            var columnsNames = dt.Columns.Cast<DataColumn>().Select(item => item.ColumnName).ToList();
            foreach (var row in dt.Rows.Cast<DataRow>())
            {
                var obj = new Dictionary<string, object>();
                foreach (var columnName in columnsNames)
                    obj[columnName] = row[columnName];
                    
                objectsList.Add(obj);
            }

            ret[entityName] = objectsList;
            excelReader.Close();    
        }

        return ret;
    }

    public static List<object> GetObjects(string entityName, string serializedObjects)
    {
        List<object> ret;
        switch (entityName)
        {
            case EntitiesDbNames.District:
                ret = JsonConvert.DeserializeObject<List<District>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.GeneratedType:
                ret = JsonConvert.DeserializeObject<List<GeneratedType>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.DriedFruit:
                ret = JsonConvert.DeserializeObject<List<DriedFruit>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Lamp:
                ret = JsonConvert.DeserializeObject<List<Lamp>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Manure:
                ret = JsonConvert.DeserializeObject<List<Manure>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Pot:
                ret = JsonConvert.DeserializeObject<List<Pot>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Seed:
                ret = JsonConvert.DeserializeObject<List<Seed>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Soil:
                ret = JsonConvert.DeserializeObject<List<Soil>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Water:
                ret = JsonConvert.DeserializeObject<List<Water>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Drop:
                ret = JsonConvert.DeserializeObject<List<Drop>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Quest:
                ret = JsonConvert.DeserializeObject<List<Quest>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Bonus:
                ret = JsonConvert.DeserializeObject<List<Bonus>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.Requirement:
                ret = JsonConvert.DeserializeObject<List<Requirement>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.QuestRequirementsProgress:
                ret = JsonConvert.DeserializeObject<List<QuestRequirementsProgress>>(serializedObjects)?.Cast<object>().ToList();
                break;
            case EntitiesDbNames.DropQuest:
                ret = JsonConvert.DeserializeObject<List<DropQuest>>(serializedObjects)?.Cast<object>().ToList();
                break;
            default:
                throw new ArgumentOutOfRangeException(entityName);
        }
            
        return ret;
    }

    public static Dictionary<string, List<Dictionary<string, object>>> ArrangeInOrder(Dictionary<string, List<Dictionary<string, object>>> entityNameToObjects)
    {
        var ret = new Dictionary<string, List<Dictionary<string, object>>>();

        var district = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.District);
        ret[EntitiesDbNames.District] = district.Value;
            
        var generatedType = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.GeneratedType);
        ret[EntitiesDbNames.GeneratedType] = generatedType.Value;
            
        var requirement = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Requirement);
        ret[EntitiesDbNames.Requirement] = requirement.Value;
                        
        var drop = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Drop);
        ret[EntitiesDbNames.Drop] = drop.Value;
            
        var driedFruit = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.DriedFruit);
        ret[EntitiesDbNames.DriedFruit] = driedFruit.Value;
            
        var lamp = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Lamp);
        ret[EntitiesDbNames.Lamp] = lamp.Value;
            
        var manure = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Manure);
        ret[EntitiesDbNames.Manure] = manure.Value;
            
        var pot = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Pot);
        ret[EntitiesDbNames.Pot] = pot.Value;
            
        var seed = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Seed);
        ret[EntitiesDbNames.Seed] = seed.Value;
            
        var soil = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Soil);
        ret[EntitiesDbNames.Soil] = soil.Value;
            
        var water = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Water);
        ret[EntitiesDbNames.Water] = water.Value;
            
        var quest = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Quest);
        ret[EntitiesDbNames.Quest] = quest.Value;
            
        var bonus = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.Bonus);
        ret[EntitiesDbNames.Bonus] = bonus.Value;
            
        var questRequirementsProgress = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.QuestRequirementsProgress);
        ret[EntitiesDbNames.QuestRequirementsProgress] = questRequirementsProgress.Value;
            
        var dropQuest = entityNameToObjects.SingleOrDefault(item => item.Key == EntitiesDbNames.DropQuest);
        ret[EntitiesDbNames.DropQuest] = dropQuest.Value;

        return ret;
    }

    public static async Task<ClonedObjectsIds> GenerateNewObjectsAndClone(DistrictContext districtContext, AbpDbContext ctx,
        List<Tuple<string, int, int>> entityNameOldIdNewIdList, Dictionary<string, int> entitiesMaxIdNumbers, long wardenId)
    {
        var ret = new ClonedObjectsIds();
        
        // Ustawianie nowego opiekuna dopiero po stworzeniu nowego obiektu. Jeśli ustawiane wcześniej na starym to jednostka pracy zapisywała to jako zmianę.
        var newDistricts = GetObjects(EntitiesDbNames.District, JsonConvert.SerializeObject(new List<object>{districtContext.District}));
        var newDistrict = (District) newDistricts.Single();
        newDistrict.UserId = wardenId;
        newDistrict.IsDefined = false;

        var newGeneratedTypes = GetObjects(EntitiesDbNames.GeneratedType, JsonConvert.SerializeObject(districtContext.GeneratedTypes));
        var newRequirements = GetObjects(EntitiesDbNames.Requirement, JsonConvert.SerializeObject(districtContext.Requirements));
        var newDrops = GetObjects(EntitiesDbNames.Drop, JsonConvert.SerializeObject(districtContext.Drops));
        var newDriedFruits = GetObjects(EntitiesDbNames.DriedFruit, JsonConvert.SerializeObject(districtContext.DriedFruits));
        var newLamps = GetObjects(EntitiesDbNames.Lamp, JsonConvert.SerializeObject(districtContext.Lamps));
        var newManures = GetObjects(EntitiesDbNames.Manure, JsonConvert.SerializeObject(districtContext.Manures));
        var newPots = GetObjects(EntitiesDbNames.Pot, JsonConvert.SerializeObject(districtContext.Pots));
        var newSeeds = GetObjects(EntitiesDbNames.Seed, JsonConvert.SerializeObject(districtContext.Seeds));
        var newSoils = GetObjects(EntitiesDbNames.Soil, JsonConvert.SerializeObject(districtContext.Soils));
        var newWaters = GetObjects(EntitiesDbNames.Water, JsonConvert.SerializeObject(districtContext.Waters));
        var newBonuses = GetObjects(EntitiesDbNames.Bonus, JsonConvert.SerializeObject(districtContext.Bonuses));
        var newQuests = GetObjects(EntitiesDbNames.Quest, JsonConvert.SerializeObject(districtContext.Quests));
        var newQuestsRequirementsProgress = GetObjects(EntitiesDbNames.QuestRequirementsProgress, JsonConvert.SerializeObject(districtContext.QuestsRequirementsProgress));
        var newDropSQuests = GetObjects(EntitiesDbNames.DropQuest, JsonConvert.SerializeObject(districtContext.DropsQuests));
            
        // kolejność ma znaczenie
        ret.DistrictIds = await CloneObjects(ctx, newDistricts, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.GeneratedTypeIds = await CloneObjects(ctx, newGeneratedTypes, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.RequirementIds = await CloneObjects(ctx, newRequirements, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.DropIds = await CloneObjects(ctx, newDrops, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.DriedFruitIds = await CloneObjects(ctx, newDriedFruits, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.LampIds = await CloneObjects(ctx, newLamps, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.ManureIds = await CloneObjects(ctx, newManures, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.PotIds = await CloneObjects(ctx, newPots, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.SeedIds = await CloneObjects(ctx, newSeeds, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.SoilIds = await CloneObjects(ctx, newSoils, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.WaterIds = await CloneObjects(ctx, newWaters, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.BonusIds = await CloneObjects(ctx, newBonuses, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.QuestIds = await CloneObjects(ctx, newQuests, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.QuestRequirementsProgressIds = await CloneObjects(ctx, newQuestsRequirementsProgress, entityNameOldIdNewIdList, entitiesMaxIdNumbers);
        ret.DropQuestIds = await CloneObjects(ctx, newDropSQuests, entityNameOldIdNewIdList, entitiesMaxIdNumbers);

        return ret;
    }

    public static DistrictContext GetAsDistrictContext(Dictionary<string, List<object>> createdObjects)
    {
        return new DistrictContext
        {
            WardenName = createdObjects[WardenNameField].Cast<string>().Single(),
            WardenPassword = createdObjects[WardenPasswordField].Cast<string>().Single(),
            District = createdObjects[EntitiesDbNames.District].Cast<District>().Single(),
            GeneratedTypes = createdObjects[EntitiesDbNames.GeneratedType].Cast<GeneratedType>().ToList(),
            Requirements = createdObjects[EntitiesDbNames.Requirement].Cast<Requirement>().ToList(),
            Drops = createdObjects[EntitiesDbNames.Drop].Cast<Drop>().ToList(),
            DriedFruits = createdObjects[EntitiesDbNames.DriedFruit].Cast<DriedFruit>().ToList(),
            Lamps = createdObjects[EntitiesDbNames.Lamp].Cast<Lamp>().ToList(),
            Manures = createdObjects[EntitiesDbNames.Manure].Cast<Manure>().ToList(),
            Pots = createdObjects[EntitiesDbNames.Pot].Cast<Pot>().ToList(),
            Seeds = createdObjects[EntitiesDbNames.Seed].Cast<Seed>().ToList(),
            Soils = createdObjects[EntitiesDbNames.Soil].Cast<Soil>().ToList(),
            Waters = createdObjects[EntitiesDbNames.Water].Cast<Water>().ToList(),
            Bonuses = createdObjects[EntitiesDbNames.Bonus].Cast<Bonus>().ToList(),
            Quests = createdObjects[EntitiesDbNames.Quest].Cast<Quest>().ToList(),
            QuestsRequirementsProgress = createdObjects[EntitiesDbNames.QuestRequirementsProgress].Cast<QuestRequirementsProgress>().ToList(),
            DropsQuests = createdObjects[EntitiesDbNames.DropQuest].Cast<DropQuest>().ToList()
        };
    }
}