using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IWayOfExceptionServices : IDisposable
    {
        WayOfException HasWayOfExceptionProductQuote(ProductQuote productQuote);

    }
}
