using System;

namespace CzuczenLand.Tests.ExtendingFunctionalities.Parser.Models;

public class DummyParserWithUnUsedTypeModel
{
    public string StringType { get; set; }

    public int IntType { get; set; }
    
    public int IdType { get; set; }

    public long LongType { get; set; }

    public double DoubleType { get; set; }

    public decimal DecimalType { get; set; }

    public bool BoolType { get; set; }

    public DateTime DateTimeType { get; set; }

    public Guid GuidType { get; set; }
    
    public int? NullableType { get; set; }
    
    public short UnusedType { get; set; }
}
