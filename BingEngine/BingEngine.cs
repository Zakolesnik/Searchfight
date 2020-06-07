using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Searchfight.Converters;
using Searchfight.SearchAPI;

namespace Searchfight.BingEngine
{
    /// <summary>
    /// Bing search engine implementation
    /// </summary>
    public class BingEngine : ISearchAPI
    {
        private const string BING_ENGINE_NAME = "MSN Search";
        private const string BING_API_KEY_PARAM_NAME = "BingEngine:APIKey";
        private const string BING_API_URI_BASE = "BingEngine:APIUriBase";

        private string accessKey;
        private string uriBase;

        /// <summary>
        /// Search engine name
        /// </summary>
        public string EngineName
        {
            get
            {
                return BING_ENGINE_NAME;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">Program parameters</param>
        public BingEngine(IConfiguration config)
        {
            ReadConfiguration(config);
        }

        private void ReadConfiguration(IConfiguration config) 
        {
            this.accessKey = config.GetValue<string>(BING_API_KEY_PARAM_NAME);
            this.uriBase = config.GetValue<string>(BING_API_URI_BASE);
        }

        /// <summary>
        /// Executes search of the query
        /// </summary>
        /// <param name="query">Source query</param>
        /// <returns>Returns how many times search engine has found source query string</returns>
        public async Task<ulong> Search(string query)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", accessKey);

            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(query) + "&responseFilter=Webpages&count=1";
            var httpResponse = await httpClient.GetAsync(new Uri(uriQuery));
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.StatusCode.Equals(HttpStatusCode.OK) || httpResponse.Content == null)
            {
                throw new Exception(responseContent);
            }

            var queryResults = JsonSerializer.Deserialize<BingQueryResult>(responseContent);

            return queryResults.WebPages.TotalEstimatedMatches;
        }

        private struct BingQueryResult
        {
            [JsonPropertyName("webPages")]
            public WebPages WebPages { get; set; }
        }

        private struct WebPages
        {
            [JsonPropertyName("totalEstimatedMatches")]
            [JsonConverter(typeof(ULongToStringConverter))]
            public ulong TotalEstimatedMatches { get; set; }
        }
    }
}