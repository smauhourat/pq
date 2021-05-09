using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(SellerCompanyMetaData))]
    public class SellerCompany
    {
        public int SellerCompanyID { get; set; }
        public string Name { get; set; }
        public string ProductQuotePdfTemplate { get; set; }
        public string ProductQuoterSmallPdfTemplate { get; set; }
    }
}
