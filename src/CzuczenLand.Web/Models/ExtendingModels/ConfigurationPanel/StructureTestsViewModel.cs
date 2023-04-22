using System.Collections.Generic;
using System.Diagnostics;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

namespace CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

public class StructureTestsViewModel
{
    private Process _currProcess;
        
        
    public Process CurrProcess => _currProcess ??= Process.GetCurrentProcess(); 

    public bool HasPermission => string.IsNullOrWhiteSpace(PermissionInfo);
        
    public string PermissionInfo { get; set; }

    public List<StructureTest> StructureTests { get; set; } = new();
}