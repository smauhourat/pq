using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using ProductQuoteApp.Persistence;
using System.Net;
using ProductQuoteApp.Services.Common;

namespace ProductQuoteApp.Services
{
    public class EmailManager : IEmailManager
    {

        public EmailManager()
        {
            //_mailMessage = mailMessage;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    // Free managed resources
            //    if (_mailMessage != null)
            //    {
            //        _mailMessage.Dispose();
            //        _mailMessage = null;
            //    }
            //}
        }

        public Task ProcessMessagesAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

#warning "Esta hardcodeado el tipo de encabezado del attach a PDF"
        public async Task SendEmail(EmailAccount emailAccount,
                                        string emailFrom,
                                        string nameFrom,
                                        string emailTo,
                                        string nameTo,
                                        string recipientDisplayName,
                                        string subject,
                                        string body,
                                        string attachmentFilePath = null,
                                        string attachmentFileName = null)
        {
            var message = new MailMessage();
            message.From = new MailAddress(emailFrom, nameFrom);
            message.To.Add(new MailAddress(emailTo, nameTo));
            message.CC.Add(new MailAddress(emailFrom, nameFrom));

            //content
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;


            if (attachmentFilePath != null && attachmentFileName != null)
            {
                //Get some binary data
                //byte[] data = GetData(attachmentFilePath + @"\" + attachmentFileName);
                byte[] data = System.IO.File.ReadAllBytes(attachmentFilePath + @"\" + attachmentFileName);

                //save the data to a memory stream
                MemoryStream ms = new MemoryStream(data);

                var contentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                message.Attachments.Add(new Attachment(ms, attachmentFileName, contentType.ToString()));
            }

            //send email
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password);
                //smtpClient.Send(message);
                await smtpClient.SendMailAsync(message);
            }
        }

    }
}
