using DataIndexingService.Interfaces;
using DataIndexingService.Models;
using DataIndexingService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

class Program
{

    public static async Task Main(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var dataTransferService = serviceProvider.GetService<DataTransferService>();
        await dataTransferService.TransferDataAsync();
    }
    private static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Set up configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           
            .Build();
     
        serviceCollection.AddSingleton<IConfiguration>(configuration);
        // Set up logging
        serviceCollection.AddLogging(configure => configure.AddConsole());
        // Register services
        var startup = new Startup(configuration);
        startup.ConfigureServices(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }
}

  
