using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IProductService : IDisposable 
    {
        List<Product> Products();
        List<Product> ProductsActive();
        Task<IEnumerable<Product>> FindProductsAsync();
        IEnumerable<Product> FindProducts();
        Product FindProductsByID(int productID);
        Task<Product> FindProductsByIDAsync(int productID);
        Task CreateAsync(Product productToAdd);
        Task CreateWithCostAsync(Product productToAdd, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, Boolean addToAllCustomer);
        Task CreateCompleteAsync(Product productToAdd, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, Boolean addToAllCustomer, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND);
        Task DeleteAsync(int productID);
        Task UpdateAsync(Product productToSave);
        Task UpdateWithCostAsync(Product productToSave, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND);
        Task UpdateCompleteAsync(Product productToSave, decimal? productCost_DL, decimal? productCost_DLP, decimal? productCost_ISL, decimal? productCost_IND, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND);
        Task CreateCopyAsync(int productID);
    }
}
