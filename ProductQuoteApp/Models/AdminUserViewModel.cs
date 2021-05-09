using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductQuoteApp.Persistence;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Models
{
    public class AdminUserViewModel
    {
    }

    public class RegisterAdminUserViewModel : RegisterViewModel
    {
        [Display(Name = "Es SuperAdmin")]
        public Boolean IsSuperAdmin { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new AdminUser()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Initials = this.Initials
            };
            return user;
        }
    }

    public class RegisterEditAdminUserViewModel : RegisterEditViewModel
    {
        [Display(Name = "Es SuperAdmin")]
        public Boolean IsSuperAdmin { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new AdminUser()
            {
                Id = this.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Initials = this.Initials
            };
            return user;
        }
    }

    public class SetPasswordAdminUserViewModel : SetPasswordViewModel
    {
        public string id { get; set; }

        [Display(Name = "PtyUserName", ResourceType = typeof(Resources.Resources))]
        public string userName { get; set; }
    }

}