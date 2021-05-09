using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    public class GeographicAreaTransportType
    {
        public int GeographicAreaTransportTypeID { get; set; }
        public int GeographicAreaID { get; set; }
        public int TransportTypeID { get; set; }

        [RegularExpression(@"^(?=,*[0-9])\d*(?:\,\d{1,2})?$", ErrorMessage = "Ingrese un valor decimal positivo, con no mas de 2 lugares decimales.")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal FreightCost { get; set; }

        public virtual GeographicArea GeographicArea { get; set; }
        public virtual TransportType TransportType { get; set; }
    }
}
