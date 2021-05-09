using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISaleModalityProductMarginRepository : IDisposable
    {
        void Create(SaleModalityProductMargin saleModalityProductMarginToAdd);
        Task CreateAsync(SaleModalityProductMargin saleModalityProductMarginToAdd);

        void DeleteByProduct(int productID);
    }
}
