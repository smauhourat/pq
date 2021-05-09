using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityRepository : IDisposable
    {
        List<SaleModality> SaleModalitys();
        Task<List<SaleModality>> FindSaleModalitysAsync();
        Task<SaleModality> FindSaleModalityByIDAsync(int saleModalityID);
    }
}
