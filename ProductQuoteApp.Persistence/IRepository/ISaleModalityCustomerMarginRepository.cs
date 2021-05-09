using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityCustomerMarginRepository : IDisposable
    {
        void Create(SaleModalityCustomerMargin saleModalityCustomerMarginToAdd);
        Task CreateAsync(SaleModalityCustomerMargin saleModalityCustomerMarginToAdd);

        void DeleteByCustomer(int CustomerID);
    }
}
