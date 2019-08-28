using System;
using FinancialRiskSystem.Styles;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("Manages reference data for all counterparties the bank interacts with.", Location.Internal)]
    public class ReferenceDataSystemV2 : SoftwareSystemBase
    {
        public ReferenceDataSystemV2(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }

        public override ElementStyleBase[] Styles => new[]
        {
            AddStyle<FutureStateStyle>()
        };
    }
}