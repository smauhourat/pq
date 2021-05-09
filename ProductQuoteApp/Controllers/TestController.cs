using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace ProductQuoteApp.Controllers
{
    public class TestController : Controller
    {
        private IEmailManager emailManager;
        private IEmailAccountRepository emailAccountRepository = null;
        private IAdminUserRepository adminUserRepository = null;

        public TestController(IEmailManager emailMan, IEmailAccountRepository emailAccountRepo, IAdminUserRepository adminUserRepo)
        {
            emailManager = emailMan;
            emailAccountRepository = emailAccountRepo;
            adminUserRepository = adminUserRepo;
        }

        private string CreateBody()
        {
            string result = "";

            result = result + "Cliente: Alper Quimica<br>";
            result = result + "Fecha y Hora: 26-Sep-18 12:19:12 PM<br>";
            result = result + "Producto: Agua Amoniacal 28% - Inquimex - IBC 1000 L (Retornable)<br>";

            return result;
        }

        private void SendMail()
        {
            var message = new MailMessage();
            message.From = new MailAddress("cotizadoronline@laquim.com.ar", "cotizadoronline@laquim.com.ar");
            message.To.Add(new MailAddress("santiagomauhourat@hotmail.com", "santiagomauhourat@hotmail.com"));

            //content
            message.Subject = "subject1";
            message.Body = CreateBody();
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = false; // emailAccount.UseDefaultCredentials;
                smtpClient.Host = "mail.laquim.com.ar";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = false; // emailAccount.EnableSsl;
                smtpClient.Credentials = new NetworkCredential("cotizadoronline@laquim.com.ar", "Cotizador01");
                //smtpClient.Send(message);
                //await smtpClient.SendMailAsync(message);
                smtpClient.Send(message);
            }

        }

        private async Task SendMail2()
        {
            var message = new MailMessage();
            message.From = new MailAddress("santiago.mauhourat@gmail.com", "santiago.mauhourat@gmail.com");
            message.To.Add(new MailAddress("santiagomauhourat@hotmail.com", "santiagomauhourat@hotmail.com"));

            //content
            message.Subject = "subject22";
            message.Body = "body";
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = false; // emailAccount.UseDefaultCredentials;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true; // emailAccount.EnableSsl;
                smtpClient.Credentials = new NetworkCredential("santiago.mauhourat@gmail.com", "maria41129177");
                //smtpClient.Send(message);
                await smtpClient.SendMailAsync(message);
                //smtpClient.Send(message);
            }

        }

        public void Index()
        {

            SendMail();

            //EmailAccount emailAccount = emailAccountRepository.FindEmailAccountsDefaultAsync();

            //emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, "santiagomauhourat@hotmail.com", "santiagomauhourat@hotmail.com", "", "subject2", "body", null, null);

            ////var emails = adminUserRepository.GetAdminUsersEmails();
            ////foreach (var email in emails)
            ////{
            //Thread senderMail = new Thread(delegate ()
            //    {
            //        emailManager.SendEmail(emailAccount, emailAccount.Email, emailAccount.DisplayName, "santiagomauhourat@hotmail.com", "Santiago Mauhourat", "", "Cotizacion Laquim S.A. - Admin", "Body", null, null);
            //    });
            //    senderMail.IsBackground = true;
            //    senderMail.Start();
            ////}

        }

        //public void Index()
        //{
        //    ProductQuote pq = new ProductQuote();
        //    pq.ProductQuoteID = 999999;
        //    pq.CustomerID = 2;
        //    pq.CustomerName = "Álcalis de la Patagonia S.A.I.C.";
        //    pq.CustomerContactName = "Mariano Emanuelli";
        //    pq.CustomerContactMail = "memanuelli@grupoindalooo.com.ar";
        //    pq.ProductName = "Amoniaco Anhidro (NH3)";
        //    pq.ProductProviderName = "Profertil S.A. – Planta Bahía Blanca";
        //    pq.ProductPackagingName = "Bolsas";
        //    pq.MinimumQuantityDelivery = 30;
        //    pq.QuantityOpenPurchaseOrder = 190;
        //    pq.Price = (decimal)1300.8;
        //    pq.PaymentDeadlineName = "90 dias FF";
        //    pq.GeographicAreaName = "CABA";
        //    pq.DateQuote = Convert.ToDateTime("28/11/2017");
        //    pq.DateDelivery = Convert.ToDateTime("01/12/2017");


        //    var ret = pdfService.ProductQuoteToPdf(pq);

        //    pq = null;

        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + ret);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    byte[] bytes = System.IO.File.ReadAllBytes("C:\\Projects\\Inquimex\\Cotizador Online\\ProductQuoteAppProject\\ProductQuoteApp\\Documents\\Export\\" + ret);
        //    //byte[] bytes = System.IO.File.ReadAllBytes("C:\\Personales\\Inquimex\\ProductQuoteAppProject\\ProductQuoteApp\\Documents\\Export\\" + ret);
        //    Response.BinaryWrite(bytes);
        //    Response.End();

        //    //return View(ret);
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

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
            }
            base.Dispose(disposing);
        }
    }
}