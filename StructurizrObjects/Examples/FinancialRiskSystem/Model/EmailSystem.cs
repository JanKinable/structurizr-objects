using System;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("The bank's Microsoft Exchange system.", Location.Internal)]
    public class EmailSystem : SoftwareSystemBase
    {
        public EmailSystem(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement)
            : base(workspace, onCreatedFromExistingElement)
        {
        }

        public override Connector[] Connectors => new []
        {
            ConnectTo<BusinessUser, AsynchronousLineStyle>("Sends a notification that a report is ready to", "E-mail message")
        };
}
}