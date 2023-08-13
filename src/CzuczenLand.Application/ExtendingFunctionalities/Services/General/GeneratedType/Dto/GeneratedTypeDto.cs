using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesFieldsNames.Hr;
using CzuczenLand.ExtendingFunctionalities.Consts.Entities.EntitiesNames.Base;

namespace CzuczenLand.ExtendingFunctionalities.Services.General.GeneratedType.Dto;

/// <summary>
/// Reprezentuje DTO dla typu generowanego.
/// </summary>
[AutoMapFrom(typeof(ExtendingModels.Models.General.GeneratedType))]
public class GeneratedTypeDto : EntityDto<int>
{
    /// <summary>
    /// Nazwa typu generowanego.
    /// </summary>
    [DisplayName(GeneratedTypeFieldsHrNames.Name)]
    public string Name { get; set; }
        
    /// <summary>
    /// Nazwa encji powiązanej z typem generowanym.
    /// </summary>
    [DisplayName(GeneratedTypeFieldsHrNames.EntityName)]
    public string EntityName { get; set; }

    
    /// <summary>
    /// Identyfikator dzielnicy, do której przypisany jest typ generowany.
    /// </summary>
    [DisplayName(EntitiesHrNames.District), Display(GroupName = EntitiesDbNames.District)]
    public int DistrictId { get; set; }
}