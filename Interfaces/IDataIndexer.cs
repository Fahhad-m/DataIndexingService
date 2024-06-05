using DataIndexingService.Models;
using System.Threading.Tasks;

namespace DataIndexingService.Interfaces
{
    public interface IDataIndexer
    {
        public interface IDataIndexer<T>
        {
            Task IndexDataAsync(List<T> data);
        }
    }
}