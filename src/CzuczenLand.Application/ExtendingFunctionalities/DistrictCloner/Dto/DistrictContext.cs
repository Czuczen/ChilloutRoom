using System.Collections.Generic;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.ManyToManyRelations;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

public class DistrictContext
{
    public string WardenName { get; set; }

    public string WardenPassword { get; set; }
    
    public District District { get; set; }
    public List<GeneratedType> GeneratedTypes { get; set; }
    public List<Requirement> Requirements { get; set; }
    public List<Drop> Drops { get; set; }
    public List<DriedFruit> DriedFruits { get; set; }
    public List<Lamp> Lamps { get; set; }
    public List<Manure> Manures { get; set; }
    public List<Pot> Pots { get; set; }
    public List<Seed> Seeds { get; set; }
    public List<Soil> Soils { get; set; }
    public List<Water> Waters { get; set; }
    public List<Bonus> Bonuses { get; set; }
    public List<Quest> Quests { get; set; }
    public List<QuestRequirementsProgress> QuestsRequirementsProgress { get; set; }
    public List<DropQuest> DropsQuests { get; set; }
}
