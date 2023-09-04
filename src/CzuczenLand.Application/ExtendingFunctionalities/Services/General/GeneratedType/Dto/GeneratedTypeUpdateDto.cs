using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Attributes;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

/// <summary>
/// Reprezentuje DTO służące do aktualizacji informacji w typie generowanym.
/// </summary>
[AutoMapTo(typeof(ExtendingModels.Models.General.GeneratedType))]
[AutoMapFrom(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeUpdateDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa typu generowanego.
    /// </summary>
    [FieldIsRequired]
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
}