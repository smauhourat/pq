using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Models
{
    public class GeographicAreaTransportTypeViewModel
    {
        public int GeographicAreaTransportTypeID { get; set; }
        public int GeographicAreaID { get; set; }
        public string GeographicAreaName { get; set; }
        public int TransportTypeID { get; set; }
        public decimal FreightCost { get; set; }
        public string TransportTypeDescription { get; set; }

        public GeographicAreaTransportTypeViewModel(GeographicAreaTransportType geographicAreaTransportType)
        {
            this.GeographicAreaTransportTypeID = geographicAreaTransportType.GeographicAreaTransportTypeID;
            this.GeographicAreaID = geographicAreaTransportType.GeographicAreaID;
            this.GeographicAreaName = geographicAreaTransportType.GeographicArea.Name;
            this.TransportTypeID = geographicAreaTransportType.TransportTypeID;
            this.TransportTypeDescription = geographicAreaTransportType.TransportType.Description;
            this.FreightCost = geographicAreaTransportType.FreightCost;
        }
    }
}