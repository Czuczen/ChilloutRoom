using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner;

/// <summary>
/// Interfejs do klonowania dzielnic.
/// </summary>
public interface IDistrictCloner : ITransientDependency
{
    /// <summary>
    /// Wykonuje klonowanie z plików zamieszczonych w projekcie do bazy danych.
    /// </summary>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    Task<DistrictContext> Clone();

    /// <summary>
    /// Wykonuje klonowanie istniejącej dzielnicy do plików projektu w formatach .csv/.xls.
    /// https://cloudconvert.com/xls-to-xlsx
    /// xls dlatego, że konwerter z csv do xls dodawał do pierwszej kolumny znaki UTF-8 BOM
    /// z xls który tak naprawdę jest sformatowany jak csv konwertujemy na xlsx i wszystko jest ok
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy do sklonowania.</param>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    Task<DistrictContext> Clone(int districtId);

    /// <summary>
    /// Wykonuje klonowanie z plików (xls) na dysku Google do bazy danych.
    /// </summary>
    /// <param name="filesIds">Identyfikatory plików na dysku Google do sklonowania.</param>
    /// <returns>Kontekst dzielnicy po klonowaniu.</returns>
    Task<DistrictContext> Clone(List<string> filesIds);

    /// <summary>
    /// Wykonuje klonowanie istniejącej dzielnicy do bazy danych w określonej liczbie kopii.
    /// </summary>
    /// <param name="districtId">Identyfikator dzielnicy do sklonowania.</param>
    /// <param name="howMany">Liczba kopii do utworzenia.</param>
    /// <returns>Lista kontekstów dzielnic po klonowaniu.</returns>
    Task<List<DistrictContext>> Clone(int districtId, int howMany);
}