using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CreditRatingPaymentDeadline
    {
        public int CreditRatingPaymentDeadlineID { get; set; }
        public int CreditRatingID { get; set; }
        public int PaymentDeadlineID { get; set; }

        public virtual CreditRating CreditRating { get; set; }
        public virtual PaymentDeadline PaymentDeadline { get; set; }

    }
}
