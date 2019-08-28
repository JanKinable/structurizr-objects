using System;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Styles;

namespace FinancialRiskSystem.Styles
{
    public class FutureStateStyle : ElementStyleBase
    {
        public FutureStateStyle(Structurizr.Styles styles, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(styles, onCreatedFromExistingElement)
        {
            SetOpacity(30);
            SetBorder(Border.Dashed);
        }
    }
}