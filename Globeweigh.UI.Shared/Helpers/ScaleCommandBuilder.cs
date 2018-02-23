using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globeweigh.Model;

namespace Globeweigh.UI.Shared.Helpers
{
    public static class ScaleCommandBuilder
    {
        public static string GetAllCommands(vwBatchView batchView)
        {
            string finalString = string.Empty;
            finalString += GetLowerLimitCommand(batchView.LowerLimit);
            finalString += GetLowerLimitCommand(batchView.UpperLimit);
            finalString += GetLowerLimitCommand(batchView.NominalWeight);
            finalString += GetLowerLimitCommand(batchView.Tare);
            finalString += "#7\n";

            return finalString;
        }

        public static string GetNominalCommand(int? nominalWeight)
        {
            string stringWeight = String.Format("{0:0,000}", nominalWeight);
            string finalString = "#4000" + stringWeight + "g \n";
            return finalString;
        }

        public static string GetLowerLimitCommand(int? lowerLimit)
        {
            string stringLowerLimit = String.Format("{0:0,000}", lowerLimit);
            string finalString = "#5000" + stringLowerLimit + "g \n";
            return finalString;
        }

        public static string GetUpperLimitCommand(int? upperLimit)
        {
            string stringWeight = String.Format("{0:0,000}", upperLimit);
            string finalString = "#6000" + stringWeight + "g \n";
            return finalString;
        }



        public static string GetTareCommand(int? tare)
        {
            string stringWeight = String.Format("{0:0,000}", tare);
            string finalString = "qS000" + stringWeight + "g \n";
            return finalString;
        }


        // qS0000,010g
    }
}
