using System.Collections.Generic;
using Abp.AutoMapper;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;

namespace CzuczenLand.Web.Models.ExtendingModels.ConfigurationPanel;

[AutoMapFrom(typeof(Logs))]
public class LogsViewModel
{
    public bool HasPermission => string.IsNullOrWhiteSpace(PermissionInfo);
        
    public string PermissionInfo { get; set; }

    public List<List<string>> InfoLogs { get; } = new();
    public List<List<string>> DebugLogs { get; } = new();
    public List<List<string>> WarnLogs { get; } = new();
    public List<List<string>> ErrorLogs { get; } = new();
}