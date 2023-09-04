using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ConfigurationPanelManager.Dto;

/// <summary>
/// Klasa reprezentująca logi w panelu konfiguracyjnym.
/// </summary>
public class Logs
{
    /// <summary>
    /// Lista logów informacyjnych.
    /// </summary>
    public List<List<string>> InfoLogs { get; } = new();
        
    /// <summary>
    /// Lista logów debugowania.
    /// </summary>
    public List<List<string>> DebugLogs { get; } = new();
        
    /// <summary>
    /// Lista logów ostrzeżeń.
    /// </summary>
    public List<List<string>> WarnLogs { get; } = new();
        
    /// <summary>
    /// Lista logów błędów.
    /// </summary>
    public List<List<string>> ErrorLogs { get; } = new();
}