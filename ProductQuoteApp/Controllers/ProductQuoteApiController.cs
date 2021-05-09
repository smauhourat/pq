using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class ProductQuoteApiController : ApiController
    {
        private IProductQuoteService productQuoteService = null;
        private IProductQuoteRepository productQuoteRepository = null;
        private IProductRepository productRepository = null;
        private ISaleModalityRepository saleModalityRepository = null;
        private IGeographicAreaRepository geographicAreaRepository = null;
        private ISaleModalityProductRepository saleModalityProductRepository = null;
        private IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepository = null;
        private ICreditRatingPaymentDeadlineRepository creditRatingPaymentDeadlineRepository = null;
        private IExchangeTypeRepository exchangeTypeRepository = null;
        private IStockTimeRepository stockTimeRepository = null;
        private IDeliveryAmountRepository deliveryAmountRepository = null;
        private ICustomerProductRepository customerProductRepository = null;
        private ICustomerRepository customerRepository = null;
        private IGlobalVariableRepository globalVariableRepository = null;
        private ILogRecordRepository logRecordRepository = null;
        private ISalesChannelUserRepository salesChannelUserRepository = null;

        public ProductQuoteApiController(IProductQuoteService productQuoteServ, IProductQuoteRepository productQuoteRepo, IProductRepository productRepo, ISaleModalityRepository saleModalityRepo, IGeographicAreaRepository geographicAreaRepo, ISaleModalityProductRepository saleModalityProductRepo, IGeographicAreaTransportTypeRepository geographicAreaTransportTypeRepo, ICreditRatingPaymentDeadlineRepository creditRatingPaymentDeadlineRepo, IExchangeTypeRepository exchangeTypeRepo, IStockTimeRepository stockTimeRepo, IDeliveryAmountRepository deliveryAmountRepo, ICustomerProductRepository customerProductRepo, ICustomerRepository customerRepo, IGlobalVariableRepository globalVariableRepo, ILogRecordRepository logRecordRepo, ISalesChannelUserRepository salesChannelUserRepo)
        {
            productQuoteService = productQuoteServ;
            productQuoteRepository = productQuoteRepo;
            productRepository = productRepo;
            saleModalityRepository = saleModalityRepo;
            geographicAreaRepository = geographicAreaRepo;
            saleModalityProductRepository = saleModalityProductRepo;
            geographicAreaTransportTypeRepository = geographicAreaTransportTypeRepo;
            creditRatingPaymentDeadlineRepository = creditRatingPaymentDeadlineRepo;
            exchangeTypeRepository = exchangeTypeRepo;
            stockTimeRepository = stockTimeRepo;
            deliveryAmountRepository = deliveryAmountRepo;
            customerProductRepository = customerProductRepo;
            customerRepository = customerRepo;
            globalVariableRepository = globalVariableRepo;
            logRecordRepository = logRecordRepo;
            salesChannelUserRepository = salesChannelUserRepo;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetGlobalVariables")]
        public GlobalVariable GetGlobalVariables()
        {
            GlobalVariable globalVariable = globalVariableRepository.FindGlobalVariables();
            return globalVariable;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetCustomersByUser/{customerID}")]
        public List<CustomerFlatViewModel> GetCustomersByUser(int customerID)
        {
            var cList = customerRepository.FindCustomers();
            if (customerID > 0)
                cList = cList.Where(p => p.CustomerID == customerID);

            cList = cList.OrderByDescending(c => c.IsSpot).ThenBy(c => c.Company);
            
            var result = new List<CustomerFlatViewModel>();

            foreach (Customer c in cList)
            {
                result.Add(new CustomerFlatViewModel(c));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetCustomersSpotByUser/{customerID}")]
        public List<CustomerFlatViewModel> GetCustomersSpotByUser(int customerID)
        {
            var cList = customerRepository.FindCustomers();
            if (customerID > 0)
                cList = cList.Where(p => p.CustomerID == customerID);

            cList = cList.Where(c => c.IsSpot).OrderByDescending(c => c.Company);

            var result = new List<CustomerFlatViewModel>();

            foreach (Customer c in cList)
            {
                result.Add(new CustomerFlatViewModel(c));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetProductsActive/{customerID}")]
        public List<ProductFlatViewModel> GetProductsActive(int customerID)
        {
            var pList = productRepository.ProductsActive(customerID);
            var result = new List<ProductFlatViewModel>();

            foreach(Product p in pList)
            {
                result.Add(new ProductFlatViewModel(p));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetProductsDistribucionLocalActive/{customerID}")]
        public List<ProductFlatViewModel> GetProductsDistribucionLocalActive(int customerID)
        {
            var pList = productRepository.ProductsActive(customerID);
            var result = new List<ProductFlatViewModel>();

            foreach (Product p in pList)
            {
                if (p.SaleModalityProducts.Any(x => x.SaleModalityID == (int)EnumSaleModality.Local))
                    result.Add(new ProductFlatViewModel(p));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetShowCosteoByProduct/{customerID}/{productID}/{seeCosting}")]
        public Boolean GetShowCosteoByProduct(int customerID, int productID, int seeCosting)
        {
            var canShowCosteo = customerProductRepository.CanShowCosteo(customerID, productID);

            //Si no se puede ver el Costeo por Customer/Product, nos fijamos si el usuario logueado es CustomerAdminUser
            if (canShowCosteo == false)
            {
                if (User.IsInRole("CustomerAdminUser") == true)
                {
                    canShowCosteo = true;
                }
                if (User.IsInRole("SellerUser") && seeCosting == 1)
                {
                    canShowCosteo = true;
                }
            }
            return canShowCosteo;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetProductDetails/{id}")]
        public async Task<ProductFlatViewModel> GetProductDetails(int id)
        {
            var product = await productRepository.FindProductsByIDAsync(id);

            return new ProductFlatViewModel(product);
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [Route("GetSaleModalityDetails/{id}")]
        public async Task<SaleModalityViewModel> GetSaleModalityDetails(int id)
        {
            SaleModality sm = await saleModalityRepository.FindSaleModalityByIDAsync(id);

            return new SaleModalityViewModel(sm);
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetSaleModalitys")]
        public List<SaleModality> GetSaleModalitys()
        {
            var result = saleModalityRepository.SaleModalitys();

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetGeographicAreas")]
        public List<GeographicAreaViewModel> GetGeographicAreas()
        {
            var gaList = geographicAreaRepository.GeographicAreas().OrderBy(g => g.Name);
            var result = new List<GeographicAreaViewModel>();

            foreach (GeographicArea ga in gaList)
            {
                result.Add(new GeographicAreaViewModel(ga));
            }

            return result;
        }
        
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetGeographicAreasBySaleModality/{id}")]
        public List<GeographicAreaViewModel> GetGeographicAreasBySaleModality(int id)
        {
            var gaList = geographicAreaRepository.FindGeographicAreasBySaleModalityID(id).OrderBy(g => g.Name);
            var result = new List<GeographicAreaViewModel>();

            foreach (GeographicArea ga in gaList)
            {
                result.Add(new GeographicAreaViewModel(ga));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetExchangeType")]
        public List<ExchangeType> GetExchangeType()
        {
            var result = exchangeTypeRepository.ExchangeTypes();

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetExchangeTypeBySaleModality/{id}")]
        public List<ExchangeType> GetExchangeTypeBySaleModality(int id)
        {
            var result = exchangeTypeRepository.FindExchangeTypesBySaleModalityID(id);

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetMaximumMonthsStock")]
        public List<StockTime> GetMaximumMonthsStock()
        {
            var result = stockTimeRepository.StockTimes();

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetMaximumMonthsStockBySaleModality/{id}")]
        public List<StockTime> GetMaximumMonthsStockBySaleModality(int id)
        {
            var result = stockTimeRepository.FindStockTimesBySaleModalityID(id);

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetDeliveryAmount")]
        public List<DeliveryAmount> GetDeliveryAmount()
        {
            var result = deliveryAmountRepository.DeliveryAmounts();

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetDeliveryAmountsBySaleModality/{id}")]
        public List<DeliveryAmountViewModel> GetDeliveryAmountsBySaleModality(int id)
        {
            var daList = deliveryAmountRepository.FindDeliveryAmountsBySaleModalityID(id);
            var result = new List<DeliveryAmountViewModel>();

            foreach (DeliveryAmount da in daList)
            {
                result.Add(new DeliveryAmountViewModel(da));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetSalesModalityByProduct/{id}")]
        public List<SaleModalityViewModel> GetSalesModalityByProduct(int id)
        {
            var smList= saleModalityProductRepository.FindSaleModalityByProduct(id);
            var result = new List<SaleModalityViewModel>();

            foreach(SaleModality sm in smList)
            {
                result.Add(new Models.SaleModalityViewModel(sm));
            }

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetTransportTypesByGeographicArea/{id}")]
        public List<Persistence.TransportType> GetTransportTypesByGeographicArea(int id)
        {
            var result = geographicAreaTransportTypeRepository.FindTransportTypeByGeographicArea(id);

            return result;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetPaymentDeadlineByCreditRating/{id}")]
        public List<PaymentDeadlineViewModel> GetPaymentDeadlineByCreditRating(int id)
        {
            var pdList = creditRatingPaymentDeadlineRepository.FindPaymentDeadlineByCreditRating(id);
            var result = new List<PaymentDeadlineViewModel>();

            foreach(PaymentDeadline pd in pdList)
            {
                result.Add(new PaymentDeadlineViewModel(pd));
            }

            return result;
        }

        private void LogProductQuoteCalc(ProductQuote productQuote, string messageTitle)
        {
            var jsonProductQuote = Newtonsoft.Json.JsonConvert.SerializeObject(productQuote);

            var log = new LogRecord
            {
                LogLevel = LogLevel.Information,
                ShortMessage = messageTitle + " - (Cliente: " + productQuote.CustomerName + " - Producto: " + productQuote.ProductName + ")",
                FullMessage = jsonProductQuote,
                CreatedOnUtc = DateTime.Now
            };
            logRecordRepository.Create(log);
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("CalculateProductQuote")]
        public ProductQuote CalculateProductQuote(ProductQuote productQuote)
        {
            LogProductQuoteCalc(productQuote, "Calculo Cotizacion - PRE");

            productQuoteService.CalculateQuote(productQuote);

            LogProductQuoteCalc(productQuote, "Calculo Cotizacion - POST");

            return productQuote;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("CreateProductQuote")]
        public IHttpActionResult CreateProductQuote(ProductQuote productQuote)
        {
            if (ModelState.IsValid)
            {
                Boolean sendNotifications = globalVariableRepository.FindGlobalVariables().EnvioCotizacionPorMail;
                Boolean sendUserCreatorNotifications = globalVariableRepository.FindGlobalVariables().EnvioCotizacionUsuarioCreadorPorMail;
                Boolean sendClientNotifications = customerRepository.FindCustomersByID(productQuote.CustomerID).SendNotifications;
                if (User.IsInRole("SellerUser") && !(User.IsInRole("AdminUser")))
                {
                    sendClientNotifications = true;
                }
                if (User.IsInRole("AdminUser"))
                {
                    sendClientNotifications = false;
                }
                productQuote.ProductQuotePDFFooter = System.Configuration.ConfigurationManager.AppSettings["ProductQuotePDFFooter"].ToString();
                productQuote.ProductQuoteSmallPDFFooter = System.Configuration.ConfigurationManager.AppSettings["ProductQuoteSmallPDFFooter"].ToString();
                productQuoteService.CreateQuote(productQuote.CustomerContactMail, productQuote, sendNotifications, sendClientNotifications, sendUserCreatorNotifications);

                var res = new { ok = true, data = productQuote, returnUrl = "../ProductQuote/Details/" + productQuote.ProductQuoteID.ToString() + "?pq=1&customerID=0" };
                return Json(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("CreateProductQuoteExpress")]
        public IHttpActionResult CreateProductQuoteExpress(ProductQuote productQuote)
        {
            if (ModelState.IsValid)
            {
                Boolean sendNotifications = globalVariableRepository.FindGlobalVariables().EnvioCotizacionPorMail;
                Boolean sendUserCreatorNotifications = globalVariableRepository.FindGlobalVariables().EnvioCotizacionUsuarioCreadorPorMail;
                Boolean sendClientNotifications = customerRepository.FindCustomersByID(productQuote.CustomerID).SendNotifications;
                if (User.IsInRole("SellerUser") && !(User.IsInRole("AdminUser")))
                {
                    sendClientNotifications = true;
                }
                if (User.IsInRole("AdminUser"))
                {
                    sendClientNotifications = false;
                }
                productQuote.ProductQuotePDFFooter = System.Configuration.ConfigurationManager.AppSettings["ProductQuotePDFFooter"].ToString();
                productQuote.ProductQuoteSmallPDFFooter = System.Configuration.ConfigurationManager.AppSettings["ProductQuoteSmallPDFFooter"].ToString();
                productQuoteService.CreateQuote(productQuote.CustomerContactMail, productQuote, sendNotifications, sendClientNotifications, sendUserCreatorNotifications);

                var res = new { ok = true, data = productQuote, returnUrl = "../ProductQuote/DetailsExpress/" + productQuote.ProductQuoteID.ToString() + "?pq=1&customerID=0" };
                return Json(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("CreateCustomerOrder")]
        public HttpResponseMessage CreateCustomerOrder(ProductQuote productQuote)
        {
            if (ModelState.IsValid)
            {
                //Si el usuario es un Vendedor Interno, entonces se crea la Cotizacion APROBADA
                productQuoteService.CreateCustomerOrder(null, productQuote, User.IsInRole("SellerUser"));

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetProductQuote/{productQuoteID}")]
        public ProductQuote GetProductQuote(int productQuoteID)
        {
            var productQuote = productQuoteRepository.FindProductQuotesByID(productQuoteID);

            return productQuote;
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [System.Web.Http.Route("GetSalesChannelsByUser/{userID}")]
        public async Task<List<SalesChannelUserSingleViewModel>> GetSalesChannelsByUser(string userID)
        {
            List<SalesChannelUserSingleViewModel> cpList = new List<SalesChannelUserSingleViewModel>();
            var result = await salesChannelUserRepository.FindSalesChannelsByUserIDAsync(userID);

            foreach (SalesChannelUser item in result)
            {
                cpList.Add(new Models.SalesChannelUserSingleViewModel(item));
            }
            return cpList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (productQuoteService != null)
                {
                    productQuoteService.Dispose();
                    productQuoteService = null;
                }
                if (productQuoteRepository != null)
                {
                    productQuoteRepository.Dispose();
                    productQuoteRepository = null;
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
                if (saleModalityProductRepository != null)
                {
                    saleModalityProductRepository.Dispose();
                    saleModalityProductRepository = null;
                }
                if (geographicAreaTransportTypeRepository != null)
                {
                    geographicAreaTransportTypeRepository.Dispose();
                    geographicAreaTransportTypeRepository = null;
                }
                if (creditRatingPaymentDeadlineRepository != null)
                {
                    creditRatingPaymentDeadlineRepository.Dispose();
                    creditRatingPaymentDeadlineRepository = null;
                }
                if (exchangeTypeRepository != null)
                {
                    exchangeTypeRepository.Dispose();
                    exchangeTypeRepository = null;
                }
                if (stockTimeRepository != null)
                {
                    stockTimeRepository.Dispose();
                    stockTimeRepository = null;
                }
                if (deliveryAmountRepository != null)
                {
                    deliveryAmountRepository.Dispose();
                    deliveryAmountRepository = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
                if (customerRepository != null)
                {
                    customerRepository.Dispose();
                    customerRepository = null;
                }
                if (globalVariableRepository != null)
                {
                    globalVariableRepository.Dispose();
                    customerRepository = null;
                }
                if (logRecordRepository != null)
                {
                    logRecordRepository.Dispose();
                    logRecordRepository = null;
                }
                if (salesChannelUserRepository != null)
                {
                    salesChannelUserRepository.Dispose();
                    salesChannelUserRepository = null;
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
