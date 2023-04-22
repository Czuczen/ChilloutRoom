using Abp.Web.Mvc.Views;

namespace CzuczenLand.Web.Views;

public abstract class CzuczenLandWebViewPageBase : CzuczenLandWebViewPageBase<dynamic>
{

}

public abstract class CzuczenLandWebViewPageBase<TModel> : AbpWebViewPage<TModel>
{
    protected CzuczenLandWebViewPageBase()
    {
        LocalizationSourceName = CzuczenLandConsts.LocalizationSourceName;
    }
}