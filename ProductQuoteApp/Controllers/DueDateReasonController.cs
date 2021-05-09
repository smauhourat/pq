using PagedList;
using ProductQuoteApp.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class DueDateReasonController : BaseController
    {
        private IDueDateReasonRepository dueDateReasonRepository = null;

        public DueDateReasonController(IDueDateReasonRepository dueDateReasonRepo)
        {
            dueDateReasonRepository = dueDateReasonRepo;

        }

        [Authorize(Roles = "AdminUser")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.filter = currentFilter;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProviderNameSortParm = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "";
            ViewBag.ProviderIDSortParm = sortOrder == "DueDateReasonID" ? "DueDateReasonID_desc" : "DueDateReasonID";

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

            var dueDateReasons = dueDateReasonRepository.FindDueDateReasonsFilter(searchString, "");

            switch (sortOrder)
            {
                case "Description_desc":
                    dueDateReasons = dueDateReasons.OrderByDescending(s => s.Description);
                    break;
                case "DueDateReasonID":
                    dueDateReasons = dueDateReasons.OrderBy(s => s.DueDateReasonID);
                    break;
                case "DueDateReasonID_desc":
                    dueDateReasons = dueDateReasons.OrderByDescending(s => s.DueDateReasonID);
                    break;
                default:  // Name ascending 
                    dueDateReasons = dueDateReasons.OrderBy(s => s.Description);
                    break;
            }

            IPagedList ret = dueDateReasons.ToPagedList(pageNumber, pageSize);


            return View(ret);
        }

        // GET: Providers/Details/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            DueDateReason dueDateReason = await dueDateReasonRepository.FindDueDateReasonsByIDAsync(id);
            if (dueDateReason == null)
            {
                return HttpNotFound();
            }
            return View(dueDateReason);
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DueDateReasonID,Description")] DueDateReason dueDateReason)
        {
            if (ModelState.IsValid)
            {
                await dueDateReasonRepository.CreateAsync(dueDateReason);
                return RedirectToAction("Index");
            }

            return View(dueDateReason);
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            DueDateReason dueDateReason = await dueDateReasonRepository.FindDueDateReasonsByIDAsync(id);
            if (dueDateReason == null)
            {
                return HttpNotFound();
            }

            return View(dueDateReason);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DueDateReasonID,Description")] DueDateReason dueDateReason)
        {
            if (ModelState.IsValid)
            {
                await dueDateReasonRepository.UpdateAsync(dueDateReason);
                return RedirectToAction("Index");
            }

            return View(dueDateReason);
        }

        // GET: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            DueDateReason dueDateReason = await dueDateReasonRepository.FindDueDateReasonsByIDAsync(id);
            if (dueDateReason == null)
            {
                return HttpNotFound();
            }
            return View(dueDateReason);
        }

        // POST: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await dueDateReasonRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && dueDateReasonRepository != null)
            {
                dueDateReasonRepository.Dispose();
                dueDateReasonRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
