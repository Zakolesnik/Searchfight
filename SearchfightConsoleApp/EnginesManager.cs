using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Searchfight.SearchAPI;

namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Search engines manager
    /// </summary>
    public static class EnginesManager
    {
        /// <summary>
        /// Executes asynchronous search for each element from args on each search engines, calculates winner and provides results
        /// </summary>
        /// <param name="args">Array of queries for search engines</param>
        /// <param name="searchEngines">List of search engine interface implementation</param>
        /// <returns>Results of winner calculation with sataistic for each query</returns>
        public static async Task<SearchReport> SearchAsync(string[] args, IEnumerable<ISearchAPI> searchEngines)
        {
            var report = new SearchReport();

            try 
            {
                var errStr = string.Empty;
                var argsIsCorrect = InputParamParser.Verify(args, ref errStr);

                if (argsIsCorrect)
                {
                    var results = await Search(args, searchEngines);

                    FillReportByResultsCalculation(report, results);

                    report.FinishedWithSuccess = true;
                }
                else
                    report.ErrorString = errStr;
            }
            catch (Exception ex) 
            {
                report.ErrorString = ex.Message;
            }

            return report;
        }

        private static async Task<IEnumerable<SearchResult>> Search(string[] args, IEnumerable<ISearchAPI> searchEngines)
        {
            var results = new List<SearchResult>();

            foreach (var arg in args)
            {
                foreach (var searchEngine in searchEngines)
                {
                    results.Add(new SearchResult() 
                    { 
                        CntLinks = await searchEngine.Search(arg), 
                        EngineName = searchEngine.EngineName, 
                        QueryStr = arg 
                    });
                }
            }

            return results;
        }

        private static void FillReportByResultsCalculation(SearchReport report, IEnumerable<SearchResult> results)
        {
            report.QueriesStatistic = results.GroupBy(v => v.QueryStr).Select(gr => gr).ToList();
            report.EnginesWinner = results.GroupBy(v => v.EngineName).SelectMany(gr => gr.Where(p => p.CntLinks == gr.Max(i => i.CntLinks))).ToList();
            report.TotalWinner = results.OrderByDescending(v => v.CntLinks).First();            
        }
    }
}