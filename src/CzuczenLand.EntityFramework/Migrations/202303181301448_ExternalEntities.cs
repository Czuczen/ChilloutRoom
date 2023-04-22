namespace CzuczenLand.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalEntities : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AbpWebhookSubscriptions", new[] { "TenantId" });
            DropPrimaryKey("dbo.AbpDynamicPropertyValues");
            DropPrimaryKey("dbo.AbpDynamicEntityPropertyValues");
            CreateTable(
                "dbo.BlackMarketTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        ItemName = c.String(),
                        ItemEntityName = c.String(),
                        SellerId = c.Long(),
                        SellerName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsCanceled = c.Boolean(nullable: false),
                        BuyerId = c.Long(),
                        GeneratedTypeId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bonus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsArtifact = c.Boolean(nullable: false),
                        ArtifactPutCost = c.Decimal(precision: 18, scale: 2),
                        ArtifactPullCost = c.Decimal(precision: 18, scale: 2),
                        RemainingActiveTime = c.Int(),
                        ActiveTimePerUse = c.Int(),
                        Color = c.String(),
                        Usages = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsStackable = c.Boolean(),
                        IncreaseDropChanceFromQuests = c.Decimal(precision: 18, scale: 2),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DistrictDons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DistrictId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(),
                        CyclicTime = c.Int(),
                        WiltSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrowingSpeedDivider = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HollowPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartGold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartExpToNextLevel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlayerLevelRequirement = c.Int(nullable: false),
                        IsDefined = c.Boolean(nullable: false),
                        HonorForTakingFirstPlace = c.Int(nullable: false),
                        HonorForTakingSecondPlace = c.Int(nullable: false),
                        HonorForTakingThirdPlace = c.Int(nullable: false),
                        HonorConsolationPrize = c.Int(nullable: false),
                        MaxBuffsSlots = c.Int(nullable: false),
                        MaxArtifactSlots = c.Int(nullable: false),
                        MaxDailyQuestsCount = c.Int(nullable: false),
                        MaxWeeklyQuestsCount = c.Int(nullable: false),
                        UnlockedBuffsSlotsOnStart = c.Int(nullable: false),
                        UnlockedArtifactSlotsOnStart = c.Int(nullable: false),
                        UnlockedDailyQuestsOnStart = c.Int(nullable: false),
                        UnlockedWeeklyQuestsOnStart = c.Int(nullable: false),
                        GoldExchangeRate = c.Int(nullable: false),
                        PrestigeExchangeRate = c.Int(nullable: false),
                        QuestTokenExchangeRate = c.Int(nullable: false),
                        DealerTokenExchangeRate = c.Int(nullable: false),
                        BlackMarketTokenExchangeRate = c.Int(nullable: false),
                        DonTokenExchangeRate = c.Int(nullable: false),
                        UnlockTokenExchangeRate = c.Int(nullable: false),
                        QuestTokenChanceFromCompleteQuest = c.Int(nullable: false),
                        DealerTokenChanceFromCustomerZone = c.Int(nullable: false),
                        BlackMarketTokenChanceForBlackMarketTransaction = c.Int(nullable: false),
                        DonTokenChanceFromSellBlackMarketTransaction = c.Int(nullable: false),
                        UnlockTokenChanceForCollectPlant = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonCharityPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrestigeToBecomeDon = c.Int(nullable: false),
                        LessLevelDoNotTakeQuests = c.Int(nullable: false),
                        ChanceForSpecialOfferInCustomerZone = c.Int(nullable: false),
                        StandardOfferQuantityLowerRangeInCustomerZone = c.Int(nullable: false),
                        StandardOfferQuantityHighRangeInCustomerZone = c.Int(nullable: false),
                        SpecialOfferQuantityLowerRangeInCustomerZone = c.Int(nullable: false),
                        SpecialOfferQuantityHighRangeInCustomerZone = c.Int(nullable: false),
                        OfferTimeInCustomerZone = c.Int(nullable: false),
                        ChanceForPayDonTribute = c.Int(nullable: false),
                        MaxBlackMarketTransactionsCount = c.Int(nullable: false),
                        ChanceForAddBuyBlackMarketTransaction = c.Int(nullable: false),
                        ChanceForIssueSeedAgainstDriedFruitInBlackMarket = c.Int(nullable: false),
                        ChanceForAddIssueBlackMarketTransaction = c.Int(nullable: false),
                        BuyUsersTransactions = c.Boolean(nullable: false),
                        NumberDrawnTransactionsToBuy = c.Int(nullable: false),
                        NumberDrawnTransactionsToIssue = c.Int(nullable: false),
                        BlackMarketIssueQuantityForDriedFruit = c.Int(nullable: false),
                        BlackMarketIssueQuantityForLamp = c.Int(nullable: false),
                        BlackMarketIssueQuantityForManure = c.Int(nullable: false),
                        BlackMarketIssueQuantityForPot = c.Int(nullable: false),
                        BlackMarketIssueQuantityForSeed = c.Int(nullable: false),
                        BlackMarketIssueQuantityForSoil = c.Int(nullable: false),
                        BlackMarketIssueQuantityForWater = c.Int(nullable: false),
                        BlackMarketIssueQuantityForBonus = c.Int(nullable: false),
                        QuestTokenForAchievementDailyOthersAmount = c.Int(nullable: false),
                        QuestTokenForEventAmount = c.Int(nullable: false),
                        QuestTokenForWeeklyAmount = c.Int(nullable: false),
                        DealerTokenWithItemAmountMoreThan100 = c.Int(nullable: false),
                        DealerTokenWithItemAmountMoreThan10 = c.Int(nullable: false),
                        DealerTokenWithItemAmountLessThan11 = c.Int(nullable: false),
                        BlackMarketTokenWithCostMoreThan100000 = c.Int(nullable: false),
                        BlackMarketTokenWithCostMoreThan10000 = c.Int(nullable: false),
                        BlackMarketTokenWithCostMoreThan5000 = c.Int(nullable: false),
                        BlackMarketTokenWithCostLessThan5001 = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DriedFruits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferChance = c.Int(),
                        AvailableInCustomerZone = c.Boolean(nullable: false),
                        CustomerZonePrice = c.Decimal(precision: 18, scale: 2),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DropQuests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DropId = c.Int(nullable: false),
                        QuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Drops",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GeneratedTypeId = c.Int(),
                        ChanceForDrop = c.Decimal(precision: 18, scale: 2),
                        ItemAmount = c.Decimal(precision: 18, scale: 2),
                        Gold = c.Decimal(precision: 18, scale: 2),
                        Prestige = c.Int(),
                        QuestToken = c.Int(),
                        DealerToken = c.Int(),
                        BlackMarketToken = c.Int(),
                        DonToken = c.Int(),
                        UnlockToken = c.Int(),
                        Honor = c.Int(),
                        Experience = c.Decimal(precision: 18, scale: 2),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneratedTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityName = c.String(),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IgnoreChanges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntityId = c.Int(nullable: false),
                        EntityName = c.String(),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lamps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InUseCount = c.Int(nullable: false),
                        CapacityInPotRequirement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Manures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CapacityInPotRequirement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        ImageUrl = c.String(),
                        TextColor = c.String(),
                        Name = c.String(),
                        MessageContentOne = c.String(),
                        MessageContentTwo = c.String(),
                        MessageContentThree = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlantationStorages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpToNextLevel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuestToken = c.Int(nullable: false),
                        DealerToken = c.Int(nullable: false),
                        BlackMarketToken = c.Int(nullable: false),
                        DonToken = c.Int(nullable: false),
                        UnlockToken = c.Int(nullable: false),
                        Prestige = c.Int(nullable: false),
                        BuffSlotsInUse = c.Int(nullable: false),
                        ArtifactSlotsInUse = c.Int(nullable: false),
                        StartedDailyQuestsCount = c.Int(nullable: false),
                        StartedWeeklyQuestsCount = c.Int(nullable: false),
                        MaxBuffsSlots = c.Int(nullable: false),
                        MaxArtifactSlots = c.Int(nullable: false),
                        MaxDailyQuestsCount = c.Int(nullable: false),
                        MaxWeeklyQuestsCount = c.Int(nullable: false),
                        UnlockedBuffsSlots = c.Int(nullable: false),
                        UnlockedArtifactSlots = c.Int(nullable: false),
                        UnlockedDailyQuestsCount = c.Int(nullable: false),
                        UnlockedWeeklyQuestsCount = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        GainedExperience = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Plants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        GrowingLevel = c.Double(nullable: false),
                        WiltLevel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ImageUrl = c.String(),
                        GrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ChanceForSeed = c.Int(nullable: false),
                        TimeOfInsensitivity = c.Int(nullable: false),
                        DriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SeedAmount = c.Int(nullable: false),
                        GainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeRemaining = c.Int(nullable: false),
                        GrowingSpeedDivider = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WiltSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetsBaf = c.Int(nullable: false),
                        LampId = c.Int(nullable: false),
                        SoilId = c.Int(nullable: false),
                        PotId = c.Int(nullable: false),
                        SeedId = c.Int(nullable: false),
                        WaterId = c.Int(nullable: false),
                        ManureId = c.Int(nullable: false),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerStorages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        Honor = c.Int(nullable: false),
                        LastSelectedDistrictId = c.Int(),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        GainedExperience = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Gold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaxRangeOfSoilClass = c.Int(nullable: false),
                        Capacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InUseCount = c.Int(nullable: false),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestRequirementsProgresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestId = c.Int(nullable: false),
                        RequirementsProgress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Quests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Duration = c.Decimal(precision: 18, scale: 2),
                        CurrentDuration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAvailableInitially = c.Boolean(nullable: false),
                        IsRepetitive = c.Boolean(nullable: false),
                        IsComplete = c.Boolean(nullable: false),
                        WorkerSent = c.Boolean(nullable: false),
                        CyclicTime = c.Int(),
                        CompletedAmount = c.Int(nullable: false),
                        PlantationLevelRequirement = c.Int(),
                        QuestType = c.String(),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        InProgress = c.Boolean(nullable: false),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                        PlayerStorageId = c.Int(),
                        Name = c.String(),
                        MessageContentOne = c.String(),
                        MessageContentTwo = c.String(),
                        MessageContentThree = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomEntityName = c.String(),
                        GeneratedTypeId = c.Int(),
                        Condition = c.String(),
                        Comparer = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Seeds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManureConsumption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaterConsumption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        CapacityInPotRequirement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Soils",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CapacityInPotRequirement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoilClass = c.Int(nullable: false),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicTacToeStorages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                        GamesPlayed = c.Int(nullable: false),
                        GamesWon = c.Int(nullable: false),
                        GamesLost = c.Int(nullable: false),
                        TiedGames = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Waters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CapacityInPotRequirement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseGrowingSpeed = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseTimeOfInsensitivity = c.Int(nullable: false),
                        IncreaseChanceForSeed = c.Int(nullable: false),
                        IncreaseDriedFruitAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncreaseSeedAmount = c.Int(nullable: false),
                        IncreaseGainedExp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SetName = c.String(),
                        Name = c.String(),
                        PlantationLevelRequirement = c.Int(nullable: false),
                        SellPrice = c.Decimal(precision: 18, scale: 2),
                        BuyPrice = c.Decimal(precision: 18, scale: 2),
                        IsShopItem = c.Boolean(nullable: false),
                        BlackMarketMinSellPrice = c.Decimal(precision: 18, scale: 2),
                        BlackMarketMaxSellPrice = c.Decimal(precision: 18, scale: 2),
                        IsBlackMarketWorkerItem = c.Boolean(nullable: false),
                        PlayerCanSellInBlackMarket = c.Boolean(nullable: false),
                        IgnoreChangeGuid = c.Guid(nullable: false),
                        OwnedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GeneratedTypeId = c.Int(nullable: false),
                        PlantationStorageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterTableAnnotations(
                "dbo.AbpWebhookSubscriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Int(),
                        WebhookUri = c.String(nullable: false),
                        Secret = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Webhooks = c.String(),
                        Headers = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_WebhookSubscriptionInfo_MayHaveTenant",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                });
            
            AddColumn("dbo.AbpAuditLogs", "ExceptionMessage", c => c.String(maxLength: 1024));
            AddColumn("dbo.AbpNotifications", "TargetNotifiers", c => c.String());
            AddColumn("dbo.AbpUserNotifications", "TargetNotifiers", c => c.String());
            AlterColumn("dbo.AbpDynamicPropertyValues", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.AbpDynamicEntityPropertyValues", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.AbpDynamicPropertyValues", "Id");
            AddPrimaryKey("dbo.AbpDynamicEntityPropertyValues", "Id");
            CreateIndex("dbo.AbpUserLogins", new[] { "ProviderKey", "TenantId" }, unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.AbpUserLogins", new[] { "ProviderKey", "TenantId" });
            DropPrimaryKey("dbo.AbpDynamicEntityPropertyValues");
            DropPrimaryKey("dbo.AbpDynamicPropertyValues");
            AlterColumn("dbo.AbpDynamicEntityPropertyValues", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AbpDynamicPropertyValues", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AbpUserNotifications", "TargetNotifiers");
            DropColumn("dbo.AbpNotifications", "TargetNotifiers");
            DropColumn("dbo.AbpAuditLogs", "ExceptionMessage");
            AlterTableAnnotations(
                "dbo.AbpWebhookSubscriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TenantId = c.Int(),
                        WebhookUri = c.String(nullable: false),
                        Secret = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Webhooks = c.String(),
                        Headers = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        CreatorUserId = c.Long(),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_WebhookSubscriptionInfo_MayHaveTenant",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                });
            
            DropTable("dbo.Waters");
            DropTable("dbo.TicTacToeStorages");
            DropTable("dbo.Soils");
            DropTable("dbo.Seeds");
            DropTable("dbo.Requirements");
            DropTable("dbo.Quests");
            DropTable("dbo.QuestRequirementsProgresses");
            DropTable("dbo.Pots");
            DropTable("dbo.PlayerStorages");
            DropTable("dbo.Plants");
            DropTable("dbo.PlantationStorages");
            DropTable("dbo.News");
            DropTable("dbo.Manures");
            DropTable("dbo.Lamps");
            DropTable("dbo.IgnoreChanges");
            DropTable("dbo.GeneratedTypes");
            DropTable("dbo.Drops");
            DropTable("dbo.DropQuests");
            DropTable("dbo.DriedFruits");
            DropTable("dbo.Districts");
            DropTable("dbo.DistrictDons");
            DropTable("dbo.Bonus");
            DropTable("dbo.BlackMarketTransactions");
            AddPrimaryKey("dbo.AbpDynamicEntityPropertyValues", "Id");
            AddPrimaryKey("dbo.AbpDynamicPropertyValues", "Id");
            CreateIndex("dbo.AbpWebhookSubscriptions", "TenantId");
        }
    }
}
