using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class ProductFlatViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Brand { get; set; }
        public int PackagingID { get; set; }
        public string PackagingDescription { get; set; }
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
        public string FreightTypeDescription { get; set; }
        public string Observations { get; set; }
        public string ClientObservations { get; set; }
        public int? ProviderPaymentDeadline { get; set; }
        public int? LeadTime { get; set; }
        public Boolean BuyAndSellDirect { get; set; }
        public Boolean InOutStorage { get; set; }

        public Boolean Active { get; set; }

        public int SellerCompanyID { get; set; }
        public string SellerCompanyName { get; set; }

        public virtual ICollection<ProductDocumentViewModel> ProductDocuments { get; set; }
        public ProductFlatViewModel(Product product)
        {
            this.ProductID = product.ProductID;
            this.Name = product.Name;
            this.FullName = product.FullName;
            this.ProviderID = product.ProviderID;
            this.ProviderName = product.Provider.ProviderName;
            this.Brand = product.Brand;
            this.PackagingID = product.PackagingID;
            this.PackagingDescription = product.Packaging.Description;
            this.PositionKilogram = product.PositionKilogram;
            this.FCLKilogram = product.FCLKilogram;
            this.WarehouseCost = product.WarehouseCost;
            this.ValidityPeriodOfPrice = product.ValidityPeriodOfPrice;
            this.ValidityOfPrice = product.ValidityOfPrice;
            this.QuoteToRevision = product.QuoteToRevision;
            this.MinimumMarginPercentage = product.MinimumMarginPercentage;
            this.MinimumMarginUSD = product.MinimumMarginUSD;
            this.Waste = product.Waste;
            this.FreightTypeID = product.FreightTypeID;
            this.FreightTypeDescription = product.FreightType.Description;
            this.Observations = product.Observations;
            this.ClientObservations = product.ClientObservations;
            this.ProviderPaymentDeadline = product.ProviderPaymentDeadline;
            this.LeadTime = product.LeadTime;
            this.BuyAndSellDirect = product.BuyAndSellDirect;
            this.InOutStorage = product.InOutStorage;
            this.Active = product.Active;
            this.SellerCompanyID = product.SellerCompanyID;
            this.SellerCompanyName = product.SellerCompany.Name;

            this.ProductDocuments = new List<ProductDocumentViewModel>();
            
            foreach (ProductDocument pd in product.ProductDocuments)
            {
                //if (this.ProductDocuments == null)
                //    this.ProductDocuments = new List<ProductDocumentViewModel>();
                this.ProductDocuments.Add(new Models.ProductDocumentViewModel(pd));
            }
        }
    }
}