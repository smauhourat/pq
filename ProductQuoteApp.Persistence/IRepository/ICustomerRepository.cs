using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ICustomerRepository : IDisposable
    {
        Task<IEnumerable<Customer>> FindCustomersAsync();
        IEnumerable<Customer> FindCustomers();
        Customer FindCustomersByID(int customerID);
        Task<Customer> FindCustomersByIDAsync(int customerID);
        Task CreateAsync(Customer customerToAdd);
        Task DeleteAsync(int customerID);
        Task DeleteCascadeAsync(int customerID);
        Task UpdateAsync(Customer customerToSave);

    }
}
