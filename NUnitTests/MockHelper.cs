using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using Searchfight.SearchAPI;

namespace Searchfight.Tests
{
    public static class MockHelper
    {
        public static List<ISearchAPI> ConstructEnginesByParam(string[] engineNames, string[] queries, ulong[][] searchResults)
        {
            var searchEngines = new List<ISearchAPI>();
            for (int e = 0; e < engineNames.Count(); e++)
            {
                var mockEngine = new Mock<ISearchAPI>();
                mockEngine.Setup(m => m.EngineName).Returns(engineNames[e]);
                for (int q = 0; q < queries.Count(); q++)
                {
                    mockEngine.Setup(m => m.Search(queries[q])).Returns(Task.FromResult(searchResults[e][q]));
                }
                searchEngines.Add(mockEngine.Object);
            }
            return searchEngines;
        }
    }
}