namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Search result from one search engine
    /// </summary>
    public struct SearchResult 
    {
        /// <summary>
        /// Search engine name
        /// </summary>
        public string EngineName { get; set; }

        /// <summary>
        /// Source query string
        /// </summary>
        public string QueryStr { get; set; }

        /// <summary>
        /// Returns how many times search engine has found source query string
        /// </summary>
        public ulong CntLinks { get; set; }
    }
}