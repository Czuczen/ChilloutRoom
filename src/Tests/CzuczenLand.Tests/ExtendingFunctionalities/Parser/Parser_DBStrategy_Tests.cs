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

public class Parser_DBStrategy_Tests : CzuczenLandTestBase
{
    private readonly IParser _dbParser;
    
    public Parser_DBStrategy_Tests()
    {
        _dbParser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
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
        var intTypeValue = 10000;
        var idTypeValue = 10000;
        var longTypeValue = 1000000;
        var doubleTypeValue = 234234.8394644;
        var decimalTypeValue = 9999999999999999999.00;
        var boolTypeValue = true;
        var dateTimeTypeValue = DateTime.Parse("2023-02-25T15:12:34");
        var guidTypeValue = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1");
        
        string nullableTypeValue = null;

        // Act
        var stringTypeResult = _dbParser.Parse(stringTypeProp, "string type test");
        var intTypeResult = _dbParser.Parse(intTypeProp, 10000);
        var idTypeResult = _dbParser.Parse(idTypeProp, 10000);
        var longTypeResult = _dbParser.Parse(longTypeProp, 1000000);
        var doubleTypeResult = _dbParser.Parse(doubleTypeProp, 234234.8394644);
        var decimalTypeResult = _dbParser.Parse(decimalTypeProp, 9999999999999999999);
        var boolTypeResult = _dbParser.Parse(boolTypeProp, true);
        var dateTimeTypeResult = _dbParser.Parse(dateTimeTypeProp, new DateTime(2023, 02, 25, 15, 12, 34));
        var guidTypeResult = _dbParser.Parse(guidTypeProp, Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"));
        
        var nullableTypeResult = _dbParser.Parse(nullableTypeProp, null);
        
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(unusedTypeProp, 30000));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(nullProp, 23000.23333));
        Should.Throw<ArgumentNullException>(() => _dbParser.Parse(intTypeProp, null));
        
