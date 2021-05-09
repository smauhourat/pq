using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ProductQuoteApp.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Required")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPassword_Rule", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PtyPassword", ResourceType = typeof(Resources.Resources))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmPasswordNoMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyCurrentPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyCurrentPassword", ResourceType = typeof(Resources.Resources))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyNewPassword_Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyPasswordRules", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PtyNewPassword", ResourceType = typeof(Resources.Resources))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmNewPassword_Required")]
        [DataType(DataType.Password)]
        [Display(Name = "PtyConfirmNewPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "PtyConfirmNewPasswordCompare")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}