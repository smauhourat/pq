using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(TransportTypeMetaData))]
    public class TransportType
    {
        public int TransportTypeID { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int PositionQuantityFrom { get; set; }
        public int PositionQuantityTo { get; set; }

        public int FreightTypeID { get; set; }
        public virtual FreightType FreightType { get; set; }
        public virtual ICollection<GeographicAreaTransportType> GeographicAreaTransportTypes { get; set; }
    }
}
