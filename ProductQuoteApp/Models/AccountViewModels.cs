using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductQuoteApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyEmail_Required")]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }
        
        [Display(Name = "PtyRememberMe", ResourceType = typeof(Resources.Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyEmail_Required")]
        [EmailAddress]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Display(Name = "PtyFirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyFirstName_Required")]
        public string FirstName { get; set; }

        [Display(Name = "PtyLastName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyLastName_Required")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Required")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Rule", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PtyPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPasswordNoMatch")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "PtyInitials", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyInitials_Required")]
        [StringLength(3, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyInitials_Rule", MinimumLength = 3)]
        public string Initials { get; set; }
    }

    public class RegisterEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyEmail_Required")]
        [EmailAddress]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Display(Name = "PtyFirstName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyFirstName_Required")]
        public string FirstName { get; set; }

        [Display(Name = "PtyLastName", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyLastName_Required")]
        public string LastName { get; set; }

        [Display(Name = "PtyInitials", ResourceType = typeof(Resources.Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyInitials_Required")]
        public string Initials { get; set; }
    }

    public class UserViewModel
    {
        [EmailAddress]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Display(Name = "PtyFirstName", ResourceType = typeof(Resources.Resources))]
        public string FirstName { get; set; }

        [Display(Name = "PtyLastName", ResourceType = typeof(Resources.Resources))]
        public string LastName { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyEmail_Required")]
        [EmailAddress]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Rule", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PtyPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPasswordNoMatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyEmail_Required")]
        [EmailAddress]
        [Display(Name = "PtyEmail", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
    }
}
