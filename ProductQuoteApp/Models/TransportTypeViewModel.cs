using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductQuoteApp.Models
{
    public class TransportTypeViewModel
    {
        public int TransportTypeID { get; set; }
        public string Description { get; set; }
        public int PositionQuantityFrom { get; set; }
        public int PositionQuantityTo { get; set; }
        public int FreightTypeID { get; set; }
        public string FreightTypeDescription { get; set; }

        public TransportTypeViewModel(TransportType transportType)
        {
            this.TransportTypeID = transportType.TransportTypeID;
            this.Description = transportType.Description;
            this.PositionQuantityFrom = transportType.PositionQuantityFrom;
            this.PositionQuantityTo = transportType.PositionQuantityTo;
            this.FreightTypeID = transportType.FreightTypeID;
            this.FreightTypeDescription = transportType.FreightType.Description;
        }
    }
}