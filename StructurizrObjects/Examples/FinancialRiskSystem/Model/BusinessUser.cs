using System;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Model
{
    [Person("A regular business user.", Location.Internal)]
    public class BusinessUser : PersonBase
    {
        public BusinessUser(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }

        public override Connector[] Connectors => new[]
        {
            ConnectTo<FinancialRiskSystem, SynchronousLineStyle>("Views reports using", "")
        };
    }
}