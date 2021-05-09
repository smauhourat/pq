using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Persistence
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Display(Name = "PtyFirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyFirstName_Required")]
        public string FirstName { get; set; }

        [Display(Name = "PtyLastName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyLastName_Required")]
        public string LastName { get; set; }


        [Display(Name = "PtyIsCustomerAdmin", ResourceType = typeof(Resources.Resources))]
        public Boolean IsCustomerAdmin { get; set; }

        [Display(Name = "PtyEditGlobalVariables", ResourceType = typeof(Resources.Resources))]
        public Boolean EditGlobalVariables { get; set; }

        [Display(Name = "PtyEditMarginOrPrice", ResourceType = typeof(Resources.Resources))]
        public Boolean EditMarginOrPrice { get; set; }

        [Display(Name = "PtySeeCosting", ResourceType = typeof(Resources.Resources))]
        public Boolean SeeCosting { get; set; }

        [Display(Name = "PtyInitials", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyInitials_Required")]
        public string Initials { get; set; }
    }

    public class ProductQuoteAppConfiguration : DbConfiguration
    {
        public ProductQuoteAppConfiguration()
        {
            //posta
            //SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            this.SetExecutionStrategy("System.Data.SqlClient", () => SuspendExecutionStrategy
              ? (IDbExecutionStrategy)new DefaultExecutionStrategy()
              : new SqlAzureExecutionStrategy());

            ////https://msdn.microsoft.com/en-us/data/dn456835
            ////https://msdn.microsoft.com/en-us/data/jj680699

            ////maximum number of retries to 1 and the maximum delay to 30 seconds
            //SetExecutionStrategy(
            //    "System.Data.SqlClient",
            //    () => new SqlAzureExecutionStrategy(1, TimeSpan.FromSeconds(30)));
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }


}
