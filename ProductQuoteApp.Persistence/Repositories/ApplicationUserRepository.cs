using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace ProductQuoteApp.Persistence
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private UserManager<ApplicationUser> userManager = null;

        public string GetEmailUserById(string id)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            return userManager.FindById(id).Email;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }
        }
    }
}
