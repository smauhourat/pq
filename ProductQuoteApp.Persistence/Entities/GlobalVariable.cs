using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(GlobalVariableMetaData))]
    public class GlobalVariable
    {
        public int GlobalVariableID { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal CostoAlmacenamientoMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal CostoInOut { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostoFinancieroMensual { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal ImpuestoDebitoCredito { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal GastosFijos { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Obsolete("No usar, utilizar el porcentaje de IIBB de cada producto, que se aplica sobre el precio de venta para los casos Fabricación y Reventa y sobre (precio de venta - precio de compra) para el caso de cuenta y orden")]
        public decimal IIBBAlicuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        public decimal TipoCambio { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Precision(25, 10)]
        public decimal FactorCostoAlmacenamientoMensual { get; set; }

        public int DiasStockPromedioDistLocal { get; set; }

        public Boolean EnvioCotizacionPorMail { get; set; }
        public Boolean EnvioCotizacionUsuarioCreadorPorMail { get; set; }
        
    }
}
