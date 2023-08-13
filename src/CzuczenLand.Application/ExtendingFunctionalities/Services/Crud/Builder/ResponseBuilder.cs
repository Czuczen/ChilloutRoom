using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Castle.Core.Logging;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.ModelsFactory;
using CzuczenLand.ExtendingFunctionalities.PlantationManager;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.SignalRHubs.Plantation.Info;
using CzuczenLand.ExtendingModels.Models.General;
using Microsoft.AspNet.SignalR;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

/// <summary>
/// Klasa budująca odpowiedź związana z operacjami CRUD na encjach.
/// Klasa ta jest ręcznie rejestrowana do iniekcji zależności w CzuczenLandApplicationModule.
/// </summary>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
public class ResponseBuilder<TEntityDto> : IResponseBuilder<TEntityDto>
    where TEntityDto : class, IEntityDto<int>
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    private long? _userId;
    
    /// <summary>
    /// Kontekst huba informacyjnego.
    /// </summary>
    private readonly IHubContext _infoHub;
    
    /// <summary>
    /// Lista właściwości encji.
    /// </summary>
    private List<PropertyInfo> _properties;
    
    /// <summary>
    /// Parser wartości pól do wyświetlenia.
    /// </summary>
    private readonly IViewParser _viewParser;
    
    /// <summary>
    /// Repozytorium dzielnicy.
    /// </summary>
    private readonly IRepository<District, int> _districtRepository;
    
    /// <summary>
    /// Lista rekordów/identyfikatorów.
    /// </summary>
    private readonly List<object> _items = new();
    
    /// <summary>
    /// Odpowiedź CRUD encji.
    /// </summary>
    private readonly EntityAsyncCrudResponse _response = new();
        
    
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
    /// Identyfikator opiekuna dzielnicy.
    /// </summary>
    public int? DistrictWardenId { get; private set; }
        
    /// <summary>
    /// Właściwości encji.
    /// </summary>
    private List<PropertyInfo> Properties => _properties ??= typeof(TEntityDto).GetProperties().ToList();
        
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public long UserId
    {
        get
        {
            if (_userId != null) 
                return (long) _userId;
            
            _userId = AbpSession.GetUserId();
            
            return (long) _userId;
        }
    }
        
    
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="districtRepository">Repozytorium dzielnicy.</param>
    /// <param name="viewParser">Parser wartości pól do wyświetlenia.</param>
    public ResponseBuilder(
        IRepository<District, int> districtRepository,
        IViewParser viewParser
    )
    {
        _districtRepository = districtRepository;
        _viewParser = viewParser;
            
        AbpSession = NullAbpSession.Instance;
        Logger = NullLogger.Instance;
            
        _infoHub = GlobalHost.ConnectionManager.GetHubContext<InfoHub>();
    }
    
    /// <summary>
    /// Ustawia identyfikator opiekuna dzielnicy.
    /// </summary>
    /// <returns>Obiekt ResponseBuilder z ustawionym identyfikatorem opiekuna dzielnicy.</returns>
    public ResponseBuilder<TEntityDto> WithDistrictWardenId()
    {
        DistrictWardenId = _districtRepository.GetAllList(item => item.UserId == UserId).SingleOrDefault()?.Id;
        
        return this;
    }
        
    /// <summary>
    /// Ustawia flagę określającą możliwość tworzenia nowych rekordów encji.
    /// </summary>
    /// <param name="canCreate">Flaga informująca o możliwości tworzenia.</param>
    /// <returns>Obiekt ResponseBuilder z ustawioną flagą możliwości tworzenia.</returns>
    public ResponseBuilder<TEntityDto> WithCanCreate(bool canCreate)
    {
        _response.CanCreate = canCreate;
        
        return this;
    }

    /// <summary>
    /// Ustawia wiadomość informacyjną.
    /// </summary>
    /// <param name="info">Wiadomość informacyjna.</param>
    /// <returns>Obiekt ResponseBuilder z ustawioną wiadomością informacyjną.</returns>
    public ResponseBuilder<TEntityDto> WithInfo(string info)
    {
        _response.InfoMsg = info;
        
        return this;
    }
       
    /// <summary>
    /// Dodaje rekordy lub identyfikatory encji do listy.
    /// </summary>
    /// <param name="obj">Obiekt/identyfikator lub kolekcja obiektów/identyfikatorów do dodania.</param>
    /// <returns>Obiekt ResponseBuilder z dodanymi rekordami/identyfikatorami.</returns>
    public ResponseBuilder<TEntityDto> AddItems(object obj)
    {
        if (obj == null) return this;
        
        if (obj is IEnumerable enumerable)
            _items.AddRange(enumerable.Cast<object>().Where(castedObj => castedObj != null && !_items.Contains(castedObj)));
        else if (!_items.Contains(obj))
            _items.Add(obj);

        return this;
    }

    /// <summary>
    /// Tworzy odpowiedź asynchroniczną związaną z operacjami CRUD na encjach.
    /// </summary>
    /// <param name="crudAction">Akcja CRUD.</param>
    /// <returns>Odpowiedź asynchroniczna z informacjami o operacji.</returns>
    public async Task<EntityAsyncCrudResponse> Build(string crudAction)
    {
        _response.DbProperties = PlantationManagerHelper.GetPropList(typeof(TEntityDto));
        _response.HrProperties = PlantationManagerHelper.GetHrPropList(typeof(TEntityDto));
            
        _response.Records = _items.All(x => x is int) ? _items : await _viewParser.ParseObjectsValues(_items, Properties);
            
        if (crudAction != EntityAsyncCrudActions.ActionGetAvailableRecords)
            SendInfoToDistrictPlayers();
        
        return _response;
    }

    /// <summary>
    /// Wysyła informacje o zmianie do graczy związanych z dzielnicą opiekuna.
    /// Jeśli administrator dokonał zmiany na innej encji niż dzielnica to identyfikator dzielnicy jest ustawiany na 0 i informacja o zmianie jest wysyłana do wszystkich graczy.
    /// --------------------------
    /// </summary>
    private void SendInfoToDistrictPlayers()
    {
        var entityType = DbModelFactory.GetDbEntityTypeByEntityDtoName(typeof(TEntityDto).Name);
        if (entityType.Name is EntitiesDbNames.News or EntitiesDbNames.PlantationStorage or EntitiesDbNames.PlayerStorage) return;

        var isDistrictEntity = entityType.Name == EntitiesDbNames.District;
        var firstElement = _items.FirstOrDefault();
        var editedDistrictId = isDistrictEntity ? firstElement is int element ? element : ((IEntityDto<int>) firstElement)?.Id : 0;

        var infoMessage = ResponseBuilderHelper.GetChangeInfoMessage(isDistrictEntity);
        var districtId = DistrictWardenId ?? editedDistrictId ?? 0 ;
        var info = new ChangeInfo{ InfoMessage = infoMessage, DistrictId = districtId };
                     
        _infoHub.Clients.All.changeInfo(info);
    }
}