using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using PagedList;
using ProductQuoteApp.Helpers;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ProductQuoteController : BaseController
    {
        private IProductQuoteRepository productQuoteRepository = null;
        private IProductQuoteService productQuoteService = null;
        private ICustomerRepository customerRepository = null;
        private ICustomerProductRepository customerProductRepository = null;
        private IDueDateReasonRepository dueDateReasonRepository = null;
        private IGenericRepository<ReasonsForClosure> reasonsForClosureRepository = null;

        private UserManager<ApplicationUser> userManager = null;

        public ProductQuoteController(IProductQuoteRepository productQuoteRepo, 
            ICustomerRepository customerRepo, 
            ICustomerProductRepository customerProductRepo, 
            IProductQuoteService productQuoteServ, 
            IDueDateReasonRepository dueDateReasonRepo,
            IGenericRepository<ReasonsForClosure> reasonsForClosureRepo)
        {
            productQuoteRepository = productQuoteRepo;
            customerRepository = customerRepo;
            customerProductRepository = customerProductRepo;
            productQuoteService = productQuoteServ;
            dueDateReasonRepository = dueDateReasonRepo;
            reasonsForClosureRepository = reasonsForClosureRepo;
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        private class ProductQuoteStatusFilterItem
        {
            public string ProductQuoteStatusID { get; set; }
            public string ProductQuoteStatusDescription { get; set; }
        }

        private List<ProductQuoteStatusFilterItem> GetProductQuoteStatusToFilter()
        {
            return new List<ProductQuoteStatusFilterItem>() {
                new ProductQuoteStatusFilterItem { ProductQuoteStatusID = "Seleccione", ProductQuoteStatusDescription = "Seleccione Estado" },
                new ProductQuoteStatusFilterItem { ProductQuoteStatusID = "Abierta", ProductQuoteStatusDescription = "Abierta" },
                new ProductQuoteStatusFilterItem { ProductQuoteStatusID = "Cerrada", ProductQuoteStatusDescription = "Cerrada" },
                new ProductQuoteStatusFilterItem { ProductQuoteStatusID = "Enviada", ProductQuoteStatusDescription = "Enviada" },
            };
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> Index(int pq, int customerID, string sortOrder, string currentFilter, string searchString, string currentFechaDesde, string fechaDesde, string currentFechaHasta, string fechaHasta, string productQuoteStatusID, string currentProductQuoteStatusID, int? page)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;
            //Configuracion de la paginacion y el filtro
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.fechaDesde = currentFechaDesde;
            ViewBag.fechaHasta = currentFechaHasta;
            ViewBag.ProductQuoteStatusID = new SelectList(GetProductQuoteStatusToFilter(), "ProductQuoteStatusID", "ProductQuoteStatusDescription", productQuoteStatusID != null ? productQuoteStatusID : currentProductQuoteStatusID);
            ViewBag.SelectedProductQuoteStatusID = productQuoteStatusID != null ? productQuoteStatusID : currentProductQuoteStatusID;

            ViewBag.ProductQuoteCodeSortParm = String.IsNullOrEmpty(sortOrder) ? "ProductQuoteCode_asc" : "";
            ViewBag.UserFullNameSortParm = sortOrder == "UserFullName" ? "UserFullName_desc" : "UserFullName";
            ViewBag.CustomerCompanySortParm = sortOrder == "CustomerCompany" ? "CustomerCompany_desc" : "CustomerCompany";
            ViewBag.FechaSortParm = sortOrder == "Fecha" ? "Fecha_desc" : "Fecha";
            ViewBag.ProductSortParm = sortOrder == "Product" ? "Product_desc" : "Product";
            ViewBag.ProviderSortParm = sortOrder == "Provider" ? "Provider_desc" : "Provider";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            if (fechaDesde != null)
                page = 1;
            else
                fechaDesde = currentFechaDesde;


            ViewBag.CurrentFechaDesde = fechaDesde;
            ViewBag.CurrentFechaHasta = fechaHasta;

            //Recuperacion de los datos
            List<ProductQuote> productQuotes = await GetProductQuotes(searchString, fechaDesde, fechaHasta, pq, ViewBag.SelectedProductQuoteStatusID);


            //Mapeamos contra el ViewModel
            AutoMapper.Mapper.CreateMap<ProductQuote, ProductQuoteViewModel>();

            List<ProductQuoteViewModel> productQuotesViewModel = Mapper.Map<List<ProductQuote>, List<ProductQuoteViewModel>>(productQuotes);


            //Filtramos el resultado
            var result = from s in productQuotesViewModel
                         select s;


            if (customerID > 0)
            {
                result = result.Where(s => s.CustomerID == customerID);
            }

            switch (sortOrder)
            {
                case "ProductQuoteCode_asc":
                    result = result.OrderBy(s => s.ProductQuoteCode);
                    break;
                case "UserFullName":
                    result = result.OrderBy(s => s.UserFullName);
                    break;
                case "UserFullName_desc":
                    result = result.OrderByDescending(s => s.UserFullName);
                    break;
                case "CustomerCompany":
                    result = result.OrderBy(s => s.CustomerCompany);
                    break;
                case "CustomerCompany_desc":
                    result = result.OrderByDescending(s => s.CustomerCompany);
                    break;
                case "Fecha":
                    if (pq == 1)
                        result = result.OrderBy(s => s.DateQuote);
                    else
                        result = result.OrderBy(s => s.CustomerOrder.DateOrder);
                    break;
                case "Fecha_desc":
                    if (pq == 1)
                        result = result.OrderByDescending(s => s.DateQuote);
                    else
                        result = result.OrderByDescending(s => s.CustomerOrder.DateOrder);
                    break;
                case "Product":
                    result = result.OrderBy(s => s.ProductSingleName);
                    break;
                case "Product_desc":
                    result = result.OrderByDescending(s => s.ProductSingleName);
                    break;
                case "Provider":
                    result = result.OrderBy(s => s.ProductProviderName);
                    break;
                case "Provider_desc":
                    result = result.OrderByDescending(s => s.ProductProviderName);
                    break;
                default:  // ProductQuoteCode ascending 
                    //result = result.OrderByDescending(s => s.ProductQuoteCode);
                    result = result.OrderByDescending(s => s.DateQuote);
                    break;
            }

            int pageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ui_pageListSize"].ToString());
            int pageNumber = (page ?? 1);

            return View(result.ToPagedList(pageNumber, pageSize));
        }

        private async Task<List<ProductQuote>> GetProductQuotes(string search, string dateFrom, string dateTo, int pq, string productQuoteStatusID)
        {
            //Recuperacion de los datos
            List<ProductQuote> productQuotes = null;

            if (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SuperAdminUser"))
            {
                productQuotes = await productQuoteService.FindProductQuotesByAdminUserAsync(base.CurrentUserId, search, pq == 1, !string.IsNullOrEmpty(dateFrom) ? Convert.ToDateTime(dateFrom) : default(DateTime), !string.IsNullOrEmpty(dateTo) ? Convert.ToDateTime(dateTo) : default(DateTime));
            }
            else
            {
                if (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SellerUser") || userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "Admin"))
                {
                    productQuotes = await productQuoteService.FindProductQuotesBySellerUserAsync(base.CurrentUserId, search, pq == 1, !string.IsNullOrEmpty(dateFrom) ? Convert.ToDateTime(dateFrom) : default(DateTime), !string.IsNullOrEmpty(dateTo) ? Convert.ToDateTime(dateTo) : default(DateTime));
                }
                else
                {
                    productQuotes = await productQuoteService.FindProductQuotesByCustomerAndUserIDAsync(base.CurrentCustomerID, base.CurrentUserId, search, pq == 1, !string.IsNullOrEmpty(dateFrom) ? Convert.ToDateTime(dateFrom) : default(DateTime), !string.IsNullOrEmpty(dateTo) ? Convert.ToDateTime(dateTo) : default(DateTime));
                }
            }

            return productQuotes.Where(p => p.ProductQuoteStatus == productQuoteStatusID || string.IsNullOrEmpty(productQuoteStatusID) || productQuoteStatusID == "Seleccione").ToList();
        }


        public async Task<ActionResult> ExportToExcel()
        {
            var queryString = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);

            var searchString = queryString["SearchString"];
            var fechaDesde = queryString["FechaDesde"];
            var fechaHasta = queryString["FechaHasta"];
            var pq = int.Parse(queryString["pq"].ToString());
            var productQuoteStatusID = queryString["ProductQuoteStatusID"];

            //Recuperacion de los datos
            List<ProductQuote> productoQuotes = await GetProductQuotes(searchString, fechaDesde, fechaHasta, pq, productQuoteStatusID);

            if (productoQuotes.Count == 0)
            {
                var res = new { ok = false, data = "La lista no tiene elementos" };
                return Json(res);
            }

            var result = from s in productoQuotes
                         select new ProductQuoteReportViewModel(s);


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] response;
            using (var excelFile = new ExcelPackage())
            {
                excelFile.Workbook.Properties.Title = "Cotizaciones";
                var worksheet = excelFile.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"]
                    .LoadFromCollection(Collection: result, PrintHeaders: true);

                using (ExcelRange Rng = worksheet.Cells["A1:FZ1"])
                {
                    Rng.Style.Font.Bold = true;
                    //Rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                }

                //Eliminamos CustomerId
                worksheet.DeleteColumn(EpPlusExtension.ColumnLetterToColumnIndex("A"));

                string DateCellFormat = "dd/MM/yyyy";

                //Fecha Cotizacion
                using (ExcelRange Rng = worksheet.Cells["D2:D" + (result.Count() + 1).ToString()])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                //Fecha Entrega
                using (ExcelRange Rng = worksheet.Cells["E2:E" + (result.Count() + 1).ToString()])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                //Validez Precio
                using (ExcelRange Rng = worksheet.Cells["Q2:Q" + (result.Count() + 1).ToString()])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }
                //Fecha Cierra
                using (ExcelRange Rng = worksheet.Cells["CI2:CI" + (result.Count() + 1).ToString()])
                {
                    Rng.Style.Numberformat.Format = DateCellFormat;
                }

                //Ocultamos campos extras
                for (int i = 90; i < 166; i++) {
                    worksheet.Column(i).Hidden = true;
                }

                worksheet.Cells.AutoFitColumns();

                response = excelFile.GetAsByteArray();
            }

            // Save dialog appears through browser for user to save file as desired.
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Cotizaciones-" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm") + ".xlsx");
        }

        //Refactorizar
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public ActionResult Create(int? id)
        {
            ViewBag.CurrentUserFullName = base.CurrentUserFullName;
            ViewBag.CurrentUserEmail = base.CurrentUserEmail;

            if ((User.IsInRole("SellerUser")) && !(User.IsInRole("AdminUser")))
            {
                ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;

                ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                ViewBag.CurrentUserId = base.CurrentUserId;
            }
            else
            {
                if (User.IsInRole("AdminUser"))
                {
                    ViewBag.CurrentUserIsSellerUser = true;
                    ViewBag.CurrentUserEditGlobalVariables = true;
                    ViewBag.CurrentUserEditMarginOrPrice = true;
                    ViewBag.CurrentUserSeeCosting = true;

                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;

                }
                else
                {
                    ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                    ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                    ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                    ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;


                    Customer customer = customerRepository.FindCustomersByID(base.CurrentCustomerID);
                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;
                    
                    //ViewBag.CurrentUserFullName = base.CurrentUserFullName;
                    ViewBag.CurrentUserEmail = base.CurrentUserEmail;
                    ViewBag.CurrenCustomerName = customer != null ? customer.Company : "";
                    ViewBag.CurrentCustomerCompany = customer != null ? customer.Company : "";
                }
            }

            return View();
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public ActionResult CreateExpress(int? id)
        {
            ViewBag.CurrentUserFullName = base.CurrentUserFullName;
            ViewBag.CurrentUserEmail = base.CurrentUserEmail;

            if ((User.IsInRole("SellerUser")) && !(User.IsInRole("AdminUser")))
            {
                ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;

                ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                ViewBag.CurrentUserId = base.CurrentUserId;
            }
            else
            {
                if (User.IsInRole("AdminUser"))
                {
                    ViewBag.CurrentUserIsSellerUser = true;
                    ViewBag.CurrentUserEditGlobalVariables = true;
                    ViewBag.CurrentUserEditMarginOrPrice = true;
                    ViewBag.CurrentUserSeeCosting = true;

                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;

                }
                else
                {
                    ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                    ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                    ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                    ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;


                    Customer customer = customerRepository.FindCustomersByID(base.CurrentCustomerID);
                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;

                    //ViewBag.CurrentUserFullName = base.CurrentUserFullName;
                    ViewBag.CurrentUserEmail = base.CurrentUserEmail;
                    ViewBag.CurrenCustomerName = customer != null ? customer.Company : "";
                    ViewBag.CurrentCustomerCompany = customer != null ? customer.Company : "";
                }
            }

            return View();
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public ActionResult Edit(int? id)
        {
            ViewBag.CurrentUserFullName = base.CurrentUserFullName;
            ViewBag.CurrentUserEmail = base.CurrentUserEmail;
            ViewBag.ProductQuoteID = id;

            if ((User.IsInRole("SellerUser")) && !(User.IsInRole("AdminUser")))
            {
                ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;

                ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                ViewBag.CurrentUserId = base.CurrentUserId;
            }
            else
            {
                if (User.IsInRole("AdminUser"))
                {
                    ViewBag.CurrentUserIsSellerUser = true;
                    ViewBag.CurrentUserEditGlobalVariables = true;
                    ViewBag.CurrentUserEditMarginOrPrice = true;
                    ViewBag.CurrentUserSeeCosting = true;

                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;

                }
                else
                {
                    ViewBag.CurrentUserIsSellerUser = base.CurrentUserIsSellerUser;
                    ViewBag.CurrentUserEditGlobalVariables = base.CurrentUserEditGlobalVariables;
                    ViewBag.CurrentUserEditMarginOrPrice = base.CurrentUserEditMarginOrPrice;
                    ViewBag.CurrentUserSeeCosting = base.CurrentUserSeeCosting;


                    Customer customer = customerRepository.FindCustomersByID(base.CurrentCustomerID);
                    ViewBag.CurrentCustomerID = Newtonsoft.Json.JsonConvert.SerializeObject(base.CurrentCustomerID);
                    ViewBag.CurrentUserId = base.CurrentUserId;

                    ViewBag.CurrentUserEmail = base.CurrentUserEmail;
                    ViewBag.CurrenCustomerName = customer != null ? customer.Company : "";
                    ViewBag.CurrentCustomerCompany = customer != null ? customer.Company : "";
                }
            }

            return View();
        }

        // GET: ProductQuote/Details/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> Details(int pq, int customerID, int id)
        {
            ProductQuote productQuote;

            if (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SuperAdminUser"))
            {
                productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);
            }
            else
            {
                if ((userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "AdminUser")) || (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SellerUser")))
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDUserIDAsync(id, base.CurrentUserId);
                }
                else
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDCustomerAndUserIDAsync(id, base.CurrentCustomerID, base.CurrentUserId);
                }
            }

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (productQuote == null)
            {
                return HttpNotFound();
            }

            Boolean canShowCosteo = await customerProductRepository.CanShowCosteoAsync(productQuote.CustomerID, productQuote.ProductID);
            
            //Si no se puede ver el Costeo por Customer/Product, nos fijamos si el usuario logueado es CustomerAdminUser
            if (!canShowCosteo && User.IsInRole("CustomerAdminUser"))
            {
                canShowCosteo = true;
            }

            productQuote.CanShowCosteo = canShowCosteo;
            return View(new ProductQuoteViewModel(productQuote));
        }

        // GET: ProductQuote/DetailsExpress/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> DetailsExpress(int pq, int customerID, int id)
        {
            ProductQuote productQuote;

            if (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SuperAdminUser"))
            {
                productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);
            }
            else
            {
                if ((userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "AdminUser")) || (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SellerUser")))
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDUserIDAsync(id, base.CurrentUserId);
                }
                else
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDCustomerAndUserIDAsync(id, base.CurrentCustomerID, base.CurrentUserId);
                }
            }

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (productQuote == null)
            {
                return HttpNotFound();
            }

            Boolean canShowCosteo = await customerProductRepository.CanShowCosteoAsync(productQuote.CustomerID, productQuote.ProductID);

            //Si no se puede ver el Costeo por Customer/Product, nos fijamos si el usuario logueado es CustomerAdminUser
            if (!canShowCosteo && User.IsInRole("CustomerAdminUser"))
            {
                canShowCosteo = true;
            }

            productQuote.CanShowCosteo = canShowCosteo;
            return View(new ProductQuoteViewModel(productQuote));
        }


        // GET: ProductQuote/Delete/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> Delete(int pq, int customerID, int id)
        {
            ProductQuote productQuote;

            if (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SuperAdminUser"))
            {
                productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);
            }
            else
            {
                if ((userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "AdminUser")) || (userManager.IsInRole(HttpContext.User.Identity.GetUserId(), "SellerUser")))
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDUserIDAsync(id, base.CurrentUserId);
                }
                else
                {
                    productQuote = await productQuoteRepository.FindProductQuotesByIDCustomerAndUserIDAsync(id, base.CurrentCustomerID, base.CurrentUserId);
                }
            }

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (productQuote == null)
            {
                return HttpNotFound();
            }
            return View(productQuote);
        }

        // POST: ProductQuote/Delete/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int pq, int customerID, int id)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            try
            {
                await productQuoteService.DeleteAsync(id);
                return RedirectToAction("Index", new { pq = ViewBag.Pq, customerID = ViewBag.customerID });
            }
            catch (ProductQuoteAppException e)
            {
                ModelState.AddModelError("", e.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            ProductQuote productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);

            return View(productQuote);
        }

        // GET: ProductQuote/DueDateReason/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> DueDateReason(int pq, int customerID, int id)
        {
            ProductQuote productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (productQuote == null)
            {
                return HttpNotFound();
            }

            ViewBag.DueDateReasonID = new SelectList(dueDateReasonRepository.DueDateReasons(), "DueDateReasonID", "Description", productQuote.DueDateReasonID);

            return View(new ProductQuoteViewModel(productQuote));
        }

        // POST: ProductQuote/DueDateReason/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DueDateReason(int pq, int customerID, [Bind(Include = "ProductQuoteID, ProductQuoteCode, DueDateReasonID, Observations")] ProductQuoteViewModel productQuoteVM)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (ModelState.IsValid)
            {
                ProductQuote productQuote = null;
                productQuote = new ProductQuote
                {
                    ProductQuoteID = productQuoteVM.ProductQuoteID,
                    DueDateReasonID = productQuoteVM.DueDateReasonID,
                    Observations = productQuoteVM.Observations
                };
                await productQuoteRepository.UpdateDueDateReasonAsync(productQuote);
                return RedirectToAction("Index", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
            }

            ViewBag.DueDateReasonID = new SelectList(dueDateReasonRepository.DueDateReasons(), "DueDateReasonID", "Description", productQuoteVM.DueDateReasonID);

            return View(productQuoteVM);
        }

        // GET: ProductQuote/ReasonsForClosure/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> ReasonsForClosure(int pq, int customerID, int id)
        {
            ProductQuote productQuote = await productQuoteRepository.FindProductQuotesByIDAsync(id);

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (productQuote == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReasonsForClosureID = new SelectList(reasonsForClosureRepository.GetAll(), "ReasonsForClosureID", "Description", productQuote.ReasonsForClosureID);

            return View(new ProductQuoteReasonsForClosureViewModel(productQuote));
        }

        // POST: ProductQuote/ReasonsForClosure/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ReasonsForClosure(int pq, int customerID, [Bind(Include = "ProductQuoteID, ProductQuoteCode, ReasonsForClosureID, ClosureDate, ClosureObservations")] ProductQuoteReasonsForClosureViewModel productQuoteVM)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (ModelState.IsValid)
            {
                ProductQuote productQuote = null;
                productQuote = new ProductQuote
                {
                    ProductQuoteID = productQuoteVM.ProductQuoteID,
                    ReasonsForClosureID = productQuoteVM.ReasonsForClosureID,
                    ClosureDate = productQuoteVM.ClosureDate,
                    ClosureObservations = productQuoteVM.ClosureObservations
                };
                await productQuoteRepository.UpdateReasonsForClosureAsync(productQuote);
                return RedirectToAction("Index", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
            }

            ViewBag.ReasonsForClosureID = new SelectList(reasonsForClosureRepository.GetAll(), "ReasonsForClosureID", "Description", productQuoteVM.ReasonsForClosureID);

            return View(productQuoteVM);
        }

        // POST: ProductQuote/Sent/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [HttpGet, ActionName("Sent")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Sent(int pq, int customerID, int id)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (ModelState.IsValid)
            {
                ProductQuote productQuote = null;
                productQuote = new ProductQuote
                {
                    ProductQuoteID = id,
                    DateSent = DateTime.Now
                };
                await productQuoteRepository.UpdateDateSentAsync(productQuote);
                return RedirectToAction("Index", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (productQuoteRepository != null)
                {
                    productQuoteRepository.Dispose();
                    productQuoteRepository = null;
                }
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
                if (productQuoteService != null)
                {
                    productQuoteService.Dispose();
                    productQuoteService = null;
                }
                if (dueDateReasonRepository != null)
                {
                    dueDateReasonRepository.Dispose();
                    dueDateReasonRepository = null;
                }
                if (reasonsForClosureRepository != null)
                {
                    reasonsForClosureRepository.Dispose();
                    reasonsForClosureRepository = null;
                }

            }
            base.Dispose(disposing);
        }
    }
}