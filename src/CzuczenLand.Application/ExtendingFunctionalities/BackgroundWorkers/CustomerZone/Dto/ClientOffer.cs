using System;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.CustomerZone.Dto;

/// <summary>
/// Klasa reprezentująca ofertę klienta w strefie klienta.
/// </summary>
public class ClientOffer
{
    /// <summary>
    /// Identyfikator oferty klienta.
    /// </summary>
    public Guid Id { get; set; }
        
    /// <summary>
    /// Nazwa suszu dla oferty.
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// Identyfikator dzielnicy.
    /// </summary>
    public int DistrictId { get; set; }
        
    /// <summary>
    /// Identyfikator typu generowanego dla suszu.
    /// </summary>
    public int TypeId { get; set; }
        
    /// <summary>
    /// Czas trwania oferty.
    /// </summary>
    public int OfferTime { get; set; }
        
    /// <summary>
    /// Ilość produktu w ofercie.
    /// </summary>
    public decimal Amount { get; set; }
        
    /// <summary>
    /// Jednostka miary produktu.
    /// </summary>
    public string MeasureUnit { get; set; }
    
    /// <summary>
    /// Cena jednej sztuki produktu.
    /// </summary>
    public decimal BuyPrice { get; set; }
}