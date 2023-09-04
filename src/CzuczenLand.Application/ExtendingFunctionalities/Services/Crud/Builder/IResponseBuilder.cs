using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder;

/// <summary>
/// Interfejs budowniczego odpowiedzi związanej z operacjami CRUD na encjach.
/// </summary>
/// <typeparam name="TEntityDto">Typ DTO encji.</typeparam>
public interface IResponseBuilder<TEntityDto>
    where TEntityDto : class, IEntityDto<int>
{
    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    long UserId { get; }
        
    /// <summary>
    /// Identyfikator opiekuna dzielnicy.
    /// </summary>
    int? DistrictWardenId { get; }
        
    /// <summary>
    /// Ustawia identyfikator opiekuna dzielnicy.
    /// </summary>
    /// <returns>Obiekt ResponseBuilder z ustawionym identyfikatorem opiekuna dzielnicy.</returns>
    ResponseBuilder<TEntityDto> WithDistrictWardenId();

    /// <summary>
    /// Ustawia flagę określającą możliwość tworzenia nowych rekordów encji.
    /// </summary>
    /// <param name="canCreate">Flaga informująca o możliwości tworzenia.</param>
    /// <returns>Obiekt ResponseBuilder z ustawioną flagą możliwości tworzenia.</returns>
    ResponseBuilder<TEntityDto> WithCanCreate(bool canCreate);

    /// <summary>
    /// Ustawia wiadomość informacyjną.
    /// </summary>
    /// <param name="info">Wiadomość informacyjna.</param>
    /// <returns>Obiekt ResponseBuilder z ustawioną wiadomością informacyjną.</returns>
    ResponseBuilder<TEntityDto> WithInfo(string info);

    /// <summary>
    /// Dodaje rekordy lub identyfikatory encji do listy.
    /// </summary>
    /// <param name="obj">Obiekt/identyfikator lub kolekcja obiektów/identyfikatorów do dodania.</param>
    /// <returns>Obiekt ResponseBuilder z dodanymi rekordami/identyfikatorami.</returns>
    ResponseBuilder<TEntityDto> AddItems(object obj);

    /// <summary>
    /// Tworzy odpowiedź asynchroniczną związaną z operacjami CRUD na encjach.
    /// </summary>
    /// <param name="crudAction">Akcja CRUD.</param>
    /// <returns>Odpowiedź asynchroniczna z informacjami o operacji.</returns>
    Task<EntityAsyncCrudResponse> Build(string crudAction);
}