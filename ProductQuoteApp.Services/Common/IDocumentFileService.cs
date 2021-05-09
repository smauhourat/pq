using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProductQuoteApp.Services
{
    public interface  IDocumentFileService
    {
        Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload);
        Task DeleteDocumentFileAsync(string fileName);
    }
}
