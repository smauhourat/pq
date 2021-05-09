using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ProductQuoteApp.Services;
using System.Net;
using PagedList;
using System.Data.Entity.Validation;

namespace ProductQuoteApp.Controllers
{
    public class CustomerController : BaseController
    {
        private ICustomerService customerService = null;
        private ICreditRatingRepository creditRatingRepository = null;
        private UserManager<CustomerUser> userManager = null;
        private IWorkflowMessageService workflowMessageService = null;
        private ICustomerProductRepository customerProductRepository = null;
        private IContactRepository contactRepository = null;
        private IContactTypeRepository contactTypeRepository = null;
        private IGenericRepository<ContactClass> contactClassRepository = null;
        private ISalesChannelRepository salesChannelRepository = null;
        private ISalesChannelUserRepository salesChannelUserRepository = null;

        public CustomerController(ICustomerService customerServ, 
            ICreditRatingRepository creditRatingRepo, 
            IWorkflowMessageService workflowMessageServ, 
            ICustomerProductRepository customerProductRepo,
            IContactRepository contactRepo,
            IContactTypeRepository contactTypeRepo,
            IGenericRepository<ContactClass> contactClassRepo,
            ISalesChannelRepository salesChannelRepo,
            ISalesChannelUserRepository salesChannelUserRepo)
        {
            customerService = customerServ;
            creditRatingRepository = creditRatingRepo;
            workflowMessageService = workflowMessageServ;
            customerProductRepository = customerProductRepo;
            contactRepository = contactRepo;
            contactTypeRepository = contactTypeRepo;
            contactClassRepository = contactClassRepo;
            salesChannelRepository = salesChannelRepo;
            salesChannelUserRepository = salesChannelUserRepo;

            userManager = new UserManager<CustomerUser>(new UserStore<CustomerUser>(new ApplicationDbContext()));
            userManager.UserValidator = new UserValidator<CustomerUser>(userManager)
            {
                //AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CompanySortParm = sortOrder == "Company" ? "Company_desc" : "Company";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ui_pageListSize"].ToString());
            int pageNumber = (page ?? 1);

            var customers = await customerService.FindCustomersAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => (
                                                    (s.Company != null) && s.Company.ToLower().Contains(searchString.ToLower())
                                                )
                                         );
            }

            switch (sortOrder)
            {
                case "Company":
                    customers = customers.OrderBy(s => s.Company);
                    break;
                case "Company_desc":
                    customers = customers.OrderByDescending(s => s.Company);
                    break;
                default:  // Name ascending 
                    customers = customers.OrderBy(s => s.Company);
                    break;
            }

            IPagedList ret = customers.ToPagedList(pageNumber, pageSize);

            return View(ret);

        }

