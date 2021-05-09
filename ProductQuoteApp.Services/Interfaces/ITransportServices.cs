using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface ITransportServices : IDisposable
    {
        decimal GetFleteFRMT_PorItem(ProductQuote productQuote, Product product);
    }
}
