// DataTransferService.cs
using DataIndexingService.Interfaces;
using DataIndexingService.Models;
using System.Threading.Tasks;
using static DataIndexingService.Interfaces.IDataIndexer;

public class DataTransferService
{
    private readonly IDataRetriever<Products> _dataRetriever;
    private readonly IDataIndexer<Products> _dataIndexer;

    public DataTransferService(IDataRetriever<Products> dataRetriever, IDataIndexer<Products> dataIndexer)
    {
        _dataRetriever = dataRetriever;
        _dataIndexer = dataIndexer;
    }

    public async Task TransferDataAsync()
    {
        var data = await _dataRetriever.RetrieveDataAsync();
        await _dataIndexer.IndexDataAsync(data);
    }
}
