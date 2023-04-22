namespace CzuczenLand.ExtendingFunctionalities.DistrictCloner.Dto;

public class SqlCommandContext
{
    public string Query { get; set; }
        
    public object[] SqlParameters { get; set; } 
}