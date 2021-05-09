using ProductQuoteApp.Services;

namespace ProductQuoteApp.Controllers
{
    public class CustomerRegisterController : BaseController
    {
        private IWorkflowMessageService workflowMessageService = null;

        public CustomerRegisterController(IWorkflowMessageService workflowMessageServ)
        {
            workflowMessageService = workflowMessageServ;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && workflowMessageService != null)
            {
                workflowMessageService.Dispose();
                workflowMessageService = null;
            }

            base.Dispose(disposing);
        }

    }
}