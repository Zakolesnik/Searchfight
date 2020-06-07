using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using Searchfight.SearchAPI;
using Searchfight.ConsoleApp;

namespace Searchfight.Tests
{
    [TestFixture]
    public class TestsWithRealEngines
    {
        IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            serviceProvider = Program.InitServiceProvider();
        }

        static string[][] LanguagesVarants = new string[][] {
            new string[] {".Net", "Java" },
            new string[] { "Python", "Java Script", "Java"},};

        [TestCaseSource("LanguagesVarants")]
        public async Task TestQueriesStatistic(string[] queries)
        {
            var searchEngines = serviceProvider.GetServices<ISearchAPI>();
            var rep = await EnginesManager.SearchAsync(queries, searchEngines);
            Assert.IsTrue(rep != null, "Search report should be filled");
            Assert.IsTrue(rep.QueriesStatistic != null, "Queries statistic should be filled");
            Assert.IsTrue(rep.QueriesStatistic.Count() == queries.Count(), "Each query should has his own statistic");
            for (int i = 0; i < queries.Count(); i++)
                Assert.IsTrue(rep.QueriesStatistic.ElementAt(i).First().QueryStr == queries[i],
                    "Queries statistic should have the same order as in source queries");
        }

        static string[][] WrongQueries = new string[][] {
            new string[] {},
            new string[] {".Net"},
            new string[] {".Net", ".Net"},
            new string[] {string.Empty},
            new string[] {string.Empty, string.Empty}};

        [TestCaseSource("WrongQueries")]
        public async Task TestWrongQueries(string[] queries)
        {
            var searchEngines = serviceProvider.GetServices<ISearchAPI>();
            var rep = await EnginesManager.SearchAsync(queries, searchEngines);
            Assert.IsTrue(rep != null, "Search report should be filled");
            Assert.IsFalse(rep.FinishedWithSuccess, "With such parameters flag FinishedWithSuccess should be reseted");
            Assert.IsTrue(!string.IsNullOrWhiteSpace(rep.ErrorString), "With such parameters ErrorString should be filled");
        }
    }
}