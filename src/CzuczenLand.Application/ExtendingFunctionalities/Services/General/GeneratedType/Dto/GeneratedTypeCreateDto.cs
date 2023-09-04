using System.ComponentModel;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

/// <summary>
/// Reprezentuje DTO służące do tworzenia typu generowanego.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeCreateDto
{
    /// <summary>
    /// Nazwa typu generowanego.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Nazwa encji powiązanej z typem generowanym.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.EntityName)]
    public string EntityName { get; set; }
        
        
    /// <summary>
    /// Identyfikator dzielnicy, do której przypisany jest typ generowany.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(EntitiesHrNames.District)]
    public int DistrictId { get; set; }
}