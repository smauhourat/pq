using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductQuoteApp.Persistence
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private UserManager<AdminUser> userManager = null;

        public List<string> GetAdminUsersEmails()
        {
            userManager = new UserManager<AdminUser>(new UserStore<AdminUser>(new ApplicationDbContext()));
            var users = userManager.Users.OfType<AdminUser>();
            var emails = new List<String>();

            foreach (var user in users)
            {
                emails.Add(user.Email);
            }

            return emails;
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
