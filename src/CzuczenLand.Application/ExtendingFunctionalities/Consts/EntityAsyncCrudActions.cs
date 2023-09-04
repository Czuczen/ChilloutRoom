namespace CzuczenLand.ExtendingFunctionalities.Consts;

/// <summary>
/// Klasa przechowująca nazwy akcji asynchronicznych operacji CRUD na encjach.
/// </summary>
public static class EntityAsyncCrudActions
{
    /// <summary>
    /// Nazwa akcji pobierającej dostępne rekordy.
    /// </summary>
    public const string ActionGetAvailableRecords = "ActionGetAvailableRecords";
    
    /// <summary>
    /// Nazwa akcji tworzącej rekord.
    /// </summary>
    public const string ActionCreate = "ActionCreate";
    
    /// <summary>
    /// Nazwa akcji aktualizującej rekord.
    /// </summary>
    public const string ActionUpdate = "ActionUpdate";
    
    /// <summary>
    /// Nazwa akcji aktualizującej wiele rekordów.
    /// </summary>
    public const string ActionUpdateMany = "ActionUpdateMany";
    
    /// <summary>
    /// Nazwa akcji usuwającej rekord.
    /// </summary>
    public const string ActionDelete = "ActionDelete";
    
    /// <summary>
    /// Nazwa akcji usuwającej wiele rekordów.
    /// </summary>
    public const string ActionDeleteMany = "ActionDeleteMany";
}