using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;
using CzuczenLand.ExtendingModels.Models.General;
using CzuczenLand.ExtendingModels.Models.Products;

namespace CzuczenLand.ExtendingFunctionalities.NewPlayerGenerator.Dto;

/// <summary>
/// Reprezentuje obiekt plantacji gracza.
/// Zawiera wszystkie informacje poza czarnym rynkiem potrzebne do załadowania strony www z plantacją.
/// </summary>
public class Plantation
{
    /// <summary>
    /// Identyfikator wybranej dzielnicy.
    /// </summary>
    public int? SelectedDistrictId { get; set; }
        
    /// <summary>
    /// Lista danych dla Select2 o dostępnych dzielnicach.
    /// </summary>
    public List<Select2Data> S2DistrictsList { get; set; }
        
    /// <summary>
    /// Określa, czy gracz jest nowym graczem.
    /// </summary>
    public bool IsNewPlayer { get; set; }
       
    /// <summary>
    /// Określa, czy poziom gracza jest zbyt niski dla danej dzielnicy.
    /// </summary>
    public bool TooLowLevel { get; set; }

    /// <summary>
    /// Stan przechowujący informacje o graczu (magazyn gracza).
    /// </summary>
    public PlayerStorage PlayerStorage { get; set; }
        
    /// <summary>
    /// Stan przechowujący informacje o plantacji gracza (magazyn plantacji).
    /// </summary>
    public PlantationStorage PlantationStorage { get; set; }
        
    /// <summary>
    /// Dzielnica, w której znajduje się plantacja.
    /// </summary>
    public District District { get; set; }
    
    /// <summary>
    /// Lista dostępnych dzielnic lub też dzielnice tydzień przed startem.
    /// </summary>
    public List<District> AvailableDistricts { get; set; }
    
    /// <summary>
    /// Słownik zawierający identyfikatory i nazwy opiekunów.
    /// </summary>
    public Dictionary<int, string> WardensNames { get; set; }

    /// <summary>
    /// Lista roślin na plantacji.
    /// </summary>
    public List<Plant> Plants { get; set; }
        
    /// <summary>
    /// Określa, czy gracz musi kupić dziuple.
    /// </summary>
    public bool MustBuyHollow { get; set; }
        
    /// <summary>
    /// Wiadomość o ładowaniu dzielnicy.
    /// Jeśli np. trzeba kupić dziuple, za mało kasy na dziuple itp.
    /// </summary>
    public string DistrictLoadingMessage { get; set; }
        
    /// <summary>
    /// Cena dziupli.
    /// </summary>
    public decimal HollowPrice { get; set; }
        
    /// <summary>
    /// Lista przefiltrowanych zadań.
    /// Zadania w prawym panelu bocznym strony www z plantacją.
    /// </summary>
    public List<Quest> FilteredQuests { get; set; }
        
    /// <summary>
    /// Lista suszów gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<DriedFruit> UserDriedFruits { get; set; }
    
    /// <summary>
    /// Lista lamp gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Lamp> UserLamps { get; set; }
    
    /// <summary>
    /// Lista nawozów gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Manure> UserManures { get; set; }
    
    /// <summary>
    /// Lista gleb gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Soil> UserSoils { get; set; }
    
    /// <summary>
    /// Lista wód gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Water> UserWaters { get; set; }
    
    /// <summary>
    /// Lista nasion gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Seed> UserSeeds { get; set; }
    
    /// <summary>
    /// Lista doniczek gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Pot> UserPots { get; set; }
        
    /// <summary>
    /// Lista bonusów gracza. Dla wyświetlania w zasobach.
    /// </summary>
    public List<Bonus> UserBonuses { get; set; }

    /// <summary>
    /// Lista wymian walut. Dla wyświetlania w zasobach.
    /// </summary>
    public List<CurrencyExchange> CurrencyExchanges { get; } = new();

    /// <summary>
    /// Dane dotyczące dona dzielnicy na której jest plantacja.
    /// </summary>
    public DonData DonData { get; set; } = new();

    /// <summary>
    /// Zestaw danych potrzebnych do tworzenia informacji o zadaniach.
    /// </summary>
    public QuestInfoCreation QuestInfoCreation { get; set; } = new();
}