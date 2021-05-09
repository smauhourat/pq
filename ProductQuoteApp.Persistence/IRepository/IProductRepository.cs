using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IProductRepository : IDisposable
    {
        List<Product> Products();
        List<Product> ProductsActive();
        List<Product> ProductsActive(int customerID);
        Task<IEnumerable<Product>> FindProductsAsync();
        IEnumerable<Product> FindProducts();
        Product FindProductsByID(int productID);
        Task<Product> FindProductsByIDAsync(int productID);
        Task CreateAsync(Product productToAdd);
        Task DeleteAsync(int productID);
        Task UpdateAsync(Product productToSave);
        Task<Product> CreateCopyAsync(int productID);
    }
}
