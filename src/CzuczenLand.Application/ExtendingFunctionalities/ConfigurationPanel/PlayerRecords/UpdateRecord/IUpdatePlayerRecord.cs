using System.Collections.Generic;
using System.Threading.Tasks;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.PlayerRecords.UpdateRecord;

/// <summary>
/// Interfejs dla aktualizacji rekordów graczy generowanych na podstawie definicji.
/// </summary>
/// <typeparam name="TUpdateDto">Typ DTO używany do aktualizacji.</typeparam>
public interface IUpdatePlayerRecord<in TUpdateDto>
    where TUpdateDto : class
{
    /// <summary>
    /// Aktualizuje rekordy graczy na podstawie zaktualizowanej definicji.
    /// Aktualizuje tylko pola zawarte w modelu - TUpdatePlayerRecordDto.
    /// </summary>
    /// <param name="entity">DTO do aktualizacji encji.</param>
    Task Update(TUpdateDto entity);

    /// <summary>
    /// Aktualizuje rekordy graczy na podstawie zaktualizowanych pól dla wielu definicji.
    /// Aktualizuje tylko pola zawarte w modelu - TUpdatePlayerRecordDto.
    /// </summary>
    /// <param name="updatedFields">Zaktualizowane pola encji.</param>
    /// <param name="ids">Lista identyfikatorów zaktualizowanych definicji.</param>
    Task Update(object updatedFields, List<int> ids);
}