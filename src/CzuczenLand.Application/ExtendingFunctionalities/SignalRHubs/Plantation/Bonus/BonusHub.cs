using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.Services.General.IgnoreChange.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Bonus;

[AbpMvcAuthorize]
public class BonusHub : Hub, ITransientDependency
{
    private readonly IRepository<ExtendingModels.Models.Products.Bonus> _bonusRepository;
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    private readonly IgnoreChangeService _ignoreChangeService;
    
    /// <summary>
    /// Musi być public i musi mieć setter
    /// </summary>
    public IAbpSession AbpSession { get; set; }
    
    public ILogger Logger { get; set; }

    
    public BonusHub(
        IRepository<ExtendingModels.Models.Products.Bonus> bonusRepository,
        IRepository<PlantationStorage> plantationStorageRepository,
        IgnoreChangeService ignoreChangeService
    )
    {
        _bonusRepository = bonusRepository;
        _plantationStorageRepository = plantationStorageRepository;
        _ignoreChangeService = ignoreChangeService;
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

    [UnitOfWork]
    public virtual async Task<BonusActivation> BonusAction(int bonusId, string artifactAction = "")
    {
        try
        {
            var ret = new BonusActivation();
            var bonus = await _bonusRepository.GetAsync(bonusId);
            var playerPlantationStorage = await _plantationStorageRepository.GetAsync((int) bonus.PlantationStorageId);
                
            if (bonus.PlantationLevelRequirement > playerPlantationStorage.Level)
            {
                ret.SuccessfulActivation = false;
                ret.InfoMessage.Add("Za niski poziom");
            }
            else
            {
                if (bonus.IsArtifact)
                    await BonusHubHelper.ProcessArtifact(ret, bonus, playerPlantationStorage, artifactAction, _ignoreChangeService);
                else
                    await BonusHubHelper.ProcessBuff(ret, bonus, playerPlantationStorage, _ignoreChangeService);   
            }

            return ret;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
            return null;
        }
    }
        
    [UnitOfWork]
    public virtual async Task<BonusActivation> UnlockBonusSlot(int districtId, string unlockType)
    {
        try
        {
            var ret = new BonusActivation();
            var userId = AbpSession.GetUserId();
            var playerPlantationStorage = await _plantationStorageRepository.SingleAsync(item =>
                item.UserId == userId && item.DistrictId == districtId);
                
            if (TokensOperator.UnlockTokenFee(playerPlantationStorage, ret, unlockType)) return ret;
                
            ret.SuccessfulActivation = false;
            ret.InfoMessage.Add("Brak żetonów odblokowania");

            return ret;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
            return null;
        }
    }
}