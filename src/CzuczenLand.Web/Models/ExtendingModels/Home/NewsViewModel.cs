using Abp.AutoMapper;
using CzuczenLand.ExtendingModels.Models.Others;

namespace CzuczenLand.Web.Models.ExtendingModels.Home;

[AutoMapFrom(typeof(News))]
public class NewsViewModel : News
{
        
}