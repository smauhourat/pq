using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ProductWithCostController : BaseController
    {
        private IProductService productService = null;
        private IProviderRepository providerRepository = null;
        private IPackagingRepository packagingRepository = null;
        private ICurrencyTypeRepository currencyTypeRepository = null;
        private IProductDocumentService productDocumentService = null;

        public ProductWithCostController(IProductService productServ, 
            IProviderRepository providerRepo, 
            IPackagingRepository packagingRepo, 
            ICurrencyTypeRepository currencyTypeRepo, 
            IProductDocumentService productDocumentServ)
        {
            productService = productServ;
            providerRepository = providerRepo;
            packagingRepository = packagingRepo;
            currencyTypeRepository = currencyTypeRepo;
            productDocumentService = productDocumentServ;
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var products = await productService.FindProductsAsync();
            return View(products.ToList());
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

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName");
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description");
            
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,Name,ProviderID,Brand,PackagingID,PositionKilogram,FCLKilogram,ValidityOfPrice,Active,MinimumMarginPercentage,MinimumMarginUSD,Observations")] Product product)
        {
            if (ModelState.IsValid)
            {
                await productService.CreateAsync(product);
            }
            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName", product.ProviderID);
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description", product.PackagingID);

            return RedirectToAction("Index");
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

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,Name,ProviderID,Brand,PackagingID,PositionKilogram,FCLKilogram,ValidityOfPrice,QuoteToRevision,MinimumMarginPercentage,MinimumMarginUSD,Observations")] Product product)
        {
            if (ModelState.IsValid)
            {
                await productService.UpdateAsync(product);
                return RedirectToAction("Index");
            }
            ViewBag.ProviderID = new SelectList(providerRepository.Providers(), "ProviderID", "ProviderName", product.ProviderID);
            ViewBag.PackagingID = new SelectList(packagingRepository.Packagings(), "PackagingID", "Description", product.PackagingID);

            return View(product);
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
                return RedirectToAction("Index");
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
        public async Task<ActionResult> ProductDocumentsDeleteConfirmed(int id)
        {
            await productDocumentService.DeleteAsync(id);
            return RedirectToAction("Index");
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
            }
            base.Dispose(disposing);
        }
    }
}
