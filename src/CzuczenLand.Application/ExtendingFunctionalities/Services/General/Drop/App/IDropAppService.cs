using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.General.Drop.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.Drop.App;

public interface IDropAppService : 
    IAsyncCrudAppService<DropDto, int, PagedResultRequestDto, DropCreateDto, DropUpdateDto>,
    IEntityAsyncCrudAppService<DropCreateDto, DropUpdateDto>
{
        
}