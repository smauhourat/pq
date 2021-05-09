using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class SellerUserController : BaseController
    {
        private UserManager<SellerUser> userManager = null;

        public SellerUserController()
        {
            userManager = new UserManager<SellerUser>(new UserStore<SellerUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<SellerUser>(userManager)
            {
                //AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
        }

        // GET: AdminUser
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Index()
        {
            var users = userManager.Users.OfType<SellerUser>();

            var userList = users.ToList();

            return View(userList);
        }

        private void SetRole(string userId)
        {
            userManager.AddToRole(userId, "SellerUser");
        }

        // GET: SellerUser/Create
        public ActionResult Create()
        {
            RegisterSellerUserViewModel model = new RegisterSellerUserViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterSellerUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                SellerUser user = (SellerUser)model.GetUser();
                user.IsCustomerAdmin = false;
                user.UserName = model.Email;
                user.EditGlobalVariables = model.EditGlobalVariables;
                user.EditMarginOrPrice = model.EditMarginOrPrice;
                user.SeeCosting = model.SeeCosting;
                user.Initials = model.Initials;

                try
                {
                    var result = userManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        SetRole(user.Id);
                        return RedirectToAction("Index", "SellerUser");
                    }
                    else
                    {
                        var errors = string.Join(",", result.Errors);
                        ModelState.AddModelError(string.Empty, errors);
                    }
                }
                catch (DbEntityValidationException e)
                {
                    var errors = string.Join("; ", e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                    ModelState.AddModelError(string.Empty, errors);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e);
                }
            }

            return View(model);
        }


        // GET: SellerUser/Edit
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Edit(string userId)
        {
            RegisterEditSellerUserViewModel model = new RegisterEditSellerUserViewModel();

            SellerUser user = userManager.FindById(userId);
            if (user != null)
            {
                model.Id = user.Id;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.EditGlobalVariables = user.EditGlobalVariables;
                model.EditMarginOrPrice = user.EditMarginOrPrice;
                model.SeeCosting = user.SeeCosting;
                model.Initials = user.Initials; 
            }

            return View(model);
        }


        // GET: SellerUser/Edit
        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string userId, RegisterEditSellerUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                SellerUser user = userManager.FindById(userId);
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.EditGlobalVariables = model.EditGlobalVariables;
                user.EditMarginOrPrice = model.EditMarginOrPrice;
                user.SeeCosting = model.SeeCosting;
                user.Initials = model.Initials;

                var result = userManager.Update(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "SellerUser", new { id = model.Id });
                }
                else
                {
                    var errors = string.Join(",", result.Errors);
                    ModelState.AddModelError(string.Empty, errors);
                }

            }

            return View(model);
        }

        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult ResetPassword(string id, string userName)
        {
            SetPasswordSellerUserViewModel model = new SetPasswordSellerUserViewModel();
            model.id = id;
            model.userName = userName;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(SetPasswordSellerUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                userManager.RemovePassword(model.id);
                var result = userManager.AddPassword(model.id, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "SellerUser");
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: SellerUser/Delete/5
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Delete(string userId)
        {
            if ((userId == null) || (userId.Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SellerUser user = userManager.FindById(userId);

            RegisterDeleteSellerUserViewModel model = new RegisterDeleteSellerUserViewModel();
            model.Id = userId;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;

            return View(model);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "SuperAdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(RegisterDeleteSellerUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = userManager.Delete(userManager.FindById(model.Id));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "SellerUser");
                }
                AddErrors(result);
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }
            base.Dispose(disposing);
        }
    }
}