using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class ProductDocumentViewModel
    {
        public int ProductDocumentID { get; set; }
        public string Description { get; set; }
        public string ProductDocumentPDF { get; set; }


        public ProductDocumentViewModel(ProductDocument productDocument)
        {
            this.ProductDocumentID = productDocument.ProductDocumentID;
            this.Description = productDocument.Description;
            this.ProductDocumentPDF = productDocument.ProductDocumentPDF;
        }
    }
}