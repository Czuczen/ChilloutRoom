using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe.Dto;
using CzuczenLand.ExtendingModels.Models.Others;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.SignalRHubs.TicTacToe;

/// <summary>
/// Hub SignalR obsługujący grę w kółko i krzyżyk.
/// [AbpAuthorize] dawało wyjątek - Abp.Authorization.AbpAuthorizationException: Current user did not login to the application!
/// Nic nie psuł ale jednak wyskakiwał. W każdym hub'ie wyskakiwał. Użycie [AbpMvcAuthorize] rozwiązało problem.
/// </summary>
[AbpMvcAuthorize]
public class TicTacToeHub : Hub, ITransientDependency
{
    /// <summary>
    /// Klucz obiektu gry, który jest tworzony dla każdego nowo połączonego użytkownika.
    /// Obiekt jest umieszczany we właściwościach obiektu IOnlineClient.
    /// </summary>
    private const string GameDataKey = "ticTacToe";
    
    /// <summary>
    /// Wartość do porównania czy gra zakończyła się remisem
    /// </summary>
    private const string Tied = "Tied";
    
    /// <summary>
    /// Wartość do porównania czy gra zakończyła się wygraną
    /// </summary>
    private const string Win = "Win";
    
    /// <summary>
    /// Wartość do porównania czy gra zakończyła się przegraną
    /// </summary>
    private const string Lose = "Lose";

    
    /// <summary>
    /// Pole zawierające instancję klasy Random.
    /// </summary>
    private readonly Random _random = new();
    
    /// <summary>
    /// Repozytorium przechowujące dane gry w bazie danych.
    /// </summary>
    private readonly IRepository<TicTacToeStorage> _ticTacToeStorageRepository;
    
    /// <summary>
    /// Manager klientów do uzyskiwania informacji o użytkownikach online.
    /// </summary>
    private readonly IOnlineClientManager _onlineClientManager;
    
    /// <summary>
    /// Mapper obiektów wykorzystywany do przekształcania obiektów z jednego typu na inny.
    /// </summary>
    private readonly IObjectMapper _objectMapper;
        
    
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
    /// <param name="onlineClientManager">Manager klientów do uzyskiwania informacji o użytkownikach online.</param>
    /// <param name="objectMapper">Mapper obiektów wykorzystywany do przekształcania obiektów z jednego typu na inny.</param>
    /// <param name="ticTacToeStorageRepository">Repozytorium przechowujące dane gry w bazie danych.</param>
    public TicTacToeHub(
        IOnlineClientManager onlineClientManager,
        IObjectMapper objectMapper, 
        IRepository<TicTacToeStorage> ticTacToeStorageRepository
    )
    {
        _ticTacToeStorageRepository = ticTacToeStorageRepository;
        _onlineClientManager = onlineClientManager;
        _objectMapper = objectMapper;
            
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
    }

