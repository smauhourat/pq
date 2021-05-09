using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ProductQuoteApp.Persistence;

namespace ProductQuoteApp.Models
{
    public class SellerUserViewModel
    {
    }

    public class RegisterSellerUserViewModel : RegisterViewModel
    {

        [Display(Name = "PtyEditGlobalVariables", ResourceType = typeof(Resources.Resources))]
        public Boolean EditGlobalVariables { get; set; }

        [Display(Name = "PtyEditMarginOrPrice", ResourceType = typeof(Resources.Resources))]
        public Boolean EditMarginOrPrice { get; set; }

        [Display(Name = "PtySeeCosting", ResourceType = typeof(Resources.Resources))]
        public Boolean SeeCosting { get; set; }



        public ApplicationUser GetUser()
        {
            var user = new SellerUser()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                EditGlobalVariables = this.EditGlobalVariables,
                EditMarginOrPrice = this.EditMarginOrPrice,
                SeeCosting = this.SeeCosting,
                Initials = this.Initials
            };
            return user;
        }
    }

    public class RegisterEditSellerUserViewModel : RegisterEditViewModel
    {
        [Display(Name = "PtyEditGlobalVariables", ResourceType = typeof(Resources.Resources))]
        public Boolean EditGlobalVariables { get; set; }

        [Display(Name = "PtyEditMarginOrPrice", ResourceType = typeof(Resources.Resources))]
        public Boolean EditMarginOrPrice { get; set; }

        [Display(Name = "PtySeeCosting", ResourceType = typeof(Resources.Resources))]
        public Boolean SeeCosting { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new SellerUser()
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

    public class RegisterDeleteSellerUserViewModel : UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "PtyEditGlobalVariables", ResourceType = typeof(Resources.Resources))]
        public Boolean EditGlobalVariables { get; set; }

        [Display(Name = "PtyEditMarginOrPrice", ResourceType = typeof(Resources.Resources))]
        public Boolean EditMarginOrPrice { get; set; }

        [Display(Name = "PtySeeCosting", ResourceType = typeof(Resources.Resources))]
        public Boolean SeeCosting { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new SellerUser()
            {
                Id = this.Id,
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
            return user;
        }
    }

    public class SetPasswordSellerUserViewModel : SetPasswordViewModel
    {
        public string id { get; set; }

        [Display(Name = "PtyUserName", ResourceType = typeof(Resources.Resources))]
        public string userName { get; set; }
    }

}