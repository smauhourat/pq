using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISellerCompanyRepository : IDisposable
    {
        List<SellerCompany> SellerCompanys();
    }
}
