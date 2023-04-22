using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.Utils;
using CzuczenLand.Tests.ExtendingFunctionalities.Parser.Models;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace CzuczenLand.Tests.ExtendingFunctionalities.Parser;

public class Parser_DisplayStrategy_Tests : CzuczenLandTestBase
{
    private readonly IParser _displayParser;
    
    public Parser_DisplayStrategy_Tests()
    {
        _displayParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);
    }

    [Fact]
    public void Parse_Property_Test()
    {
        // Arrange
        var dummyType = typeof(DummyParserWithUnUsedTypeModel);
        
        var stringTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.StringType));
        var intTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.IntType));
        var idTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.IdType));
        var longTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.LongType));
        var doubleTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.DoubleType));
        var decimalTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.DecimalType));
        var boolTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.BoolType));
        var dateTimeTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.DateTimeType));
        var guidTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.GuidType));
        
        var nullableTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.NullableType));
        var unusedTypeProp = dummyType.GetProperty(nameof(DummyParserWithUnUsedTypeModel.UnusedType));
        PropertyInfo nullProp = null;
        
        var stringTypeValue = "string type test";
        var intTypeValue = "10 000";
        var idTypeValue = "10000";
        var longTypeValue = "1 000 000";
        var doubleTypeValue = "234 234,8394644";
        var decimalTypeValue = "9 999 999 999 999 999 999,00";
        var boolTypeValue = "Tak";
        var dateTimeTypeValue = "25.02.2023 15:12:34";
        var guidTypeValue = "d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1";
        
        var nullableTypeValue = "";

        // Act
        var stringTypeResult = _displayParser.Parse(stringTypeProp, "string type test");
        var intTypeResult = _displayParser.Parse(intTypeProp, 10000);
        var idTypeResult = _displayParser.Parse(idTypeProp, 10000);
        var longTypeResult = _displayParser.Parse(longTypeProp, 1000000);
        var doubleTypeResult = _displayParser.Parse(doubleTypeProp, 234234.8394644);
        var decimalTypeResult = _displayParser.Parse(decimalTypeProp, 9999999999999999999);
        var boolTypeResult = _displayParser.Parse(boolTypeProp, true);
        var dateTimeTypeResult = _displayParser.Parse(dateTimeTypeProp, new DateTime(2023, 02, 25, 15, 12, 34));
        var guidTypeResult = _displayParser.Parse(guidTypeProp, Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"));
        
        var nullableTypeResult = _displayParser.Parse(nullableTypeProp, null);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(unusedTypeProp, 30000));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(nullProp, 23000.23333));
        Should.Throw<ArgumentNullException>(() => _displayParser.Parse(intTypeProp, null));

        Should.Throw<FormatException>(() => _displayParser.Parse(intTypeProp, "wrong int type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(longTypeProp, "wrong long type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(doubleTypeProp, "wrong double type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(decimalTypeProp, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(boolTypeProp, "wrong bool type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(dateTimeTypeProp, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse(guidTypeProp, "wrong guid type value"));
        
        // Assert
        stringTypeResult.ShouldBe(stringTypeValue);
        intTypeResult.ShouldBe(intTypeValue);
        idTypeResult.ShouldBe(idTypeValue);
        longTypeResult.ShouldBe(longTypeValue);
        doubleTypeResult.ShouldBe(doubleTypeValue);
        decimalTypeResult.ShouldBe(decimalTypeValue);
        boolTypeResult.ShouldBe(boolTypeValue);
        dateTimeTypeResult.ShouldBe(dateTimeTypeValue);
        guidTypeResult.ShouldBe(guidTypeValue);
        
        nullableTypeResult.ShouldBe(nullableTypeValue);
    }
    
    [Fact]
    public void Parse_Generic_Test()
    {
        // Arrange
        var stringTypeKey = nameof(DummyParserWithUnUsedTypeModel.StringType);
        var intTypeKey = nameof(DummyParserWithUnUsedTypeModel.IntType);
        var idTypeKey = nameof(DummyParserWithUnUsedTypeModel.IdType);
        var longTypeKey = nameof(DummyParserWithUnUsedTypeModel.LongType);
        var doubleTypeKey = nameof(DummyParserWithUnUsedTypeModel.DoubleType);
        var decimalTypeKey = nameof(DummyParserWithUnUsedTypeModel.DecimalType);
        var boolTypeKey = nameof(DummyParserWithUnUsedTypeModel.BoolType);
        var dateTimeTypeKey = nameof(DummyParserWithUnUsedTypeModel.DateTimeType);
        var guidTypeKey = nameof(DummyParserWithUnUsedTypeModel.GuidType);
        
        var nullableTypeKey = nameof(DummyParserWithUnUsedTypeModel.NullableType);
        var unusedTypeKey = nameof(DummyParserWithUnUsedTypeModel.UnusedType);
        var wrongKey = nameof(WrongDummyParserModel.FirstWrongProp);
        var emptyKey = "";
        var whiteSpaceKey = " ";
        string nullKey = null;
        
        var stringTypeValue = "string type test !!..";
        var intTypeValue = "103 123";
        var idTypeValue = "10234";
        var longTypeValue = "1 020 000";
        var doubleTypeValue = "94 763,5362745378";
        var decimalTypeValue = "12 000,12";
        var boolTypeValue = "Nie";
        var dateTimeTypeValue = "25.02.2000 15:12:34";
        var guidTypeValue = "d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1";
        
        var nullableTypeValue = "";

        // Act
        var stringTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(stringTypeKey, "string type test !!..");
        var intTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, 103123.00);
        var idTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(idTypeKey, 10234);
        var longTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, 1020000);
        var doubleTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, 94763.5362745378.ToString(CultureInfo.InvariantCulture));
        var decimalTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, 12000.120.ToString(CultureInfo.InvariantCulture));
        var boolTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, false);
        var dateTimeTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, new DateTime(2000, 02, 25, 15, 12, 34));
        var guidTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"));
        
        var nullableTypeResult = _displayParser.Parse<DummyParserWithUnUsedTypeModel>(nullableTypeKey, null);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(unusedTypeKey, 3453453));
        Should.Throw<ArgumentNullException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(nullKey, "null key test 121212,121212"));
        Should.Throw<ArgumentNullException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, null));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(wrongKey, 12.3333300));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(emptyKey, 454554.4540545));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(whiteSpaceKey, "white space test XD"));
        
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, "wrong int type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, "wrong long type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, "wrong double type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, "wrong bool type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, "wrong guid type value"));
        
        // Assert
        stringTypeResult.ShouldBe(stringTypeValue);
        intTypeResult.ShouldBe(intTypeValue);
        idTypeResult.ShouldBe(idTypeValue);
        longTypeResult.ShouldBe(longTypeValue);
        doubleTypeResult.ShouldBe(doubleTypeValue);
        decimalTypeResult.ShouldBe(decimalTypeValue);
        boolTypeResult.ShouldBe(boolTypeValue);
        dateTimeTypeResult.ShouldBe(dateTimeTypeValue);
        guidTypeResult.ShouldBe(guidTypeValue);
        
        nullableTypeResult.ShouldBe(nullableTypeValue);
    }
    
    [Fact]
    public void Parse_Type_Test()
    {
        // Arrange
        var type = typeof(DummyParserWithUnUsedTypeModel);
        Type nullType = null;
        
        var stringTypeKey = nameof(DummyParserWithUnUsedTypeModel.StringType);
        var intTypeKey = nameof(DummyParserWithUnUsedTypeModel.IntType);
        var idTypeKey = nameof(DummyParserWithUnUsedTypeModel.IdType);
        var longTypeKey = nameof(DummyParserWithUnUsedTypeModel.LongType);
        var doubleTypeKey = nameof(DummyParserWithUnUsedTypeModel.DoubleType);
        var decimalTypeKey = nameof(DummyParserWithUnUsedTypeModel.DecimalType);
        var boolTypeKey = nameof(DummyParserWithUnUsedTypeModel.BoolType);
        var dateTimeTypeKey = nameof(DummyParserWithUnUsedTypeModel.DateTimeType);
        var guidTypeKey = nameof(DummyParserWithUnUsedTypeModel.GuidType);
        
        var nullableTypeKey = nameof(DummyParserWithUnUsedTypeModel.NullableType);
        var unusedTypeKey = nameof(DummyParserWithUnUsedTypeModel.UnusedType);
        var wrongKey = nameof(WrongDummyParserModel.FirstWrongProp);
        var emptyKey = "";
        var whiteSpaceKey = " ";
        string nullKey = null;
        
        var stringTypeValue = "string type test :>'";
        var intTypeValue = "0";
        var idTypeValue = "0";
        var longTypeValue = "0";
        var doubleTypeValue = "0";
        var decimalTypeValue = "0,00";
        var boolTypeValue = "Tak";
        var dateTimeTypeValue = "01.01.0001 00:00:00";
        var guidTypeValue = "c7d41721-9588-4a09-a62d-2dc2eb612345";
        
        var nullableTypeValue = "";

        // Act
        var stringTypeResult = _displayParser.Parse(type, stringTypeKey, "string type test :>'");
        var intTypeResult = _displayParser.Parse(type, intTypeKey, 0);
        var idTypeResult = _displayParser.Parse(type, idTypeKey, 0);
        var longTypeResult = _displayParser.Parse(type, longTypeKey, 0);
        var doubleTypeResult = _displayParser.Parse(type, doubleTypeKey, 0);
        var decimalTypeResult = _displayParser.Parse(type, decimalTypeKey, 0);
        var boolTypeResult = _displayParser.Parse(type, boolTypeKey, true);
        var dateTimeTypeResult = _displayParser.Parse(type, dateTimeTypeKey, DateTime.MinValue);
        var guidTypeResult = _displayParser.Parse(type, guidTypeKey, Guid.Parse("c7d41721-9588-4a09-a62d-2dc2eb612345"));
        
        var nullableTypeResult = _displayParser.Parse(type, nullableTypeKey, null);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(type, unusedTypeKey, 9999999));
        Should.Throw<ArgumentNullException>(() => _displayParser.Parse(type, nullKey, "null key test 121212,121212"));
        Should.Throw<ArgumentNullException>(() => _displayParser.Parse(type, intTypeKey, null));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(type, wrongKey, 12000.3333300));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(type, emptyKey, 454554.4545450000));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(type, whiteSpaceKey, "white space test {}"));
        Should.Throw<NullReferenceException>(() => _displayParser.Parse(nullType, intTypeKey, "null type test 454545454,454545454...!"));

        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, "wrong int type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, "wrong long type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, "wrong double type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, "wrong bool type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _displayParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, "wrong guid type value"));
        
        // Assert
        stringTypeResult.ShouldBe(stringTypeValue);
        intTypeResult.ShouldBe(intTypeValue);
        idTypeResult.ShouldBe(idTypeValue);
        longTypeResult.ShouldBe(longTypeValue);
        doubleTypeResult.ShouldBe(doubleTypeValue);
        decimalTypeResult.ShouldBe(decimalTypeValue);
        boolTypeResult.ShouldBe(boolTypeValue);
        dateTimeTypeResult.ShouldBe(dateTimeTypeValue);
        guidTypeResult.ShouldBe(guidTypeValue);
        
        nullableTypeResult.ShouldBe(nullableTypeValue);
    }
    
    [Fact]
    public void Parse_Dictionary_Test()
    {
        // Arrange
        var dummyModel = new DummyParserUsedTypesModel
        {
            StringType = "string type test aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            IntType = int.MaxValue,
            IdType = int.MaxValue,
            LongType = long.MaxValue,
            DoubleType = 0.000000000000067,
            DecimalType = (decimal) 0.005,
            BoolType = true,
            DateTimeType = DateTime.MaxValue,
            GuidType = Guid.Empty,
            NullableType = null,
        };

        var dummyModelWithUnUsedField = new DummyParserWithUnUsedTypeModel
        {
            StringType = "string type test aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            IntType = int.MaxValue,
            IdType = int.MaxValue,
            LongType = long.MaxValue,
            DoubleType = 0.000000000000067,
            DecimalType = (decimal) 0.005,
            BoolType = true,
            DateTimeType = DateTime.MaxValue,
            GuidType = Guid.Empty,
            NullableType = null,
            UnusedType = 0
        };
        
        var properties = typeof(DummyParserUsedTypesModel).GetProperties().ToList();
        var propertiesWithUnUsedField = typeof(DummyParserWithUnUsedTypeModel).GetProperties().ToList();
        var fullDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(dummyModel));
        var fullDictionaryWithUnUsedFiled = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(dummyModelWithUnUsedField));
        var emptyDictionary = new Dictionary<string, object>();
        var emptyProperties = new List<PropertyInfo>();
        Dictionary<string, object> nullDictionary = null;
        List<PropertyInfo> nullProperties = null;
        var dictionaryWithWrongKey = new Dictionary<string, object> {[nameof(WrongDummyParserModel.FirstWrongProp)] = "wrong key"};
        var wrongProperty = new List<PropertyInfo> {typeof(WrongDummyParserModel).GetProperty(nameof(WrongDummyParserModel.SecondWrongProp))};

        var stringTypeValue = "string type test aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        var intTypeValue = "2 147 483 647";
        var idTypeValue = "2147483647";
        var longTypeValue = "9 223 372 036 854 775 807";
        var doubleTypeValue = "0,000000000000067";
        var decimalTypeValue = "0,01";
        var boolTypeValue = "Tak";
        var dateTimeTypeValue = "31.12.9999 23:59:59";
        var guidTypeValue = "00000000-0000-0000-0000-000000000000";
        
        var nullableTypeValue = "";
        var wrongPossibilitiesValue = 0;
        
        // Act
        var fullDictionaryResult = _displayParser.Parse(fullDictionary, properties);
        var emptyDictionaryResult = _displayParser.Parse(emptyDictionary, properties);
        var emptyPropertiesResult = _displayParser.Parse(fullDictionary, emptyProperties);
        var nullDictionaryResult = _displayParser.Parse(nullDictionary, properties);
        var nullPropertiesResult = _displayParser.Parse(fullDictionary, nullProperties);
        
        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _displayParser.Parse(dictionaryWithWrongKey, properties));
        Should.Throw<InvalidOperationException>(() => _displayParser.Parse(fullDictionary, wrongProperty));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(fullDictionaryWithUnUsedFiled, propertiesWithUnUsedField));
        
        // Assert
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.StringType)].ShouldBe(stringTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.IntType)].ShouldBe(intTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.IdType)].ShouldBe(idTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.LongType)].ShouldBe(longTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.DoubleType)].ShouldBe(doubleTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.DecimalType)].ShouldBe(decimalTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.BoolType)].ShouldBe(boolTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.DateTimeType)].ShouldBe(dateTimeTypeValue);
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.GuidType)].ShouldBe(guidTypeValue);
        
        fullDictionaryResult[nameof(DummyParserWithUnUsedTypeModel.NullableType)].ShouldBe(nullableTypeValue);

        emptyDictionaryResult.Count.ShouldBe(wrongPossibilitiesValue);
        emptyPropertiesResult.Count.ShouldBe(wrongPossibilitiesValue);
        nullDictionaryResult.Count.ShouldBe(wrongPossibilitiesValue);
        nullPropertiesResult.Count.ShouldBe(wrongPossibilitiesValue);
    }
    
    [Fact]
    public void Parse_ListObjects_Test()
    {
        // Arrange
        var fullObjectsListWithUnUsedField = new List<object>
        {
            new DummyParserWithUnUsedTypeModel
            {
                StringType = "string type test",
                IntType = 99999999,
                IdType = 999999,
                LongType = 9999999999999,
                DoubleType = 7854785478.8765400,
                DecimalType = (decimal) 12000.00000,
                BoolType = true,
                DateTimeType = new DateTime(2023, 02, 25, 00, 00, 00),
                GuidType = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"),
                NullableType = null,
                UnusedType = 234
            }
        };
        
        var fullObjectsList = new List<object>();
        for (var i = 0; i < 16; i++)
        {
            fullObjectsList.Add(new DummyParserUsedTypesModel
            {
                StringType = "string type test",
                IntType = 99999999,
                IdType = 999999,
                LongType = 9999999999999,
                DoubleType = 7854785478.8765400,
                DecimalType = (decimal) 12000.00000,
                BoolType = true,
                DateTimeType = new DateTime(2023, 02, 25, 00, 00, 00),
                GuidType = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"),
                NullableType = null
            });
        }

        var emptyObjectsList = new List<object>();
        List<object> nullObjectsList = null;

        var stringTypeValue = "string type test";
        var intTypeValue = "99 999 999";
        var idTypeValue = "999999";
        var longTypeValue = "9 999 999 999 999";
        var doubleTypeValue = "7 854 785 478,87654";
        var decimalTypeValue = "12 000,00";
        var boolTypeValue = "Tak";
        var dateTimeTypeValue = "25.02.2023 00:00:00";
        var guidTypeValue = "d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1";
        
        var nullableTypeValue = "";
        var wrongPossibilitiesValue = 0;
        
        // Act
        var fullObjectsResult = _displayParser.Parse(fullObjectsList);
        var emptyObjectsResult = _displayParser.Parse(emptyObjectsList);
        var nullObjectsResult = _displayParser.Parse(nullObjectsList);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(fullObjectsListWithUnUsedField));

        // Assert
        emptyObjectsResult.Count.ShouldBe(wrongPossibilitiesValue);
        nullObjectsResult.Count.ShouldBe(wrongPossibilitiesValue);

        foreach (var obj in fullObjectsResult)
        {
            obj.ShouldBeOfType<Dictionary<string, object>>();
            var currObj = (Dictionary<string, object>) obj;
            
            currObj[nameof(DummyParserWithUnUsedTypeModel.StringType)].ShouldBe(stringTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.IntType)].ShouldBe(intTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.IdType)].ShouldBe(idTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.LongType)].ShouldBe(longTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.DoubleType)].ShouldBe(doubleTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.DecimalType)].ShouldBe(decimalTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.BoolType)].ShouldBe(boolTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.DateTimeType)].ShouldBe(dateTimeTypeValue);
            currObj[nameof(DummyParserWithUnUsedTypeModel.GuidType)].ShouldBe(guidTypeValue);
        
            currObj[nameof(DummyParserWithUnUsedTypeModel.NullableType)].ShouldBe(nullableTypeValue);
        }
    }

    [Fact]
    public void Parse_ListOfValuesAndProperties_Test()
    {
        // Arrange
        var dummyWithUnUsedField = new DummyParserWithUnUsedTypeModel
        {
            StringType = "string type test",
            IntType = 10000,
            IdType = 10000,
            LongType = 1000000,
            DoubleType = 999999999999999,
            DecimalType = (decimal) 0.004,
            BoolType = true,
            DateTimeType = new DateTime(2023, 02, 25, 15, 12, 34),
            GuidType = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"),
            NullableType = null,
            UnusedType = 32000
        };
        
        var dummyModel = new DummyParserUsedTypesModel
        {
            StringType = "string type test",
            IntType = 10000,
            IdType = 10000,
            LongType = 1000000,
            DoubleType = 999999999999999,
            DecimalType = (decimal) 0.004,
            BoolType = true,
            DateTimeType = new DateTime(2023, 02, 25, 15, 12, 34),
            GuidType = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"),
            NullableType = null
        };
        
        var properties = typeof(DummyParserUsedTypesModel).GetProperties().ToList();
        var propertiesWithUnUsedField = typeof(DummyParserWithUnUsedTypeModel).GetProperties().ToList();
        var valuesWithUnUsedField = propertiesWithUnUsedField.Select(prop => prop.GetValue(dummyWithUnUsedField)).ToList();
        var values = properties.Select(prop => prop.GetValue(dummyModel)).ToList();
        var emptyValues = new List<object>();
        var emptyProperties = new List<PropertyInfo>();
        List<object> nullValues = null;
        List<PropertyInfo> nullProperties = null;
        var wrongNumberValues = values.Take(2).ToList();

        var stringTypeValue = "string type test";
        var intTypeValue = "10 000";
        var idTypeValue = "10000";
        var longTypeValue = "1 000 000";
        var doubleTypeValue = "999 999 999 999 999";
        var decimalTypeValue = "0,00";
        var boolTypeValue = "Tak";
        var dateTimeTypeValue = "25.02.2023 15:12:34";
        var guidTypeValue = "d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1";
        
        var nullableTypeValue = "";
        var wrongPossibilitiesValue = 0;
        
        // Act
        var valuesResult = _displayParser.Parse(values, properties);
        var emptyValuesResult = _displayParser.Parse(emptyValues, properties);
        var emptyPropertiesResult = _displayParser.Parse(values, emptyProperties);
        var nullValuesResult = _displayParser.Parse(nullValues, properties);
        var nullPropertiesResult = _displayParser.Parse(values, nullProperties);
    
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(wrongNumberValues, properties));
        Should.Throw<ArgumentOutOfRangeException>(() => _displayParser.Parse(valuesWithUnUsedField, propertiesWithUnUsedField));

        // Assert
        valuesResult[0].ShouldBe(stringTypeValue);
        valuesResult[1].ShouldBe(intTypeValue);
        valuesResult[2].ShouldBe(idTypeValue);
        valuesResult[3].ShouldBe(longTypeValue);
        valuesResult[4].ShouldBe(doubleTypeValue);
        valuesResult[5].ShouldBe(decimalTypeValue);
        valuesResult[6].ShouldBe(boolTypeValue);
        valuesResult[7].ShouldBe(dateTimeTypeValue);
        valuesResult[8].ShouldBe(guidTypeValue);
        
        valuesResult[9].ShouldBe(nullableTypeValue);

        emptyValuesResult.Count.ShouldBe(wrongPossibilitiesValue);
        emptyPropertiesResult.Count.ShouldBe(wrongPossibilitiesValue);
        nullValuesResult.Count.ShouldBe(wrongPossibilitiesValue);
        nullPropertiesResult.Count.ShouldBe(wrongPossibilitiesValue);
    }
}
