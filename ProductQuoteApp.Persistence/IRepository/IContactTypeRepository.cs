using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IContactTypeRepository : IDisposable
    {
        List<ContactType> ContactTypes();
    }
}
