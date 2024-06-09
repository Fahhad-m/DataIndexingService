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
using Microsoft.Extensions.Options;
using System.Collections.Specialized;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    //public void ConnectToElasticsearch(_configuration)
    //{
    //    var url = _configuration.Url;
    //    var apiKey = _elasticsearchSettings.ApiKey;
    //    var username = _elasticsearchSettings.Username;
    //    var password = _elasticsearchSettings.Value;

    //    // Use these settings to connect to Elasticsearch
    //}
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
      
        var node = new Uri(_configuration["Elasticsearch:Uri"]);
        var settings = new ConnectionSettings(node)
          .DefaultIndex("products")
          .ThrowExceptions(alwaysThrow: true) // I like exceptions
          .PrettyJson() // Good for DEBUG
          .RequestTimeout(TimeSpan.FromSeconds(300))
          .ApiKeyAuthentication("Authorization", "ApiKey MFdmci1JOEJLUHRQSmRaYXNVVms6anh0MUw0MENUd2UtNW9zbFJYNFZPQQ==")
          .GlobalHeaders(new NameValueCollection 
    { 
        { "Authorization", "ApiKey MFdmci1JOEJLUHRQSmRaYXNVVms6anh0MUw0MENUd2UtNW9zbFJYNFZPQQ==" } 
    });;
          // .BasicAuthentication("enterprise_search", "Netsolpk111!");
          //.ApiKeyAuthentication("Authorization", "ApiKey MFdmci1JOEJLUHRQSmRaYXNVVms6anh0MUw0MENUd2UtNW9zbFJYNFZPQQ==");


        var client = new ElasticClient(settings);

       
        var existsResponse = client.Indices.Exists("products");
        
        if (existsResponse != null)
        {

            services.AddSingleton(existsResponse);

        }
        else
        {
            client.Indices.Create("products");

        }

        client = new ElasticClient(settings);



        services.AddSingleton(client);





        var sqlConnectionString = _configuration.GetConnectionString("DefaultConnection");
      //  var sqlConnectionString = _configuration.GetConnectionString("ConnectionStrings");
        services.AddSingleton<IDataRetriever<Products>>(provider => new SqlDataService(sqlConnectionString));
        services.AddSingleton<IDataIndexer<Products>, ElasticsearchService>();
        services.AddTransient<DataTransferService>();
    }
}
