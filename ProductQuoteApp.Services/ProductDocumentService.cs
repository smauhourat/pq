using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;
using System.Web;
using System.Diagnostics;
using ProductQuoteApp.Logging;

namespace ProductQuoteApp.Services
{
    public class ProductDocumentService : IProductDocumentService
    {
        private ILogger log = null;
        private IProductDocumentRepository productDocumentRepository = null;
        private IDocumentFileService documentFileService = null;

        public ProductDocumentService(ILogger logger, IProductDocumentRepository productDocumentRepo, IDocumentFileService documentFileServ)
        {
            log = logger;
            productDocumentRepository = productDocumentRepo;
            documentFileService = documentFileServ;
        }

        public async Task CreateAsync(ProductDocument productDocumentToAdd, HttpPostedFileBase documentFileToUpload)
        {
            try
            {
                productDocumentToAdd.ProductDocumentPDF = await documentFileService.UploadDocumentFileAsync(documentFileToUpload);

                //creamos la entidad documento
                await productDocumentRepository.CreateAsync(productDocumentToAdd);
            }
            catch (Exception e)
            {
                log.Error(e, "Error in ProductDocumentService.CreateAsync()");
                throw;
            }
        }

        public async Task<ProductDocument> FindProductDocumentsByIDAsync(int productDocumentID)
        {
            return await productDocumentRepository.FindProductDocumentsByIDAsync(productDocumentID);
        }

        public async Task<List<ProductDocument>> FindProductDocumentsByProductIDAsync(int productID)
        {
            return await productDocumentRepository.FindProductDocumentsByProductIDAsync(productID);
        }

        public void DeleteByProductIDAsync(int productID)
        {
            //Eliminamos los archivos fisicos
            var productDocuments = productDocumentRepository.FindProductDocumentsByProductID(productID);
            if (productDocuments != null)
            {
                foreach (ProductDocument pd in productDocuments)
                {
                    documentFileService.DeleteDocumentFileAsync(pd.ProductDocumentPDF);
                }
            }
            //Eliminamos la tabla que lo contiene
            productDocumentRepository.DeleteByProductIDAsync(productID);
        }

        public async Task DeleteAsync(int productDocumentID)
        {
            await productDocumentRepository.DeleteAsync(productDocumentID);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && productDocumentRepository != null)
            {
                productDocumentRepository.Dispose();
                productDocumentRepository = null;
            }
        }

    }
}
