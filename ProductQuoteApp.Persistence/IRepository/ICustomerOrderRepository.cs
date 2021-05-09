using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ICustomerOrderRepository : IDisposable
    {
        List<CustomerOrder> CustomerOrders();
        Task<List<CustomerOrder>> CustomerOrdersAsync();
        Task<CustomerOrder> FindCustomerOrdersByIDAsync(int customerOrderID);
        Task<List<CustomerOrder>> FindCustomerOrdersByCustomerIDAsync(int customerID);
        void Create(CustomerOrder customerOrderToAdd);
        Task CreateAsync(CustomerOrder customerOrderToAdd);
        Task DeleteAsync(int customerOrderID);
        Task UpdateAsync(CustomerOrder customerOrderToSave);

        void Approve(int customerOrderID);
        void Reject(int customerOrderID);
    }
}
