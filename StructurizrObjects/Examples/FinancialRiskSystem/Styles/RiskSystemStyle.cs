using System;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Styles
{
    public class RiskSystemStyle: ElementStyleBase
    {
        public RiskSystemStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(styles, onCreatedFromExistingElement)
        {
            SetBackgroundColor(System.Drawing.Color.Red);
            SetColor(System.Drawing.Color.GhostWhite);
        }

        
    }
}