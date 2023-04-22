using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;

public class Logs
{
    public List<List<string>> InfoLogs { get; } = new();
        
    public List<List<string>> DebugLogs { get; } = new();
        
    public List<List<string>> WarnLogs { get; } = new();
        
    public List<List<string>> ErrorLogs { get; } = new();
}