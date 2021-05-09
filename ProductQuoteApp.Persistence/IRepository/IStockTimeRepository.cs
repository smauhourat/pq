using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IStockTimeRepository : IDisposable
    {
        List<StockTime> StockTimes();
        Task<List<StockTime>> FindStockTimesAsync();
        StockTime FindStockTimesByID(int stockTimeID);
        Task<StockTime> FindStockTimesByIDAsync(int stockTimeID);
        List<StockTime> FindStockTimesBySaleModalityID(int saleModalityID);

    }
}
