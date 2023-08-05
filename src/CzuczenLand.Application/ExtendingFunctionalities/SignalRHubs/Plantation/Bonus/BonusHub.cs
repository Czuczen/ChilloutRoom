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

/// <summary>
/// Hub SignalR obsługujący bonusy.
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class BonusHub : Hub, ITransientDependency
{
    /// <summary>
    /// Repozytorium przechowujące bonusy w bazie danych.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.Products.Bonus> _bonusRepository;
    
    /// <summary>
    /// Repozytorium przechowujące magazyny plantacji w bazie danych.
    /// </summary>
    private readonly IRepository<PlantationStorage> _plantationStorageRepository;
    
    /// <summary>
    /// Serwis do zarządzania ignorowaniem zmian.
    /// </summary>
    private readonly IgnoreChangeService _ignoreChangeService;
    
    
    /// <summary>
    /// Właściwość pozwalająca na uzyskanie dostępu do sesji Abp, która przechowuje informacje dotyczące aktualnie zalogowanego użytkownika.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public IAbpSession AbpSession { get; set; }
    
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }

    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="bonusRepository"></param>
    /// <param name="plantationStorageRepository"></param>
    /// <param name="ignoreChangeService"></param>
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

    /// <summary>
    /// Metoda asynchroniczna, obsługująca akcje bonusów.
    /// </summary>
    /// <param name="bonusId">Identyfikator bonusu.</param>
    /// <param name="artifactAction">Akcja związana z artefaktem. Domyślnie wartość jest pusta.</param>
    /// <returns>Obiekt zawierający informacje o aktywacji bonusu.</returns>
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

    /// <summary>
    /// Metoda służy do odblokowania miejsca na bonus w określonej dzielnicy dla danego użytkownika.
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy.</param>
    /// <param name="unlockType">Typ odblokowania (artefakt lub wzmocnienie).</param>
    /// <returns>Obiekt zawierający informacje o aktywacji odblokowania.</returns>
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