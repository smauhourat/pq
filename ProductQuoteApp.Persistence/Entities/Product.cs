using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(ProductMetaData))]
    public class Product : ICloneable
    {
        public int ProductID { get; set; }
        public string Name { get; set; }

        [NotMapped()]
        [DisplayName("Nombre Completo en P")]
        public string FullName
        {
            get { return Name + " - " + (Brand == null ? "" : Brand) + " - " + Packaging.Description; }
        }
        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        public string Brand { get; set; }
        public int PackagingID { get; set; }
        public virtual Packaging Packaging { get; set; }
        public decimal PositionKilogram { get; set; }
        public decimal FCLKilogram { get; set; }
        public decimal? WarehouseCost { get; set; }
        public int? ValidityPeriodOfPrice { get; set; }
        public DateTime ValidityOfPrice { get; set; }
        public Boolean QuoteToRevision { get; set; }
        public decimal? MinimumMarginPercentage { get; set; }
        public decimal? MinimumMarginUSD { get; set; }
        public decimal? Waste { get; set; }

        public int FreightTypeID { get; set; }
        public virtual FreightType FreightType { get; set; }

        public string Observations { get; set; }

        public string ClientObservations { get; set; }
        public int ProviderPaymentDeadline { get; set; }
        public int LeadTime { get; set; }

        public Boolean BuyAndSellDirect { get; set; }

        public virtual ICollection<SaleModalityProduct> SaleModalityProducts { get; set; }
        public virtual ICollection<ProductDocument> ProductDocuments { get; set; }

        public virtual ICollection<CustomerProduct> CustomerProducts { get; set; }

        public int SellerCompanyID { get; set; }
        public virtual SellerCompany SellerCompany { get; set; }

        public int IIBBTreatmentID { get; set; }
        public virtual IIBBTreatment IIBBTreatment { get; set; }

        public Boolean InOutStorage { get; set; }

        public Boolean Active { get; set; }

        public virtual ICollection<SaleModalityProductMargin> SaleModalityProductMargins { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
