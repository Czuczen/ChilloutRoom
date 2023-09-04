namespace CzuczenLand.ExtendingFunctionalities.PlantationManager.Dto;

/// <summary>
/// Reprezentuje wymiany walut.
/// </summary>
public class CurrencyExchange
{
    /// <summary>
    /// Opis wymiany walut.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Aktualny kurs wymiany.
    /// </summary>
    public int ExchangeRate { get; set; }
        
    /// <summary>
    /// Identyfikator magazynu plantacji.
    /// </summary>
    public int PlantationStorageId { get; set; }
        
    /// <summary>
    /// Ilość posiadanej waluty.
    /// </summary>
    public decimal OwnedAmount { get; set; }
        
    /// <summary>
    /// Nazwa waluty.
    /// </summary>
    public string CurrencyName { get; set; }
        
    /// <summary>
    /// Cena sprzedaży jednostki waluty.
    /// </summary>
    public decimal SellPrice { get; set; }
        
    /// <summary>
    /// Cena kupna jednostki waluty.
    /// </summary>
    public decimal BuyPrice { get; set; }
        
    /// <summary>
    /// Określa, czy wymiana dotyczy tylko transferu złota do magazynu plantacji.
    /// </summary>
    public bool IsOnlyToPlantationGoldTransfer { get; set; }
}