        // GET: Customers/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            Customer customer = await customerService.FindCustomersByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            ViewBag.CreditRatingID = new SelectList(creditRatingRepository.CreditRatings(), "CreditRatingID", "Description");
            ViewBag.ContactClassID = new SelectList(contactClassRepository.GetAll(), "ContactClassID", "Description");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Create([Bind(Include = "CustomerID,Company,CreditRatingID,SellerCommission,IsSpot,IsProspect,SendNotifications,DelayAverageDays,Observation,ContactName,ContactTE,ContactEmail,ContactClassID,MinimumMarginPercentage_DL, MinimumMarginPercentage_DLP, MinimumMarginPercentage_ISL, MinimumMarginPercentage_IND, MinimumMarginUSD_DL, MinimumMarginUSD_DLP, MinimumMarginUSD_ISL, MinimumMarginUSD_IND")] CustomerViewModel customerVM)
        {
            Customer customer = null;

            if (ModelState.IsValid)
            {
                customer = new Customer
                {
                    CustomerID = customerVM.CustomerID,
                    Company = customerVM.Company,
                    CreditRatingID = customerVM.CreditRatingID,
                    SellerCommission = customerVM.SellerCommission,
                    IsSpot = customerVM.IsSpot,
                    IsProspect = customerVM.IsProspect,
                    SendNotifications = customerVM.SendNotifications,
                    DelayAverageDays = customerVM.DelayAverageDays,
                    Observation = customerVM.Observation,
                    ContactName = customerVM.ContactName,
                    ContactTE = customerVM.ContactTE,
                    ContactEmail = customerVM.ContactEmail,
                    ContactClassID = customerVM.ContactClassID
                };

                await customerService.CreateCompleteAsync(customer, customerVM.MinimumMarginPercentage_DL, customerVM.MinimumMarginPercentage_DLP, customerVM.MinimumMarginPercentage_ISL, customerVM.MinimumMarginPercentage_IND, customerVM.MinimumMarginUSD_DL, customerVM.MinimumMarginUSD_DLP, customerVM.MinimumMarginUSD_ISL, customerVM.MinimumMarginUSD_IND);

                return RedirectToAction("Index");
            }

            ViewBag.CreditRatingID = new SelectList(creditRatingRepository.CreditRatings(), "CreditRatingID", "Description", customer.CreditRatingID);
            ViewBag.ContactClassID = new SelectList(contactClassRepository.GetAll(), "ContactClassID", "Description", customer.ContactClassID);

            return View(customerVM);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            Customer customer = await customerService.FindCustomersByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreditRatingID = new SelectList(creditRatingRepository.CreditRatings(), "CreditRatingID", "Description", customer.CreditRatingID);
            ViewBag.ContactClassID = new SelectList(contactClassRepository.GetAll(), "ContactClassID", "Description", customer.ContactClassID);

            return View(new CustomerViewModel(customer));
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerID,Company,CreditRatingID,SellerCommission,IsSpot, IsProspect, SendNotifications,DelayAverageDays,Observation,ContactName,ContactTE,ContactEmail,ContactClassID,MinimumMarginPercentage_DL, MinimumMarginPercentage_DLP, MinimumMarginPercentage_ISL, MinimumMarginPercentage_IND, MinimumMarginUSD_DL, MinimumMarginUSD_DLP, MinimumMarginUSD_ISL, MinimumMarginUSD_IND")] CustomerViewModel customerVM, string saveAction, string saveAndQuitAction)
        {
            Customer customer = null;

            if (ModelState.IsValid)
            {
                customer = new Customer
                {
                    CustomerID = customerVM.CustomerID,
                    Company = customerVM.Company,
                    CreditRatingID = customerVM.CreditRatingID,
                    SellerCommission = customerVM.SellerCommission,
                    IsSpot = customerVM.IsSpot,
                    IsProspect = customerVM.IsProspect,
                    SendNotifications = customerVM.SendNotifications,
                    DelayAverageDays = customerVM.DelayAverageDays,
                    Observation = customerVM.Observation,
                    ContactName = customerVM.ContactName,
                    ContactEmail = customerVM.ContactEmail,
                    ContactTE = customerVM.ContactTE,
                    ContactClassID = customerVM.ContactClassID
                };

                await customerService.UpdateCompleteAsync(customer, customerVM.MinimumMarginPercentage_DL, customerVM.MinimumMarginPercentage_DLP, customerVM.MinimumMarginPercentage_ISL, customerVM.MinimumMarginPercentage_IND, customerVM.MinimumMarginUSD_DL, customerVM.MinimumMarginUSD_DLP, customerVM.MinimumMarginUSD_ISL, customerVM.MinimumMarginUSD_IND);

                if (!string.IsNullOrEmpty(saveAndQuitAction))
                {
                    return RedirectToAction("Index");
                }
                if (!string.IsNullOrEmpty(saveAction))
                {
                    return RedirectToAction("Edit", "Customer", new { id = customerVM.CustomerID });
                }

                return RedirectToAction("Index");
            }

            ViewBag.CreditRatingID = new SelectList(creditRatingRepository.CreditRatings(), "CreditRatingID", "Description", customer.CreditRatingID);
            ViewBag.ContactClassID = new SelectList(contactClassRepository.GetAll(), "ContactClassID", "Description", customer.ContactClassID);

            return View(customerVM);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            Customer customer = await customerService.FindCustomersByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await customerService.DeleteAsync(id);
                //Prueba para eliminar info relacionada (PQ) y usuarios esta con borrado en cascada
                //await customerRepository.DeleteCascadeAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            Customer customer = await customerService.FindCustomersByIDAsync(id);
            return View(customer);
        }

        // GET: Customers/Users/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Users(int id)
        {
            Customer customer = await customerService.FindCustomersByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var model = new CustomerUserViewModel(customer);
            return View(model);
        }

        private int FindSalesChannelsByUserID(string userID)
        {
            var salesChannel = salesChannelUserRepository.FindSalesChannelsByUserID(userID).FirstOrDefault();
            if (salesChannel != null)
                return salesChannel.SalesChannelID;
            return 0;
        }

        // GET: Customers/UsersEdit
        [Authorize(Roles = "AdminUser")]
        public ActionResult UsersEdit(int customerID, string userId)
        {
            RegisterEditCustomerUserViewModel model = new RegisterEditCustomerUserViewModel();
            CustomerUser user = userManager.FindById(userId);
            if (user != null)
            {
                model.CustomerID = customerID;
                model.Email = user.Email;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.IsCustomerAdmin = user.IsCustomerAdmin;
                model.JobPosition = user.JobPosition;
                model.Initials = user.Initials;
            }
            model.SalesChannelID = FindSalesChannelsByUserID(userId);
            var salesChannles = salesChannelRepository.FindSalesChannels().ConvertAll(x => new SalesChannelSingleViewModel(x));
            ViewBag.SalesChannelID = new SelectList(salesChannles as IEnumerable<SalesChannelSingleViewModel>, "SalesChannelID", "FullName", model.SalesChannelID);

            return View(model);
        }

        // GET: Customers/UsersEdit
        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersEdit(string userId, RegisterEditCustomerUserViewModel model)        
        {
            if (ModelState.IsValid)
            {
                CustomerUser user = userManager.FindById(userId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IsCustomerAdmin = model.IsCustomerAdmin;
                user.JobPosition = model.JobPosition;
                user.Initials = model.Initials;

                //if (model.IsCustomerAdmin)
                //{
                //    userManager.RemoveFromRole(user.Id, "CustomerUser");
                //    userManager.AddToRole(user.Id, "CustomerAdminUser");
                //}
                //else
                //{
                //    userManager.RemoveFromRole(user.Id, "CustomerAdminUser");
                //    userManager.AddToRole(user.Id, "CustomerUser");
                //}

                var result = userManager.Update(user);

                if (result.Succeeded)
                {
                    //Se asigna el Canal de Venta
                    AssignSalesChannel(user.Id, model.SalesChannelID);

                    return RedirectToAction("Users", "Customer", new { id = model.CustomerID });
                }
                else
                {
                    var errors = string.Join(",", result.Errors);
                    ModelState.AddModelError(string.Empty, errors);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult CustomerProduct(int customerID)
        {
            Customer customer = customerService.FindCustomersByID(customerID);
            ViewBag.CustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(customerID);
            ViewBag.CustomerName = customer.Company;

            return View();
        }

        private void AssignSalesChannel(string userID, int salesChannelID)
        {
            SalesChannelUser salesChannelUser = new SalesChannelUser();
            salesChannelUser.UserID = userID;
            salesChannelUser.SalesChannelID = salesChannelID;
            salesChannelUserRepository.DeleteByUser(userID);
            salesChannelUserRepository.Create(salesChannelUser);
        }

        // GET: Customers/UsersCreate
        [Authorize(Roles = "AdminUser")]
        public ActionResult UsersCreate(int customerID)
        {
            RegisterCustomerUserViewModel model = new RegisterCustomerUserViewModel();
            model.CustomerID = customerID;
            var salesChannles = salesChannelRepository.FindSalesChannels().ConvertAll(x => new SalesChannelSingleViewModel(x));
            ViewBag.SalesChannelID = new SelectList(salesChannles as IEnumerable<SalesChannelSingleViewModel>, "SalesChannelID", "FullName");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersCreate(RegisterCustomerUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                CustomerUser user = (CustomerUser)model.GetUser();
                user.CustomerID = model.CustomerID;
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.IsCustomerAdmin = model.IsCustomerAdmin;
                user.JobPosition = model.JobPosition;
                user.Initials = model.Initials;
                try
                {
                    var result = userManager.Create(user, model.Password);

                    if (result.Succeeded)
                    {
                        //NO SE ESTA USANDO
                        //if (model.IsCustomerAdmin)
                        //{
                        //    userManager.AddToRole(user.Id, "CustomerAdminUser"); 
                        //}
                        //else
                        //{
                        //    userManager.AddToRole(user.Id, "CustomerUser");
                        //}

                        //Se crea el Usuario
                        userManager.AddToRole(user.Id, "CustomerUser");

                        //Se envian mail de Activacion
                        var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("Sample");
                        userManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<CustomerUser>(provider.Create("EmailConfirmation"));
                        string code = userManager.GenerateEmailConfirmationToken(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        workflowMessageService.SendConfirmEmail(user.Email, "Confirme su cuenta de acceso", "Por favor confirme su cuenta haciendo click <a href=\"" + callbackUrl + "\">aqui</a>");

                        //Se asigna el Canal de Venta
                        AssignSalesChannel(user.Id, model.SalesChannelID);

                        //Se redireccion al Listado de Usuarios
                        return RedirectToAction("Users", "Customer", new { id = model.CustomerID });
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

        [Authorize(Roles = "AdminUser")]
        public ActionResult UserSetPassword(int customerID, string id)
        {
            SetPasswordCustomerUserViewModel model = new SetPasswordCustomerUserViewModel();
            model.CustomerID = customerID;
            model.id = id;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserSetPassword(SetPasswordCustomerUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                userManager.RemovePassword(model.id);
                var result = userManager.AddPassword(model.id, model.NewPassword);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Customer", new { id = model.CustomerID });
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: Users/Delete/5
        public ActionResult UserDelete(int customerID, string id)
        {
            if ((id == null) || (id.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerUser user = userManager.FindById(id);

            EditUserViewModel model = new EditUserViewModel();
            model.CustomerID = customerID;
            model.id = id;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDelete(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = userManager.Delete(userManager.FindById(model.id));

                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Customer", new { id = model.CustomerID });
                }
                AddErrors(result);
            }

            return View(model);
        }

        // GET: Customers/Contacts/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Contacts(int id)
        {
            Customer customer = await customerService.FindCustomersByIDAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var model = new CustomerContactViewModel(customer);
            return View(model);
        }

        // GET: Customers/ContactsCreate
        [Authorize(Roles = "AdminUser")]
        public ActionResult ContactsCreate(int customerID)
        {
            ContactViewModel model = new ContactViewModel();
            model.DateContact = DateTime.Now;
            model.CustomerID = customerID;
            ViewBag.ContactTypeID = new SelectList(contactTypeRepository.ContactTypes(), "ContactTypeID", "Description");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactsCreate(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = model.GetContact();

                try
                {
                    await contactRepository.CreateAsync(contact);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e);
                }

                return RedirectToAction("Contacts", "Customer", new { id = model.CustomerID });
            }

            ViewBag.ContactTypeID = new SelectList(contactTypeRepository.ContactTypes(), "ContactTypeID", "Description", model.ContactTypeID);

            return View(model);
        }

        // GET: Customers/ContactsEdit
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> ContactsEdit(int contactID)
        {
            if (contactID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = await contactRepository.FindContactByIDAsync(contactID);

            if (contact == null)
            {
                return HttpNotFound();
            }

            ContactViewModel model = new ContactViewModel()
            {
                CustomerID = contact.CustomerID,
                ContactID = contact.ContactID,
                DateContact = contact.DateContact,
                ContactTypeID = contact.ContactTypeID,
                ContactType = contact.ContactType,
                Details = contact.Details
            };

            ViewBag.ContactTypeID = new SelectList(contactTypeRepository.ContactTypes(), "ContactTypeID", "Description", contact.ContactTypeID);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactsEdit(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact()
                {
                    ContactID = model.ContactID,
                    ContactTypeID = model.ContactTypeID,
                    CustomerID = model.CustomerID,
                    DateContact = model.DateContact,
                    Details = model.Details 
                };

                await contactRepository.UpdateAsync(contact);

                return RedirectToAction("Contacts", "Customer", new { id = model.CustomerID });
            }

            return View(model);
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> ContactsDelete(int customerID, int contactID)
        {
            if (contactID == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = await contactRepository.FindContactByIDAsync(contactID);

            if (contact == null)
            {
                return HttpNotFound();
            }

            ContactViewModel model = new ContactViewModel()
            {
                CustomerID = contact.CustomerID,
                ContactID  = contact.ContactID,
                DateContact = contact.DateContact,
                ContactTypeID = contact.ContactTypeID,
                ContactType = contact.ContactType,
                Details = contact.Details
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "AdminUser")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactsDelete(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await contactRepository.DeleteAsync(model.ContactID);

                return RedirectToAction("Contacts", "Customer", new { id = model.CustomerID });
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                if (contactTypeRepository != null)
                {
                    contactTypeRepository.Dispose();
                    contactTypeRepository = null;
                }
                if (contactRepository != null)
                {
                    contactRepository.Dispose();
                    contactRepository = null;
                }
                if (customerService != null)
                {
                    customerService.Dispose();
                    customerService = null;
                }
                if (creditRatingRepository != null)
                {
                    creditRatingRepository.Dispose();
                    creditRatingRepository = null;
                }
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }
                if (workflowMessageService != null)
                {
                    workflowMessageService.Dispose();
                    workflowMessageService = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
                if (contactClassRepository != null)
                {
                    contactClassRepository.Dispose();
                    contactClassRepository = null;
                }
                if (salesChannelRepository != null)
                {
                    salesChannelRepository.Dispose();
                    salesChannelRepository = null;
                }
                if (salesChannelUserRepository != null)
                {
                    salesChannelUserRepository.Dispose();
                    salesChannelUserRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
