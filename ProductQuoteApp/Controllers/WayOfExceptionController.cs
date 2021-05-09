using PagedList;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class WayOfExceptionController : BaseController
    {
        private IWayOfExceptionRepository wayOfExceptionRepository = null;
        private ICustomerRepository customerRepository = null;
        private IProductRepository productRepository = null;
        private ISaleModalityRepository saleModalityRepository = null;
        private IGeographicAreaRepository geographicAreaRepository = null;
        private IPaymentDeadlineRepository paymentDeadlineRepository = null;
        private IExchangeTypeRepository exchangeTypeRepository = null;
        private IProductQuoteService productQuoteService = null;
        private IDeliveryAmountRepository deliveryAmountRepository = null;
        private IStockTimeRepository stockTimeRepository = null;
        private IGlobalVariableRepository globalVariableRepository = null;

        public WayOfExceptionController(IWayOfExceptionRepository wayOfExceptionRepo, 
            ICustomerRepository customerRepo, 
            IProductRepository productRepo, 
            ISaleModalityRepository saleModalityRepo, 
            IGeographicAreaRepository geographicAreaRepo, 
            IPaymentDeadlineRepository paymentDeadlineRepo, 
            IExchangeTypeRepository exchangeTypeRepo, 
            IProductQuoteService productQuoteServ, 
            IDeliveryAmountRepository deliveryAmountRepo, 
            IStockTimeRepository stockTimeRepo, 
            IGlobalVariableRepository globalVariableRepo)
        {
            wayOfExceptionRepository = wayOfExceptionRepo;
            customerRepository = customerRepo;
            productRepository = productRepo;
            saleModalityRepository = saleModalityRepo;
            geographicAreaRepository = geographicAreaRepo;
            paymentDeadlineRepository = paymentDeadlineRepo;
            exchangeTypeRepository = exchangeTypeRepo;
            productQuoteService = productQuoteServ;
            deliveryAmountRepository = deliveryAmountRepo;
            stockTimeRepository = stockTimeRepo;
            globalVariableRepository = globalVariableRepo;
        }

        [Authorize(Roles = "AdminUser")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CustomerNameSortParm = String.IsNullOrEmpty(sortOrder) ? "CustomerName_desc" : "";

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

            var wayOfExceptions = wayOfExceptionRepository.FindWayOfExceptionsFilter(searchString, "");

            switch (sortOrder)
            {
                case "CustomerName_desc":
                    wayOfExceptions = wayOfExceptions.OrderByDescending(s => s.Customer.Company);
                    break;
                default:  // Name ascending 
                    wayOfExceptions = wayOfExceptions.OrderBy(s => s.Customer.Company);
                    break;
            }

            IPagedList ret = wayOfExceptions.ToPagedList(pageNumber, pageSize);

            return View(ret);
        }

        // GET: WayOfException/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            WayOfException wayOfException = await wayOfExceptionRepository.FindWayOfExceptionsByIDAsync(id);
            if (wayOfException == null)
            {
                return HttpNotFound();
            }
            return View(wayOfException);
        }

        [Authorize(Roles = "AdminUser")]
        //[HttpPost]
        public ActionResult CreateWhitDefault(ProductQuote productQuote)
        {
            ViewBag.CustomerID = new SelectList(customerRepository.FindCustomers(), "CustomerID", "Company");
            ViewBag.ProductID = new SelectList(productRepository.Products(), "ProductID", "FullName");
            ViewBag.SaleModalityID = new SelectList(saleModalityRepository.SaleModalitys(), "SaleModalityID", "Description");
            ViewBag.GeographicAreaID = new SelectList(geographicAreaRepository.GeographicAreas(), "GeographicAreaID", "Name");
            ViewBag.PaymentDeadlineID = new SelectList(paymentDeadlineRepository.PaymentDeadlines(), "PaymentDeadlineID", "Description");
            ViewBag.ExchangeTypeID = new SelectList(exchangeTypeRepository.ExchangeTypes(), "ExchangeTypeID", "Description");
            ViewBag.DeliveryAmount = new SelectList(deliveryAmountRepository.DeliveryAmounts(), "DeliveryAmountID", "Description");
            ViewBag.MaximumMonthsStock = new SelectList(stockTimeRepository.StockTimes(), "StockTimeID", "Description");

            HttpContext.RewritePath("/WayOfException/CreateWhitDefault");

            return View();
        }

        private string IsValidProductQuoteInput(WayOfException wayOfException)
        {
            ProductQuote productQuote = new ProductQuote
            {
                ProductID = wayOfException.ProductID,
                CustomerID = wayOfException.CustomerID, 
                SaleModalityID = wayOfException.SaleModalityID,
                QuantityOpenPurchaseOrder = wayOfException.QuantityOpenPurchaseOrder,
                DeliveryAmount = wayOfException.DeliveryAmount,
                MinimumQuantityDelivery = wayOfException.QuantityOpenPurchaseOrder / wayOfException.DeliveryAmount,
                MaximumMonthsStock = wayOfException.MaximumMonthsStock
            };

            GlobalVariable globalVariable = globalVariableRepository.FindGlobalVariables();
            productQuote.GVD_CostoAlmacenamientoMensual = globalVariable.CostoAlmacenamientoMensual;
            productQuote.GVD_CostoInOut = globalVariable.CostoInOut;
            productQuote.GVD_CostoFinancieroMensual = globalVariable.CostoFinancieroMensual;
            productQuote.GVD_ImpuestoDebitoCredito = globalVariable.ImpuestoDebitoCredito;
            productQuote.GVD_GastosFijos = globalVariable.GastosFijos;
            productQuote.GVD_IIBBAlicuota = globalVariable.IIBBAlicuota;
            productQuote.GVD_TipoCambio = globalVariable.TipoCambio;
            productQuote.GVD_FactorCostoAlmacenamientoMensual = globalVariable.FactorCostoAlmacenamientoMensual;
            productQuote.GVD_DiasStockPromedioDistLocal = globalVariable.DiasStockPromedioDistLocal;


            productQuote.GV_CostoAlmacenamientoMensual = productQuote.GVD_CostoAlmacenamientoMensual;
            productQuote.GV_CostoFinancieroMensual = productQuote.GVD_CostoFinancieroMensual;
            productQuote.GV_FactorCostoAlmacenamientoMensual = productQuote.GVD_FactorCostoAlmacenamientoMensual;
            productQuote.GV_DiasStockPromedioDistLocal = productQuote.GVD_DiasStockPromedioDistLocal;


            if (!productQuoteService.isValidProductQuoteInput(productQuote))
                return productQuote.Message;
            return string.Empty;
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(customerRepository.FindCustomers(), "CustomerID", "Company");
            ViewBag.ProductID = new SelectList(productRepository.Products(), "ProductID", "FullName");
            ViewBag.SaleModalityID = new SelectList(saleModalityRepository.SaleModalitys(), "SaleModalityID", "Description");
            ViewBag.GeographicAreaID = new SelectList(geographicAreaRepository.GeographicAreas(), "GeographicAreaID", "Name");
            ViewBag.PaymentDeadlineID = new SelectList(paymentDeadlineRepository.PaymentDeadlines(), "PaymentDeadlineID", "Description");
            ViewBag.ExchangeTypeID = new SelectList(exchangeTypeRepository.ExchangeTypes(), "ExchangeTypeID", "Description");
            ViewBag.DeliveryAmount = new SelectList(deliveryAmountRepository.DeliveryAmounts(), "DeliveryAmountID", "Description");
            ViewBag.MaximumMonthsStock = new SelectList(stockTimeRepository.StockTimes(), "StockTimeID", "Description");

            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WayOfExceptionID,CustomerID,ProductID,SaleModalityID,IsSaleModalitySearchParam,GeographicAreaID,IsGeographicAreaSearchParam,PaymentDeadlineID,IsPaymentDeadlineSearchParam,QuantityOpenPurchaseOrder,IsQuantityOpenPurchaseOrderSearchParam,DeliveryAmount,IsDeliveryAmountSearchParam,MaximumMonthsStock,IsMaximumMonthsStockSearchParam,ExchangeTypeID,IsExchangeTypeSearchParam,ExceptionPrice,ExceptionApplyType,IsMinimumQuantityDeliverySearchParam")] WayOfException wayOfException)
        {
            string resultValid = IsValidProductQuoteInput(wayOfException);
            if (resultValid != string.Empty)
                ModelState.AddModelError(string.Empty, resultValid);

            if (ModelState.IsValid)
            {
                await wayOfExceptionRepository.CreateAsync(wayOfException);

                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(customerRepository.FindCustomers(), "CustomerID", "Company", wayOfException.CustomerID);
            ViewBag.ProductID = new SelectList(productRepository.Products(), "ProductID", "FullName", wayOfException.ProductID);
            ViewBag.SaleModalityID = new SelectList(saleModalityRepository.SaleModalitys(), "SaleModalityID", "Description", wayOfException.SaleModalityID);
            ViewBag.GeographicAreaID = new SelectList(geographicAreaRepository.GeographicAreas(), "GeographicAreaID", "Name", wayOfException.GeographicAreaID);
            ViewBag.PaymentDeadlineID = new SelectList(paymentDeadlineRepository.PaymentDeadlines(), "PaymentDeadlineID", "Description", wayOfException.PaymentDeadlineID);
            ViewBag.ExchangeTypeID = new SelectList(exchangeTypeRepository.ExchangeTypes(), "ExchangeTypeID", "Description", wayOfException.ExchangeTypeID);
            ViewBag.DeliveryAmount = new SelectList(deliveryAmountRepository.DeliveryAmounts(), "DeliveryAmountID", "Description", wayOfException.DeliveryAmount);
            ViewBag.MaximumMonthsStock = new SelectList(stockTimeRepository.StockTimes(), "StockTimeID", "Description", wayOfException.MaximumMonthsStock);

            return View(wayOfException);
        }

        // GET: WayOfException/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            WayOfException wayOfException = await wayOfExceptionRepository.FindWayOfExceptionsByIDAsync(id);
            if (wayOfException == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerID = new SelectList(customerRepository.FindCustomers(), "CustomerID", "Company", wayOfException.CustomerID);
            ViewBag.ProductID = new SelectList(productRepository.Products(), "ProductID", "FullName", wayOfException.ProductID);
            ViewBag.SaleModalityID = new SelectList(saleModalityRepository.SaleModalitys(), "SaleModalityID", "Description", wayOfException.SaleModalityID);
            ViewBag.GeographicAreaID = new SelectList(geographicAreaRepository.GeographicAreas(), "GeographicAreaID", "Name", wayOfException.GeographicAreaID);
            ViewBag.PaymentDeadlineID = new SelectList(paymentDeadlineRepository.PaymentDeadlines(), "PaymentDeadlineID", "Description", wayOfException.PaymentDeadlineID);
            ViewBag.ExchangeTypeID = new SelectList(exchangeTypeRepository.ExchangeTypes(), "ExchangeTypeID", "Description", wayOfException.ExchangeTypeID);
            ViewBag.DeliveryAmount = new SelectList(deliveryAmountRepository.DeliveryAmounts(), "DeliveryAmountID", "Description", wayOfException.DeliveryAmount);
            ViewBag.MaximumMonthsStock = new SelectList(stockTimeRepository.StockTimes(), "StockTimeID", "Description", wayOfException.MaximumMonthsStock);

            return View(wayOfException);
        }

        // POST: WayOfException/Edit/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WayOfExceptionID,CustomerID,ProductID,SaleModalityID,IsSaleModalitySearchParam,GeographicAreaID,IsGeographicAreaSearchParam,PaymentDeadlineID,IsPaymentDeadlineSearchParam,QuantityOpenPurchaseOrder,IsQuantityOpenPurchaseOrderSearchParam,DeliveryAmount,IsDeliveryAmountSearchParam,MaximumMonthsStock,IsMaximumMonthsStockSearchParam,ExchangeTypeID,IsExchangeTypeSearchParam,ExceptionPrice,ExceptionApplyType,IsMinimumQuantityDeliverySearchParam")] WayOfException wayOfException)
        {
            string resultValid = IsValidProductQuoteInput(wayOfException);
            if (resultValid != string.Empty)
                ModelState.AddModelError(string.Empty, resultValid);

            if (ModelState.IsValid)
            {
                await wayOfExceptionRepository.UpdateAsync(wayOfException);

                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(customerRepository.FindCustomers(), "CustomerID", "Company", wayOfException.CustomerID);
            ViewBag.ProductID = new SelectList(productRepository.Products(), "ProductID", "FullName", wayOfException.ProductID);
            ViewBag.SaleModalityID = new SelectList(saleModalityRepository.SaleModalitys(), "SaleModalityID", "Description", wayOfException.SaleModalityID);
            ViewBag.GeographicAreaID = new SelectList(geographicAreaRepository.GeographicAreas(), "GeographicAreaID", "Name", wayOfException.GeographicAreaID);
            ViewBag.PaymentDeadlineID = new SelectList(paymentDeadlineRepository.PaymentDeadlines(), "PaymentDeadlineID", "Description", wayOfException.PaymentDeadlineID);
            ViewBag.ExchangeTypeID = new SelectList(exchangeTypeRepository.ExchangeTypes(), "ExchangeTypeID", "Description", wayOfException.ExchangeTypeID);
            ViewBag.DeliveryAmount = new SelectList(deliveryAmountRepository.DeliveryAmounts(), "DeliveryAmountID", "Description", wayOfException.DeliveryAmount);
            ViewBag.MaximumMonthsStock = new SelectList(stockTimeRepository.StockTimes(), "StockTimeID", "Description", wayOfException.MaximumMonthsStock);

            return View(wayOfException);
        }

        // GET: WayOfException/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            WayOfException wayOfException = await wayOfExceptionRepository.FindWayOfExceptionsByIDAsync(id);
            if (wayOfException == null)
            {
                return HttpNotFound();
            }
            return View(wayOfException);
        }

        // POST: WayOfException/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await wayOfExceptionRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (wayOfExceptionRepository != null)
                {
                    wayOfExceptionRepository.Dispose();
                    wayOfExceptionRepository = null;
                }
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }
                if (productRepository != null)
                {
                    productRepository.Dispose();
                    productRepository = null;
                }
                if (saleModalityRepository != null)
                {
                    saleModalityRepository.Dispose();
                    saleModalityRepository = null;
                }
                if (geographicAreaRepository != null)
                {
                    geographicAreaRepository.Dispose();
                    geographicAreaRepository = null;
                }
                if (paymentDeadlineRepository != null)
                {
                    paymentDeadlineRepository.Dispose();
                    paymentDeadlineRepository = null;
                }
                if (exchangeTypeRepository != null)
                {
                    exchangeTypeRepository.Dispose();
                    exchangeTypeRepository = null;
                }
                if (productQuoteService != null)
                {
                    productQuoteService.Dispose();
                    productQuoteService = null;
                }
                if (deliveryAmountRepository != null)
                {
                    deliveryAmountRepository.Dispose();
                    deliveryAmountRepository = null;
                }
                if (stockTimeRepository != null)
                {
                    stockTimeRepository.Dispose();
                    stockTimeRepository = null;
                }
                if (globalVariableRepository != null)
                {
                    globalVariableRepository.Dispose();
                    globalVariableRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}