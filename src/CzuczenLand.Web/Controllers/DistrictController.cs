using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;
using CzuczenLand.Authorization;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;
using CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;
using CzuczenLand.Web.ViewModelsFactory;

namespace CzuczenLand.Web.Controllers;

[AbpMvcAuthorize(PermissionNames.Crud_Admin)]
public class DistrictController : CzuczenLandControllerBase
{
    private readonly IDistrictCloner _districtCloner;
        
    
    public DistrictController(IDistrictCloner districtCloner)
    {
        _districtCloner = districtCloner;
    }

    public async Task<ActionResult> CloneDistrictFromAppFolder()
    {
        var clone = await _districtCloner.Clone();
        var ret = ViewModelFactory.DistrictCloneViewModel(new List<DistrictContext> {clone});

        return PartialView("~/Views/District/_districtsClone.cshtml", ret);
    }
        
    public async Task<ActionResult> CloneExistingDistrictToAppFolderAsCsv(int id)
    {
        var ret = new DistrictCloneViewModel();

        if (id > 0)
        {
            var clone = await _districtCloner.Clone(id);
            ret = ViewModelFactory.DistrictCloneViewModel(new List<DistrictContext> {clone});
        }
        else
        {
            ret.ValidationMessage = "Id musi być większe niż 0";
        }

        return PartialView("~/Views/District/_districtsClone.cshtml", ret);
    }
        
    public async Task<ActionResult> CloneDistrictFromXlsFiles(string filesIds)
    {
        var ret = new DistrictCloneViewModel();
        
        if (string.IsNullOrWhiteSpace(filesIds))
        {
            ret.ValidationMessage = "Brak id";
            return PartialView("~/Views/District/_districtsClone.cshtml", ret);
        }
        
        var stringSeparators = new[] { "," };
        var filesIdsList = filesIds.Replace(" ", "").Split(stringSeparators, StringSplitOptions.None).ToList();

        if (filesIdsList.Count == 15 && filesIdsList.Count == filesIdsList.Distinct().Count())
        {
            var clone = await _districtCloner.Clone(filesIdsList);
            ret = ViewModelFactory.DistrictCloneViewModel(new List<DistrictContext>{clone});    
        }
        else
        {
            ret.ValidationMessage = "Wymagane jest id do 15 różnych plików";
        }
        
        return PartialView("~/Views/District/_districtsClone.cshtml", ret);
    }

    public async Task<ActionResult> CloneDistrictByAnotherExistingDistrict(int id, int howMany)
    {
        var ret = new DistrictCloneViewModel();

        if (id > 0 && howMany > 0)
        {
            var clones = await _districtCloner.Clone(id, howMany);
            ret = ViewModelFactory.DistrictCloneViewModel(clones);
        }
        else
        {
            ret.ValidationMessage = "Id i ilość muszą być wieksze niż 0";
        }
        
        return PartialView("~/Views/District/_districtsClone.cshtml", ret);
    }
}