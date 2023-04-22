using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

public interface IStructureTests : ITransientDependency
{
    Task<List<StructureTest>> BeginTests(bool isAdmin, long userId);
}