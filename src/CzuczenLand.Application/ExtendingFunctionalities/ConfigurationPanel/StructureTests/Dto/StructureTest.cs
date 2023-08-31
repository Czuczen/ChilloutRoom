using System;
using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

/// <summary>
/// Klasa reprezentująca test struktury.
/// </summary>
public class StructureTest
{
    private Guid _testId;
        

    /// <summary>
    /// Nazwa testu.
    /// </summary>
    public string TestName { get; set; }
        
    /// <summary>
    /// Dodatkowe informacje dotyczące testu.
    /// </summary>
    public string AdditionalInfos { get; set; }
        
    /// <summary>
    /// Identyfikator testu.
    /// </summary>
    public Guid TestId
    {
        get
        {
            if (_testId == Guid.Empty)
                _testId = Guid.NewGuid();

            return _testId;
        }
    }

    /// <summary>
    /// Status testu struktury.
    /// </summary>
    private EnumUtils.StructureTestsStatuses Status
    {
        get
        {
            var ret = EnumUtils.StructureTestsStatuses.Ok;
            foreach (var minorTest in MinorTests)
            {
                if (minorTest.Status == EnumUtils.StructureTestsStatuses.Error)
                {
                    ret = EnumUtils.StructureTestsStatuses.Error;
                    break;
                }

                if (minorTest.Status == EnumUtils.StructureTestsStatuses.Warn)
                {
                    ret = EnumUtils.StructureTestsStatuses.Warn;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// Kolor statusu testu.
    /// </summary>
    public string StatusColor => StructureTestsHelper.GetStatusColor(Status);
        
    /// <summary>
    /// Tekst reprezentujący status testu.
    /// </summary>
    public string StatusText => StructureTestsHelper.GetStatusText(Status);
        
    /// <summary>
    /// Lista podtestów.
    /// </summary>
    public List<MinorTest> MinorTests { get; } = new();
}