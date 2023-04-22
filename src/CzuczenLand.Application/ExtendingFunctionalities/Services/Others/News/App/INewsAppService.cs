using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.App;

public interface INewsAppService : 
    IAsyncCrudAppService<NewsDto, int, PagedResultRequestDto, NewsCreateDto, NewsUpdateDto>,
    IEntityAsyncCrudAppService<NewsCreateDto, NewsUpdateDto>
{
        
}