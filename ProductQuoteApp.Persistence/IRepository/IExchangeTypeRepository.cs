using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IExchangeTypeRepository : IDisposable
    {
        List<ExchangeType> ExchangeTypes();
        List<ExchangeType> FindExchangeTypesBySaleModalityID(int saleModalityID);
    }
}
