using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public class CustomerUser : ApplicationUser 
    {
        public int CustomerID { get; set; }
        public string JobPosition { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
