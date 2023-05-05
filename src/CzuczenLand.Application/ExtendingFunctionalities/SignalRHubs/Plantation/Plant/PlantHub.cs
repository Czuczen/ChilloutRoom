using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.Services.General.PlantationStorage.Base;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Plant.Dto;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Plant;

/// <summary>
/// Hub SignalR obsługujący rośliny plantacji.
/// </summary>
[AbpMvcAuthorize]
public class PlantHub : Hub, ITransientDependency
{
    /// <summary>
    /// Serwis do zarządzania magazynami plantacji.
    /// </summary>
    private readonly IPlantationStorageService _plantationStorageService;
    
    /// <summary>
    /// Repozytorium przechowujące rośliny w bazie danych.
    /// </summary>
    private readonly IRepository<ExtendingModels.Models.General.Plant> _plantRepository;
    
    
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
    /// <param name="plantationStorageService">Serwis magazynu plantacji.</param>
    /// <param name="plantRepository">Repozytorium roślin.</param>
    public PlantHub(
        IPlantationStorageService plantationStorageService,
        IRepository<ExtendingModels.Models.General.Plant> plantRepository
    )
    {
        _plantRepository = plantRepository;
        _plantationStorageService = plantationStorageService;
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

    /// <summary>
    /// Metoda asynchroniczna, odpowiedzialna za odświeżenie stanu roślin gracza na podstawie przekazanych identyfikatorów roślin.
    /// </summary>
    /// <param name="plantsIds">Lista identyfikatorów roślin do odświeżenia.</param>
    /// <returns>Informacje o aktualnym stanie odświeżonych roślin gracza.</returns>
    [UnitOfWork]
    public virtual async Task<List<RefreshPlant>> RefreshPlayerPlants(List<int> plantsIds)
    {
        try
        {
            var listRefreshPlants = new List<RefreshPlant>();
            var plantationStorage = await _plantationStorageService.GetPlayerPlantationStorageForLastSelectedDistrictAsync(AbpSession.GetUserId());
            var plants = await _plantRepository.GetAllListAsync(item => item.PlantationStorageId == plantationStorage.Id);

            foreach (var id in plantsIds)
            {
                try
                {
                    var refreshPlant = new RefreshPlant();
                    var plant = plants.Single(item => item.Id == id);
                
                    refreshPlant.GrowingLevel = (decimal) plant.GrowingLevel;
                    refreshPlant.WiltLevel = plant.WiltLevel;
                    refreshPlant.TimeOfInsensitivity = plant.TimeOfInsensitivity;
                    refreshPlant.TimeRemaining = plant.TimeRemaining;
                    refreshPlant.PlantId = plant.Id;
                    
                    listRefreshPlants.Add(refreshPlant);
                }
                catch (Exception ex)
                {
                    if (ex is InvalidOperationException && ex.Message.Contains("Sequence contains no matching element")) 
                        Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                    else
                        Logger.Error("Blad =======//========", ex);
                }
            }
  
            return listRefreshPlants;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad =======//========", ex);
            await Clients.User(AbpSession.GetUserId().ToString()).errorOccured(ex.Message);
            return null; 
        }
    }
}