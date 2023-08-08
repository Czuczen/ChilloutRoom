using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.SharedEntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.SharedDto.Base;

/// <summary>
/// Klasa reprezentująca dane transferowe (DTO) części magazynu, dziedzicząca po EntityDto int.
/// </summary>
public class PartStorageDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa magazynu.
    /// </summary>
    [DisplayName(PartStorageFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Poziom magazynu plantacji/gracza. 
    /// </summary>
    [DisplayName(PartStorageFieldsHrNames.Level)]
    public int Level { get; set; }
        
    /// <summary>
    /// Zdobyte doświadczenie.
    /// </summary>
    [DisplayName(PartStorageFieldsHrNames.GainedExperience)]
    public decimal GainedExperience { get; set; }
        
    /// <summary>
    /// Ilość złota w magazynie plantacji/gracza.
    /// </summary>
    [DisplayName(PartStorageFieldsHrNames.Gold)]
    public decimal Gold { get; set; }
    
    /// <summary>
    /// Identyfikator powiązanego użytkownika.
    /// </summary>
    [DisplayName(EntitiesHrNames.User), Display(GroupName = EntitiesDbNames.User)]
    public long UserId { get; set; }
}