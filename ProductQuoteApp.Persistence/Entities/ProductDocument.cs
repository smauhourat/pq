using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ProductDocumentMetaData))]
    public class ProductDocument
    {
        public int ProductDocumentID { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public string Description { get; set; }
        public string ProductDocumentPDF { get; set; }
    }
}
