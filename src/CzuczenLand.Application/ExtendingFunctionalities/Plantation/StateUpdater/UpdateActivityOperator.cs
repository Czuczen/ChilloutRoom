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
/// Operator do aktualizacji stanu gry gracza.
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
    /// <summary>
    /// Akcja reprezentująca utworzenie nowej encji.
    /// </summary>
    public const string ActionCreated = "ActionCreated";
    
    /// <summary>
    /// Akcja reprezentująca aktualizację istniejącej encji.
    /// </summary>
    public const string ActionUpdated = "ActionUpdated";
    
    /// <summary>
    /// Akcja reprezentująca usunięcie istniejącej encji.
    /// </summary>
    public const string ActionDeleted = "ActionDeleted";
    
    /// <summary>
    /// Akcja reprezentująca zmianę w encji (modyfikację, usunięcie lub inny rodzaj zmiany).
    /// </summary>
    public const string ActionChanged = "ActionChanged";
        
    /// <summary>
    /// Akcja reprezentująca tworzenie nowej encji.
    /// </summary>
    public const string ActionCreating = "ActionCreating";
    
    /// <summary>
    /// Akcja reprezentująca aktualizację istniejącej encji.
    /// </summary>
    public const string ActionUpdating = "ActionUpdating";
    
    /// <summary>
    /// Akcja reprezentująca usuwanie istniejącej encji.
    /// </summary>
    public const string ActionDeleting = "ActionDeleting";
    
    /// <summary>
    /// Akcja reprezentująca trwającą zmianę w encji (modyfikację, usunięcie lub inny rodzaj zmiany).
    /// </summary>
    public const string ActionChanging = "ActionChanging";
    
    /// <summary>
    /// Budowniczy stanu analizy encji.
    /// </summary>
    private readonly IEntityAnalysisStateBuilder _entityAnalysisStateBuilder;
    
    
    /// <summary>
    /// Interfejs ILogger służy do rejestrowania komunikatów z aplikacji.
    /// Właściwość musi być public oraz mieć getter i setter dla poprawnego działania wstrzykiwania właściwości.
    /// </summary>
    public ILogger Logger { get; set; }
        
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="entityAnalysisStateBuilder">Budowniczy stanu analizy encji.</param>
    public UpdateActivityOperator(IEntityAnalysisStateBuilder entityAnalysisStateBuilder)
    {
        Logger = NullLogger.Instance;
        _entityAnalysisStateBuilder = entityAnalysisStateBuilder;
    }
    
    // =================================================================================================================
    // Przechwytywanie wyjątków działa dla Handle event ale dla task run już nie. Handle event wysyła wyjątek przez signalr do użytkownika ale nie loguje do pliku.
    // Dla task run wyjątek do użytkownika nie jest wysyłany ale jest logowanie do pliku.
    // Przychodzą tu encje aktualizowane przez workery dlatego zweryfikowanie użytkownika, który miał by być poinformowany o błędzie może być nie możliwe.
    // =================================================================================================================

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "DriedFruit".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "DriedFruit".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Lamp".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Lamp".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Manure".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Manure".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Pot".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Pot".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Seed".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Seed".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Soil".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Soil".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Water".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Water".</param>
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
        
    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Bonus".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Bonus".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "Quest".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "Quest".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "PlantationStorage".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "PlantationStorage".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie aktualizacji encji "PlayerStorage".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia aktualizacji encji "PlayerStorage".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie usunięcia encji "Plant".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia usunięcia encji "Plant".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie utworzenia encji "Plant".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia utworzenia encji "Plant".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie utworzenia encji "BlackMarketTransaction".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia utworzenia encji "BlackMarketTransaction".</param>
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

    /// <summary>
    /// Obsługuje zdarzenie usunięcia encji "BlackMarketTransaction".
    /// </summary>
    /// <param name="eventData">Dane zdarzenia usunięcia encji "BlackMarketTransaction".</param>
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