using ProductQuoteApp.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProductQuoteApp.Services
{
    public interface IProductDocumentService: IDisposable
    {
        Task<ProductDocument> FindProductDocumentsByIDAsync(int productDocumentID);
        Task<List<ProductDocument>> FindProductDocumentsByProductIDAsync(int productID);
        Task CreateAsync(ProductDocument productDocumentToAdd, HttpPostedFileBase documentFileToUpload);
        void DeleteByProductIDAsync(int productID);
        Task DeleteAsync(int productDocumentID);
    }
}
