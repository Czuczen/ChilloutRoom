using System;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.District.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlayerStorage.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.General.Requirement.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Bonus.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.DriedFruit.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Manure.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Pot.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Seed.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Soil.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Water.Dto;

namespace CzuczenLand.ExtendingFunctionalities.ModelsFactory;

/// <summary>
/// Fabryka do tworzenia obiektów DTO dla operacji aktualizacji.
/// </summary>
public static class UpdateDtoModelFactory
{
    /// <summary>
    /// Zwraca nowy obiekt DTO na podstawie nazwy encji dla operacji aktualizacji.
    /// </summary>
    /// <param name="entityUpdateDtoName">Nazwa encji DTO do aktualizacji.</param>
    /// <returns>Nowy obiekt DTO dla operacji aktualizacji.</returns>
    public static object GetUpdateDtoNewObjectByEntityUpdateDtoName(string entityUpdateDtoName)
    {
        switch (entityUpdateDtoName)
        {
            case UpdateDtoEntitiesNames.DriedFruitUpdateDto:
                return new DriedFruitUpdateDto();
            case UpdateDtoEntitiesNames.LampUpdateDto:
                return new LampUpdateDto();
            case UpdateDtoEntitiesNames.ManureUpdateDto:
                return new ManureUpdateDto();
            case UpdateDtoEntitiesNames.PotUpdateDto:
                return new PotUpdateDto();
            case UpdateDtoEntitiesNames.SeedUpdateDto:
                return new SeedUpdateDto();
            case UpdateDtoEntitiesNames.SoilUpdateDto:
                return new SoilUpdateDto();
            case UpdateDtoEntitiesNames.WaterUpdateDto:
                return new WaterUpdateDto();
            case UpdateDtoEntitiesNames.NewsUpdateDto:
                return new NewsUpdateDto();
            case UpdateDtoEntitiesNames.DropUpdateDto:
                return new DropUpdateDto();
            case UpdateDtoEntitiesNames.PlantationStorageUpdateDto:
                return new PlantationStorageUpdateDto();
            case UpdateDtoEntitiesNames.PlayerStorageUpdateDto:
                return new PlayerStorageUpdateDto();
            case UpdateDtoEntitiesNames.QuestUpdateDto:
                return new QuestUpdateDto();
            case UpdateDtoEntitiesNames.BonusUpdateDto:
                return new BonusUpdateDto();
            case UpdateDtoEntitiesNames.RequirementUpdateDto:
                return new RequirementUpdateDto();
            case UpdateDtoEntitiesNames.DistrictUpdateDto:
                return new DistrictUpdateDto();
            case UpdateDtoEntitiesNames.GeneratedTypeUpdateDto:
                return new GeneratedTypeUpdateDto();
            default:
                throw new ArgumentOutOfRangeException(entityUpdateDtoName);
        }
    }
        
    /// <summary>
    /// Zwraca typ encji DTO aktualizacji.
    /// </summary>
    /// <param name="entityDbName">Nazwa encji bazodanowej.</param>
    /// <returns>Typ encji DTO dla operacji aktualizacji.</returns>
    public static Type GetUpdateDtoEntityTypeByEntityDbName(string entityDbName)
    {
        switch (entityDbName)
        {
            case EntitiesDbNames.DriedFruit:
                return typeof(DriedFruitUpdateDto);
            case EntitiesDbNames.Lamp:
                return typeof(LampUpdateDto);
            case EntitiesDbNames.Manure:
                return typeof(ManureUpdateDto);
            case EntitiesDbNames.Pot:
                return typeof(PotUpdateDto);
            case EntitiesDbNames.Seed:
                return typeof(SeedUpdateDto);
            case EntitiesDbNames.Soil:
                return typeof(SoilUpdateDto);
            case EntitiesDbNames.Water:
                return typeof(WaterUpdateDto);
            case EntitiesDbNames.Bonus:
                return typeof(BonusUpdateDto);
            case EntitiesDbNames.News:
                return typeof(NewsUpdateDto);
            case EntitiesDbNames.Drop:
                return typeof(DropUpdateDto);
            case EntitiesDbNames.PlantationStorage:
                return typeof(PlantationStorageUpdateDto);
            case EntitiesDbNames.PlayerStorage:
                return typeof(PlayerStorageUpdateDto);
            case EntitiesDbNames.Quest:
                return typeof(QuestUpdateDto);
            case EntitiesDbNames.Requirement:
                return typeof(RequirementUpdateDto);
            case EntitiesDbNames.District:
                return typeof(DistrictUpdateDto);
            case EntitiesDbNames.GeneratedType:
                return typeof(GeneratedTypeUpdateDto);
            default:
                throw new ArgumentOutOfRangeException(entityDbName);
        }
    }
}