using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using Searchfight.ConsoleApp;

namespace Searchfight.Tests
{
    [TestFixture]
    public class TestsWithMockEngines
    {
        static object[] TestSourceData = new object[] {

            new object[] {
                new string[] { ".net", "java" },
                new string[] { "Google", "Own" },
                new ulong[][] {
                    new ulong[] { 10, 8 },                    
                    new ulong[] { 5, 4 } } } ,

            new object[] {
                new string[] { "Python", "Basic", "c++" },
                new string[] { "Google", "MSN", "Own", "Yahhoo" },
                new ulong[][] {
                    new ulong[] { 10, 80 , 55},
                    new ulong[] { 60, 70, 20 },
                    new ulong[] { 10, 40, 2000 },
                    new ulong[] { 50, 80, 800 } } }
        };
            
        [TestCaseSource("TestSourceData")]
        public async Task TestLogicOfWinnerCalculation(string[] queries, string[] engines, ulong[][] searchResults)
        {
            var searchEngines = MockHelper.ConstructEnginesByParam(engines, queries, searchResults);

            var rep = await EnginesManager.SearchAsync(queries, searchEngines);
            Assert.IsTrue(rep != null, "Search report should be filled");
            Assert.IsTrue(rep.FinishedWithSuccess, "With such search parameters flag FinishedWithSuccess should be set to true");

            var queryWinner = queries[searchResults.IndexOfMaxElement()];
            Assert.IsTrue(rep.TotalWinner.QueryStr == queryWinner, string.Format("Total winner should be: {0}", queryWinner));
            for (int e = 0; e < engines.Count(); e++)
            {
                var engWinner = rep.EnginesWinner.FirstOrDefault(v => v.EngineName == engines[e]);
                Assert.IsTrue(engWinner.EngineName == engines[e], string.Format("Engine Winners should contain {0}", engines[e]));
                var currWinnerQuery = queries[searchResults[e].ToList().IndexOf(searchResults[e].Max())];
                Assert.IsTrue(engWinner.CntLinks == searchResults[e].Max() && engWinner.QueryStr == currWinnerQuery,
                    string.Format("{0} winner should have {1} results for {2}", engines[e], searchResults[e].Max(), currWinnerQuery));
            }
        }
    }
}