    /// <summary>
    /// Metoda asynchroniczna ustawiająca gracza w kolejce oczekujących na przeciwnika do gry w kółko i krzyżyk.
    /// Wyszukuje przeciwnika spośród wszystkich połączonych klientów i łączy graczy w parę do gry.
    /// </summary>
    public async Task SetInQueueRoom()
    {
        try
        {
            var groupWasFound = false;
            
            var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            var allClients = GetConnectedClients();
            var currClientTicTacToeData = (TicTacToeClientData) currClient.Properties[GameDataKey];

            foreach (var currOpponent in allClients)
            {
                try
                {
                    if (currClient.UserId == currOpponent.UserId) continue;
                
                    var opponentTicTacToeData = (TicTacToeClientData) currOpponent.Properties[GameDataKey];
                    if (opponentTicTacToeData.WaitForOpponent)
                    {
                        opponentTicTacToeData.WaitForOpponent = false;
                            
                        currClientTicTacToeData.OpponentConnectionId = currOpponent.ConnectionId;
                        opponentTicTacToeData.OpponentConnectionId = Context.ConnectionId;
                            
                        await Clients.Client(currOpponent.ConnectionId).opponentFound(currClient.ConnectionId);
                        await Clients.Client(currClient.ConnectionId).opponentFound(currOpponent.ConnectionId);

                        groupWasFound = true;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Blad=====/=====", ex);
                    await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
                }
            }
            
            if (!groupWasFound)
            {
                currClientTicTacToeData.WaitForOpponent = true;
                Clients.Caller.mustWaitForOpponent();
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
    }

    /// <summary>
    /// Metoda anulująca oczekiwanie na przeciwnika do gry w kółko i krzyżyk.
    /// Ustawia flagę WaitForOpponent na false w danych gracza.
    /// </summary>
    public void CancelWaitForOpponent()
    {
        try
        {
            var client = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            var ticTacToeData = (TicTacToeClientData) client.Properties[GameDataKey];
            ticTacToeData.WaitForOpponent = false;
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
    }
        
    /// <summary>
    /// Metoda asynchroniczna wywoływana po odrzuceniu prośby o grę przez przeciwnika w kółko i krzyżyk.
    /// Zmienia flagi WaitForOpponent oraz WantToPlayWithOpponent w danych graczy, którzy mieli do siebie dopasowanie.
    /// Wysyła wiadomość do gracza o odrzuceniu prośby o grę.
    /// </summary>
    /// <param name="opponentConnectionId">Identyfikator połączenia odrzucającego grę przeciwnika.</param>
    public async Task MatchRejected(string opponentConnectionId)
    {
        try
        {
            var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            var currClientTicTacToeData = (TicTacToeClientData) currClient.Properties[GameDataKey];
            currClientTicTacToeData.WaitForOpponent = false;
            currClientTicTacToeData.WantToPlayWithOpponent = false;
            
            var currOpponent = _onlineClientManager.GetByConnectionIdOrNull(opponentConnectionId);
            var currOpponentTicTacToeData = (TicTacToeClientData) currOpponent.Properties[GameDataKey];
            currOpponentTicTacToeData.WaitForOpponent = true;
            currOpponentTicTacToeData.WantToPlayWithOpponent = false;

            await Clients.Client(opponentConnectionId).opponentRejectMatch();
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
    }
        
    /// <summary>
    /// Metoda wywoływana podczas próby rozpoczęcia gry przez użytkownika.
    /// W zależności od stanu użytkownika (połączonego z serwerem), łączy użytkownika z jego przeciwnikiem lub oczekuje na wybór przeciwnika.
    /// Jeśli obaj użytkownicy wyrazili chęć rozpoczęcia gry z konkretnym przeciwnikiem, tworzy nową grupę dla gry, przypisuje graczom odpowiednie symbole i rozpoczyna grę.
    /// W przypadku błędu, wysyła informację o błędzie do użytkownika.
    /// </summary>
    /// <param name="opponentConnectionId">Identyfikator połączenia przeciwnika.</param>
    [UnitOfWork]
    public virtual async Task StartMatch(string opponentConnectionId)
    {
        try
        {
            var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            var currClientStorage = await _ticTacToeStorageRepository.SingleAsync(item => item.UserId == currClient.UserId);
            var currClientTicTacToeData = (TicTacToeClientData) currClient.Properties[GameDataKey];

            var currOpponent = _onlineClientManager.GetByConnectionIdOrNull(opponentConnectionId);
            var opponentTicTacToeData = (TicTacToeClientData) currOpponent?.Properties[GameDataKey];

            currClientTicTacToeData.WantToPlayWithOpponent = true;
            if (opponentTicTacToeData != null && opponentTicTacToeData.WantToPlayWithOpponent)
            {
                var groupName = Guid.NewGuid().ToString();
                opponentTicTacToeData.GroupName = groupName;
                currClientTicTacToeData.GroupName = groupName;
                var opponentStorage = await _ticTacToeStorageRepository.SingleAsync(item => item.UserId == currOpponent.UserId);
                    
                var symbolNumber = _random.Next(1, 3);
                var currPlayerSymbol = symbolNumber == 1 ? "X" : "O";
                var opponentSymbol = symbolNumber == 2 ? "X" : "O";

                currClientTicTacToeData.OpponentUserId = currOpponent.UserId;
                opponentTicTacToeData.OpponentUserId = currClient.UserId;
                
                currClientTicTacToeData.GameInProgress = true;
                opponentTicTacToeData.GameInProgress = true;
                
                currClientTicTacToeData.Symbol = currPlayerSymbol;
                opponentTicTacToeData.Symbol = opponentSymbol;
                
                var currPlayerData = new TicTacToeStartMatchData();
                var currPlayerMappedData = _objectMapper.Map(currClientStorage, currPlayerData);
                currPlayerMappedData.Symbol = currPlayerSymbol;
                
                var opponentData = new TicTacToeStartMatchData();
                var opponentMappedData = _objectMapper.Map(opponentStorage, opponentData);
                opponentMappedData.Symbol = opponentSymbol;
                    
                opponentTicTacToeData.WantToPlayWithOpponent = false;
                currClientTicTacToeData.WantToPlayWithOpponent = false;
                
                await Groups.Add(currOpponent.ConnectionId, groupName);
                await Groups.Add(Context.ConnectionId, groupName);
                    
                await Clients.Group(groupName, Context.ConnectionId).matchStarted(groupName, currPlayerMappedData);
                await Clients.Group(groupName, currOpponent.ConnectionId).matchStarted(groupName, opponentMappedData);
            }
            else if (opponentTicTacToeData != null)
            {
                await Clients.Client(Context.ConnectionId).mustWaitForOpponentChoice();
            }
            else
            {
                currClientTicTacToeData.WantToPlayWithOpponent = false;
                currClientTicTacToeData.WaitForOpponent = true;
                await Clients.Client(Context.ConnectionId).opponentDisconnectedFromQueue();
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
    }

    /// <summary>
    /// Metoda wywoływana podczas wykonania ruchu przez użytkownika.
    /// Pobiera dane użytkownika, który wykonał ruch w celu ustawienia symbolu na wybranym elemencie planszy.
    /// Następnie wysyła informację do wszystkich użytkowników w grupie o wykonanym ruchu.
    /// W przypadku błędu, wysyła informację o błędzie do użytkownika.
    /// </summary>
    /// <param name="groupName">Nazwa grupy, w której toczy się gra.</param>
    /// <param name="elementId">Identyfikator elementu planszy, na którym został wykonany ruch.</param>
    public async Task MakeMove(string groupName, string elementId)
    {
        try
        {
            var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            var clientData = (TicTacToeClientData) currClient.Properties[GameDataKey];
             
            await Clients.Group(groupName).setMove(elementId, clientData.Symbol);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
    }
        
    /// <summary>
    /// Metoda kończąca rozgrywkę w grze kółko i krzyżyk. Zapisuje wynik meczu w bazie danych użytkownika, który grał w grę.
    /// </summary>
    /// <param name="matchResult">Wynik meczu - Tied, Win lub Lose</param>
    /// <param name="fromBackend">Flaga określająca, czy zakończenie gry zostało wywołane przez klienta lub serwer na utratę połączenia przeciwnika.</param>
    /// <param name="opponentUserId">Id użytkownika będącego przeciwnikiem, który utracił połączenie.</param>
    /// <returns>Dane zakończonej rozgrywki w postaci obiektu klasy TicTacToeEndMatchData</returns>
    [UnitOfWork]
    public virtual async Task<TicTacToeEndMatchData> EndMatch(string matchResult, bool fromBackend = false, long? opponentUserId = null)
    {
        var ret = new TicTacToeEndMatchData();
        try
        {
            TicTacToeStorage storage;
            if (!fromBackend)
            {
                var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
                var ticTacToeData = (TicTacToeClientData) currClient.Properties[GameDataKey];
                await Groups.Remove(Context.ConnectionId, ticTacToeData.GroupName);
                    
                ticTacToeData.GameInProgress = false;
                ticTacToeData.OpponentConnectionId = null;
                ticTacToeData.GroupName = null;
                ticTacToeData.OpponentUserId = null;
                ticTacToeData.Symbol = null;
                ticTacToeData.WantToPlayWithOpponent = false;
                ticTacToeData.WaitForOpponent = false;

                storage = await _ticTacToeStorageRepository.SingleAsync(item => item.UserId == currClient.UserId);
            }
            else
            {
                storage = await _ticTacToeStorageRepository.SingleAsync(item => item.UserId == opponentUserId);
            }

            storage.GamesPlayed++;
            switch (matchResult)
            {
                case Tied:
                    storage.TiedGames++;
                    break;
                case Win:
                    storage.GamesWon++;
                    break;
                case Lose:
                    storage.GamesLost++;
                    break;
            }
         
            ret = _objectMapper.Map(storage, ret);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }
            
        return ret;
    }

    /// <summary>
    /// Metoda wywoływana przy połączeniu klienta z hub'em.
    /// Przypisuje klientowi nowy obiekt TicTacToeClientData przechowujący informacje o grze.
    /// Następnie pobiera listę połączonych klientów do gry i przesyła jej liczbę do wszystkich klientów.
    /// </summary>
    public override async Task OnConnected()
    {
        try
        {
            var currClient = _onlineClientManager.GetByConnectionIdOrNull(Context.ConnectionId);
            currClient.Properties[GameDataKey] = new TicTacToeClientData{ConnectionId = Context.ConnectionId};
            var allClients = GetConnectedClients();

            await Clients.All.usersCount(allClients.Count);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }

        await base.OnConnected();
    }

    /// <summary>
    /// Metoda wywoływana przy rozłączeniu klienta z hub'em.
    /// Sprawdza czy klient był w trakcie rozgrywki z innym klientem i w przypadku takiej sytuacji informuje drugiego gracza o rozłączeniu oraz kończy grę wynikiem porażki dla klienta, który się rozłączył.
    /// W przypadku, gdy klient był w kolejce do rozgrywki i akceptował dopasowanie a przeciwnik utracił połączenie akceptacja dopasowania jest anulowana.
    /// Pobiera listę połączonych klientów do gry i przesyła jej liczbę do wszystkich klientów.
    /// </summary>
    /// <param name="stopCalled">Czy rozłączenie zostało wywołane przez klienta.</param>
    public override async Task OnDisconnected(bool stopCalled)
    {
        try
        {
            var allClients = GetConnectedClients();
            foreach (var client in allClients)
            {
                try
                {
                    var ticTacToeData = (TicTacToeClientData) client.Properties[GameDataKey];
            
                    if (ticTacToeData.GameInProgress && !string.IsNullOrWhiteSpace(ticTacToeData.GroupName) && 
                        ticTacToeData.OpponentConnectionId == Context.ConnectionId)
                    {
                        await Clients.Group(ticTacToeData.GroupName).opponentDisconnected();
                        await EndMatch(Lose, true, ticTacToeData.OpponentUserId);
                    }
                    else if (ticTacToeData.OpponentConnectionId == Context.ConnectionId && ticTacToeData.WantToPlayWithOpponent)
                    {
                        ticTacToeData.WantToPlayWithOpponent = false;
                        ticTacToeData.WaitForOpponent = true;
                        await Clients.Client(ticTacToeData.ConnectionId).opponentDisconnectedFromQueue();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Blad=====/=====", ex);
                    await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
                }
            }

            await Clients.All.usersCount(allClients.Count);
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            await Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }

        await base.OnDisconnected(stopCalled);
    }

    /// <summary>
    /// Metoda zwracająca listę połączonych klientów, którzy posiadają dane do gry TicTacToe czyli są połączeni do hub'a gry w kółko i krzyżyk.
    /// </summary>
    /// <returns>Lista połączonych klientów do hub'a gry w kółko i krzyżyk.</returns>
    private List<IOnlineClient> GetConnectedClients()
    {
        var ret = new List<IOnlineClient>();
        try
        {
            var allClients = _onlineClientManager.GetAllClients();
            foreach (var client in allClients)
            {
                if (client.Properties.ContainsKey(GameDataKey))
                {
                    ret.Add(client);    
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Blad=====/=====", ex);
            Clients.Client(Context.ConnectionId).errorOccured(ex.Message);
        }

        return ret;
    }
}