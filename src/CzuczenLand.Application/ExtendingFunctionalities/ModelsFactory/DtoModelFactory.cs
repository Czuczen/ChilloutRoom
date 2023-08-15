using System;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
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
/// Fabryka do tworzenia obiektów DTO.
/// </summary>
public static class DtoModelFactory
{
    /// <summary>
    /// Zwraca nowy obiekt DTO na podstawie nazwy encji bazodanowej.
    /// </summary>
    /// <param name="entityDbName">Nazwa encji bazodanowej do stworzenia DTO.</param>
    /// <returns>Nowy obiekt DTO odpowiadający nazwie encji bazodanowej.</returns>
    public static object GetDtoNewObjectByEntityDbName(string entityDbName)
    {
        switch (entityDbName)
        {
            case EntitiesDbNames.DriedFruit:
                return new DriedFruitDto();
            case EntitiesDbNames.Lamp:
                return new LampDto();
            case EntitiesDbNames.Manure:
                return new ManureDto();
            case EntitiesDbNames.Pot:
                return new PotDto();
            case EntitiesDbNames.Seed:
                return new SeedDto();
            case EntitiesDbNames.Soil:
                return new SoilDto();
            case EntitiesDbNames.Water:
                return new WaterDto();
            case EntitiesDbNames.News:
                return new NewsDto();
            case EntitiesDbNames.Drop:
                return new DropDto();
            case EntitiesDbNames.PlantationStorage:
                return new PlantationStorageDto();
            case EntitiesDbNames.PlayerStorage:
                return new PlayerStorageDto();
            case EntitiesDbNames.Quest:
                return new QuestDto();
            case EntitiesDbNames.Requirement:
                return new RequirementDto();
            case EntitiesDbNames.District:
                return new DistrictDto();
            case EntitiesDbNames.GeneratedType:
                return new GeneratedTypeDto();
            default:
                throw new ArgumentOutOfRangeException(entityDbName);
        }
    }

    /// <summary>
    /// Zwraca typ encji DTO na podstawie nazwy encji bazodanowej.
    /// </summary>
    /// <param name="entityDbName">Nazwa encji bazodanowej.</param>
    /// <returns>Typ encji DTO odpowiadający nazwie encji bazodanowej.</returns>
    public static Type GetDtoEntityTypeByEntityDbName(string entityDbName)
    {
        switch (entityDbName)
        {
            case EntitiesDbNames.DriedFruit:
                return typeof(DriedFruitDto);
            case EntitiesDbNames.Lamp:
                return typeof(LampDto);
            case EntitiesDbNames.Manure:
                return typeof(ManureDto);
            case EntitiesDbNames.Pot:
                return typeof(PotDto);
            case EntitiesDbNames.Seed:
                return typeof(SeedDto);
            case EntitiesDbNames.Soil:
                return typeof(SoilDto);
            case EntitiesDbNames.Water:
                return typeof(WaterDto);
            case EntitiesDbNames.Bonus:
                return typeof(BonusDto);
            case EntitiesDbNames.News:
                return typeof(NewsDto);
            case EntitiesDbNames.Drop:
                return typeof(DropDto);
            case EntitiesDbNames.PlantationStorage:
                return typeof(PlantationStorageDto);
            case EntitiesDbNames.PlayerStorage:
                return typeof(PlayerStorageDto);
            case EntitiesDbNames.Quest:
                return typeof(QuestDto);
            case EntitiesDbNames.Requirement:
                return typeof(RequirementDto);
            case EntitiesDbNames.District:
                return typeof(DistrictDto);
            case EntitiesDbNames.GeneratedType:
                return typeof(GeneratedTypeDto);
            default:
                throw new ArgumentOutOfRangeException(entityDbName);
        }
    }
}