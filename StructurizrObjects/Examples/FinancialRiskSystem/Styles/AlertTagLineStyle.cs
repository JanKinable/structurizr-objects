using System;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Styles
{
    public class AlertTagLineStyle : RelationshipStyleBase
    {
        public AlertTagLineStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(styles, onCreatedFromExistingElement)
        {
            SetColor(System.Drawing.Color.Red);
        }
    }
}