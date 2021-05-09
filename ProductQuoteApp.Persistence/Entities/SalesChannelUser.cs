using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SalesChannelUser
    {
        public int SalesChannelUserID { get; set; }
        public string UserID { get; set; }
        public int SalesChannelID { get; set; }
        public virtual SalesChannel SalesChannel { get; set; }
    }
}
