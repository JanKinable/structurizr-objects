using System;
using Structurizr;
using StructurizrObjects;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("The system of record for trades of type X.", Location.Internal)]
    public class TradeDataSystem: SoftwareSystemBase
    {
        public TradeDataSystem(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }
    }
}