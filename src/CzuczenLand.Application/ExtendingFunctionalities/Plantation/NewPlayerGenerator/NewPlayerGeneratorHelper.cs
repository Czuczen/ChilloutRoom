using System.Collections.Generic;
using System.Linq;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Db;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using Newtonsoft.Json;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator;

/// <summary>
/// Klasa pomocnicza dla generatora danych gracza.
/// </summary>
public static class NewPlayerGeneratorHelper
{
    /// <summary>
    /// Tworzy listę danych dla biblioteki Select2 na podstawie dostępnych dzielnic i tych w których posiada już plantacje.
    /// </summary>
    /// <param name="districts">Lista dostępnych dzielnic.</param>
    /// <param name="userDistricts">Lista dzielnic w których gracz posiada już plantacje.</param>
    /// <returns>Lista danych dla biblioteki Select2.</returns>
    public static List<Select2Data> GetS2DistrictsList(List<District> districts, List<District> userDistricts)
    {
        var ret = new List<Select2Data>();
        foreach (var district in districts)
        {
            var isUserDistrict = userDistricts.SingleOrDefault(currUserDistrict => currUserDistrict.Id == district.Id);
            if (isUserDistrict != null)
            {
                Select2Data element;
                if (isUserDistrict.EndTime != null)
                    element = new Select2Data{ Id = district.Id, Text = "*=" + district.Name };
                else
                    element = new Select2Data{ Id = district.Id, Text = "* " + district.Name };
                    
                ret.Add(element);
            }
            else
            {
                Select2Data element;
                if (district.EndTime != null)
                    element = new Select2Data{ Id = district.Id, Text = "=" + district.Name};                        
                else
                    element = new Select2Data{ Id = district.Id, Text = district.Name};
                    
                ret.Add(element);
            }
        }

        return ret;
    }
    
    /// <summary>
    /// Tworzy dla gracza nowy obiekt na podstawie definicji.
    /// </summary>
    /// <typeparam name="TEntity">Typ encji.</typeparam>
    /// <param name="definition">Definicja obiektu jako JSON.</param>
    /// <param name="plantationStorageId">Id magazynu plantacji.</param>
    /// <param name="entityEnum">Enum reprezentujący typ encji.</param>
    /// <param name="playerStorageId">Id magazynu gracza.</param>
    /// <returns>Nowy obiekt utworzony na podstawie definicji.</returns>
    public static TEntity GetNewObjectByDefinition<TEntity>(string definition, int plantationStorageId, EnumUtils.Entities? entityEnum, int playerStorageId)
    {
        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(definition);
        
        dict.Remove(EntitiesFieldsDbNames.Id);
        dict[RelationFieldsNames.PlantationStorageId] = plantationStorageId;
        
        if (entityEnum == EnumUtils.Entities.Quest)
            dict[RelationFieldsNames.PlayerStorageId] = playerStorageId;

        var serializedDict = JsonConvert.SerializeObject(dict);
        var ret = JsonConvert.DeserializeObject<TEntity>(serializedDict);
            
        return ret;
    }
}