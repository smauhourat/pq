using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class ExchangeType
    {
        public int ExchangeTypeID { get; set; }
        public string Description { get; set; }
        public string LargeDescription { get; set; }
        public int Rofex { get; set; }
    }
}
