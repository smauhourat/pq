using ProductQuoteApp.Persistence;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductQuoteApp.Controllers
{
    public class GlobalVariableController : BaseController
    {
        private IGlobalVariableRepository globalVariableRepository = null;

        public GlobalVariableController(IGlobalVariableRepository globalVariableRepo)
        {
            globalVariableRepository = globalVariableRepo;
        }

        // GET: GlobalVariable/Edit/5
        [Authorize(Roles = "AdminUser")]
        public async Task<ActionResult> Edit(int id)
        {
            GlobalVariable globalVariable = await globalVariableRepository.FindGlobalVariablesAsync();
            if (globalVariable == null)
            {
                return HttpNotFound();
            }

            return View(globalVariable);
        }

        // POST: Packagings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GlobalVariableID,CostoAlmacenamientoMensual,CostoInOut,CostoFinancieroMensual,ImpuestoDebitoCredito,GastosFijos,TipoCambio,FactorCostoAlmacenamientoMensual,DiasStockPromedioDistLocal,EnvioCotizacionPorMail,EnvioCotizacionUsuarioCreadorPorMail")] GlobalVariable globalVariable)
        //public async Task<ActionResult> Edit([Bind(Include = "GlobalVariableID,CostoAlmacenamientoMensual,CostoInOut,CostoFinancieroMensual,ImpuestoDebitoCredito,GastosFijos,TipoCambio,FactorCostoAlmacenamientoMensual,DiasStockPromedioDistLocal")] GlobalVariable globalVariable)
        {
            if (ModelState.IsValid)
            {
                await globalVariableRepository.UpdateAsync(globalVariable);
                return RedirectToAction("../Home/Default");
            }

            return View(globalVariable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && globalVariableRepository != null)
            {
                globalVariableRepository.Dispose();
                globalVariableRepository = null;
            }
            base.Dispose(disposing);
        }

    }
}