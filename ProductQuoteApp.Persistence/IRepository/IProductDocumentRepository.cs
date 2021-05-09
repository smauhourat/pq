using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductQuoteApp.Persistence
{
    public interface IProductDocumentRepository : IDisposable 
    {
        Task<List<ProductDocument>> FindProductDocumentsAsync();
        Task<ProductDocument> FindProductDocumentsByIDAsync(int productDocumentID);
        List<ProductDocument> FindProductDocumentsByProductID(int productID);
        Task<List<ProductDocument>> FindProductDocumentsByProductIDAsync(int productID);
        Task CreateAsync(ProductDocument productDocumentToAdd);
        void Delete(int productDocumentID);
        Task DeleteAsync(int productDocumentID);
        Task UpdateAsync(ProductDocument productDocumentToSave);
        void DeleteByProductIDAsync(int productID);
    }
}
