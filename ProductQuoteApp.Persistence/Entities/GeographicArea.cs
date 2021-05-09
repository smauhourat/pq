using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(GeographicAreaMetaData))]
    public class GeographicArea
    {
        public int GeographicAreaID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GeographicAreaTransportType> GeographicAreaTransportTypes { get; set; }
        public virtual ICollection<SaleModalityGeographicArea> SaleModalityGeographicAreas { get; set; }
    }
}
