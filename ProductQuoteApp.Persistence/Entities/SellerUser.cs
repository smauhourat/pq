using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class SellerUser : ApplicationUser
    {
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
