using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner;

public interface IDistrictCloner : ITransientDependency
{
    Task<DistrictContext> Clone();

    Task<DistrictContext> Clone(int districtId);

    Task<DistrictContext> Clone(List<string> filesIds);

    Task<List<DistrictContext>> Clone(int districtId, int howMany);
}