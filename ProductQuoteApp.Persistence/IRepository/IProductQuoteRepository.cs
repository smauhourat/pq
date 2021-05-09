using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IProductQuoteRepository : IDisposable
    {
        //Siempre las PQ van a estar filtradas por los Canales de Venta del Usuario
        //Task<List<ProductQuote>> FindProductQuotesAsync(string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);
        Task<List<ProductQuote>> FindProductQuotesByUserIDAsync(string ownerUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);
        Task<List<ProductQuote>> FindProductQuotesByCustomerAndUserIDAsync(int customerID, string userId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);

        ProductQuote FindProductQuotesByID(int productQuoteID);

        Task<ProductQuote> FindProductQuotesByIDAsync(int productQuoteID);
        Task<ProductQuote> FindProductQuotesByIDUserIDAsync(int productQuoteID, string userId);
        Task<ProductQuote> FindProductQuotesByIDCustomerAndUserIDAsync(int productQuoteID, int customerID, string userId);

        void Create(ProductQuote productQuoteToAdd);
        Task CreateAsync(ProductQuote productQuoteToAdd);

        Task DeleteAsync(int productQuoteID);

        Task UpdateAsync(ProductQuote productQuoteToSave);
        void Update(ProductQuote productQuoteToSave);

        void UpdatePdf(ProductQuote productQuoteToSave);
        Task UpdateDueDateReasonAsync(ProductQuote productQuoteToSave);
        Task UpdateReasonsForClosureAsync(ProductQuote productQuoteToSave);
        Task UpdateDateSentAsync(ProductQuote productQuoteToSave);
    }
}
