using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.ExtendingModels.Models.Products;
using CzuczenLand.Web.Models.ExtendingModels.Plantation.ProductInfo;

namespace CzuczenLand.Web.Models.ExtendingModels.Plantation;

[AutoMapFrom(typeof(Bonus))]
public class BonusViewModel : BonusInfoViewModel
{
    public int Id { get; set; }
        
    public int? RemainingActiveTime { get; set; }
    
    public string RemainingActiveTimeAsDdHhMmSs => RemainingActiveTime != null ? DateTimeUtils.ConvertSecondsToDdHhMmSs((int) RemainingActiveTime) : "";
}