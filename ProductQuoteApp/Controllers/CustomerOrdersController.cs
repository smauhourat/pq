using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{

    public class CustomerOrdersController : BaseController
    {
        private ICustomerOrderRepository customerOrderRepository = null;
        private ICustomerOrderService customerOrderService = null;
        private IShipmentTrackingRepository shipmentTrackingRepository = null;
        private IProductQuoteRepository productQuoteRepository = null;
        private ICustomerProductRepository customerProductRepository = null;

        public CustomerOrdersController(ICustomerOrderRepository customerOrderRepo, 
            IShipmentTrackingRepository shipmentTrackingRepo, 
            ICustomerOrderService customerOrderServ, 
            IProductQuoteRepository productQuoteRepo, 
            ICustomerProductRepository customerProductRepo)
        {
            customerOrderRepository = customerOrderRepo;
            shipmentTrackingRepository = shipmentTrackingRepo;
            customerOrderService = customerOrderServ;
            productQuoteRepository = productQuoteRepo;
            customerProductRepository = customerProductRepo;
        }

        // GET: CustomerOrders
        [Authorize(Roles = "AdminUser, SellerUser")]
        public async Task<ActionResult> Index()
        {
            var customerOrders = await customerOrderRepository.FindCustomerOrdersByCustomerIDAsync(base.CurrentCustomerID);
            if (customerOrders == null)
            {
                return HttpNotFound();
            }

            return View(customerOrders.ToList());
        }

        private bool IsValidProductQuote(int productQuoteID)
        {
            ProductQuote pq = productQuoteRepository.FindProductQuotesByID(productQuoteID);
            if (pq.ProductValidityOfPrice < DateTime.Now.Date)
                return false;
            return true;
        }

        // GET: CustomerOrders/Create/id
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public ActionResult Create(int pq, int customerID, int productQuoteID)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            CustomerOrder customerOrder = new CustomerOrder();
            customerOrder.DateOrder = DateTime.Today;
            return View(customerOrder);
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int pq, int customerID, [Bind(Include = "ProductQuoteID,DateOrder")] CustomerOrder customerOrder)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (ModelState.IsValid)
            {
                if (!IsValidProductQuote(customerOrder.ProductQuoteID))
                {
                    ModelState.AddModelError("", "La Cotización esta fuera de término");
                    return View(customerOrder);
                }

                await customerOrderRepository.CreateAsync(customerOrder);

                ShipmentTracking shipmentTracking = shipmentTrackingRepository.FindShipmentTrackingByProductQuoteID(customerOrder.ProductQuoteID);

                shipmentTracking.CustomerOrderEstimatedDate = customerOrder.DateOrder;
                shipmentTracking.CustomerOrderRealDate = customerOrder.DateOrder;
                shipmentTracking.CustomerOrderCompleted = true;

                //Si no es un usuario Cliente, se crea la OC directamente como APROBADA
                if (User.IsInRole("SellerUser"))
                {
                    shipmentTracking.ApprovedEstimatedDate = customerOrder.DateOrder;
                    shipmentTracking.ApprovedRealDate = customerOrder.DateOrder;
                    shipmentTracking.ApprovedCompleted = true;
                }

                await shipmentTrackingRepository.UpdateAsync(shipmentTracking);


                return RedirectToAction("../ProductQuote", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID});
            }

            return View(customerOrder);
        }

        // GET: CustomerOrders/Details/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> Details(int pq, int customerID, int? id)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerOrder customerOrder = await customerOrderRepository.FindCustomerOrdersByIDAsync((int)id);

            if (customerOrder == null)
            {
                return HttpNotFound();
            }

            Boolean canShowCosteo = await customerProductRepository.CanShowCosteoAsync(customerOrder.ProductQuote.CustomerID, customerOrder.ProductQuote.ProductID);

            //Si no se puede ver el Costeo por Customer/Product, nos fijamos si el usuario logueado es CustomerAdminUser
            if (!canShowCosteo && User.IsInRole("CustomerAdminUser"))
            {
                canShowCosteo = true;
            }

            customerOrder.ProductQuote.CanShowCosteo = canShowCosteo;
            return View(new CustomerOrderViewModel(customerOrder));
        }

        // GET: CustomerOrders/Details/5
        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> DetailsExpress(int pq, int customerID, int? id)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerOrder customerOrder = await customerOrderRepository.FindCustomerOrdersByIDAsync((int)id);

            if (customerOrder == null)
            {
                return HttpNotFound();
            }

            Boolean canShowCosteo = await customerProductRepository.CanShowCosteoAsync(customerOrder.ProductQuote.CustomerID, customerOrder.ProductQuote.ProductID);

            //Si no se puede ver el Costeo por Customer/Product, nos fijamos si el usuario logueado es CustomerAdminUser
            if (!canShowCosteo && User.IsInRole("CustomerAdminUser"))
            {
                canShowCosteo = true;
            }

            customerOrder.ProductQuote.CanShowCosteo = canShowCosteo;
            return View(new CustomerOrderViewModel(customerOrder));
        }


        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        public async Task<ActionResult> Delete(int pq, int customerID, int id)
        {
            CustomerOrder customerOrder = await customerOrderRepository.FindCustomerOrdersByIDAsync(id);

            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if (customerOrder == null)
            {
                return HttpNotFound();
            }
            return View(customerOrder);
        }

        [Authorize(Roles = "AdminUser, CustomerUser, CustomerAdminUser, SellerUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int pq, int customerID, int id)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            try
            {
                await customerOrderService.DeleteAsync(id);
                return RedirectToAction("../ProductQuote", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            CustomerOrder customerOrder = await customerOrderRepository.FindCustomerOrdersByIDAsync(id);
            return View("Details", customerOrder);
            
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Approve(int pq, int customerID, int productQuoteID)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if ((productQuoteID == 0) || (productQuoteID.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            customerOrderRepository.Approve(productQuoteID);

            ShipmentTracking shipmentTracking = shipmentTrackingRepository.FindShipmentTrackingByProductQuoteID(productQuoteID);

            shipmentTracking.ApprovedRealDate = DateTime.Now;
            shipmentTracking.ApprovedEstimatedDate = DateTime.Now;
            shipmentTracking.ApprovedCompleted = true;

            shipmentTrackingRepository.Update(shipmentTracking);

            return RedirectToAction("../ProductQuote", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Reject(int pq, int customerID, int productQuoteID)
        {
            ViewBag.Pq = pq;
            ViewBag.CustomerID = customerID;

            if ((productQuoteID == 0) || (productQuoteID.ToString().Length == 0))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            customerOrderRepository.Reject(productQuoteID);

            return RedirectToAction("../ProductQuote", new { pq = ViewBag.Pq, customerID = ViewBag.CustomerID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (customerOrderRepository != null)
                {
                    customerOrderRepository.Dispose();
                    customerOrderRepository = null;
                }
                if (shipmentTrackingRepository != null)
                {
                    shipmentTrackingRepository.Dispose();
                    shipmentTrackingRepository = null;
                }
                if (customerOrderService != null)
                {
                    customerOrderService.Dispose();
                    customerOrderService = null;
                }
                if (productQuoteRepository != null)
                {
                    productQuoteRepository.Dispose();
                    productQuoteRepository = null;
                }
                if (customerProductRepository != null)
                {
                    customerProductRepository.Dispose();
                    customerProductRepository = null;
                }
            }
            base.Dispose(disposing);
        }

    }
}
