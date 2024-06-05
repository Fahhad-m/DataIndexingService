using DataIndexingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIndexingService.Interfaces
{
    public interface IDataRetriever<T>
    {
        Task<List<T>> RetrieveDataAsync();
    }

}
