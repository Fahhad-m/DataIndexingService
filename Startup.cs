// Startup.cs
using DataIndexingService.Interfaces;
using DataIndexingService.Models;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using Microsoft.Data.SqlClient;

using static DataIndexingService.Interfaces.IDataIndexer;
using DataIndexingService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        var elasticsearchUri = new Uri(_configuration["Elasticsearch:Uri"]);
        var settings = new ConnectionSettings(elasticsearchUri).DefaultIndex("products");


        var client = new ElasticClient(
               new ConnectionSettings(new Uri(_configuration.GetValue<string>("Elasticsearch:Uri")))
                   .DefaultIndex("products")
                 //  .BasicAuthentication(_configuration.GetValue<string>("ElasticCloud:BasicAuthUser"),
                  //     _configuration.GetValue<string>("ElasticCloud:BasicAuthPassword"))
                       );


         client = new ElasticClient(settings);


        //var elasticsearchSettings = _configuration.GetSection("Elasticsearch");
        //var uri = new Uri(elasticsearchSettings["Uri"]);
        //var defaultIndex = elasticsearchSettings["Index"];

        //var settings = new ConnectionSettings(uri).DefaultIndex(defaultIndex);
        //var client = new ElasticClient(settings);

        services.AddSingleton(client);





        var sqlConnectionString = _configuration.GetConnectionString("DefaultConnection");
      //  var sqlConnectionString = _configuration.GetConnectionString("ConnectionStrings");
        services.AddSingleton<IDataRetriever<Products>>(provider => new SqlDataService(sqlConnectionString));
        services.AddSingleton<IDataIndexer<Products>, ElasticsearchService>();
        services.AddTransient<DataTransferService>();
    }
}
