namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// Interface for report viewer
    /// </summary>
    public interface IReportViewer
    {
        /// <summary>
        /// Show search report
        /// </summary>
        /// <param name="report">Search report</param>
        void Show(SearchReport report);
    }
}