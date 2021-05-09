using ProductQuoteApp.Models;
using ProductQuoteApp.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductQuoteApp.Controllers
{
    public class CreditRatingPaymentDeadlineApiController : ApiController
    {
        private ICreditRatingRepository creditRatingRepository = null;
        private ICreditRatingPaymentDeadlineRepository creditRatingPaymentDeadlineRepository = null;

        public CreditRatingPaymentDeadlineApiController(ICreditRatingRepository creditRatingRepo, 
            ICreditRatingPaymentDeadlineRepository creditRatingPaymentDeadlineRepo)
        {
            creditRatingRepository = creditRatingRepo;
            creditRatingPaymentDeadlineRepository = creditRatingPaymentDeadlineRepo;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetCreditRatingsMM")]
        public async Task<List<CreditRatingViewModel>> GetCreditRatingsMM()
        {
            var creditRatings = (await creditRatingRepository.FindCreditRatingsAsync())
                .Select(CreditRatingViewModel.FromCreditRating);

            return creditRatings.ToList();

            //CreditRating cr = null;
            //List<CreditRating> result = new List<CreditRating>();
            //List<CreditRating> crList = await creditRatingRepository.FindCreditRatingsAsync();

            //foreach (CreditRating item in crList)
            //{
            //    cr = new CreditRating();
            //    cr.CreditRatingID = item.CreditRatingID;
            //    cr.Description = item.Description;
            //    result.Add(cr);
            //    cr = null;
            //}
            //crList = null;
            //return result;
        }

        [Authorize(Roles = "AdminUser")]
        [Route("GetPaymentDeadlineByCreditRatingApi/{id}")]
        public async Task<List<CreditRatingPaymentDeadlineViewModel>> GetPaymentDeadlineByCreditRatingApi(int id)
        {
            var creditRatingPaymentDeadlines = (await creditRatingPaymentDeadlineRepository.FindCreditRatingPaymentDeadlinesByCreditRatingAsync(id))
                .Select(CreditRatingPaymentDeadlineViewModel.FromCreditRatingPaymentDeadline);

            return creditRatingPaymentDeadlines.ToList();

            //CreditRatingPaymentDeadlineViewModel crpdVM = null;
            //List<CreditRatingPaymentDeadlineViewModel> result = new List<CreditRatingPaymentDeadlineViewModel>();
            //List<CreditRatingPaymentDeadline> crpdList = await creditRatingPaymentDeadlineRepository.FindCreditRatingPaymentDeadlinesByCreditRatingAsync(id);

            //foreach (CreditRatingPaymentDeadline item in crpdList)
            //{
            //    crpdVM = new CreditRatingPaymentDeadlineViewModel(item);

            //    result.Add(crpdVM);
            //    crpdVM = null;
            //}
            //crpdList = null;

            //return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("GetPaymentDeadlineAvailables/{id}")]
        public List<PaymentDeadlineViewModel> GetPaymentDeadlineAvailables(int id)
        {
            var paymentDeadLines = (creditRatingPaymentDeadlineRepository.FindPaymentDeadlineAvailableByCreditRating(id))
                .Select(PaymentDeadlineViewModel.FromPaymentDeadline);

            return paymentDeadLines.ToList();

            //List<PaymentDeadlineViewModel> result = new List<PaymentDeadlineViewModel>();
            //List<PaymentDeadline> pdList = creditRatingPaymentDeadlineRepository.FindPaymentDeadlineAvailableByCreditRating(id);

            //foreach (PaymentDeadline item in pdList)
            //{
            //    result.Add(new PaymentDeadlineViewModel(item));
            //}

            //return result;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("CreateCreditRatingPaymentDeadline")]
        public CreditRatingPaymentDeadline CreateCreditRatingPaymentDeadline(CreditRatingPaymentDeadline creditRatingPaymentDeadline)
        {
            creditRatingPaymentDeadlineRepository.Create(creditRatingPaymentDeadline);

            return creditRatingPaymentDeadline;
        }

        [Authorize(Roles = "AdminUser")]
        [System.Web.Http.Route("UpdateCreditRatingPaymentDeadlineList")]
        public void UpdateCreditRatingPaymentDeadlineList(List<CreditRatingPaymentDeadline> creditRatingPaymentDeadlineList)
        {
            foreach (CreditRatingPaymentDeadline crpd in creditRatingPaymentDeadlineList)
            {
                crpd.CreditRating = null;
                crpd.PaymentDeadline = null;
                creditRatingPaymentDeadlineRepository.Update(crpd);
            }
        }

        [Authorize(Roles = "AdminUser")]
        public HttpResponseMessage DeleteCreditRatingPaymentDeadlineApi(int id)
        {
            try
            {
                creditRatingPaymentDeadlineRepository.Delete(id);

                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (creditRatingRepository != null)
                {
                    creditRatingRepository.Dispose();
                    creditRatingRepository = null;
                }
                if (creditRatingPaymentDeadlineRepository != null)
                {
                    creditRatingPaymentDeadlineRepository.Dispose();
                    creditRatingPaymentDeadlineRepository = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}