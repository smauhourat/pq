using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IMailMessage
    {
        string From { get; set; }
        string To { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
    }
}
