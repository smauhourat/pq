using System;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class GlobalVariableMetaData
    {
        [Display(Name = "PtyCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCostoAlmacenamientoMensual_Required")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal CostoAlmacenamientoMensual { get; set; }

        [Display(Name = "PtyCostoInOut", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCostoInOut_Required")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal CostoInOut { get; set; }

        [Display(Name = "PtyCostoFinancieroMensual", ResourceType = typeof(Resources.Resources))]
        [Range(0.0,100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCostoFinancieroMensual_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostoFinancieroMensual { get; set; }

        [Display(Name = "PtyImpuestoDebitoCredito", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyImpuestoDebitoCredito_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal ImpuestoDebitoCredito { get; set; }

        [Display(Name = "PtyGastosFijos", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyGastosFijos_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal GastosFijos { get; set; }

        [Display(Name = "PtyIIBBAlicuota", ResourceType = typeof(Resources.Resources))]
        [Range(0.0, 100.0, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "GralPercentageValue")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyIIBBAlicuota_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal IIBBAlicuota { get; set; }

        [Display(Name = "PtyTipoCambio", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyTipoCambio_Required")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = true)]
        public decimal TipoCambio { get; set; }

        [Display(Name = "PtyFactorCostoAlmacenamientoMensual", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyFactorCostoAlmacenamientoMensual_Required")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal FactorCostoAlmacenamientoMensual { get; set; }

        [Display(Name = "PtyDiasStockPromedioDistLocal", ResourceType = typeof(Resources.Resources))]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int DiasStockPromedioDistLocal { get; set; }

        [Display(Name = "Enviar cotizaciones por email a Administradores")]
        public Boolean EnvioCotizacionPorMail { get; set; }

        [Display(Name = "Enviar cotizaciones por email a Usuario")]
        public Boolean EnvioCotizacionUsuarioCreadorPorMail { get; set; }
    }
}
