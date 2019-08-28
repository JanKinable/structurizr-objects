using System;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Model
{
    [Person("A regular business user who can also configure the parameters used in the risk calculations.", Location.Internal)]
    public class ConfigurationUser: PersonBase
    {
        public ConfigurationUser(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) : base(workspace, onCreatedFromExistingElement)
        {
            
        }
        public override Connector[] Connectors => new[]
        {
            ConnectTo<FinancialRiskSystem, SynchronousLineStyle>("Configures parameters using", "")
        };
        
    }
}