        Should.Throw<FormatException>(() => _dbParser.Parse(intTypeProp, "wrong int type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(longTypeProp, "wrong long type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(doubleTypeProp, "wrong double type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(decimalTypeProp, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(boolTypeProp, "wrong bool type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(dateTimeTypeProp, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse(guidTypeProp, "wrong guid type value"));
        
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
        var intTypeValue = 103123;
        var idTypeValue = 10234;
        var longTypeValue = 1020000;
        var doubleTypeValue = 94763.5362745378;
        var decimalTypeValue = 12000.12;
        var boolTypeValue = false;
        var dateTimeTypeValue = DateTime.Parse("2000-02-25T15:12:34");
        var guidTypeValue = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1");
        
        string nullableTypeValue = null;

        // Act
        var stringTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(stringTypeKey, "string type test !!..");
        var intTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, 103123.00);
        var idTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(idTypeKey, 10234);
        var longTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, 1020000);
        var doubleTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, 94763.5362745378.ToString(CultureInfo.InvariantCulture));
        var decimalTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, 12000.120.ToString(CultureInfo.InvariantCulture));
        var boolTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, false);
        var dateTimeTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, new DateTime(2000, 02, 25, 15, 12, 34));
        var guidTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1"));
        
        var nullableTypeResult = _dbParser.Parse<DummyParserWithUnUsedTypeModel>(nullableTypeKey, null);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(unusedTypeKey, 3453453));
        Should.Throw<ArgumentNullException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(nullKey, "null key test 121212,121212"));
        Should.Throw<ArgumentNullException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, null));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(wrongKey, 12.3333300));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(emptyKey, 454554.4540545));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(whiteSpaceKey, "white space test XD"));
        
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, "wrong int type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, "wrong long type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, "wrong double type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, "wrong bool type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, "wrong guid type value"));
        
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
        var intTypeValue = 0;
        var idTypeValue = 0;
        var longTypeValue = 0;
        var doubleTypeValue = 0;
        var decimalTypeValue = 0.00;
        var boolTypeValue = true;
        var dateTimeTypeValue = DateTime.Parse("0001-01-01T00:00:00");
        var guidTypeValue = Guid.Parse("c7d41721-9588-4a09-a62d-2dc2eb612345");
        
        string nullableTypeValue = null;
        
        // Act
        var stringTypeResult = _dbParser.Parse(type, stringTypeKey, "string type test :>'");
        var intTypeResult = _dbParser.Parse(type, intTypeKey, 0);
        var idTypeResult = _dbParser.Parse(type, idTypeKey, 0);
        var longTypeResult = _dbParser.Parse(type, longTypeKey, 0);
        var doubleTypeResult = _dbParser.Parse(type, doubleTypeKey, 0);
        var decimalTypeResult = _dbParser.Parse(type, decimalTypeKey, 0);
        var boolTypeResult = _dbParser.Parse(type, boolTypeKey, true);
        var dateTimeTypeResult = _dbParser.Parse(type, dateTimeTypeKey, DateTime.MinValue);
        var guidTypeResult = _dbParser.Parse(type, guidTypeKey, Guid.Parse("c7d41721-9588-4a09-a62d-2dc2eb612345"));
        
        var nullableTypeResult = _dbParser.Parse(type, nullableTypeKey, null);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(type, unusedTypeKey, 9999999));
        Should.Throw<ArgumentNullException>(() => _dbParser.Parse(type, nullKey, "null key test 121212,121212"));
        Should.Throw<ArgumentNullException>(() => _dbParser.Parse(type, intTypeKey, null));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(type, wrongKey, 12000.3333300));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(type, emptyKey, 454554.4545450000));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(type, whiteSpaceKey, "white space test {}"));
        Should.Throw<NullReferenceException>(() => _dbParser.Parse(nullType, intTypeKey, "null type test 454545454,454545454...!"));
        
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(intTypeKey, "wrong int type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(longTypeKey, "wrong long type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(doubleTypeKey, "wrong double type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(decimalTypeKey, "wrong decimal type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(boolTypeKey, "wrong bool type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(dateTimeTypeKey, "wrong datetime type value"));
        Should.Throw<FormatException>(() => _dbParser.Parse<DummyParserWithUnUsedTypeModel>(guidTypeKey, "wrong guid type value"));
        
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
            NullableType = null
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
        var intTypeValue = 2147483647;
        var idTypeValue = 2147483647;
        var longTypeValue = 9223372036854775807;
        var doubleTypeValue = 0.000000000000067;
        var decimalTypeValue = 0.01;
        var boolTypeValue = true;
        var dateTimeTypeValue = DateTime.Parse("9999-12-31T23:59:59");
        var guidTypeValue = Guid.Empty;
        
        string nullableTypeValue = null;
        var wrongPossibilitiesValue = 0;
        
        // Act
        var fullDictionaryResult = _dbParser.Parse(fullDictionary, properties);
        var emptyDictionaryResult = _dbParser.Parse(emptyDictionary, properties);
        var emptyPropertiesResult = _dbParser.Parse(fullDictionary, emptyProperties);
        var nullDictionaryResult = _dbParser.Parse(nullDictionary, properties);
        var nullPropertiesResult = _dbParser.Parse(fullDictionary, nullProperties);
        
        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _dbParser.Parse(dictionaryWithWrongKey, properties));
        Should.Throw<InvalidOperationException>(() => _dbParser.Parse(fullDictionary, wrongProperty));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(fullDictionaryWithUnUsedFiled, propertiesWithUnUsedField));
        
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
        var intTypeValue = 99999999;
        var idTypeValue = 999999;
        var longTypeValue = 9999999999999;
        var doubleTypeValue = 7854785478.87654;
        var decimalTypeValue = 12000.00;
        var boolTypeValue = true;
        var dateTimeTypeValue = DateTime.Parse("2023-02-25T00:00:00");
        var guidTypeValue = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1");
        
        string nullableTypeValue = null;
        var wrongPossibilitiesValue = 0;
        
        // Act
        var fullObjectsResult = _dbParser.Parse(fullObjectsList);
        var emptyObjectsResult = _dbParser.Parse(emptyObjectsList);
        var nullObjectsResult = _dbParser.Parse(nullObjectsList);

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(fullObjectsListWithUnUsedField));
        
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
        var intTypeValue = 10000;
        var idTypeValue = 10000;
        var longTypeValue = 1000000;
        var doubleTypeValue = 999999999999999;
        var decimalTypeValue = 0.00;
        var boolTypeValue = true;
        var dateTimeTypeValue = DateTime.Parse("2023-02-25T15:12:34");
        var guidTypeValue = Guid.Parse("d624e35c-0fa1-4b9f-a7f5-fcb96c7a68b1");
        
        string nullableTypeValue = null;
        var wrongPossibilitiesValue = 0;

        // Act
        var valuesResult = _dbParser.Parse(values, properties);
        var emptyValuesResult = _dbParser.Parse(emptyValues, properties);
        var emptyPropertiesResult = _dbParser.Parse(values, emptyProperties);
        var nullValuesResult = _dbParser.Parse(nullValues, properties);
        var nullPropertiesResult = _dbParser.Parse(values, nullProperties);
    
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(wrongNumberValues, properties));
        Should.Throw<ArgumentOutOfRangeException>(() => _dbParser.Parse(valuesWithUnUsedField, propertiesWithUnUsedField));
        
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
