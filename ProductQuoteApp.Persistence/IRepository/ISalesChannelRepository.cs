using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface ISalesChannelRepository : IDisposable
    {
        List<SalesChannel> SalesChannels();
        Task<List<SalesChannel>> SalesChannelsAsync();

        Task<List<SalesChannel>> FindSalesChannelsAsync();
        List<SalesChannel> FindSalesChannels();
        Task<SalesChannel> FindSalesChannelByIDAsync(int salesChannelID);

        Task CreateAsync(SalesChannel salesChannelToAdd);
        Task DeleteAsync(int salesChannelID);
        Task UpdateAsync(SalesChannel salesChannelToSave);

    }
}
