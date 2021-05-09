using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    [MetadataType(typeof(ProductMetaData))]
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        [DisplayName("Nombre Completo en VM")]
        public string FullName { get; set; }

        //public string FullName
        //{
        //    get { return Name + " - " + (Brand == null ? "" : Brand) + " - " + Packaging.Description; }
        //    //get { return Name + " - " + Provider.ProviderName + " - " + Packaging.Description;  }
        //}
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

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int ProviderPaymentDeadline { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int LeadTime { get; set; }

        public Boolean BuyAndSellDirect { get; set; }


        [Display(Name = "PtyProductCost_DL", ResourceType = typeof(Resources.Resources))]
        //[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        //[Range(0.000, 9999999.999, ErrorMessage = "El Valor debe ser entre {1} y {2}.")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal? ProductCost_DL { get; set; } //Distribución Local


        [Display(Name = "PtyProductCost_DLP", ResourceType = typeof(Resources.Resources))]
        //[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        //[Range(0, 9999999.999, ErrorMessage = "El Valor debe ser entre {1} y {2}.")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal? ProductCost_DLP { get; set; } //Distribución Local Programada


        [Display(Name = "PtyProductCost_ISL", ResourceType = typeof(Resources.Resources))]
        //[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        //[Range(0, 9999999.999, ErrorMessage = "El Valor debe ser entre {1} y {2}.")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal? ProductCost_ISL { get; set; } //Indent + Servicio logistico / financiero de guarda


        [Display(Name = "PtyProductCost_IND", ResourceType = typeof(Resources.Resources))]
        //[DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        //[Range(0, 9999999.999, ErrorMessage = "El Valor debe ser entre {1} y {2}.")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal? ProductCost_IND { get; set; } //Indent

        public int SellerCompanyID { get; set; }
        public virtual SellerCompany SellerCompany { get; set; }

        public int IIBBTreatmentID { get; set; }
        public virtual IIBBTreatment IIBBTreatment { get; set; }

        public Boolean InOutStorage { get; set; }

        public Boolean Active { get; set; }

        [Display(Name = "PtyAddToAllCustomer", ResourceType = typeof(Resources.Resources))]
        public Boolean AddToAllCustomer { get; set; }


        [Display(Name = "PtyMinimumMarginPercentage_DL", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        public decimal? MinimumMarginPercentage_DL { get; set; }

        [Display(Name = "PtyMinimumMarginPercentage_DLP", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        public decimal? MinimumMarginPercentage_DLP { get; set; }

        [Display(Name = "PtyMinimumMarginPercentage_ISL", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        public decimal? MinimumMarginPercentage_ISL { get; set; }

        [Display(Name = "PtyMinimumMarginPercentage_IND", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        public decimal? MinimumMarginPercentage_IND { get; set; }


        [Display(Name = "PtyMinimumMarginUSD_DL", ResourceType = typeof(Resources.Resources))]
        public decimal? MinimumMarginUSD_DL { get; set; }

        [Display(Name = "PtyMinimumMarginUSD_DLP", ResourceType = typeof(Resources.Resources))]
        public decimal? MinimumMarginUSD_DLP { get; set; }

        [Display(Name = "PtyMinimumMarginUSD_ISL", ResourceType = typeof(Resources.Resources))]
        public decimal? MinimumMarginUSD_ISL { get; set; }

        [Display(Name = "PtyMinimumMarginUSD_IND", ResourceType = typeof(Resources.Resources))]
        public decimal? MinimumMarginUSD_IND { get; set; }

        public ProductViewModel() { }

        public ProductViewModel(Product product)
        {
            this.ProductID = product.ProductID;
            this.Name = product.Name;
            this.FullName = product.FullName;
            this.ProviderID = product.ProviderID;
            this.Provider = product.Provider;
            this.Brand = product.Brand;
            this.PackagingID = product.PackagingID;
            this.Packaging = product.Packaging;
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
            this.FreightType = product.FreightType;
            this.Observations = product.Observations;
            this.ClientObservations = product.ClientObservations;
            this.ProviderPaymentDeadline = product.ProviderPaymentDeadline;
            this.LeadTime = product.LeadTime;
            this.BuyAndSellDirect = product.BuyAndSellDirect;
            this.SellerCompanyID = product.SellerCompanyID;
            this.SellerCompany = product.SellerCompany;
            this.IIBBTreatmentID = product.IIBBTreatmentID;
            this.IIBBTreatment = product.IIBBTreatment;
            this.InOutStorage = product.InOutStorage;
            this.Active = product.Active;

            if (product.SaleModalityProducts != null)
            { 
                if (product.SaleModalityProducts.Any(p => p.SaleModalityID == (int)EnumSaleModality.Local))
                    ProductCost_DL = product.SaleModalityProducts.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Local).ProductCost;

                if (product.SaleModalityProducts.Any(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada))
                    ProductCost_DLP = product.SaleModalityProducts.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).ProductCost;

                if (product.SaleModalityProducts.Any(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL))
                    ProductCost_ISL = product.SaleModalityProducts.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).ProductCost;

                if (product.SaleModalityProducts.Any(p => p.SaleModalityID == (int)EnumSaleModality.Indent))
                    ProductCost_IND = product.SaleModalityProducts.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Indent).ProductCost;
            }

            if (product.SaleModalityProductMargins != null)
            {
                if (product.SaleModalityProductMargins.Any(p => p.SaleModalityID == (int)EnumSaleModality.Local))
                { 
                    MinimumMarginPercentage_DL = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Local).MinimumMarginPercentage;
                    MinimumMarginUSD_DL = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Local).MinimumMarginUSD;
                }

                if (product.SaleModalityProductMargins.Any(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada))
                {
                    MinimumMarginPercentage_DLP = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).MinimumMarginPercentage;
                    MinimumMarginUSD_DLP = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).MinimumMarginUSD;
                }

                if (product.SaleModalityProductMargins.Any(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL))
                {
                    MinimumMarginPercentage_ISL = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).MinimumMarginPercentage;
                    MinimumMarginUSD_ISL = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).MinimumMarginUSD;
                }

                if (product.SaleModalityProductMargins.Any(p => p.SaleModalityID == (int)EnumSaleModality.Indent))
                {
                    MinimumMarginPercentage_IND = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Indent).MinimumMarginPercentage;
                    MinimumMarginUSD_IND = product.SaleModalityProductMargins.SingleOrDefault(p => p.SaleModalityID == (int)EnumSaleModality.Indent).MinimumMarginUSD;
                }
                    
            }
        }
    }
}