using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//http://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx
namespace ProductQuoteApp.Persistence
{
    [CustomValidation(typeof(ShipmentTrackingRules), "ValidateShipmentTracking")]
    public class ShipmentTracking
    {
        [Key, ForeignKey("ProductQuote")]
        public int ProductQuoteID { get; set; }
        public virtual ProductQuote ProductQuote { get; set; }


        //Quoted
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime QuotedEstimatedDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime QuotedRealDate { get; set; }
        public Boolean QuotedCompleted { get; set; }


        //CustomerOrder
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CustomerOrderEstimatedDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CustomerOrderRealDate { get; set; }
        public Boolean CustomerOrderCompleted { get; set; }


        //Approved
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedEstimatedDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedRealDate { get; set; }
        public Boolean ApprovedCompleted { get; set; }


        //InProduction
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? InProductionEstimatedDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? InProductionRealDate { get; set; }
        public Boolean InProductionCompleted { get; set; }


        //EstimatedTimeOfDeparture
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ETDEstimatedDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ETDRealDate { get; set; }
        public Boolean ETDCompleted { get; set; }


        //EstimatedTimeOfArrival
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ETAEstimatedDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ETARealDate { get; set; }
        public Boolean ETACompleted { get; set; }


        //Nationalized
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NationalizedEstimatedDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? NationalizedRealDate { get; set; }
        public Boolean NationalizedCompleted { get; set; }


        //Delivered
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredEstimatedDate { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveredRealDate { get; set; }
        public Boolean DeliveredCompleted { get; set; }


        public Boolean InProductionEnabled { get; set; }
        public Boolean ETDEnabled { get; set; }
        public Boolean ETAEnabled { get; set; }
        public Boolean NationalizedEnabled { get; set; }
    }
}
