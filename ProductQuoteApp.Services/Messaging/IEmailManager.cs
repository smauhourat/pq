using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ProductQuoteApp.Services
{
    public interface IEmailManager : IDisposable
    {
        //void SendEmail(string subject, string body, IMailMessage from, IMailMessage to, IEnumerable<string> bcc, IEnumerable<string> cc);

        //void SendSimpleMessage(string emailFrom, string emailTo, string recipientDisplayName, string subject, string body);
        Task SendEmail(EmailAccount emailAccount,
                                        string emailFrom,
                                        string nameFrom,
                                        string emailTo,
                                        string nameTo,
                                        string recipientDisplayName,
                                        string subject,
                                        string body,
                                        string attachmentFilePath = null,
                                        string attachmentFileName = null);

        Task ProcessMessagesAsync(CancellationToken token);
    }
}
