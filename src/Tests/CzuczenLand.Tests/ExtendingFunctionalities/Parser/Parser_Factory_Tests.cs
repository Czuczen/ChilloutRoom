using System;
using CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser;
using CzuczenLand.ExtendingFunctionalities.Utils;
using Shouldly;
using Xunit;

namespace CzuczenLand.Tests.ExtendingFunctionalities.Parser;

public class Parser_Factory_Tests : CzuczenLandTestBase
{
    [Fact]
    public void GetParser_DbStrategy_Test()
    {
        // Act
        var parser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Db);
        
        // Assert
        parser.ShouldBeOfType<CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Parser>();
    }

    [Fact]
    public void GetParser_DisplayStrategy_Test()
    {
        // Act
        var parser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Display);

        // Assert
        parser.ShouldBeOfType<CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Parser>();
    }

    [Fact]
    public void GetParser_EditStrategy_Test()
    {
        // Act
        var parser = ParserFactory.GetParser(EnumUtils.ParseStrategies.Edit);

        // Assert
        parser.ShouldBeOfType<CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.Parser.Parser>();
    }

    [Fact]
    public void GetParser_InvalidStrategy_Test()
    {
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => ParserFactory.GetParser((EnumUtils.ParseStrategies) 99));
    }
}
