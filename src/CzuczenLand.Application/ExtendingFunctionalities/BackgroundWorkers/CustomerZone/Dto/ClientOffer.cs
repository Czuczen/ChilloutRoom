using System;

namespace CzuczenLand.ExtendingFunctionalities.BackgroundWorkers.CustomerZone.Dto;

public class ClientOffer
{
    public Guid Id { get; set; }
        
    public string Name { get; set; }
        
    public int DistrictId { get; set; }
        
    public int TypeId { get; set; }
        
    public int OfferTime { get; set; }
        
    public decimal Amount { get; set; }
        
    public string MeasureUnit { get; set; }
        
    public decimal BuyPrice { get; set; }
}