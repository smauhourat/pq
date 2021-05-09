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
    public class AdminUserController : BaseController
    {
        private UserManager<AdminUser> userManager = null;

        public AdminUserController()
        {
            userManager = new UserManager<AdminUser>(new UserStore<AdminUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<AdminUser>(userManager)
            {
                //AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
        }

        // GET: AdminUser
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Index()
        {
            var users = userManager.Users.OfType<AdminUser>();

            var userList = users.ToList();

            return View(userList);

        }

        private void SetRoles(string userId)
        {
            userManager.AddToRole(userId, "AdminUser");
            userManager.AddToRole(userId, "SellerUser");
        }

        private void SetRoleSuperAdmin(string userId, Boolean isSuperAdmin)
        {
            userManager.RemoveFromRole(userId, "SuperAdminUser");
            if (isSuperAdmin)
                userManager.AddToRole(userId, "SuperAdminUser");
        }


        // GET: AdminUser/Create
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Create()
        {
            RegisterAdminUserViewModel model = new RegisterAdminUserViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterAdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminUser user = (AdminUser)model.GetUser();
                user.IsCustomerAdmin = false;
                user.UserName = model.Email;

                try
                {
                    var result = userManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        SetRoles(user.Id);
                        SetRoleSuperAdmin(user.Id, model.IsSuperAdmin);
                        return RedirectToAction("Index", "AdminUser");
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


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: AdminUser/Edit
        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Edit(string userId)
        {
            RegisterEditAdminUserViewModel model = new RegisterEditAdminUserViewModel();

            AdminUser user = userManager.FindById(userId);
            if (user != null)
            {
                model.Id = user.Id;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Initials = user.Initials;
                model.IsSuperAdmin = userManager.IsInRole(user.Id, "SuperAdminUser");
            }

            return View(model);
        }

        // GET: AdminUser/Edit
        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string userId, RegisterEditAdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminUser user = userManager.FindById(userId);
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Initials = model.Initials;

                var result = userManager.Update(user);

                if (result.Succeeded)
                {
                    SetRoleSuperAdmin(user.Id, model.IsSuperAdmin);
                    return RedirectToAction("Index", "AdminUser", new { id = model.Id });
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
            SetPasswordAdminUserViewModel model = new SetPasswordAdminUserViewModel();
            model.id = id;
            model.userName = userName;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(SetPasswordAdminUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                userManager.RemovePassword(model.id);
                var result = userManager.AddPassword(model.id, model.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AdminUser");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Delete(string userid)
        {
            if ((userid == null) || (userid.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminUser user = userManager.FindById(userid);
            if (user == null)
            {
                return HttpNotFound();
            }

            RegisterEditAdminUserViewModel model = new RegisterEditAdminUserViewModel();
            model.Id = userid;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            return View(model);
        }

        [Authorize(Roles = "SuperAdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string userid)
        {
            AdminUser user = userManager.FindById(userid);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (userManager.IsInRole(userid, "SuperAdminUser"))
            {
                ModelState.AddModelError("ERROR_MSG", "No puede eliminarse este tipo de Usuario");

                RegisterEditAdminUserViewModel model = new RegisterEditAdminUserViewModel();
                model.Id = userid;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;

                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var result = userManager.Delete(userManager.FindById(userid));

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "AdminUser");
                    }
                    AddErrors(result);
                }
                return View();
            }
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