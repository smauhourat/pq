using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ICreditRatingPaymentDeadlineRepository : IDisposable
    {
        List<PaymentDeadline> FindPaymentDeadlineByCreditRating(int creditRatingID);
        Task<List<CreditRatingPaymentDeadline>> FindCreditRatingPaymentDeadlinesByCreditRatingAsync(int creditRatingID);
        List<PaymentDeadline> FindPaymentDeadlineAvailableByCreditRating(int creditRatingID);
        void Create(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToAdd);
        Task CreateAsync(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToAdd);
        void Delete(int creditRatingPaymentDeadlineID);
        Task DeleteAsync(int creditRatingPaymentDeadlineID);
        void Update(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToSave);
        Task UpdateAsync(CreditRatingPaymentDeadline creditRatingPaymentDeadlineToSave);

    }
}
