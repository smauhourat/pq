using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IWorkflowMessageService: IDisposable
    {
        #region Customer workflow

        void SendCustomerProductQuote(string customerEmail, ProductQuote productQuote);
        void SendAdministratorProductQuote(ProductQuote productQuote);
        void SendUserCreatorProductQuote(ProductQuote productQuote);

        void SendConfirmEmail(string destinationEmail, string subject, string body);

        void SendRegisterInfo(string destinationEmail, string subject, string body);
        #endregion
    }
}
