using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProductQuoteApp.Persistence
{
    public class CurrencyType
    {
        public int CurrencyTypeID { get; set; }
        public string Description { get; set; }
    }
}
