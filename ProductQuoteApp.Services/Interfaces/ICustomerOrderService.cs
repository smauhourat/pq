using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface ICustomerOrderService : IDisposable
    {
        Task CreateAsync(CustomerOrder customerOrderToAdd);
        Task DeleteAsync(int customerOrderID);

        void Approve(int customerOrderID);
        void Reject(int customerOrderID);
    }
}
