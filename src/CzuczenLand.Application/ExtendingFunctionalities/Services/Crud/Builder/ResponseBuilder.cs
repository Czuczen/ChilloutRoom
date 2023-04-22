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

public class ResponseBuilder<TEntityDto> : IResponseBuilder<TEntityDto>
    where TEntityDto : class, IEntityDto<int>
{
    private long? _userId;
    private readonly IHubContext _infoHub;
    private List<PropertyInfo> _properties;
    private readonly IViewParser _viewParser;
    private readonly IRepository<District, int> _districtRepository;
    private readonly List<object> _items = new();
    private readonly EntityAsyncCrudResponse _response = new();
        
    public IAbpSession AbpSession { get; set; }
    
    public ILogger Logger { get; set; }

    public int? DistrictWardenId { get; private set; }
        
    private List<PropertyInfo> Properties => _properties ??= typeof(TEntityDto).GetProperties().ToList();
        
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
    
    public ResponseBuilder<TEntityDto> WithDistrictWardenId()
    {
        DistrictWardenId = _districtRepository.GetAllList(item => item.UserId == UserId).SingleOrDefault()?.Id;
        
        return this;
    }
        
    public ResponseBuilder<TEntityDto> WithCanCreate(bool canCreate)
    {
        _response.CanCreate = canCreate;
        
        return this;
    }

    public ResponseBuilder<TEntityDto> WithInfo(string info)
    {
        _response.InfoMsg = info;
        
        return this;
    }
        
    public ResponseBuilder<TEntityDto> AddItems(object obj)
    {
        if (obj == null) return this;
        
        if (obj is IEnumerable enumerable)
            _items.AddRange(enumerable.Cast<object>().Where(castedObj => castedObj != null && !_items.Contains(castedObj)));
        else if (!_items.Contains(obj))
            _items.Add(obj);

        return this;
    }

    public async Task<EntityAsyncCrudResponse> Build(string crudAction)
    {
        _response.DbProperties = PlantationManagerHelper.GetPropList(typeof(TEntityDto));
        _response.HrProperties = PlantationManagerHelper.GetHrPropList(typeof(TEntityDto));
            
        _response.Records = _items.All(x => x is int) ? _items : await _viewParser.ParseObjectsValues(_items, Properties);
            
        if (crudAction != EntityAsyncCrudActions.ActionGetAvailableRecords)
            SendInfoToDistrictPlayers();
        
        return _response;
    }

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