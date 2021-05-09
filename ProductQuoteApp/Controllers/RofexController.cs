using ProductQuoteApp.Persistence;
using ProductQuoteApp.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class RofexController : BaseController
    {
        private IRofexService rofexService = null;

        public RofexController(IRofexService rofexServ)
        {
            rofexService = rofexServ;

        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var rofex = await rofexService.FindRofexsAsync();
            return View(rofex.ToList());
        }

        [Authorize(Roles = "AdminUser")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RofexID,Days,DollarQuotation")] Rofex rofex)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await rofexService.CreateAsync(rofex);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(rofex);
            }
        }

        // GET: Providers/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            Rofex rofex = await rofexService.FindRofexByIDAsync(id);
            if (rofex == null)
            {
                return HttpNotFound();
            }

            return View(rofex);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RofexID,Days,DollarQuotation")] Rofex rofex)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await rofexService.UpdateAsync(rofex);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(rofex);
            }
        }

        // GET: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            Rofex rofex= await rofexService.FindRofexByIDAsync(id);
            if (rofex == null)
            {
                return HttpNotFound();
            }
            return View(rofex);
        }

        // POST: Providers/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await rofexService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && rofexService != null)
            {
                rofexService.Dispose();
                rofexService = null;
            }
            base.Dispose(disposing);
        }
    }
}
