using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IContactRepository : IDisposable
    {
        Task<List<Contact>> FindContactsByCustomerIDAsync(int customerID);
        Task<Contact> FindContactByIDAsync(int contactID);
        Task CreateAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(int contactID);
    }
}
