using System.Threading.Tasks;

namespace Searchfight.SearchAPI
{
    /// <summary>
    /// Interface for the search engine
    /// </summary>
    public interface ISearchAPI
    {
        /// <summary>
        /// Search engine name
        /// </summary>
        string EngineName { get; }

        /// <summary>
        /// Executes search of the query
        /// </summary>
        /// <param name="query">Source query</param>
        /// <returns>Returns how many times search engine has found source query string</returns>
        Task<ulong> Search(string query);    
    }
}