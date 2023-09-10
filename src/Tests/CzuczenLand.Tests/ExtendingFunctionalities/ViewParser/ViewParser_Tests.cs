using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Castle.Core.Logging;
using CzuczenLand.Authorization.Users;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Consts.ViewParser;
using CzuczenLand.ExtendingFunctionalities.Repositories.CustomRepository.Loader;
using CzuczenLand.ExtendingFunctionalities.Services.Products.Lamp.Dto;
using Shouldly;
using Xunit;

namespace CzuczenLand.Tests.ExtendingFunctionalities.ViewParser;

public class ViewParser_Tests : CzuczenLandTestBase
{
    private readonly IViewParser _viewParser;
    
    public ViewParser_Tests()
    {
        _viewParser = Resolve<IViewParser>();
    }

    [Fact]
    public async Task ParseObjectsValues_Test()
    {
        // Arrange
        var dtoObjList = new List<LampDto>().Cast<object>().ToList();
        var properties = typeof(LampDto).GetProperties().ToList();
        
        var dtoObjValue = new List<LampDto>().Cast<object>().ToList();
        
        
        // Act
        var result = await _viewParser.ParseObjectsValues(dtoObjList, properties);
        
        
        
        // Assert
        result.ShouldContain(dtoObjValue);
    }
    
    [Fact]
    public async Task ParseRelationFieldToName_Test()
    {
        // Arrange
        var privateMethod = typeof(CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.ViewParser.ViewParser)
            .GetMethod("ParseRelationFieldToName", BindingFlags.NonPublic | BindingFlags.Instance);

        var objElem = new KeyValuePair<string, object>("GeneratedTypeId", 2);
        var prop = typeof(LampDto).GetProperties().Single(item => item.Name == RelationFieldsNames.GeneratedTypeId);

        // Act
        var result = (string) privateMethod.Invoke(_viewParser, new object[] { prop, objElem });

        // var result = (Task) privateMethod.Invoke(_viewParser, new object[] { prop, objElem });
        // await result.ConfigureAwait(false);
      
        // Assert
        result.ShouldBe("Lampa poziom 1");
    }
}