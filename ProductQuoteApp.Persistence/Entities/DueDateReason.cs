using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    [MetadataType(typeof(DueDateReasonMetaData))]
    public class DueDateReason
    {
        public int DueDateReasonID { get; set; }
        public string Description { get; set; }

    }
}
