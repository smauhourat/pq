using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISalesChannelUserRepository : IDisposable
    {
        Task<List<SalesChannel>> FindSalesChannelsAvailableByUserAsync(string userID);
        Task<List<SalesChannelUser>> FindSalesChannelsByUserIDAsync(string userID);
        List<SalesChannelUser> FindSalesChannelsByUserID(string userID);

        Task CreateAsync(SalesChannelUser salesChannelUserToAdd);
        void Create(SalesChannelUser salesChannelUserToAdd);
        Task DeleteAsync(int salesChannelUserID);
        void Delete(int salesChannelUserID);

        Task DeleteByUserAsync(string userID);
        void DeleteByUser(string userID);
        Task AddAllSalesChannelsToUserAsync(string userID);
    }
}
