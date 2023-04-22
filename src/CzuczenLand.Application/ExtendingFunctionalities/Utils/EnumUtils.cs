namespace CzuczenLand.ExtendingFunctionalities.Utils;

public static class EnumUtils
{
    public enum BlackMarketPages
    {
        Seller,
        Buyer
    }
        
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

    public enum StructureTestsStatuses
    {
        Ok,
        Error,
        Warn
    }
        
    public enum ParseStrategies
    {
        Db,
        Display,
        Edit,
    }

    public enum AnalysisBuildTypes
    {
        Product,
        Storage,
        BlackMarket
    }
        
    public enum AnalysisTypes
    {
        ProductOrStorage,
        Plant,
        BlackMarket
    }
        
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
        
    public enum EntitiesPermissionAdmin
    {
        District,
        PlayerStorage,
        PlantationStorage,
        News,
    }
        
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