using System;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Others;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.ModelsFactory;

/// <summary>
/// Fabryka do tworzenia obiektów bazodanowych.
/// </summary>
public static class DbModelFactory
{
    /// <summary>
    /// Zwraca nowy obiekt bazodanowy na podstawie nazwy encji bazodanowej.
    /// </summary>
    /// <param name="entityDbName">Nazwa encji bazodanowej do stworzenia.</param>
    /// <returns>Nowy obiekt bazodanowy odpowiadający nazwie encji.</returns>
    public static object GetDbNewObjectByEntityDbName(string entityDbName)
    {
        switch (entityDbName)
        {
            case EntitiesDbNames.DriedFruit:
                return new DriedFruit();
            case EntitiesDbNames.Lamp:
                return new Lamp();
            case EntitiesDbNames.Manure:
                return new Manure();
            case EntitiesDbNames.Pot:
                return new Pot();
            case EntitiesDbNames.Seed:
                return new Seed();
            case EntitiesDbNames.Soil:
                return new Soil();
            case EntitiesDbNames.Water:
                return new Water();
            case EntitiesDbNames.News:
                return new News();
            case EntitiesDbNames.Drop:
                return new Drop();
            case EntitiesDbNames.PlantationStorage:
                return new PlantationStorage();
            case EntitiesDbNames.Plant:
                return new Plant();
            case EntitiesDbNames.PlayerStorage:
                return new PlayerStorage();
            case EntitiesDbNames.Quest:
                return new Quest();
            case EntitiesDbNames.Requirement:
                return new Requirement();
            case EntitiesDbNames.District:
                return new District();
            case EntitiesDbNames.GeneratedType:
                return new GeneratedType();
            case EntitiesDbNames.DropQuest:
                return new DropQuest();
            default:
                throw new ArgumentOutOfRangeException(entityDbName);
        }
    }
        
    /// <summary>
    /// Zwraca typ encji bazodanowej na podstawie nazwy encji bazodanowej.
    /// </summary>
    /// <param name="entityDbName">Nazwa encji bazodanowej.</param>
    /// <returns>Typ encji bazodanowej odpowiadający nazwie encji.</returns>
    public static Type GetDbEntityTypeByEntityDbName(string entityDbName)
    {
        switch (entityDbName)
        {
            case EntitiesDbNames.DriedFruit:
                return typeof(DriedFruit);
            case EntitiesDbNames.Lamp:
                return typeof(Lamp);
            case EntitiesDbNames.Manure:
                return typeof(Manure);
            case EntitiesDbNames.Pot:
                return typeof(Pot);
            case EntitiesDbNames.Seed:
                return typeof(Seed);
            case EntitiesDbNames.Soil:
                return typeof(Soil);
            case EntitiesDbNames.Water:
                return typeof(Water);
            case EntitiesDbNames.News:
                return typeof(News);
            case EntitiesDbNames.Drop:
                return typeof(Drop);
            case EntitiesDbNames.PlantationStorage:
                return typeof(PlantationStorage);
            case EntitiesDbNames.Plant:
                return typeof(Plant);
            case EntitiesDbNames.PlayerStorage:
                return typeof(PlayerStorage);
            case EntitiesDbNames.Quest:
                return typeof(Quest);
            case EntitiesDbNames.Bonus:
                return typeof(Bonus);
            case EntitiesDbNames.Requirement:
                return typeof(Requirement);
            case EntitiesDbNames.District:
                return typeof(District);
            case EntitiesDbNames.GeneratedType:
                return typeof(GeneratedType);
            case EntitiesDbNames.QuestRequirementsProgress:
                return typeof(QuestRequirementsProgress);
            case EntitiesDbNames.DropQuest:
                return typeof(DropQuest);
            default:
                throw new ArgumentOutOfRangeException(entityDbName);
        }
    }
        
    /// <summary>
    /// Zwraca typ encji bazodanowej na podstawie nazwy encji DTO.
    /// </summary>
    /// <param name="entityDtoName">Nazwa encji DTO.</param>
    /// <returns>Typ encji bazodanowej odpowiadający nazwie encji DTO.</returns>
    public static Type GetDbEntityTypeByEntityDtoName(string entityDtoName)
    {
        switch (entityDtoName)
        {
            case DtoEntitiesNames.DriedFruitDto:
                return typeof(DriedFruit);
            case DtoEntitiesNames.LampDto:
                return typeof(Lamp);
            case DtoEntitiesNames.ManureDto:
                return typeof(Manure);
            case DtoEntitiesNames.PotDto:
                return typeof(Pot);
            case DtoEntitiesNames.SeedDto:
                return typeof(Seed);
            case DtoEntitiesNames.SoilDto:
                return typeof(Soil);
            case DtoEntitiesNames.WaterDto:
                return typeof(Water);
            case DtoEntitiesNames.BonusDto:
                return typeof(Bonus);
            case DtoEntitiesNames.NewsDto:
                return typeof(News);
            case DtoEntitiesNames.DropDto:
                return typeof(Drop);
            case DtoEntitiesNames.PlantationStorageDto:
                return typeof(PlantationStorage);
            case DtoEntitiesNames.PlayerStorageDto:
                return typeof(PlayerStorage);
            case DtoEntitiesNames.QuestDto:
                return typeof(Quest);
            case DtoEntitiesNames.RequirementDto:
                return typeof(Requirement);
            case DtoEntitiesNames.DistrictDto:
                return typeof(District);
            case DtoEntitiesNames.GeneratedTypeDto:
                return typeof(GeneratedType);
            default:
                throw new ArgumentOutOfRangeException(entityDtoName);
        }
    }
}