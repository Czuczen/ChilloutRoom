using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Services.General.Quest.Dto;
using CzuczenLand.ExtendingModels.Models.General;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Definitions.UpdatePlayerDefinition.SupportedTypes;

public class UpdateQuest : UpdateDefinition<Quest, QuestUpdateDto, QuestUpdateDefinitionDto>, IUpdateDefinition<QuestUpdateDto>
{
    public UpdateQuest(
        IRepository<Quest> repository,
        IRepository<District> districtRepository,
        IRepository<GeneratedType> generatedTypeRepository 
    ) 
        : base(repository, districtRepository, generatedTypeRepository)
    {
    }
}