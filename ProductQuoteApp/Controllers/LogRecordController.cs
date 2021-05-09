using ProductQuoteApp.Persistence;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class LogRecordController : BaseController
    {
        private ILogRecordRepository logRecordRepository = null;

        public LogRecordController(ILogRecordRepository logRecordRepo)
        {
            logRecordRepository = logRecordRepo;
        }

        // GET: LogRecord
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Index()
        {
            var result = await logRecordRepository.FindLogRecordsAsync();
            return View(result.ToList().OrderByDescending(s=>s.CreatedOnUtc));
        }

        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Details(int id)
        {
            LogRecord logRecord = await logRecordRepository.FindLogRecordByIDAsync(id);
            if (logRecord == null)
            {
                return HttpNotFound();
            }
            return View(logRecord);
        }

        // GET: LogRecord/Delete/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Delete(int id)
        {
            LogRecord logRecord = await logRecordRepository.FindLogRecordByIDAsync(id);
            if (logRecord == null)
            {
                return HttpNotFound();
            }
            return View(logRecord);
        }

        // POST: LogRecord/Delete/5
        [Authorize(Roles = "AdminUser")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await logRecordRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && logRecordRepository != null)
            {
                logRecordRepository.Dispose();
                logRecordRepository = null;
            }
            base.Dispose(disposing);
        }
    }
}