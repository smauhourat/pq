using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services.Common;
using System.Threading;

namespace ProductQuoteApp.Services
{
    public class WorkflowMessageService : IWorkflowMessageService
    {
        private IEmailManager emailManager;
        private IEmailAccountRepository emailAccountRepository = null;
        private IAdminUserRepository adminUserRepository = null;
        private IApplicationUserRepository applicationUserRepository = null;

        public WorkflowMessageService(IEmailManager emailMan, 
            IEmailAccountRepository emailAccountRepo, 
            IAdminUserRepository adminUserRepo, 
            IApplicationUserRepository applicationUserRepo)
        {
            emailManager = emailMan;
            emailAccountRepository = emailAccountRepo;
            adminUserRepository = adminUserRepo;
            applicationUserRepository = applicationUserRepo;
        }

        public int SendCustomerProductOrder(CustomerOrder customerProductOrder)
        {

            throw new NotImplementedException();
        }

        private string CreateBody(ProductQuote productQuote)
        {
            string result = "";
            
            result = result + "<b>Usuario: </b>" + productQuote.UserFullName + "<br>";
            result = result + "<b>Cliente: </b>" + productQuote.CustomerCompany + "<br>";
            result = result + "<b>Fecha y Hora: </b>" + productQuote.DateQuote + "<br>";
            result = result + "<b>Producto: </b>" + productQuote.ProductName + "<br>";

            return result;
        }

        public void SendCustomerProductQuote(string customerEmail, ProductQuote productQuote)
        {
            if (productQuote == null)
                throw new ArgumentNullException("productQuote");

            string productQuotePDF = productQuote.ExpressCalc ? productQuote.ProductQuoteSmallPDF : productQuote.ProductQuotePDF;

            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();

            Thread senderMail = new Thread(delegate()
            {
                emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, customerEmail, customerEmail, "", "Cotización On-Line de Producto -" + productQuote.ProductQuoteCode, CreateBody(productQuote), CommonHelper.MapPath("~/Documents/Export"), productQuotePDF);
            });
            senderMail.IsBackground = true;
            senderMail.Start();
        }

        public void SendAdministratorProductQuote(ProductQuote productQuote)
        {
            if (productQuote == null)
                throw new ArgumentNullException("productQuote");

            string productQuotePDF = productQuote.ExpressCalc ? productQuote.ProductQuoteSmallPDF : productQuote.ProductQuotePDF;

            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();

            var emails = adminUserRepository.GetAdminUsersEmails();
            foreach (var email in emails)
            {
                Thread senderMail = new Thread(delegate ()
                {
                    emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, email, email, "", "Cotización On-Line de Producto - " + productQuote.ProductQuoteCode, CreateBody(productQuote), CommonHelper.MapPath("~/Documents/Export"), productQuotePDF);
                });
                senderMail.IsBackground = true;
                senderMail.Start();
            }
        }

        public void SendUserCreatorProductQuote(ProductQuote productQuote)
        {
            if (productQuote == null)
                throw new ArgumentNullException("productQuote");
            string productQuotePDF = productQuote.ExpressCalc ? productQuote.ProductQuoteSmallPDF : productQuote.ProductQuotePDF;

            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();

            var userEmail = applicationUserRepository.GetEmailUserById(productQuote.UserId);

            Thread senderMail = new Thread(delegate ()
            {
                emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, userEmail, userEmail, "", "Cotización On-Line de Producto - " + productQuote.ProductQuoteCode, CreateBody(productQuote), CommonHelper.MapPath("~/Documents/Export"), productQuotePDF);
            });
            senderMail.IsBackground = true;
            senderMail.Start();
        }

        public void SendConfirmEmail(string destinationEmail, string subject, string body)
        {
            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();
            emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, destinationEmail, destinationEmail, "", subject, body);
        }

        public void SendRegisterInfo(string destinationEmail, string subject, string body)
        {
            EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();
            var emails = adminUserRepository.GetAdminUsersEmails();
            foreach (var email in emails)
            {
                Thread senderMail = new Thread(delegate ()
                {
                    emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, email, email, "", subject, body, null);
                });
                senderMail.IsBackground = true;
                senderMail.Start();
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
                if (emailManager != null)
                {
                    emailManager.Dispose();
                    emailManager = null;
                }
                if (emailAccountRepository != null)
                {
                    emailAccountRepository.Dispose();
                    emailAccountRepository = null;
                }
                if (adminUserRepository != null)
                {
                    adminUserRepository.Dispose();
                    adminUserRepository = null;
                }
                if (applicationUserRepository != null)
                {
                    applicationUserRepository.Dispose();
                    applicationUserRepository = null;
                }
            }
        }
    }
}
