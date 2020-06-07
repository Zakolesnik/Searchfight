using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Searchfight.Converters;
using Searchfight.SearchAPI;

namespace Searchfight.GoogleEngine
{
    /// <summary>
    /// Google search engine implemenation
    /// </summary>
    public class GoogleEngine : ISearchAPI
    {
        private const string GOOGLE_ENGINE_NAME = "Google";
        private const string GOOGLE_API_KEY_PARAM_NAME = "GoogleEngine:APIKey";
        private const string GOOGLE_SEARCH_ENGINE_PARAM_NAME = "GoogleEngine:SearchEngineKey";
        private const string GOOGLE_API_URI_BASE_PARAM_NAME = "GoogleEngine:APIUriBase";

        private string apiKey;
        private string searchEngineKey;
        private string uriBase;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">Program configuration</param>
        public GoogleEngine(IConfiguration config) 
        {
            ReadConfiguration(config);
        }

        /// <summary>
        /// Search engine name
        /// </summary>
        public string EngineName 
        {
            get 
            {
                return GOOGLE_ENGINE_NAME;
            }
        }

        /// <summary>
        /// Executes search of the query
        /// </summary>
        /// <param name="query">Source query</param>
        /// <returns>Returns how many times search engine has found source query string</returns>
        public async Task<ulong> Search(string query)
        {
            using var httpClient = new HttpClient();
            var requestURI = string.Format("{0}?key={1}&cx={2}&q={3}&num=1&fields=searchInformation(totalResults)",
                uriBase, apiKey, searchEngineKey, Uri.EscapeDataString(query));

            var httpResponse = await httpClient.GetAsync(new Uri(requestURI));
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.StatusCode.Equals(HttpStatusCode.OK) || httpResponse.Content == null)
            {
                throw new Exception(responseContent);
            }
            var queryResults = JsonSerializer.Deserialize<GoogleQueryResult>(responseContent);

            return queryResults.SearchInformation.TotalResults;
        }

        private void ReadConfiguration(IConfiguration config)
        {
            this.apiKey = config.GetValue<string>(GOOGLE_API_KEY_PARAM_NAME);
            this.searchEngineKey = config.GetValue<string>(GOOGLE_SEARCH_ENGINE_PARAM_NAME);
            this.uriBase = config.GetValue<string>(GOOGLE_API_URI_BASE_PARAM_NAME);
        }

        private struct GoogleQueryResult
        {
            [JsonPropertyName("searchInformation")]
            public SearchInformation SearchInformation { get; set; }

        }

        private struct SearchInformation
        {
            [JsonPropertyName("totalResults")]
            [JsonConverter(typeof(ULongToStringConverter))]
            public ulong TotalResults { get; set; }
        }
    }
}