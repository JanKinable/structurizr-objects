using System;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Styles
{
    public class FutureStateLineStyle: RelationshipStyleBase
    {
        public FutureStateLineStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(styles, onCreatedFromExistingElement)
        {
            SetOpacity(30);
            SetDashed(true);
        }
    }
}