using AutoMapper;
using OfficeOpenXml;
using PagedList;
using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductQuoteApp.Controllers
{
    public class ProductController : BaseController
    {
        private IProductService productService = null;
        private IProviderRepository providerRepository = null;
        private IPackagingRepository packagingRepository = null;
        private IFreightTypeRepository freightTypeRepository = null;
        private ICurrencyTypeRepository currencyTypeRepository = null;
        private IProductDocumentService productDocumentService = null;
        private ISellerCompanyRepository sellerCompanyRepository = null;
        private IGenericRepository<IIBBTreatment> iibbTreatmentRepository = null;

        public ProductController(
            IProductService productServ, 
            IProviderRepository providerRepo, 
            IPackagingRepository packagingRepo, 
            IFreightTypeRepository freightTypeRepo, 
            ICurrencyTypeRepository currencyTypeRepo, 
            IProductDocumentService productDocumentServ, 
            ISellerCompanyRepository sellerCompanyRepo,
            IGenericRepository<IIBBTreatment> iibbTreatmentRepo)
        {
            productService = productServ;
            providerRepository = providerRepo;
            packagingRepository = packagingRepo;
            freightTypeRepository = freightTypeRepo;
            currencyTypeRepository = currencyTypeRepo;
            productDocumentService = productDocumentServ;
            sellerCompanyRepository = sellerCompanyRepo;
            iibbTreatmentRepository = iibbTreatmentRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page, bool active = true)
        {
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "ProductName_desc" : "";
            ViewBag.ProviderNameSortParm = sortOrder == "ProviderName" ? "ProviderName_desc" : "ProviderName";
            ViewBag.BrandNameSortParm = sortOrder == "BrandName" ? "BrandName_desc" : "BrandName";
            ViewBag.ValidityOfPriceSortParm = sortOrder == "ValidityOfPrice" ? "ValidityOfPrice_desc" : "ValidityOfPrice";
            ViewBag.PackagingSortName = sortOrder == "Packaging" ? "Packaging_desc" : "Packaging";
            ViewBag.Active = active;

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

            var products = await productService.FindProductsAsync();

            products = products.Where(s => s.Active == active);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => (
                                                (s.Name != null && s.Name.ToLower().Contains(searchString.ToLower()))
                                                ||
                                                (s.Brand != null && s.Brand.ToLower().Contains(searchString.ToLower()))
                                                ||
                                                (s.Packaging.Description != null && s.Packaging.Description.ToLower().Contains(searchString.ToLower()))
                                                || 
                                                (s.Provider.ProviderName != null && s.Provider.ProviderName.ToLower().Contains(searchString.ToLower()))
                                                )
                                         );
            }

            switch (sortOrder)
            {
                case "ProductName_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "ProviderName":
                    products = products.OrderBy(s => s.Provider.ProviderName);
                    break;
                case "ProviderName_desc":
                    products = products.OrderByDescending(s => s.Provider.ProviderName);
                    break;
                case "BrandName":
                    products = products.OrderBy(s => s.Brand);
                    break;
                case "BrandName_desc":
                    products = products.OrderByDescending(s => s.Brand);
                    break;
                case "ValidityOfPrice":
                    products = products.OrderBy(s => s.ValidityOfPrice);
                    break;
                case "ValidityOfPrice_desc":
                    products = products.OrderByDescending(s => s.ValidityOfPrice);
                    break;
                case "Packaging":
                    products = products.OrderBy(s => s.Packaging.Description);
                    break;
                case "Packaging_desc":
                    products = products.OrderByDescending(s => s.Packaging.Description);
                    break;
                default:  // Name ascending 
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            IPagedList ret = products.ToPagedList(pageNumber, pageSize);

            return View(ret);
        }

        // GET: Products/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            Product product = await productService.FindProductsByIDAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(new ProductViewModel(product));
        }

        //https://stackoverflow.com/questions/33903172/how-to-use-displayname-as-column-headers-with-loadfromcollection
        public async Task<ActionResult> ExportToExcel2()
        {
            var products = await productService.FindProductsAsync();
            var searchString = "";
            var pathQuery = Request.UrlReferrer.PathAndQuery;
            var indexSearch = pathQuery.IndexOf("SearchString=") > 0 ? pathQuery.IndexOf("SearchString=") : pathQuery.IndexOf("currentFilter=");
            var offset = pathQuery.IndexOf("SearchString=") > 0 ? 13 : 14;

            if (indexSearch > 0)
                searchString = pathQuery.Substring(indexSearch + offset, pathQuery.Length - indexSearch - offset);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => (
                                                    (s.Name != null && s.Name.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Brand != null && s.Brand.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Packaging.Description != null && s.Packaging.Description.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Provider.ProviderName != null && s.Provider.ProviderName.ToLower().Contains(searchString.ToLower()))
                                                )
                                         );
            }

            AutoMapper.Mapper.CreateMap<Product, ProductViewModel>();

            List<ProductViewModel> productsVM = Mapper.Map<List<Product>, List<ProductViewModel>>(products.ToList());


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] response;
            using (var excelFile = new ExcelPackage())
            {
                excelFile.Workbook.Properties.Title = "Productos";
                var worksheet = excelFile.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"]
                    .LoadFromCollection(Collection: productsVM, PrintHeaders: true);
                response = excelFile.GetAsByteArray();
            }

            // Save dialog appears through browser for user to save file as desired.
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Productos.xlsx");
        }

        public async Task<ActionResult> ExportToExcel()
        {
            var gv = new GridView();
            //var products = new List<Product>(productService.FindProducts());
            var products = await productService.FindProductsAsync();
            var searchString = "";
            var pathQuery = Request.UrlReferrer.PathAndQuery;
            var indexSearch = pathQuery.IndexOf("SearchString=") > 0 ? pathQuery.IndexOf("SearchString=") : pathQuery.IndexOf("currentFilter=");
            var offset = pathQuery.IndexOf("SearchString=") > 0 ? 13 : 14;

            if (indexSearch > 0)
                searchString = pathQuery.Substring(indexSearch + offset, pathQuery.Length - indexSearch - offset);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => (
                                                    (s.Name != null && s.Name.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Brand != null && s.Brand.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Packaging.Description != null && s.Packaging.Description.ToLower().Contains(searchString.ToLower()))
                                                    ||
                                                    (s.Provider.ProviderName != null && s.Provider.ProviderName.ToLower().Contains(searchString.ToLower()))
                                                )
                                         );
            }

            gv.DataSource = products;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            //return View("Index");
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName");
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description");
            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description");
            ViewBag.SellerCompanyID = new SelectList(sellerCompanyRepository.SellerCompanys(), "SellerCompanyID", "Name");
            ViewBag.IIBBTreatmentID = new SelectList(iibbTreatmentRepository.GetAll(), "IIBBTreatmentID", "Description");

            return View(new ProductViewModel() { ValidityOfPrice = DateTime.Now.AddDays(30), InOutStorage = true, Active = true });
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,Name,ProviderID,Brand,PackagingID,PositionKilogram,FCLKilogram,ValidityOfPrice,Active,Waste,FreightTypeID,Observations, ProductCost_DL, ProductCost_DLP, ProductCost_ISL, ProductCost_IND, ClientObservations, ProviderPaymentDeadline, LeadTime, BuyAndSellDirect, SellerCompanyID, IIBBTreatmentID, InOutStorage, AddToAllCustomer, MinimumMarginPercentage_DL, MinimumMarginPercentage_DLP, MinimumMarginPercentage_ISL, MinimumMarginPercentage_IND, MinimumMarginUSD_DL, MinimumMarginUSD_DLP, MinimumMarginUSD_ISL, MinimumMarginUSD_IND")] ProductViewModel productVM)
        {
            Product product = null;

            if (ModelState.IsValid)
            {
                product = new Product
                {
                    ProductID = productVM.ProductID,
                    Name = productVM.Name,
                    ProviderID = productVM.ProviderID,
                    Brand = productVM.Brand,
                    PackagingID = productVM.PackagingID,
                    PositionKilogram = productVM.PositionKilogram,
                    FCLKilogram = productVM.FCLKilogram == null ? 0 : productVM.FCLKilogram,
                    ValidityOfPrice = productVM.ValidityOfPrice,
                    Active = productVM.Active,
                    Waste = productVM.Waste,
                    FreightTypeID = productVM.FreightTypeID,
                    Observations = productVM.Observations,
                    ClientObservations = productVM.ClientObservations,
                    ProviderPaymentDeadline = productVM.ProviderPaymentDeadline,
                    LeadTime = productVM.LeadTime,
                    BuyAndSellDirect = productVM.BuyAndSellDirect,
                    SellerCompanyID = productVM.SellerCompanyID,
                    IIBBTreatmentID = productVM.IIBBTreatmentID,
                    InOutStorage = productVM.InOutStorage
                };

                await productService.CreateCompleteAsync(product, productVM.ProductCost_DL, productVM.ProductCost_DLP, productVM.ProductCost_ISL, productVM.ProductCost_IND, productVM.AddToAllCustomer, productVM.MinimumMarginPercentage_DL, productVM.MinimumMarginPercentage_DLP, productVM.MinimumMarginPercentage_ISL, productVM.MinimumMarginPercentage_IND, productVM.MinimumMarginUSD_DL, productVM.MinimumMarginUSD_DLP, productVM.MinimumMarginUSD_ISL, productVM.MinimumMarginUSD_IND);

                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName", productVM.ProviderID);
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description", productVM.PackagingID);
            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", productVM.FreightTypeID);
            ViewBag.SellerCompanyID = new SelectList(sellerCompanyRepository.SellerCompanys(), "SellerCompanyID", "Name", productVM.SellerCompanyID);
            ViewBag.IIBBTreatmentID = new SelectList(iibbTreatmentRepository.GetAll(), "IIBBTreatmentID", "Description", productVM.IIBBTreatmentID);

            return View(productVM);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            Product product = await productService.FindProductsByIDAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName", product.ProviderID);
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description", product.PackagingID);
            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", product.FreightTypeID);
            ViewBag.SellerCompanyID = new SelectList(sellerCompanyRepository.SellerCompanys(), "SellerCompanyID", "Name", product.SellerCompanyID);
            ViewBag.IIBBTreatmentID = new SelectList(iibbTreatmentRepository.GetAll(), "IIBBTreatmentID", "Description", product.IIBBTreatmentID);

            return View(new ProductViewModel(product));
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,ProviderID,Brand,PackagingID,PositionKilogram,FCLKilogram,ValidityOfPrice,Active,Waste,FreightTypeID,Observations,ProductCost_DL, ProductCost_DLP, ProductCost_ISL, ProductCost_IND, ClientObservations, ProviderPaymentDeadline, LeadTime, BuyAndSellDirect, SellerCompanyID, IIBBTreatmentID, InOutStorage, MinimumMarginPercentage_DL, MinimumMarginPercentage_DLP, MinimumMarginPercentage_ISL, MinimumMarginPercentage_IND, MinimumMarginUSD_DL, MinimumMarginUSD_DLP, MinimumMarginUSD_ISL, MinimumMarginUSD_IND")] ProductViewModel productVM, string saveAction, string saveAndQuitAction)
        {
            Product product = null;

            if (ModelState.IsValid)
            {
                product = new Product
                {
                    ProductID = productVM.ProductID,
                    Name = productVM.Name,
                    ProviderID = productVM.ProviderID,
                    Brand = productVM.Brand,
                    PackagingID = productVM.PackagingID,
                    PositionKilogram = productVM.PositionKilogram,
                    FCLKilogram = productVM.FCLKilogram == null ? 0 : productVM.FCLKilogram,
                    ValidityOfPrice = productVM.ValidityOfPrice,
                    Active = productVM.Active,
                    Waste = productVM.Waste,
                    FreightTypeID = productVM.FreightTypeID,
                    Observations = productVM.Observations,
                    ClientObservations = productVM.ClientObservations,
                    ProviderPaymentDeadline = productVM.ProviderPaymentDeadline,
                    LeadTime = productVM.LeadTime,
                    BuyAndSellDirect = productVM.BuyAndSellDirect,
                    SellerCompanyID = productVM.SellerCompanyID,
                    IIBBTreatmentID = productVM.IIBBTreatmentID,
                    InOutStorage = productVM.InOutStorage
                };

                await productService.UpdateCompleteAsync(product, productVM.ProductCost_DL, productVM.ProductCost_DLP, productVM.ProductCost_ISL, productVM.ProductCost_IND, productVM.MinimumMarginPercentage_DL, productVM.MinimumMarginPercentage_DLP, productVM.MinimumMarginPercentage_ISL, productVM.MinimumMarginPercentage_IND, productVM.MinimumMarginUSD_DL, productVM.MinimumMarginUSD_DLP, productVM.MinimumMarginUSD_ISL, productVM.MinimumMarginUSD_IND);

                if (!string.IsNullOrEmpty(saveAndQuitAction))
                {
                    //TempData["toast_type"] = "error";
                    //TempData["toast_msg"] = "Algo ha ocurrido y no Grabo Correctamente";
                    return RedirectToAction("Index");
                }
                if (!string.IsNullOrEmpty(saveAction))
                {
                    TempData["toast_type"] = "success";
                    TempData["toast_msg"] = "Grabo Correctamente";
                    return RedirectToAction("Edit", "Product", new { id = productVM.ProductID });
                }

                return RedirectToAction("Index");
            }

            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName", productVM.ProviderID);
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description", productVM.PackagingID);
            ViewBag.FreightTypeID = new SelectList(freightTypeRepository.FreightTypes(), "FreightTypeID", "Description", productVM.FreightTypeID);
            ViewBag.SellerCompanyID = new SelectList(sellerCompanyRepository.SellerCompanys(), "SellerCompanyID", "Name", productVM.SellerCompanyID);
            ViewBag.IIBBTreatmentID = new SelectList(iibbTreatmentRepository.GetAll(), "IIBBTreatmentID", "Description", productVM.IIBBTreatmentID);

            return View(productVM);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            Product product = await productService.FindProductsByIDAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpGet, ActionName("CreateCopy")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCopy(int id)
        {
            await productService.CreateCopyAsync(id);
            return RedirectToAction("Index");
        }

        // GET: ProductDocuments
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> ProductDocuments(int id)
        {
            var productDocuments = await productDocumentService.FindProductDocumentsByProductIDAsync(id);
            ViewBag.ProductID = id;
            return View(productDocuments);
        }

        // GET: Product/ProductDocumentsCreate
        [Authorize(Roles = "AdminUser")]
        public ActionResult ProductDocumentsCreate(int productID)
        {
            var productDocument = new ProductDocument();
            productDocument.ProductID = productID;
            return View(productDocument);
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductDocumentsCreate([Bind(Include = "ProductDocumentID,ProductID, Description")] ProductDocument productDocument, HttpPostedFileBase documentFiles)
        {
            if (ModelState.IsValid)
            {
                await productDocumentService.CreateAsync(productDocument, documentFiles);
                return RedirectToAction("ProductDocuments", "Product", new { id = productDocument.ProductID });
            }
            return View();
        }

        // GET: Product/ProductDocumentsDelete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> ProductDocumentsDelete(int id)
        {
            ProductDocument productDocument = await productDocumentService.FindProductDocumentsByIDAsync(id);
            if (productDocument == null)
            {
                return HttpNotFound();
            }
            return View(productDocument);
        }

        // POST: Product/ProductDocumentsDelete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("ProductDocumentsDelete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProductDocumentsDeleteConfirmed(int productID, int id)
        {
            await productDocumentService.DeleteAsync(id);
            return RedirectToAction("ProductDocuments", "Product", new { id = productID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (productService != null)
                {
                    productService.Dispose();
                    productService = null;
                }
                if (providerRepository != null)
                {
                    providerRepository.Dispose();
                    providerRepository = null;
                }
                if (packagingRepository != null)
                {
                    packagingRepository.Dispose();
                    packagingRepository = null;
                }
                if (freightTypeRepository != null)
                {
                    freightTypeRepository.Dispose();
                    freightTypeRepository = null;
                }
                if (currencyTypeRepository != null)
                {
                    currencyTypeRepository.Dispose();
                    currencyTypeRepository = null;
                }
                if (productDocumentService != null)
                {
                    productDocumentService.Dispose();
                    productDocumentService = null;
                }
                if (sellerCompanyRepository != null)
                {
                    sellerCompanyRepository.Dispose();
                    sellerCompanyRepository = null;
                }
                if (iibbTreatmentRepository != null)
                {
                    iibbTreatmentRepository.Dispose();
                    iibbTreatmentRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
