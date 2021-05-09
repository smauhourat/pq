using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProductQuoteApp.Services
{
    public interface ICustomerService : IDisposable
    {
        Task<IEnumerable<Customer>> FindCustomersAsync();
        IEnumerable<Customer> FindCustomers();
        Customer FindCustomersByID(int customerID);
        Task<Customer> FindCustomersByIDAsync(int customerID);
        Task CreateAsync(Customer customerToAdd);
        Task CreateCompleteAsync(Customer customerToAdd, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND);
        Task DeleteAsync(int customerID);
        Task DeleteCascadeAsync(int customerID);
        Task UpdateAsync(Customer customerToSave);
        Task UpdateCompleteAsync(Customer customerToSave, decimal? minimumMarginPercentage_DL, decimal? minimumMarginPercentage_DLP, decimal? minimumMarginPercentage_ISL, decimal? minimumMarginPercentage_IND, decimal? minimumMarginUSD_DL, decimal? minimumMarginUSD_DLP, decimal? minimumMarginUSD_ISL, decimal? minimumMarginUSD_IND);
    }
}
