using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityProductRepository : IDisposable
    {
        Task<List<SaleModalityProduct>> FindProductsBySaleModalityAsync(int saleModalityID);
        List<SaleModalityProduct> FindProductsBySaleModality(int saleModalityID);
        List<SaleModality> FindSaleModalityByProduct(int productID);
        SaleModalityProduct FindBySaleModalityAndProduct(int saleModalityID, int productID);
        Task<List<SaleModalityProduct>> FindSaleModalityProductsBySaleModalityAsync(int saleModalityID);
        List<Product> FindProductAvailableBySaleModality(int saleModalityID);
        void Create(SaleModalityProduct saleModalityProductToAdd);
        Task CreateAsync(SaleModalityProduct saleModalityProductToAdd);
        void Update(SaleModalityProduct saleModalityProductToSave);
        void Delete(int saleModalityProductID);
        void DeleteByProduct(int productID);

    }
}
