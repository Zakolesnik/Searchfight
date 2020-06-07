using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Searchfight.SearchAPI;

namespace Searchfight.ConsoleApp
{
    /// <summary>
    /// This program provide functionality to determine the popularity of programming languages on the internet,
    /// it queries search engines (Google, Bing) and compares how many results they return, calculate winner and 
    /// print all results to the console screen. Command line should contain at least two names of any programming languages    
    /// </summary>    
    public static class Program
    {
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Entry point to the program
        /// </summary>
        /// <param name="args">Command line arguments</param>        
        static async Task Main(string[] args)
        {           
            var serviceProvider = InitServiceProvider();
            var searchEngines = serviceProvider.GetServices<ISearchAPI>();

            var report = await EnginesManager.SearchAsync(args, searchEngines);
            var viewer = serviceProvider.GetService<IReportViewer>();

            viewer.Show(report);
        }

        /// <summary>
        /// Initialize program services
        /// </summary>        
        public static IServiceProvider InitServiceProvider() 
        {
            InitConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        private static void InitConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);
            services.AddTransient<ISearchAPI, GoogleEngine.GoogleEngine>();
            services.AddTransient<ISearchAPI, BingEngine.BingEngine>();
            services.AddTransient<IReportViewer, ConsoleViewer>();            
        }        
    }
}