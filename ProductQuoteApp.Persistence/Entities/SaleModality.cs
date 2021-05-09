using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    public enum EnumSaleModality
    {
        Local = 1,
        LocalProgramada = 2,
        IndentSL = 3,
        Indent = 4
    };

    public class SaleModality
    {
        public int SaleModalityID { get; set; }
        public string DescriptionShort { get; set; }
        public string Description { get; set; }
        public string Resume { get; set; }
        public int OrderView { get; set; }

        public virtual ICollection<SaleModalityProduct> SaleModalityProducts { get; set; }
        public virtual ICollection<SaleModalityCreditRating> SaleModalityCreditRatings { get; set; }
        public virtual ICollection<SaleModalityGeographicArea> SaleModalityGeographicAreas { get; set; }
        public virtual ICollection<SaleModalityDeliveryAmount> SaleModalityDeliveryAmounts { get; set; }
    }
}
