using System;
using Structurizr;
using StructurizrObjects;

namespace FinancialRiskSystem.Model
{
    [SoftwareSystem("The bank's authentication and authorization system.", Location.Internal)]
    public class ActiveDirectory : SoftwareSystemBase
    {
        public ActiveDirectory(Workspace workspace, Action<ElementType, string> onCreatedFromExistingElement) 
            : base(workspace, onCreatedFromExistingElement)
        {
        }
    }
}