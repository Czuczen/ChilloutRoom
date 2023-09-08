using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.App;

/// <summary>
/// Interfejs serwisu aplikacyjnego do obsługi operacji na encji "News".
/// </summary>
public interface INewsAppService : 
    IAsyncCrudAppService<NewsDto, int, PagedResultRequestDto, NewsCreateDto, NewsUpdateDto>,
    IEntityAsyncCrudAppService<NewsCreateDto, NewsUpdateDto>
{
}