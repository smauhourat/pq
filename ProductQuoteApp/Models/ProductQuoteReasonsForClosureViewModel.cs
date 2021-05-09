using ProductQuoteApp.Library;
using ProductQuoteApp.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductQuoteApp.Models
{
    public class ProductQuoteReasonsForClosureViewModel
    {
        public int ProductQuoteID { get; set; }

        public string ProductQuoteCode { get; set; }

        public int? ReasonsForClosureID { get; set; }

        public virtual ReasonsForClosure ReasonsForClosure { get; set; }

        [Display(Name = "PtyClosureDate", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyClosureDate_Required")]
        public DateTime? ClosureDate { get; set; }

        [Display(Name = "PtyClosureObservations", ResourceType = typeof(Resources.Resources))]
        public string ClosureObservations { get; set; }

        public ProductQuoteReasonsForClosureViewModel()
        { }

        public ProductQuoteReasonsForClosureViewModel(ProductQuote pq)
        {
            this.ProductQuoteID = pq.ProductQuoteID;
            this.ProductQuoteCode = pq.ProductQuoteCode;
            this.ReasonsForClosureID = pq.ReasonsForClosureID;
            this.ReasonsForClosure = pq.ReasonsForClosure;
            this.ClosureDate = pq.ClosureDate;
            this.ClosureObservations = pq.ClosureObservations;
        }
    }
}