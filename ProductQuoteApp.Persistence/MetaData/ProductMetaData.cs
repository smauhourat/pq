using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class ProductMetaData
    {
        [Display(Name = "Name_Name", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name200_Long")]
        public string Name { get; set; }

        [Display(Name = "EtyProvider", ResourceType = typeof(Resources.Resources))]
        public int ProviderID { get; set; }
        [Display(Name = "EtyProvider", ResourceType = typeof(Resources.Resources))]
        public virtual Provider Provider { get; set; }

        [Display(Name = "EtyBrand", ResourceType = typeof(Resources.Resources))]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EtyBrand_Long")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "EtyBrand_Required")]
        public string Brand { get; set; }

        [Display(Name = "EtyPackaging", ResourceType = typeof(Resources.Resources))]
        public int PackagingID { get; set; }
        [Display(Name = "EtyPackaging", ResourceType = typeof(Resources.Resources))]
        public virtual Packaging Packaging { get; set; }


        [Display(Name = "PositionKilogram", ResourceType = typeof(Resources.Resources))]
        //[RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        //[Range(0.00, 9999999.00, ErrorMessage = "Ingrese un valor positivo, entre 0 y 9999999.")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PositionKilogram_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal PositionKilogram { get; set; }

        //ANDA LA EXPRESION REGULAR
        [Display(Name = "PtyFCLKilogram", ResourceType = typeof(Resources.Resources))]
        //[RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        //[Range(0, 9999999, ErrorMessage = "Ingrese un valor positivo, entre 0 y 9999999.")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyFCLKilogram_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal FCLKilogram { get; set; }

        [Display(Name = "WarehouseCost", ResourceType = typeof(Resources.Resources))]
        //[RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,4})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 4 lugares decimales.")]
        //[Range(0, 9999999, ErrorMessage = "Ingrese un valor positivo, entre 0 y 9999999.")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        public decimal? WarehouseCost { get; set; }

        [Display(Name = "ValidityPeriodOfPrice", ResourceType = typeof(Resources.Resources))]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidityPeriodOfPrice_Required")]
        public decimal ValidityPeriodOfPrice { get; set; }

        [Display(Name = "PtyValidityOfPrice", ResourceType = typeof(Resources.Resources))]
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyValidityOfPrice_Required")]
        public DateTime ValidityOfPrice { get; set; }

        [Display(Name = "PtyQuoteToRevision", ResourceType = typeof(Resources.Resources))]
        public Boolean QuoteToRevision { get; set; }

        [Display(Name = "PtyMinimumMarginPercentage", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        public decimal? MinimumMarginPercentage { get; set; }

        [Display(Name = "PtyMinimumMarginUSD", ResourceType = typeof(Resources.Resources))]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,4})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 4 lugares decimales.")]
        [Range(0, 9999999, ErrorMessage = "Ingrese un valor positivo, entre 0 y 9999999.")]
        public decimal? MinimumMarginUSD { get; set; }

        [Display(Name = "PtyWaste", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyWaste_Required")]
        public decimal? Waste { get; set; }

        [Display(Name = "EtyFreightType", ResourceType = typeof(Resources.Resources))]
        public int FreightTypeID { get; set; }
        [Display(Name = "EtyFreightType", ResourceType = typeof(Resources.Resources))]
        public virtual FreightType FreightType { get; set; }

        [Display(Name = "PtyObservations", ResourceType = typeof(Resources.Resources))]
        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Obs500_Long")]
        public string Observations { get; set; }

        [Display(Name = "PtyClientObservations", ResourceType = typeof(Resources.Resources))]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "Name200_Long")]
        public string ClientObservations { get; set; }

        [Display(Name = "PtyProviderPaymentDeadline", ResourceType = typeof(Resources.Resources))]
        //[Range(0, int.MaxValue, ErrorMessage = "Debe ingresar un numero entero positivo entre {1} y {2}")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyProviderPaymentDeadline_Required")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIntRange1_1000000")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int ProviderPaymentDeadline { get; set; }

        [Display(Name = "PtyLeadTime", ResourceType = typeof(Resources.Resources))]
        //[Range(0, int.MaxValue, ErrorMessage = "Debe ingresar un numero entero positivo entre {1} y {2}")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyProviderPaymentDeadline_Required")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIntRange1_1000000")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int LeadTime { get; set; }

        [Display(Name = "PtyBuyAndSellDirect", ResourceType = typeof(Resources.Resources))]
        public Boolean BuyAndSellDirect { get; set; }


        [Display(Name = "EtySellerCompany", ResourceType = typeof(Resources.Resources))]
        public int SellerCompanyID { get; set; }
        [Display(Name = "EtySellerCompany", ResourceType = typeof(Resources.Resources))]
        public virtual SellerCompany SellerCompany { get; set; }


        [Display(Name = "EtyIIBBTreatment", ResourceType = typeof(Resources.Resources))]
        public int IIBBTreatmentID { get; set; }
        [Display(Name = "EtyIIBBTreatment", ResourceType = typeof(Resources.Resources))]
        public virtual IIBBTreatment IIBBTreatment { get; set; }


        [Display(Name = "PtyInOutStorage", ResourceType = typeof(Resources.Resources))]
        public Boolean InOutStorage { get; set; }

        [Display(Name = "PtyActive", ResourceType = typeof(Resources.Resources))]
        public Boolean Active { get; set; }

    }
}
