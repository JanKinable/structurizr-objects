using System;
using Structurizr;
using StructurizrObjects;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("Manages reference data for all counterparties the bank interacts with.", Location.Internal)]
    public class ReferenceDataSystem : SoftwareSystemBase
    {
        public ReferenceDataSystem(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }
    }
}