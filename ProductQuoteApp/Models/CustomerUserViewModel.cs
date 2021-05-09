using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductQuoteApp.Persistence;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Models
{
    /// <summary>
    /// Mantiene los datos del Cliente con una lista de Usuarios asociados
    /// </summary>
    public class CustomerUserViewModel
    {
        public int CustomerID { get; set; }
        //public Boolean IsCustomerAdmin {get;set;}
        public string CustomerName { get; set; }
        public ICollection<CustomerUser> Users { get; set; }
        public CustomerUserViewModel(Customer customer)
        {
            this.CustomerID = customer.CustomerID;
            //this.IsCustomerAdmin = customer.IsCustomerAdmin;
            this.CustomerName = customer.Company;
            this.Users = customer.Users;
        }
    }

    /// <summary>
    /// Utilizado para la creacion de Usuarios del Cliente
    /// </summary>
    public class RegisterCustomerUserViewModel : RegisterViewModel
    {
        public int CustomerID { get; set; }

        [Display(Name = "PtyJobPosition", ResourceType = typeof(Resources.Resources))]
        public string JobPosition { get; set; }

        [Display(Name = "PtyCustomerAdmin", ResourceType = typeof(Resources.Resources))]
        public Boolean IsCustomerAdmin { get; set; }

        [Display(Name = "Negocio")]
        public int SalesChannelID { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new CustomerUser()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                JobPosition = this.JobPosition
            };
            return user;
        }
    }


    public class RegisterEditCustomerUserViewModel : RegisterEditViewModel
    {
        public int CustomerID { get; set; }

        [Display(Name = "PtyJobPosition", ResourceType = typeof(Resources.Resources))]
        public string JobPosition { get; set; }

        [Display(Name = "PtyCustomerAdmin", ResourceType = typeof(Resources.Resources))]
        public Boolean IsCustomerAdmin { get; set; }

        [Display(Name = "Negocio")]
        public int SalesChannelID { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new CustomerUser()
            {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                JobPosition = this.JobPosition
            };
            return user;
        }
    }

    /// <summary>
    /// Utilizado para Setear la password del Usuario del Cliente
    /// </summary>
    public class SetPasswordCustomerUserViewModel : SetPasswordViewModel
    {
        public int CustomerID { get; set; }
        public string id { get; set; }
    }

    public class EditUserViewModel : UserViewModel
    {
        public int CustomerID { get; set; }
        public string id { get; set; }
    }

}