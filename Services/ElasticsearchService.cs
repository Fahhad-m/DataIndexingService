using DataIndexingService.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataIndexingService.Models;
using Nest;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DataIndexingService.Interfaces;
using static DataIndexingService.Interfaces.IDataIndexer;

namespace DataIndexingService.Services
{
    public class ElasticsearchService : IDataIndexer<Products>
    {
        // private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticsearchService> _logger;
        private readonly ElasticClient _client;
        public ElasticsearchService(ElasticClient elasticClient, ILogger<ElasticsearchService> logger)
        {
           _client = elasticClient;
            _logger = logger;
        }

        //public Task IndexDataAsync(List<Products> data)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task IndexDataAsync(List<Products> products)
        {
            try
            {
                var pingResponse = await _client.PingAsync();
                if (pingResponse.IsValid)
                {
                    foreach (var item in products)
                    {

                        var response = await _client.IndexDocumentAsync(item);

                        if (!response.IsValid)
                        {
                            Console.WriteLine($"Failed to index document Id: {item.Id}");
                            // Handle the error (e.g., log it)
                        }

                    }
                }
                else
                {
                    Console.WriteLine($"Elasticsearch Engine is not working");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while indexing +" + ex.Message + "+");
                throw;
            }
        }
    }
}
