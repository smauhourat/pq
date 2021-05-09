using ProductQuoteApp.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProductQuoteApp.Models
{
    public class ShipmentTrackingViewModel
    {
        //public int ShipmentTrackingID { get; set; }
        public int ProductQuoteID { get; set; }
        public string ProductQuoteCode { get; set; }

        //Quoted
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime QuotedEstimatedDate { get; set; }
        public DateTime QuotedRealDate { get; set; }
        public Boolean QuotedCompleted { get; set; }

        //CustomerOrder
        public DateTime? CustomerOrderEstimatedDate { get; set; }
        public DateTime? CustomerOrderRealDate { get; set; }
        public Boolean CustomerOrderCompleted { get; set; }

        //Approved
        public DateTime? ApprovedEstimatedDate { get; set; }
        public DateTime? ApprovedRealDate { get; set; }
        public Boolean ApprovedCompleted { get; set; }

        //InProduction
        public DateTime? InProductionEstimatedDate { get; set; }
        public DateTime? InProductionRealDate { get; set; }
        public Boolean InProductionCompleted { get; set; }

        //EstimatedTimeOfDeparture
        public DateTime? ETDEstimatedDate { get; set; }
        public DateTime? ETDRealDate { get; set; }
        public Boolean ETDCompleted { get; set; }

        //EstimatedTimeOfArrival
        public DateTime? ETAEstimatedDate { get; set; }
        public DateTime? ETARealDate { get; set; }
        public Boolean ETACompleted { get; set; }

        //Nationalized
        public DateTime? NationalizedEstimatedDate { get; set; }
        public DateTime? NationalizedRealDate { get; set; }
        public Boolean NationalizedCompleted { get; set; }

        //Delivered
        public DateTime? DeliveredEstimatedDate { get; set; }
        public DateTime? DeliveredRealDate { get; set; }
        public Boolean DeliveredCompleted { get; set; }

        public Boolean InProductionEnabled { get; set; }
        public Boolean ETDEnabled { get; set; }
        public Boolean ETAEnabled { get; set; }
        public Boolean NationalizedEnabled { get; set; }


        public ShipmentTrackingViewModel(ShipmentTracking shipmentTracking)
        {
            if (shipmentTracking == null) return;

            //this.ShipmentTrackingID = shipmentTracking.ShipmentTrackingID;
            this.ProductQuoteID = shipmentTracking.ProductQuoteID;
            this.ProductQuoteCode = shipmentTracking.ProductQuote.ProductQuoteCode;

            //Quoted
            this.QuotedEstimatedDate = shipmentTracking.QuotedEstimatedDate;
            this.QuotedRealDate = shipmentTracking.QuotedRealDate;
            this.QuotedCompleted = shipmentTracking.QuotedCompleted;

            //CustomerOrder
            this.CustomerOrderEstimatedDate = shipmentTracking.CustomerOrderEstimatedDate;
            this.CustomerOrderRealDate = shipmentTracking.CustomerOrderRealDate;
            this.CustomerOrderCompleted = shipmentTracking.CustomerOrderCompleted;

            //Approved
            this.ApprovedEstimatedDate = shipmentTracking.ApprovedEstimatedDate;
            this.ApprovedRealDate = shipmentTracking.ApprovedRealDate;
            this.ApprovedCompleted = shipmentTracking.ApprovedCompleted;

            //InProduction
            this.InProductionEstimatedDate = shipmentTracking.InProductionEstimatedDate;
            this.InProductionRealDate = shipmentTracking.InProductionRealDate;
            this.InProductionCompleted = shipmentTracking.InProductionCompleted;

            //EstimatedTimeOfDeparture
            this.ETDEstimatedDate = shipmentTracking.ETDEstimatedDate;
            this.ETDRealDate = shipmentTracking.ETDRealDate;
            this.ETDCompleted = shipmentTracking.ETDCompleted;

            //EstimatedTimeOfArrival
            this.ETAEstimatedDate = shipmentTracking.ETAEstimatedDate;
            this.ETARealDate = shipmentTracking.ETARealDate;
            this.ETACompleted = shipmentTracking.ETACompleted;

            //Nationalized
            this.NationalizedEstimatedDate = shipmentTracking.NationalizedEstimatedDate;
            this.NationalizedRealDate = shipmentTracking.NationalizedRealDate;
            this.NationalizedCompleted = shipmentTracking.NationalizedCompleted;

            //Delivered
            this.DeliveredEstimatedDate = shipmentTracking.DeliveredEstimatedDate;
            this.DeliveredRealDate = shipmentTracking.DeliveredRealDate;
            this.DeliveredCompleted = shipmentTracking.DeliveredCompleted;


            this.InProductionEnabled = shipmentTracking.InProductionEnabled;
            this.ETDEnabled = shipmentTracking.ETDEnabled;
            this.ETAEnabled = shipmentTracking.ETAEnabled;
            this.NationalizedEnabled = shipmentTracking.NationalizedEnabled;


    }
}
}