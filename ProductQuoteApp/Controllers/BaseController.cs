using Microsoft.AspNet.Identity;
using ProductQuoteApp.Helpers;
using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class BaseController : Controller
    {
        private int _currentCustomerID;
        private string _currentUserFullName = "";
        private string _currenUserEmail = "";
        private string _currenUserId = "";
        private string _currenCustomerCompany = "";

        private bool _currentUserIsSellerUser = false;
        private bool _currentUserEditGlobalVariables = false;
        private bool _currentUserEditMarginOrPrice = false;
        private bool _currentUserSeeCosting = false;

        public void SetCultureForce(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            SetCultureForce("es");

            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            //OJO DECIMAL
            //https://www.experts-exchange.com/questions/24196578/NET-C-decimal-separator-from-point-to-coma-and-vice-versa.html
            //Set to "."
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(cultureName);
            //ci.NumberFormat.NumberDecimalSeparator = ".";
            if (cultureName == "es")
                ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            // Cookies
            HttpCookie currentCustomerIDCookie = Request.Cookies["_customerID"];
            if (currentCustomerIDCookie != null)
                _currentCustomerID = Int32.Parse(currentCustomerIDCookie.Value);

            HttpCookie currentUserFullNameCookie = Request.Cookies["_currentUserFullName"];
            if (currentUserFullNameCookie != null)
                _currentUserFullName = currentUserFullNameCookie.Value;

            HttpCookie currentUserEmailCookie = Request.Cookies["_currentUserEmail"];
            if (currentUserEmailCookie != null)
                _currenUserEmail = currentUserEmailCookie.Value;

            HttpCookie currentUserIdCookie = Request.Cookies["_currentUserId"];
            if (currentUserIdCookie != null)
                _currenUserId = currentUserIdCookie.Value;

            HttpCookie currentUserIsSellerUserCookie = Request.Cookies["_currentUserIsSellerUser"];
            if (currentUserIsSellerUserCookie != null)
                _currentUserIsSellerUser = (currentUserIsSellerUserCookie.Value.ToLower() != "false");

            HttpCookie currentUserEditGlobalVariablesCookie = Request.Cookies["_currentUserEditGlobalVariables"];
            if (currentUserEditGlobalVariablesCookie != null)
                _currentUserEditGlobalVariables = (currentUserEditGlobalVariablesCookie.Value.ToLower() != "false");

            HttpCookie currentUserEditMarginOrPriceCookie = Request.Cookies["_currentUserEditMarginOrPrice"];
            if (currentUserEditMarginOrPriceCookie != null)
                _currentUserEditMarginOrPrice = (currentUserEditMarginOrPriceCookie.Value.ToLower() != "false");

            HttpCookie currentUserSeeCostingCookie = Request.Cookies["_currentUserSeeCosting"];
            if (currentUserSeeCostingCookie != null)
                _currentUserSeeCosting = (currentUserSeeCostingCookie.Value.ToLower() != "false");

            return base.BeginExecuteCore(callback, state);
        }

        public int CurrentCustomerID
        {
            get { return _currentCustomerID; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_customerID"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_customerID");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentCustomerID = Int32.Parse(cookie.Value);

                Response.Cookies.Add(cookie);
            }
        }
        public string CurrentUserFullName
        {
            get { return _currentUserFullName; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserFullName"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserFullName");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                //cookie.Domain = "estoesunaprueba";
                _currentUserFullName = cookie.Value;

                Response.Cookies.Add(cookie);
            }
        }
        public string CurrentUserEmail
        {
            get { return _currenUserEmail; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserEmail"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserEmail");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                //cookie.Domain = "estoesunaprueba";
                _currenUserEmail = cookie.Value;

                Response.Cookies.Add(cookie);
            }
        }
        public string CurrentUserId
        {
            get { return _currenUserId; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserId"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserId");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currenUserId = cookie.Value;

                Response.Cookies.Add(cookie);
            }
        }
        public string CurrentCustomerCompany
        {
            get { return _currenCustomerCompany; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentCustomerCompany"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentCustomerCompany");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currenCustomerCompany = cookie.Value;

                Response.Cookies.Add(cookie);
            }
        }

        public bool CurrentUserIsSellerUser
        {
            get { return _currentUserIsSellerUser; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserIsSellerUser"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserIsSellerUser");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserIsSellerUser = !(cookie.Value.ToLower() == "false");

                Response.Cookies.Add(cookie);
            }
        }

        public bool CurrentUserEditGlobalVariables
        {
            get { return _currentUserEditGlobalVariables; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserEditGlobalVariables"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserEditGlobalVariables");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserEditGlobalVariables = (cookie.Value.ToLower() != "false");

                Response.Cookies.Add(cookie);
            }
        }

        public bool CurrentUserEditMarginOrPrice
        {
            get { return _currentUserEditMarginOrPrice; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserEditMarginOrPrice"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserEditMarginOrPrice");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserEditMarginOrPrice = (cookie.Value.ToLower() != "false");

                Response.Cookies.Add(cookie);
            }
        }

        public bool CurrentUserSeeCosting
        {
            get { return _currentUserSeeCosting; }
            set
            {
                // Save culture in a cookie
                HttpCookie cookie = Request.Cookies["_currentUserSeeCosting"];
                if (cookie != null)
                    cookie.Value = value.ToString();   // update cookie value
                else
                {
                    cookie = new HttpCookie("_currentUserSeeCosting");
                    cookie.Value = value.ToString();
                    //cookie.Expires = DateTime.Now.AddYears(1);
                }

                _currentUserSeeCosting = (cookie.Value.ToLower() != "false");

                Response.Cookies.Add(cookie);
            }
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected void SendTestMail()
        {
            //emailManager.SendEmail()
        }

        //https://stackify.com/aspnet-mvc-error-handling/
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    //_Logger.Error(filterContext.Exception);

        //}
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Default", "Home");
        }
    }
}
