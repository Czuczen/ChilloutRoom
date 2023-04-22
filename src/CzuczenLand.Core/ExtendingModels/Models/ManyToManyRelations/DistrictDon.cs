using Abp.Domain.Entities;

namespace CzuczenLand.ExtendingModels.Models.ManyToManyRelations;

public class DistrictDon : Entity<int>
{
    public int DistrictId { get; set; }

    public int PlantationStorageId { get; set; }
}