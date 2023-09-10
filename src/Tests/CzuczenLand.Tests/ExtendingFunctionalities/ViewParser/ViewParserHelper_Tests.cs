using System.Linq;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using Shouldly;
using Xunit;

namespace CzuczenLand.Tests.ExtendingFunctionalities.ViewParser;

public class ViewParserHelper_Tests : CzuczenLandTestBase
{
    [Fact]
    public void GetRelationFieldEntityName_Test()
    {
        // Arrange
        var prop = typeof(LampDto).GetProperties().Single(item => item.Name == RelationFieldsNames.GeneratedTypeId);
        
        // Act
        var result = ViewParserHelper.GetRelationFieldEntityName(prop);
        
        // Assert
        result.ShouldBe("GeneratedType");
    }
    
    [Fact]
    public void GetRelationFields_Test()
    {
        // Arrange
        var type = typeof(LampDto);
        var properties = type.GetProperties().ToList();
        var generatedTypeRelProp = properties.Single(item => item.Name == RelationFieldsNames.GeneratedTypeId);
        
        // Act
        var result = ViewParserHelper.GetRelationFields(properties);
        
        // Assert
        result.ShouldContain(generatedTypeRelProp);
    }
}