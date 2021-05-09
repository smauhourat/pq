using System;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using ProductQuoteApp.Services.Common;
using iTextSharp.text.pdf;

namespace ProductQuoteApp.Services
{
    public class DocumentFileService : IDocumentFileService
    {
        private bool CheckPdfPermission(HttpPostedFileBase documentFileToUpload)
        {
            bool ret = true;
            byte[] pdfbytes = null;
            BinaryReader rdr = new BinaryReader(documentFileToUpload.InputStream);
            pdfbytes = rdr.ReadBytes(documentFileToUpload.ContentLength);

            PdfReader reader = new PdfReader(pdfbytes);

            ret = reader.IsOpenedWithFullPermissions;

            reader.Close();

            return ret;
        }

        public async Task<string> UploadDocumentFileAsync(HttpPostedFileBase documentFileToUpload)
        {
            if (!CheckPdfPermission(documentFileToUpload))
                throw (new Exception("El PDF no tiene el nivel de seguridad requerido."));

            //string fileExport = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), fileNameExport);
            string docName = String.Format("productDocument-{0}{1}",
                Guid.NewGuid().ToString(),
                Path.GetExtension(documentFileToUpload.FileName));

            string path = Path.Combine(CommonHelper.MapPath("~/Documents/Products"), docName);
            documentFileToUpload.SaveAs(path);

            await Task.Delay(0);

            return docName;
            //await Task.Delay(0);
            //return "FALTA DESARROLLAR";
        }

#warning "el delete fisico no funciona y no tira error"
        public async Task DeleteDocumentFileAsync(string fileName)
        {
            string path = Path.Combine(CommonHelper.MapPath("~/Documents/Products"), fileName);
            string fullNameFile = path + fileName;

            if (File.Exists(fullNameFile))
            {
                File.SetAttributes(fullNameFile, FileAttributes.Normal);
                File.Delete(fullNameFile);
            }
            

            await Task.Delay(0);

        }
    }
}
