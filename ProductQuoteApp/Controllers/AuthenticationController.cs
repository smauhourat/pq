using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class AuthenticationController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ICustomerRepository customerRepository;
        private ILogRecordRepository logRecordRepository = null;
        private IWorkflowMessageService workflowMessageService = null;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AuthenticationController(ICustomerRepository customerRepo, ILogRecordRepository logRecordRepo, IWorkflowMessageService workflowMessageServ)
        {
            customerRepository = customerRepo;
            logRecordRepository = logRecordRepo;
            workflowMessageService = workflowMessageServ;
        }

        public AuthenticationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private Customer GetCustomer(int customerID)
        {
            return customerRepository.FindCustomersByID(customerID);
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var modelParent = new HomeLoginViewModel();

            modelParent.LoginHomeViewModel = new LoginViewModel();
            modelParent.RegisterHomeViewModel = new RegisterHomeViewModel();

            return View(modelParent);
        }

        [HttpPost]
        [AllowAnonymous]
        //TestApp
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _LoginHomeForm(LoginViewModel model, string returnUrl)
        {
            var modelParent = new HomeLoginViewModel();

            modelParent.LoginHomeViewModel = model;
            modelParent.RegisterHomeViewModel = new RegisterHomeViewModel();

            if (!ModelState.IsValid)
            {
                return View(modelParent);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindAsync(model.Email, model.Password);
                    if (UserManager.IsInRole(user.Id, "CustomerUser") || UserManager.IsInRole(user.Id, "CustomerAdminUser"))
                    {

                        base.CurrentUserEmail = ((CustomerUser)user).Email;
                        base.CurrentUserId = ((CustomerUser)user).Id;
                        base.CurrentUserFullName = ((CustomerUser)user).FullName;

                        base.CurrentCustomerID = ((CustomerUser)user).CustomerID;
                        base.CurrentCustomerCompany = GetCustomer(((CustomerUser)user).CustomerID).Company;

                        base.CurrentUserIsSellerUser = false;
                        base.CurrentUserEditGlobalVariables = false;
                        base.CurrentUserEditMarginOrPrice = false;
                        base.CurrentUserSeeCosting = false;

                        var log = new LogRecord
                        {
                            LogLevel = LogLevel.Information,
                            ShortMessage = "Login Usuario - (Usuario Cliente: " + model.Email + ")",
                            //CreatedOnUtc = DateTime.UtcNow
                            CreatedOnUtc = DateTime.Now
                        };
                        await logRecordRepository.CreateAsync(log);

                    }
                    if (UserManager.IsInRole(user.Id, "AdminUser"))
                    {

                        base.CurrentUserEmail = ((AdminUser)user).Email;
                        base.CurrentUserId = ((AdminUser)user).Id;
                        base.CurrentUserFullName = ((AdminUser)user).FullName;

                        base.CurrentUserIsSellerUser = true;
                        base.CurrentUserEditGlobalVariables = true;
                        base.CurrentUserEditMarginOrPrice = true;
                        base.CurrentUserSeeCosting = true;

                        var log = new LogRecord
                        {
                            LogLevel = LogLevel.Information,
                            ShortMessage = "Login Usuario - (Usuario Admin: " + model.Email + ")",
                            //CreatedOnUtc = DateTime.UtcNow
                            CreatedOnUtc = DateTime.Now
                        };
                        await logRecordRepository.CreateAsync(log);
                    }
                    if ((UserManager.IsInRole(user.Id, "SellerUser")) && !UserManager.IsInRole(user.Id, "AdminUser"))
                    {
                        base.CurrentUserEmail = ((SellerUser)user).Email;
                        base.CurrentUserId = ((SellerUser)user).Id;
                        base.CurrentUserFullName = ((SellerUser)user).FullName;
                        base.CurrentCustomerCompany = "";

                        base.CurrentUserIsSellerUser = true;
                        base.CurrentUserEditGlobalVariables = ((SellerUser)user).EditGlobalVariables;
                        base.CurrentUserEditMarginOrPrice = ((SellerUser)user).EditMarginOrPrice;
                        base.CurrentUserSeeCosting = ((SellerUser)user).SeeCosting;

                        var log = new LogRecord
                        {
                            LogLevel = LogLevel.Information,
                            ShortMessage = "Login Usuario - (Usuario Vendedor: " + model.Email + ")",
                            //CreatedOnUtc = DateTime.UtcNow
                            CreatedOnUtc = DateTime.Now
                        };
                        await logRecordRepository.CreateAsync(log);
                    }

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:

                default:
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(modelParent);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult _RegisterHomeForm(RegisterHomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                workflowMessageService.SendRegisterInfo(model.Email, "Solicitud Registro Cliente Laquim S.A", CuerpoMailRegistro(model));

                return RedirectToAction("Index");
            }

            var modelParent = new HomeLoginViewModel();

            modelParent.LoginHomeViewModel = new LoginViewModel();
            modelParent.RegisterHomeViewModel = model;

            return View(modelParent);
            //return PartialView(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }

                if (logRecordRepository != null)
                {
                    logRecordRepository.Dispose();
                    logRecordRepository = null;
                }

                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
                
            }

            base.Dispose(disposing);
        }

        private string CuerpoMailRegistro(RegisterHomeViewModel model)
        {
            string result = "";

            result = result + "<b>Razon Social: </b>" + model.Company + "<br>";
            result = result + "<b>Sector: </b>" + model.Department + "<br>";
            result = result + "<b>Domicilio Legal: </b>" + model.LegalAddress + "<br>";
            result = result + "<b>Nombre del Contacto: </b>" + model.ContactName + "<br>";
            result = result + "<b>Cargo: </b>" + model.JobPosition + "<br>";
            result = result + "<b>Domicilio Legal: </b>" + model.LegalAddress + "<br>";
            result = result + "<b>Telefono: </b>" + model.PhoneNumber + "<br>";
            result = result + "<b>Email: </b>" + model.Email + "<br>";

            return result;
        }
    }
}