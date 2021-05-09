using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(PaymentDeadlineMetaData))]
    public class PaymentDeadline
    {
        public int PaymentDeadlineID { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Precision(25, 10)]
        public decimal Months { get; set; }
        public virtual ICollection<CreditRatingPaymentDeadline> CreditRatingPaymentDeadlines { get; set; }
    }
}
