using PagedList;
using ProductQuoteApp.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class ProviderController : BaseController
    {
        private IProviderRepository providerRepository = null;

        public ProviderController(IProviderRepository providerRepo)
        {
            providerRepository = providerRepo;

        }

        [Authorize(Roles = "AdminUser")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProviderNameSortParm = String.IsNullOrEmpty(sortOrder) ? "ProviderName_desc" : "";
            ViewBag.ProviderIDSortParm = sortOrder == "ProviderID" ? "ProviderID_desc" : "ProviderID";

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

            var providers = providerRepository.FindProvidersFilter(searchString, "");

            switch (sortOrder)
            {
                case "ProviderName_desc":
                    providers = providers.OrderByDescending(s => s.ProviderName);
                    break;
                case "ProviderID":
                    providers = providers.OrderBy(s => s.ProviderID);
                    break;
                case "ProviderID_desc":
                    providers = providers.OrderByDescending(s => s.ProviderID);
                    break;
                default:  // Name ascending 
                    providers = providers.OrderBy(s => s.ProviderName);
                    break;
            }

            IPagedList ret = providers.ToPagedList(pageNumber, pageSize);

            return View(ret);
        }

        // GET: Providers/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            Provider provider = await providerRepository.FindProvidersByIDAsync(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProviderID,ProviderName")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                await providerRepository.CreateAsync(provider);
                return RedirectToAction("Index");
            }

            return View(provider);
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            Provider provider = await providerRepository.FindProvidersByIDAsync(id);
            if (provider == null)
            {
                return HttpNotFound();
            }

            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProviderID,ProviderName")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                await providerRepository.UpdateAsync(provider);
                return RedirectToAction("Index");
            }

            return View(provider);
        }

        // GET: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            Provider provider = await providerRepository.FindProvidersByIDAsync(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await providerRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && providerRepository != null)
            {
                providerRepository.Dispose();
                providerRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
