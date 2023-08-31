using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests;

/// <summary>
/// Interfejs definiujący testy struktury definicji opiekunów dzielnic.
/// </summary>
public interface IStructureTests : ITransientDependency
{
    /// <summary>
    /// Rozpoczyna testy struktury aplikacji i zwraca ich wyniki.
    /// </summary>
    /// <param name="isAdmin">Określa, czy użytkownik jest administratorem.</param>
    /// <param name="userId">Identyfikator użytkownika.</param>
    /// <returns>Lista wyników testów struktury.</returns>
    Task<List<StructureTest>> BeginTests(bool isAdmin, long userId);
}