namespace CzuczenLand.ExtendingFunctionalities.Utils;

/// <summary>
/// Klasa pomocnicza dla typu - enum
/// </summary>
public static class EnumUtils
{
    /// <summary>
    /// Strony transakcji na czarnym rynku.
    /// </summary>
    public enum BlackMarketPages
    {
        Seller,
        Buyer
    }

    /// <summary>
    /// Obsługiwane typy pól
    /// </summary>
    public enum PropTypes
    {
        String,
        DateTime,
        Boolean,
        Double,
        Decimal,
        Int,
        Long,
        Guid
    }

    /// <summary>
    /// Statusy testów struktury
    /// </summary>
    public enum StructureTestsStatuses
    {
        Ok,
        Error,
        Warn
    }

    /// <summary>
    /// Strategie parsowania
    /// </summary>
    public enum ParseStrategies
    {
        Db,
        Display,
        Edit,
    }

    /// <summary>
    /// Typy ładowania danych do analizy encji na której nastąpiła zmiana
    /// </summary>
    public enum AnalysisBuildTypes
    {
        Product,
        Storage,
        BlackMarket
    }
     
    /// <summary>
    /// Typy analizy encji na której nastąpiła zmiana
    /// </summary>
    public enum AnalysisTypes
    {
        ProductOrStorage,
        Plant,
        BlackMarket
    }

    /// <summary>
    /// Encje rozszerzające boilerplate
    /// </summary>
    public enum Entities
    {
        DriedFruit,
        Lamp,
        Manure,
        Pot,
        Seed,
        Soil,
        Water,
        Bonus,

        News,
        User,

        Drop,
        PlantationStorage, 
        Plant,
        PlayerStorage,         
        Quest,
        Requirement, 
        District,
        GeneratedType,
            
        DropQuest,
    }

    /// <summary>
    /// Encje do których ma dostęp administrator w panelu konfiguracyjnym
    /// </summary>
    public enum EntitiesPermissionAdmin
    {
        District,
        PlayerStorage,
        PlantationStorage,
        News,
    }

    /// <summary>
    /// Encje do których ma dostęp opiekun dzielnicy w panelu konfiguracyjnym
    /// </summary>
    public enum EntitiesPermissionWarden
    {
        DriedFruit,
        Lamp,
        Manure,
        Pot,
        Seed,
        Soil,
        Water,
        Bonus,
            
        GeneratedType,
        PlayerStorage,
        PlantationStorage,
            
        Quest,
        Requirement,
        Drop,
            
        District,
        News,
    }
}