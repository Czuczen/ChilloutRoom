using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

public interface IResponseBuilder<TEntityDto>
    where TEntityDto : class, IEntityDto<int>
{
    long UserId { get; }
        
    int? DistrictWardenId { get; }
        
    ResponseBuilder<TEntityDto> WithDistrictWardenId();

    ResponseBuilder<TEntityDto> WithCanCreate(bool canCreate);

    ResponseBuilder<TEntityDto> WithInfo(string info);

    ResponseBuilder<TEntityDto> AddItems(object obj);

    Task<EntityAsyncCrudResponse> Build(string crudAction);
}