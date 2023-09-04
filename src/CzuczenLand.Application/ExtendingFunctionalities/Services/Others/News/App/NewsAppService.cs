using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using CzuczenLand.ExtendingFunctionalities.Consts;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.AsyncCrud.EntityAsyncCrud;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Others.News.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Others.News.App;

/// <summary>
/// Serwis aplikacyjny do obsługi operacji na encji "News".
/// Rozszerza klasę EntityAsyncCrudAppService.
/// </summary>
[AbpAuthorize]
public class NewsAppService : 
    EntityAsyncCrudAppService<ExtendingModels.Models.Others.News, NewsDto, PagedResultRequestDto, NewsCreateDto, NewsUpdateDto>, 
    INewsAppService
{
    /// <summary>
    /// Konstruktor, który ustawia wstrzykiwane zależności.
    /// </summary>
    /// <param name="repository">Repozytorium encji "News".</param>
    /// <param name="responseBuilder">Klasa budująca odpowiedzi dla encji "News".</param>
    public NewsAppService(
        IRepository<ExtendingModels.Models.Others.News, int> repository,
        IResponseBuilder<NewsDto> responseBuilder
    )
        : base(repository, responseBuilder)
    {
    }

    /// <summary>
    /// Przeprowadza akcję pobierania dostępnych rekordów.
    /// </summary>
    /// <returns>Odpowiedź zawierająca listę dostępnych rekordów encji "News".</returns>
    public override async Task<EntityAsyncCrudResponse> ActionGetAvailableRecords()
    {
        await base.ActionGetAvailableRecords();
        var ret = ObjectMapper.Map<List<NewsDto>>(await Repository.GetAllListAsync());

        return await ResponseBuilder.AddItems(ret).Build(EntityAsyncCrudActions.ActionGetAvailableRecords);
    }
}