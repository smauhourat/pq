using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IProductQuoteService : IDisposable
    {
        void CalculateQuote(ProductQuote productQuote);
        void CreateQuote(string customerUserEmail, ProductQuote productQuote, Boolean sendNotifications, Boolean sendClientNotifications, Boolean sendUserCreatorNotifications);
        void CreateCustomerOrder(string customerUserEmail, ProductQuote productQuote, Boolean createOCAsApproved);
        Task DeleteAsync(int productQuoteID);
        bool isValidProductQuoteInput(ProductQuote productQuote);
        Rofex GetRofex(int paymentDeadlineID);
        Rofex GetRofexNuevo(int paymentDeadlineID, int delayAverageDays);

        Task<List<ProductQuote>> FindProductQuotesByAdminUserAsync(string currentUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);
        Task<List<ProductQuote>> FindProductQuotesBySellerUserAsync(string ownerUserId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);
        Task<List<ProductQuote>> FindProductQuotesByCustomerAndUserIDAsync(int customerID, string userId, string search, bool isQuote, DateTime? dateFrom, DateTime? dateTo);
    }
}
