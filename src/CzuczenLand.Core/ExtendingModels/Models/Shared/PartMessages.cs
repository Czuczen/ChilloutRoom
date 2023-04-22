using Abp.Auditing;
using Abp.Domain.Entities;
using CzuczenLand.ExtendingModels.Interfaces;

namespace CzuczenLand.ExtendingModels.Models.Shared;

[Audited]
public class PartMessages : Entity<int>, INamedEntity
{
    public string Name { get; set; }
        
    public string MessageContentOne { get; set; }

    public string MessageContentTwo { get; set; }

    public string MessageContentThree { get; set; }
}