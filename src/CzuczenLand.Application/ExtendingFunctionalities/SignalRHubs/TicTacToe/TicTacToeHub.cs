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

[AbpMvcAuthorize]
public class TicTacToeHub : Hub, ITransientDependency
{
    private const string GameDataKey = "ticTacToe";
    private const string Tied = "Tied";
    private const string Win = "Win";
    private const string Lose = "Lose";

    private readonly Random _random = new();
    private readonly IRepository<TicTacToeStorage> _ticTacToeStorageRepository;
    private readonly IOnlineClientManager _onlineClientManager;
    private readonly IObjectMapper _objectMapper;
        
    public IAbpSession AbpSession { get; set; }
    
    public ILogger Logger { get; set; }
    
    
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