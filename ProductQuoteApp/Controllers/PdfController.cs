using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ProductQuoteApp.Services.Common;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
//http://johnatten.com/2013/03/09/splitting-and-merging-pdf-files-in-c-using-itextsharp/
namespace ProductQuoteApp.Controllers
{
    public class PdfController : Controller
    {
        // GET: Pdf
        public ActionResult Index()
        {
            return View();
        }

        public void DownloadPDF()
        {
            string HTMLContent = "Hello <b>World</b>";

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(GetPDF(HTMLContent));
            Response.End();
        }

        public byte[] GetPDF(string pHTML)
        {
            byte[] bPDF = null;

            MemoryStream ms = new MemoryStream();
            TextReader txtReader = new StringReader(pHTML);

            // 1: create object of a itextsharp document class  
            Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

            // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

            // 3: we create a worker parse the document  
            HTMLWorker htmlWorker = new HTMLWorker(doc);

            // 4: we open document and start the worker on the document  
            doc.Open();
            htmlWorker.StartDocument();


            // 5: parse the html into the document  
            htmlWorker.Parse(txtReader);

            // 6: close the document and the worker  
            htmlWorker.EndDocument();
            htmlWorker.Close();
            doc.Close();

            bPDF = ms.ToArray();

            return bPDF;
        }

        public void DownloadPDF2()
        {
            //Anda perfecto,
            //Hay q probar como hacerlo en memoria
            string oldFile = @"C:\Projects\Inquimex\Cotizador Online\ProductQuoteAppProject\ProductQuoteApp\bin\doc_origen.pdf";
            string newFile = @"C:\Projects\Inquimex\Cotizador Online\ProductQuoteAppProject\ProductQuoteApp\bin\doc_destino.pdf";

            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.RED);
            cb.SetFontAndSize(bf, 8);

            // write the text in the pdf content
            cb.BeginText();
            string text = "Some random blablablabla...";
            // put the alignment and coordinates here
            cb.ShowTextAligned(1, text, 520, 640, 0);
            cb.EndText();
            cb.BeginText();
            text = "Other random blabla...";
            // put the alignment and coordinates here
            cb.ShowTextAligned(2, text, 100, 200, 0);
            cb.EndText();

            //create the new page and add it to the pdf
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            //document.NewPage();

            //Paragraph p = new Paragraph("aaaaaaaaaaaaaaaaaa", new iTextSharp.text.Font(bf));
            //document.Add(p);


            //PdfImportedPage page2 = writer.GetImportedPage(reader, 2);
            //cb.AddTemplate(page2, 0, 0);

            //document.NewPage();

            //Paragraph pwe = new Paragraph("aaaaaaaaaaaaaaaaaa", new iTextSharp.text.Font(bf));
            //document.Add(p);

            //cb.EndLayer();

            //PdfImportedPage page3 = writer.GetImportedPage(reader, 3);
            //cb.AddTemplate(page3, 0, 0);



            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();
        }


        public void TestPdf2()
        {
            string fileTemplate = Path.Combine(CommonHelper.MapPath("~/Documents/Templates"), ProductQuoteApp.Resources.Resources.FileNamePdfProductQuoteTemplate);
            string fileNameExport = string.Format("productQuote_{0}_{1}.pdf", 1, Guid.NewGuid().ToString());
            string fileExport = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), fileNameExport);


            PdfReader reader = new PdfReader(fileTemplate);
            FileStream fs = new FileStream(fileExport, FileMode.Create, FileAccess.Write);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            PdfCopy copy = new PdfCopy(document, fs);

            document.Open();

            PdfImportedPage page = null;
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                page = copy.GetImportedPage(reader, i);

                copy.AddPage(page);
            }

            

            document.Close();
            reader.Close();


            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            FileStream fs2 = new FileStream(fileExport, FileMode.OpenOrCreate, FileAccess.Write);
            Document document2 = new Document(size);
            PdfWriter writer = PdfWriter.GetInstance(document2, fs2);
            
            document2.Open();

            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 9);
            string text = "";

            cb.BeginText();

            //text = "Buenos Aires, 15 de septiembre de 2017";
            text = "PRUEBA PRUEBA PRUEBA PRUEBA ";
            cb.ShowTextAligned(Element.ALIGN_RIGHT, text, 0, 0, 0);

            cb.EndText();

            //cb.AddTemplate(page, 0, 0);
            document2.Close();
            fs2.Close();
            writer.Close();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        }


        public void TestPdf()
        {
            string fileTemplate = Path.Combine(CommonHelper.MapPath("~/Documents/Templates"), ProductQuoteApp.Resources.Resources.FileNamePdfProductQuoteTemplate);
            string fileNameExport = string.Format("productQuote_{0}_{1}.pdf", 1, Guid.NewGuid().ToString());
            string fileExport = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), fileNameExport);


            PdfReader reader = new PdfReader(fileTemplate);
            FileStream fs = new FileStream(fileExport, FileMode.Create, FileAccess.Write);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            PdfCopy copy = new PdfCopy(document, fs);

            document.Open();

            PdfImportedPage page = null;
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                page = copy.GetImportedPage(reader, i);

                //En la primer pagina ponemos los datos
                if (i == 1)
                {
                    PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(page);
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf,9);

                    ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase("Buenos Aires, 15 de septiembre de 2017", fontText), 525, 720, 0);
                    //new Phrase("Buenos Aires, 15 de septiembre de 2017", fontText), page.Width / 2, page.Height - 30, page.Width < page.Height ? 0 : 1);

                    pageStamp.AlterContents();
                }

                copy.AddPage(page);
            }

            copy.FreeReader(reader);
            document.Close();
            reader.Close();
        }

    }
}