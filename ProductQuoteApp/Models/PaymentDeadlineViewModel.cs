using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Models
{
    public class PaymentDeadlineViewModel
    {
        public int PaymentDeadlineID { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }

        [Precision(25, 10)]
        public decimal Months { get; set; }

        public PaymentDeadlineViewModel(PaymentDeadline paymentDeadline)
        {
            this.PaymentDeadlineID = paymentDeadline.PaymentDeadlineID;
            this.Description = paymentDeadline.Description;
            this.Days = paymentDeadline.Days;
            this.Months = paymentDeadline.Months;
        }

        public static PaymentDeadlineViewModel FromPaymentDeadline(PaymentDeadline paymentDeadline)
        {
            return new PaymentDeadlineViewModel(paymentDeadline);
        }
    }
}
