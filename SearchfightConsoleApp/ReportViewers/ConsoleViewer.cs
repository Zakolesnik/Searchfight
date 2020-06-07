using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Shows search report on the console screen
    /// </summary>
    public class ConsoleViewer : IReportViewer
    {
        public void Show(SearchReport report)
        {
            if (report.FinishedWithSuccess)
            {
                foreach (var gr in report.QueriesStatistic)
                    Console.WriteLine(string.Format("{0}: {1}", gr.First().QueryStr, GetDescForGroup(gr)));

                foreach (var winner in report.EnginesWinner)
                    Console.WriteLine(string.Format("{0} winner: {1}", winner.EngineName, winner.QueryStr));

                Console.WriteLine(string.Format("Total winner: {0}", report.TotalWinner.QueryStr));
            }
            else
                Console.WriteLine(report.ErrorString);
        }

        private string GetDescForGroup(IEnumerable<SearchResult> group)
        {
            StringBuilder str = new StringBuilder();
            foreach (var result in group)
            {
                str.Append(string.Format("{0}: {1} ", result.EngineName, result.CntLinks.ToString("N0", CultureInfo.InvariantCulture)));
            }
            return str.ToString();
        }
    }
}