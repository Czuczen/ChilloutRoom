using System.Collections.Generic;

namespace CzuczenLand.ExtendingFunctionalities.Services.Crud.Builder.Dto;

/// <summary>
/// Klasa reprezentująca odpowiedź asynchroniczną związaną z operacjami CRUD na encjach.
/// </summary>
public class EntityAsyncCrudResponse
{
    /// <summary>
    /// Lista nazw pól w bazie danych.
    /// </summary>
    public List<string> DbProperties { get; set; }
       
    /// <summary>
    /// Lista nazw pól dla użytkownika.
    /// </summary>
    public List<string> HrProperties { get; set; }
        
    /// <summary>
    /// Flaga określająca, czy możliwe jest tworzenie nowych rekordów encji.
    /// </summary>
    public bool CanCreate { get; set; }

    /// <summary>
    /// Lista rekordów encji.
    /// Przy usuwaniu lista identyfikatorów.
    /// </summary>
    public List<object> Records { get; set; }
        
    /// <summary>
    /// Wiadomość informacyjna.
    /// </summary>
    public string InfoMsg { get; set; }
}