using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IPaymentDeadlineRepository : IDisposable
    {
        List<PaymentDeadline> PaymentDeadlines();
        Task<List<PaymentDeadline>> PaymentDeadlinesAsync();

        Task<List<PaymentDeadline>> FindPaymentDeadlinesAsync();
        PaymentDeadline FindPaymentDeadlineByID(int paymentDeadlineID);
        Task<PaymentDeadline> FindPaymentDeadlineByIDAsync(int paymentDeadlineID);
        Task CreateAsync(PaymentDeadline paymentDeadlineToAdd);
        Task DeleteAsync(int paymentDeadlineID);
        Task UpdateAsync(PaymentDeadline paymentDeadlineToSave);
    }
}
