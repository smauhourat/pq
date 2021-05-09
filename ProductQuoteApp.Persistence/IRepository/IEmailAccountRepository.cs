using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProductQuoteApp.Persistence
{
    public interface IEmailAccountRepository: IDisposable
    {
        Task<List<EmailAccount>> FindEmailAccountsAsync();
        Task<EmailAccount> FindEmailAccountsByIDAsync(int emailAccountID);
        EmailAccount FindEmailAccountsDefaultAsync();
        Task CreateAsync(EmailAccount emailAccountToAdd);
        Task DeleteAsync(int emailAccountID);
        Task UpdateAsync(EmailAccount emailAccountToSave);
    }
}
