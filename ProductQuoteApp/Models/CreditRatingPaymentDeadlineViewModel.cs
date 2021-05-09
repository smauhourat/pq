using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Models
{
    public class CreditRatingPaymentDeadlineViewModel
    {
        public int CreditRatingPaymentDeadlineID { get; set; }
        public int CreditRatingID { get; set; }
        public string CreditRatingDescription { get; set; }
        public int PaymentDeadlineID { get; set; }
        public string PaymentDeadlineDescription { get; set; }

        public CreditRatingPaymentDeadlineViewModel(CreditRatingPaymentDeadline creditRatingPaymentDeadline)
        {
            this.CreditRatingPaymentDeadlineID = creditRatingPaymentDeadline.CreditRatingPaymentDeadlineID;
            this.CreditRatingID = creditRatingPaymentDeadline.CreditRatingID;
            this.CreditRatingDescription = creditRatingPaymentDeadline.CreditRating.Description;
            this.PaymentDeadlineID = creditRatingPaymentDeadline.PaymentDeadlineID;
            this.PaymentDeadlineDescription = creditRatingPaymentDeadline.PaymentDeadline.Description;
        }

        public static CreditRatingPaymentDeadlineViewModel FromCreditRatingPaymentDeadline(CreditRatingPaymentDeadline creditRatingPaymentDeadline)
        {
            return new CreditRatingPaymentDeadlineViewModel(creditRatingPaymentDeadline);
        }
    }
}