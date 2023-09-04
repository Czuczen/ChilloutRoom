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
/// Fabryka do tworzenia obiektów DTO dla operacji tworzenia.
/// </summary>
public static class CreateDtoModelFactory
{
    /// <summary>
    /// Zwraca nowy obiekt DTO na podstawie nazwy encji dla operacji tworzenia.
    /// </summary>
    /// <param name="entityCreateDtoName">Nazwa encji DTO do stworzenia.</param>
    /// <returns>Nowy obiekt DTO dla operacji tworzenia.</returns>
    public static object GetCreateDtoNewObjectByEntityCreateDtoName(string entityCreateDtoName)
    {
        switch (entityCreateDtoName)
        {
            case CreateDtoEntitiesNames.DriedFruitCreateDto:
                return new DriedFruitCreateDto();
            case CreateDtoEntitiesNames.LampCreateDto:
                return new LampCreateDto();
            case CreateDtoEntitiesNames.ManureCreateDto:
                return new ManureCreateDto();
            case CreateDtoEntitiesNames.PotCreateDto:
                return new PotCreateDto();
            case CreateDtoEntitiesNames.SeedCreateDto:
                return new SeedCreateDto();
            case CreateDtoEntitiesNames.SoilCreateDto:
                return new SoilCreateDto();
            case CreateDtoEntitiesNames.WaterCreateDto:
                return new WaterCreateDto();
            case CreateDtoEntitiesNames.NewsCreateDto:
                return new NewsCreateDto();
            case CreateDtoEntitiesNames.DropCreateDto:
                return new DropCreateDto();
            case CreateDtoEntitiesNames.PlantationStorageCreateDto:
                return new PlantationStorageCreateDto();
            case CreateDtoEntitiesNames.PlayerStorageCreateDto:
                return new PlayerStorageCreateDto();
            case CreateDtoEntitiesNames.QuestCreateDto:
                return new QuestCreateDto();
            case CreateDtoEntitiesNames.BonusCreateDto:
                return new BonusCreateDto();
            case CreateDtoEntitiesNames.RequirementCreateDto:
                return new RequirementCreateDto();
            case CreateDtoEntitiesNames.DistrictCreateDto:
                return new DistrictCreateDto();
            case CreateDtoEntitiesNames.GeneratedTypeCreateDto:
                return new GeneratedTypeCreateDto();
            default:
                throw new ArgumentOutOfRangeException(entityCreateDtoName);
        }
    }
        
    /// <summary>
    /// Zwraca typ encji DTO tworzenia.
    /// </summary>
    /// <param name="entityCreateDtoName">Nazwa encji DTO do stworzenia.</param>
    /// <returns>Typ encji DTO dla operacji tworzenia.</returns>
    public static Type GetCreateDtoEntityTypeByEntityCreateDtoName(string entityCreateDtoName)
    {
        switch (entityCreateDtoName)
        {
            case EntitiesDbNames.DriedFruit:
                return typeof(DriedFruitCreateDto);
            case EntitiesDbNames.Lamp:
                return typeof(LampCreateDto);
            case EntitiesDbNames.Manure:
                return typeof(ManureCreateDto);
            case EntitiesDbNames.Pot:
                return typeof(PotCreateDto);
            case EntitiesDbNames.Seed:
                return typeof(SeedCreateDto);
            case EntitiesDbNames.Soil:
                return typeof(SoilCreateDto);
            case EntitiesDbNames.Water:
                return typeof(WaterCreateDto);
            case EntitiesDbNames.Bonus:
                return typeof(BonusCreateDto);
            case EntitiesDbNames.News:
                return typeof(NewsCreateDto);
            case EntitiesDbNames.Drop:
                return typeof(DropCreateDto);
            case EntitiesDbNames.PlayerStorage:
                return typeof(PlayerStorageCreateDto);
            case EntitiesDbNames.Quest:
                return typeof(QuestCreateDto);
            case EntitiesDbNames.Requirement:
                return typeof(RequirementCreateDto);
            case EntitiesDbNames.District:
                return typeof(DistrictCreateDto);
            case EntitiesDbNames.GeneratedType:
                return typeof(GeneratedTypeCreateDto);
            default:
                throw new ArgumentOutOfRangeException(entityCreateDtoName);
        }
    }
}