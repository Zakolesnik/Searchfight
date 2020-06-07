using System.Collections.Generic;

namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Common results of winner calculation
    /// </summary>
    public class SearchReport
    {
        private bool finishedWithSuccess;
        private string errorString;

        /// <summary>
        /// Return true if search has executed with success, otherwise - false
        /// </summary>
        public bool FinishedWithSuccess
        {
            get
            {
                return finishedWithSuccess;
            }
            set
            {
                finishedWithSuccess = value;
                if (value)
                    ErrorString = string.Empty;
            }
        }

        /// <summary>
        /// Return error description if search has failed
        /// </summary>
        public string ErrorString 
        {
            get 
            { 
                return errorString; 
            }
            set
            {
                errorString = value;
                if (!string.IsNullOrEmpty(value))
                    FinishedWithSuccess = false;
            }
        }

        /// <summary>
        /// Return results for each query from each search engines
        /// </summary>
        public IEnumerable<IEnumerable<SearchResult>> QueriesStatistic { get; set; }

        /// <summary>
        /// Return winner for each search engines
        /// </summary>
        public IEnumerable<SearchResult> EnginesWinner { get; set; }

        /// <summary>
        /// Total winner bitween all search engines
        /// </summary>
        public SearchResult TotalWinner { get; set; }
    }
}