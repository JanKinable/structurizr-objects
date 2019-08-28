using System;
using Structurizr;
using StructurizrObjects;
using StructurizrObjects.Utils;
using Color = System.Drawing.Color;

namespace FinancialRiskSystem
{
    public sealed class FinancialRiskSystemWorkspace : WorkspaceBase
    {
        public FinancialRiskSystemWorkspace(string pathToJson) 
            : base(pathToJson, "Financial Risk System", 
                "\"This is a simple (incomplete) example C4 model based upon the financial risk system architecture kata, which can be found at http://bit.ly/sa4d-risksystem\"")
        {
        }

        public FinancialRiskSystemWorkspace(int workspaceId, string apikey, string apiSecret) 
            : base(workspaceId, apikey, apiSecret)
        {

        }

        public override string ContextBoundName => "Financial Risk System";

        protected override void AdaptDefaultStyles()
        {
            DefaultStyles.PrimaryColor = Color.White;

            var softwareSystem = DefaultStyles.SoftwareSystem;
            softwareSystem.Color = Color.White.ToHex();
            softwareSystem.BackgroundColor = Color.FromArgb(128, 21, 21).ToHex();
            softwareSystem.Width = 650;
            softwareSystem.Height = 400;
            softwareSystem.Shape = Shape.RoundedBox;

            var person = DefaultStyles.Person;
            person.BackgroundColor = Color.FromArgb(212, 106, 106).ToHex();
            person.Width = 550;
            person.Shape = Shape.Person;

            var syncLine = DefaultStyles.SyncLineStyle;
            var asyncLine = DefaultStyles.AsyncLineStyle;
            syncLine.Thickness = asyncLine.Thickness = 4;
            syncLine.FontSize = asyncLine.FontSize = 32;
            syncLine.Width = asyncLine.Width = 400;
        }
    }
}
