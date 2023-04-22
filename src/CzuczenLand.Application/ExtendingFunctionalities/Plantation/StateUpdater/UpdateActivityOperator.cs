using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.EntityAnalysisStateBuilder;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.PlantationStateUpdater;
using CzuczenLand.ExtendingFunctionalities.StateUpdater.Updaters.QuestsStateUpdater;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.StateUpdater;

/// <summary>
/// Nie może być async bo entityhistory nie zdąży utworzyć zmiany. Musi być synchroniczny.
/// ISingletonDependency dla wiekszej wydajności. Task.Run dla wiekszej wydajności. Bez tych rozwiązań sianie rośliny trwało około 12 sekund dlatego, że każdy event wykonywał się po kolei. Dzięki Task.Run event przychodzi, wrzuca działanie do wątku i zwalnia kolejkę dla kolejnego eventu. Działania i tak wykonują się wręcz równocześnie. 
/// Z końcówkami ing (EntityUpdatingEventData) nie działa. Problem z jednostką pracy w PlantationController. Zamyka objectcontext czyli potrzebny using ale i tak nic nie pomagał bo przerywało transakcje.
/// </summary>
public class UpdateActivityOperator :
    IEventHandler<EntityUpdatedEventData<DriedFruit>>,
    IEventHandler<EntityUpdatedEventData<Lamp>>,
    IEventHandler<EntityUpdatedEventData<Manure>>,
    IEventHandler<EntityUpdatedEventData<Pot>>,
    IEventHandler<EntityUpdatedEventData<Seed>>,
    IEventHandler<EntityUpdatedEventData<Soil>>,
    IEventHandler<EntityUpdatedEventData<Water>>,
    IEventHandler<EntityUpdatedEventData<Bonus>>,
        
    IEventHandler<EntityUpdatedEventData<Quest>>,
    IEventHandler<EntityUpdatedEventData<PlantationStorage>>,
    IEventHandler<EntityUpdatedEventData<PlayerStorage>>,
    IEventHandler<EntityDeletedEventData<Plant>>,
    IEventHandler<EntityCreatedEventData<Plant>>,
    IEventHandler<EntityCreatedEventData<BlackMarketTransaction>>,
    IEventHandler<EntityDeletedEventData<BlackMarketTransaction>>,
    ISingletonDependency
{
    public const string ActionCreated = "ActionCreated";
    public const string ActionUpdated = "ActionUpdated";
    public const string ActionDeleted = "ActionDeleted";
    public const string ActionChanged = "ActionChanged";
        
    public const string ActionCreating = "ActionCreating";
    public const string ActionUpdating = "ActionUpdating";
    public const string ActionDeleting = "ActionDeleting";
    public const string ActionChanging = "ActionChanging";

    private readonly IEntityAnalysisStateBuilder _entityAnalysisStateBuilder;
    
    public ILogger Logger { get; set; }
        
    
    public UpdateActivityOperator(IEntityAnalysisStateBuilder entityAnalysisStateBuilder)
    {
        Logger = NullLogger.Instance;
        _entityAnalysisStateBuilder = entityAnalysisStateBuilder;
    }

    /// <summary>
    /// Przechwytywanie wyjątków działa dla Handle event ale dla task run już nie. Handle event wysyła wyjątek przez signalr do użytkownika ale nie loguje do pliku.
    /// Dla task run wyjątek do użytkownika nie jest wysyłany ale jest logowanie do pliku.
    /// Przychodzą tu encje aktualizowane przez workery dlatego zweryfikowanie użytkownika, który miał by być poinformowany o błędzie może być nie możliwe.
    /// </summary>
    /// <param name="eventData"></param>
    public void HandleEvent(EntityUpdatedEventData<DriedFruit> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Lamp> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Manure> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Pot> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Seed> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Soil> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Water> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }
        
    public void HandleEvent(EntityUpdatedEventData<Bonus> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductsState()
                    .UpdateBonusesState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<Quest> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity)
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<PlantationStorage> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Storage, eventData.Entity)
                    .UpdateStorageState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityUpdatedEventData<PlayerStorage> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Storage, eventData.Entity)
                    .UpdateStorageState()
                    .UpdateProductOrStorageQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityDeletedEventData<Plant> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity, ActionDeleted)
                    .UpdatePlantQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityCreatedEventData<Plant> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.Product, eventData.Entity, ActionCreated)
                    .UpdatePlantQuestsState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityCreatedEventData<BlackMarketTransaction> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.BlackMarket, eventData.Entity, ActionCreated)
                    .UpdateBlackMarketState();
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }

    public void HandleEvent(EntityDeletedEventData<BlackMarketTransaction> eventData)
    {
        Task.Run(() =>
        {
            try
            {
                _entityAnalysisStateBuilder
                    .Build(EnumUtils.AnalysisBuildTypes.BlackMarket, eventData.Entity, ActionDeleted)
                    .UpdateBlackMarketState();

                if (eventData.Entity.SellerId != null)
                {
                    _entityAnalysisStateBuilder
                        .Build(EnumUtils.AnalysisBuildTypes.BlackMarket, eventData.Entity, ActionDeleted,
                            EnumUtils.BlackMarketPages.Seller)
                        .UpdateBlackMarketQuestsState();
                }

                if (eventData.Entity.BuyerId != null)
                {
                    _entityAnalysisStateBuilder
                        .Build(EnumUtils.AnalysisBuildTypes.BlackMarket, eventData.Entity, ActionDeleted,
                            EnumUtils.BlackMarketPages.Buyer)
                        .UpdateBlackMarketQuestsState();
                }
            }
            catch (Exception ex)
            {
                if (ex is EntityNotFoundException && ex.Message.Contains("There is no such an entity")) 
                    Logger.Warn("Wyjątek wywołany prawdopodobnie w trakcie usuwania połączonych rekordów przez deleter'a. ||||||", ex);
                else
                    Logger.Error("Blad =======//========", ex);
            }
        });
    }
}