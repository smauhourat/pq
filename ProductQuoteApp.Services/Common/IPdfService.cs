using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Services
{
    public interface IPdfService: IDisposable
    {
        string ProductQuoteToPdf(ProductQuote productQuote, ICollection<ProductDocument> productDocuments, string productQuotePdfTemplate);
        string ProductQuoteToSmallPdf(ProductQuote productQuote, string productQuoteSmallPdfTemplate);

        string UpdateCodeProductQuotePdf(ProductQuote productQuote);
        void DeleteProductQuotePdf(ProductQuote productQuote);
        void DeleteProductQuoteSmallPdf(ProductQuote productQuote);
    }
}
