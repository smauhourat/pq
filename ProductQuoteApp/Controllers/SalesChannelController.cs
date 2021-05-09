using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class SalesChannelController : BaseController
    {
        private ISalesChannelRepository salesChannelRepository = null;

        public SalesChannelController(ISalesChannelRepository salesChannelRepo)
        {
            salesChannelRepository = salesChannelRepo;

        }

        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Index()
        {
            var salesChannels = await salesChannelRepository.FindSalesChannelsAsync();
            return View(salesChannels.ToList());
        }

        // GET: GeographicAreas/Details/5
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            SalesChannel salesChannel = await salesChannelRepository.FindSalesChannelByIDAsync(id);
            if (salesChannel == null)
            {
                return HttpNotFound();
            }
            return View(salesChannel);
        }

        [Authorize(Roles = "SuperAdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Create([Bind(Include = "SalesChannelID,Code, Description")] SalesChannel salesChannel)
        {
            if (ModelState.IsValid)
            {
                await salesChannelRepository.CreateAsync(salesChannel);
            }
            return RedirectToAction("Index");
        }

        // GET: GeographicAreas/Edit/5
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            SalesChannel salesChannel = await salesChannelRepository.FindSalesChannelByIDAsync(id);
            if (salesChannel == null)
            {
                return HttpNotFound();
            }

            return View(salesChannel);
        }

        // POST: GeographicAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Edit([Bind(Include = "SalesChannelID,Code,Description")] SalesChannel salesChannel)
        {
            if (ModelState.IsValid)
            {
                await salesChannelRepository.UpdateAsync(salesChannel);
                return RedirectToAction("Index");
            }

            return View(salesChannel);
        }

        // GET: GeographicAreas/Delete/5
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            SalesChannel salesChannel = await salesChannelRepository.FindSalesChannelByIDAsync(id);
            if (salesChannel == null)
            {
                return HttpNotFound();
            }
            return View(salesChannel);
        }

        // POST: GeographicAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdminUser")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await salesChannelRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && salesChannelRepository != null)
            {
                salesChannelRepository.Dispose();
                salesChannelRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}
