using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIndexingService.Models
{
    public class ElasticsearchSettings
    {
        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string Username { get; set; }
        public string Value { get; set; }
    }
}
