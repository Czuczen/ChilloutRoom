using System;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Others;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Repository;

public static class CustomRepositoryFactory
{
    public static ICustomRepository GetRepository(string entity)
    {
        switch (entity)
        {
            case "DriedFruit":
                return new CustomRepository<DriedFruit>();
            case "Lamp":
                return new CustomRepository<Lamp>();
            case "Manure":
                return new CustomRepository<Manure>();
            case "Pot":
                return new CustomRepository<Pot>();
            case "Seed":
                return new CustomRepository<Seed>();
            case "Soil":
                return new CustomRepository<Soil>();
            case "Water":
                return new CustomRepository<Water>();
            case "News":
                return new CustomRepository<News>();
            case "Drop":
                return new CustomRepository<Drop>();
            case "PlantationStorage":
                return new CustomRepository<PlantationStorage>();
            case "Plant":
                return new CustomRepository<Plant>();
            case "PlayerStorage":
                return new CustomRepository<PlayerStorage>();
            case "Quest":
                return new CustomRepository<Quest>();
            case "Bonus":
                return new CustomRepository<Bonus>();
            case "Requirement":
                return new CustomRepository<Requirement>();
            case "District":
                return new CustomRepository<District>();
            case "GeneratedType":
                return new CustomRepository<GeneratedType>();
            case "DropQuest":
                return new CustomRepository<DropQuest>();
            case "DistrictDon":
                return new CustomRepository<DistrictDon>();
            case "BlackMarketTransaction":
                return new CustomRepository<BlackMarketTransaction>();
            case "IgnoreChange":
                return new CustomRepository<IgnoreChange>();
            case "QuestRequirementsProgress":
                return new CustomRepository<QuestRequirementsProgress>();
            default:
                throw new ArgumentOutOfRangeException(entity);
        }
    }
}
