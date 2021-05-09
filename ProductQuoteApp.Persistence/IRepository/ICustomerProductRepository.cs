using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ICustomerProductRepository : IDisposable
    {
        Task<List<CustomerProduct>> FindCustomerProductsByCustomerIDAsync(int customerID);
        List<CustomerProduct> FindCustomerProductsByCustomerID(int customerID);
        List<Product> FindProductsAvailableByCustomer(int customerID);
        Task<List<Product>> FindProductsAvailableByCustomerAsync(int customerID);
        List<Customer> FindCustomerNoAssignedToProduct(int productID);

        void Create(CustomerProduct customerProductToAdd);
        Task CreateAsync(CustomerProduct customerProductToAdd);
        void Delete(int customerProductID);
        Task DeleteAsync(int customerProductID);
        void Update(CustomerProduct customerProductToSave);
        Task UpdateAsync(CustomerProduct customerProductToSave);

        Boolean CanShowCosteo(int customerID, int productID);
        Task<Boolean> CanShowCosteoAsync(int customerID, int productID);

        void DeleteByCustomer(int customerID);

        void AddAllProductsToCustomer(int customerID);
        void AddAllProductsToCustomer(int customerID, bool calculationDetails);
        void AddAllCustomersToProduct(int productID);
        void AddAllSpotCustomerToProduct(int productID);
    }
}
