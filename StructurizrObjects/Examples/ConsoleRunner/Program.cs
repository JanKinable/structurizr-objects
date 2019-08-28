using System;
using System.Data;

namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running example Financial Risk System (Json)!");

            var outputFile = "D:\\Temp\\tests\\c4-financialrisksystem.json"; //fill in the output dir, upload via Web interface

            var workspace = new FinancialRiskSystem.FinancialRiskSystemWorkspace(outputFile);
            workspace.GenerateAndSaveWorkspace();

            //Alternativaly: apply direct upload via client
            //var workspace = new FinancialRiskSystem.FinancialRiskSystemWorkspace(9999, "api-key", "api-secret");
            //workspace.GenerateAndSaveWorkspace(false);

        }
    }
}
