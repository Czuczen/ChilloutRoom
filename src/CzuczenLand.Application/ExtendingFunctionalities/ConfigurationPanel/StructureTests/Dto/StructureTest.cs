using System;
using System.Collections.Generic;
using CzuczenLand.ExtendingFunctionalities.Utils;

namespace CzuczenLand.ExtendingFunctionalities.ConfigurationPanel.StructureTests.Dto;

public class StructureTest
{
    private Guid _testId;
        

    public string TestName { get; set; }
        
    public string AdditionalInfos { get; set; }
        
    public Guid TestId
    {
        get
        {
            if (_testId == Guid.Empty)
                _testId = Guid.NewGuid();

            return _testId;
        }
    }

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

    public string StatusColor => StructureTestsHelper.GetStatusColor(Status);
        
    public string StatusText => StructureTestsHelper.GetStatusText(Status);
        
    public List<MinorTest> MinorTests { get; } = new();
}