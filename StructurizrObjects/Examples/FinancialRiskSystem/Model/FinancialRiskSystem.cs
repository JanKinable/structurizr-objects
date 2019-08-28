using System;
using FinancialRiskSystem.Styles;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("Calculates the bank's exposure to risk for product X.", Location.Internal)]
    public class FinancialRiskSystem : SoftwareSystemBase
    {
        public FinancialRiskSystem(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }

        public override Connector[] Connectors => new []
        {
            ConnectTo<TradeDataSystem, SynchronousLineStyle>("Gets trade data from", ""),
            ConnectTo<ReferenceDataSystem, SynchronousLineStyle>("Gets counter-party data from", ""),
            ConnectTo<ReferenceDataSystemV2, FutureStateLineStyle>("Gets counter-party data from", ""),
            ConnectTo<EmailSystem, SynchronousLineStyle>("Sends a notification that a report is ready to", ""),
            ConnectTo<CentralMonitoringService, AlertTagLineStyle>("Sends critical failure alerts to", "SNMP"),
            ConnectTo<ActiveDirectory, SynchronousLineStyle>("Uses for user authentication and authorization", "")
        };

        public override ElementStyleBase[] Styles => new[]
        {
            AddStyle<RiskSystemStyle>()
        };
    }
}