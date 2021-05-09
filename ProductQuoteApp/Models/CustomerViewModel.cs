using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    [MetadataType(typeof(CustomerMetaData))]
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        public string Company { get; set; }
        public int CreditRatingID { get; set; }
        public virtual CreditRating CreditRating { get; set; }
        public decimal? MinimumMarginPercentage { get; set; }
        public decimal? MinimumMarginUSD { get; set; }
        public decimal SellerCommission { get; set; }
        public string Observation { get; set; }
        public Boolean IsSpot { get; set; }
        public Boolean IsProspect { get; set; }
        public Boolean SendNotifications { get; set; }

        public string ContactName { get; set; }
        public string ContactTE { get; set; }
        public string ContactEmail { get; set; }
        public int? ContactClassID { get; set; }
        public ContactClass ContactClass { get; set; }

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

        [Display(Name = "Enviar cotizaciones por mails")]
        public int DelayAverageDays { get; set; }

        public CustomerViewModel() { }

        public CustomerViewModel(Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            this.Company = customer.Company;
            this.CreditRatingID = customer.CreditRatingID;
            this.CreditRating = customer.CreditRating;
            this.MinimumMarginPercentage = customer.MinimumMarginPercentage;
            this.MinimumMarginUSD = customer.MinimumMarginUSD;
            this.SellerCommission = customer.SellerCommission;
            this.Observation = customer.Observation;
            this.IsSpot = customer.IsSpot;
            this.IsProspect = customer.IsProspect;
            this.SendNotifications = customer.SendNotifications;
            this.DelayAverageDays = customer.DelayAverageDays;
            this.ContactName = customer.ContactName;
            this.ContactTE = customer.ContactTE;
            this.ContactEmail = customer.ContactEmail;
            this.ContactClassID = customer.ContactClassID;

            if (customer.SaleModalityCustomerMargins != null)
            {
                if (customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Local).Any())
                {
                    MinimumMarginPercentage_DL = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Local).SingleOrDefault().MinimumMarginPercentage;
                    MinimumMarginUSD_DL = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Local).SingleOrDefault().MinimumMarginUSD;
                }

                if (customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).Any())
                {
                    MinimumMarginPercentage_DLP = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).SingleOrDefault().MinimumMarginPercentage;
                    MinimumMarginUSD_DLP = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.LocalProgramada).SingleOrDefault().MinimumMarginUSD;
                }

                if (customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).Any())
                {
                    MinimumMarginPercentage_ISL = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).SingleOrDefault().MinimumMarginPercentage;
                    MinimumMarginUSD_ISL = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.IndentSL).SingleOrDefault().MinimumMarginUSD;
                }

                if (customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Indent).Any())
                {
                    MinimumMarginPercentage_IND = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Indent).SingleOrDefault().MinimumMarginPercentage;
                    MinimumMarginUSD_IND = customer.SaleModalityCustomerMargins.Where(p => p.SaleModalityID == (int)EnumSaleModality.Indent).SingleOrDefault().MinimumMarginUSD;
                }

            }

        }

    }
}