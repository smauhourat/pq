using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class CustomerMetaData
    {

        [Display(Name = "PtyCompany", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCompany_Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCompany200_Long")]
        public string Company { get; set; }

        [Display(Name = "EtyCreditRating", ResourceType = typeof(Resources.Resources))]
        public int CreditRatingID { get; set; }
        [Display(Name = "EtyCreditRating", ResourceType = typeof(Resources.Resources))]
        public virtual CreditRating CreditRating { get; set; }

        [Display(Name = "PtyMinimumMarginPercentage", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? MinimumMarginPercentage { get; set; }

        [Display(Name = "PtyMinimumMarginUSD", ResourceType = typeof(Resources.Resources))]
        //[Range(0, 9999999, ErrorMessage = "Ingrese un valor positivo, entre 0 y 9999999.")]
        [Range(0, 1000000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIntRange1_1000000")]
        public decimal? MinimumMarginUSD { get; set; }

        [Display(Name = "SellerCommission", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "SellerCommission_Required")]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal SellerCommission { get; set; }

        [Display(Name = "PtyObservation", ResourceType = typeof(Resources.Resources))]
        public string Observation { get; set; }

        [Display(Name = "PtyIsSpot", ResourceType = typeof(Resources.Resources))]
        public Boolean IsSpot { get; set; }

        [Display(Name = "PtyIsProspect", ResourceType = typeof(Resources.Resources))]
        public Boolean IsProspect { get; set; }

        [Display(Name = "Enviar cotizaciones por mails")]
        public Boolean SendNotifications { get; set; }

        [Display(Name = "Dias Atraso Promedio")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int DelayAverageDays { get; set; }

        [Display(Name = "Nombre Contacto")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyGralLong_200")]
        public string ContactName { get; set; }

        [Display(Name = "Telefono Contacto")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyGralLong_50")]
        public string ContactTE { get; set; }

        [Display(Name = "Email Contacto")]
        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyGralLong_200")]
        public string ContactEmail { get; set; }

        [Display(Name = "EtyContactClass", ResourceType = typeof(Resources.Resources))]
        public int ContactClassID { get; set; }
        [Display(Name = "EtyContactClass", ResourceType = typeof(Resources.Resources))]
        public virtual ContactClass ContactClass { get; set; }

    }

}
