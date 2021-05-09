using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class SalesChannelUserApiController : ApiController
    {
        private ISalesChannelUserRepository salesChannelUserRepository = null;

        public SalesChannelUserApiController(ISalesChannelUserRepository salesChannelUserRepo)
        {
            salesChannelUserRepository = salesChannelUserRepo;
        }

        [Authorize(Roles = "SuperAdminUser")]
        [Route("GetAvailableSalesChannels/{userID}")]
        public async Task<List<SalesChannelSingleViewModel>> GetAvailableSalesChannels(string userID)
        {
            List<SalesChannelSingleViewModel> cpList = new List<SalesChannelSingleViewModel>();
            IEnumerable<SalesChannel> result = await salesChannelUserRepository.FindSalesChannelsAvailableByUserAsync(userID);
            result = result.OrderBy(s => s.Code.ToUpper());

            foreach (SalesChannel item in result)
            {
                cpList.Add(new SalesChannelSingleViewModel(item));
            }

            return cpList;
        }

        [Authorize(Roles = "SuperAdminUser")]
        [Route("GetAssignedSalesChannels/{userID}")]
        public async Task<List<SalesChannelUserSingleViewModel>> GetAssignedSalesChannels(string userID)
        {
            List<SalesChannelUserSingleViewModel> cpList = new List<SalesChannelUserSingleViewModel>();
            IEnumerable<SalesChannelUser> result = await salesChannelUserRepository.FindSalesChannelsByUserIDAsync(userID);
            result = result.OrderBy(s => s.SalesChannel.Code.ToUpper());

            foreach (SalesChannelUser item in result)
            {
                cpList.Add(new Models.SalesChannelUserSingleViewModel(item));
            }
            return cpList;
        }

        [Authorize(Roles = "SuperAdminUser")]
        [System.Web.Http.Route("AddSalesChannelToUser/{userID}")]
        public async Task AddSalesChannelToUser(SalesChannelSingleViewModel model, string userID)
        {
            SalesChannelUser salesChannelUser = new SalesChannelUser();
            salesChannelUser.UserID = userID;
            salesChannelUser.SalesChannelID = model.SalesChannelID;

            await salesChannelUserRepository.CreateAsync(salesChannelUser);
        }

        [Authorize(Roles = "SuperAdminUser")]
        [System.Web.Http.Route("DelSalesChannelToUser/{id}")]
        public async Task DelSalesChannelToUser(int id)
        {
            await salesChannelUserRepository.DeleteAsync(id);
        }

        [Authorize(Roles = "SuperAdminUser")]
        [System.Web.Http.Route("DelAllSalesChannelByUser/{userID}")]
        public async Task DelAllSalesChannelByUser(string userID)
        {
            await salesChannelUserRepository.DeleteByUserAsync(userID);
        }

        [Authorize(Roles = "SuperAdminUser")]
        [System.Web.Http.Route("AddAllSalesChannelsToUser/{userID}")]
        public async Task AddAllSalesChannelsToUser(string userID)
        {
            await salesChannelUserRepository.AddAllSalesChannelsToUserAsync(userID);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && salesChannelUserRepository != null)
            {
                salesChannelUserRepository.Dispose();
                salesChannelUserRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}