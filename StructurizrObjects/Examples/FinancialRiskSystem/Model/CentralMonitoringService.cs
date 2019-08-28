using System;
using Structurizr;
using StructurizrObjects;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("The bank's central monitoring and alerting dashboard.", Location.Internal)]
    public class CentralMonitoringService : SoftwareSystemBase
    {
        public CentralMonitoringService(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }
    }
}