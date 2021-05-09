using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductQuoteApp.Persistence;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ProductQuoteApp.Services.Common;
using System.Threading;
using ProductQuoteApp.Library;
//https://stackoverflow.com/questions/3992617/itextsharp-insert-text-to-an-existing-pdf
namespace ProductQuoteApp.Services
{
    public class PdfService : IPdfService
    {
#warning "Esta hardcoded el cultureInfo"

        private void StampFooterDataIntoPdf(PdfCopy.PageStamp pageStamp, string labelFooter)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf, 8);

            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(labelFooter, fontText), 315, 20, 0);

        }

        //https://www.codeproject.com/Articles/28283/Simple-NET-PDF-Merger
        private void StampDataIntoPdf(PdfCopy.PageStamp pageStamp, ProductQuote productQuote)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf, 9);

            string fechaString = productQuote.DateQuote.ToLongDateString();
            string text = "Buenos Aires, " + fechaString.Substring(fechaString.IndexOf(",") + 2, fechaString.Length - (fechaString.IndexOf(",") + 2));
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase(text, fontText), 525, 720, 0);

            text = productQuote.CustomerCompany;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 102, 686, 0);

            if (productQuote.CustomerContactName != null)
            {
                text = productQuote.CustomerContactName.ToString();
                ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 102, 671, 0);
            }

            text = productQuote.ProductQuoteCode;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 225, 649, 0);//OK

            text = productQuote.ProductSingleName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 631, 0);

            text = productQuote.ProductBrandName.ToString();
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 613, 0);

            text = productQuote.ProductPackagingName.ToString();
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 595, 0);

            if (productQuote.ProductValidityOfPrice != null)
            {
                text = ((DateTime)productQuote.ProductValidityOfPrice).ToString("dd/MM/yyyy");
                ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 577, 0);
            }

            text = productQuote.SaleModalityName.ToString();
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 559, 0);

            if ((productQuote.DeliveryAddress != null) && (productQuote.DeliveryAddress != ""))
                text = productQuote.DeliveryAddress;
            else
                text = productQuote.GeographicAreaName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 542, 0);

            text = productQuote.PaymentDeadlineName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 524, 0);

            text = productQuote.QuantityOpenPurchaseOrder.ToString("#,##0");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 506, 0);

            text = productQuote.DeliveryAmount.ToString();
            if (productQuote.DeliveryAmount == 1)
                text = text + " Entrega";
            else
                text = text + " Entregas";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 488, 0);

            text = productQuote.MinimumQuantityDelivery.ToString("#,##0");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 470, 0);

            text = productQuote.MaximumMonthsStock.ToString() + " Meses";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 453, 0);

            text = productQuote.ExchangeTypeName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 436, 0);

            text = Helper.RoundDecimal(productQuote.Price, 3).ToString("#,###0.000");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 418, 0);

            text = Helper.RoundDecimal(productQuote.Price * productQuote.MinimumQuantityDelivery, 2).ToString("#,##0.00");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 400, 0);

            text = Helper.RoundDecimal(productQuote.Price * productQuote.QuantityOpenPurchaseOrder, 2).ToString("#,##0.00");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 243, 382, 0);

            text = productQuote.UserObservations + Environment.NewLine + productQuote.Observations;
            ColumnText ct = new ColumnText(pageStamp.GetOverContent());
            ct.SetSimpleColumn(243, 375, 520, 230, 14, Element.ALIGN_JUSTIFIED);
            ct.AddElement(new Paragraph(new Phrase(text, fontText)));
            ct.Go();

            text = productQuote.UserFullName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase(text, fontText), 500, 120, 0);

            text = "Inquimex SACI";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase(text, fontText), 500, 105, 0);
        }

        private void StampDataIntoSmallPdf(PdfCopy.PageStamp pageStamp, ProductQuote productQuote)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontText = new iTextSharp.text.Font(bf, 9);
            const float OFFSET_GRILLA = 215;

            string fechaString = productQuote.DateQuote.ToLongDateString();
            string text = "Buenos Aires, " + fechaString.Substring(fechaString.IndexOf(",") + 2, fechaString.Length - (fechaString.IndexOf(",") + 2));
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase(text, fontText), 525, 720, 0);

            //CONTACTO
            text = !string.IsNullOrEmpty(productQuote.CustomerContactName) ? productQuote.CustomerContactName : string.Empty;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 105, 685, 0);

            //EMPRESA
            text = !string.IsNullOrEmpty(productQuote.CustomerCompany) ? productQuote.CustomerCompany : string.Empty;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 105, 673, 0);

            //CODIGO COTIZACION
            text = productQuote.ProductQuoteCode;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), 135, 652, 0);

            //PRODUCTO
            text = productQuote.ProductSingleName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 632, 0);

            //CANTIDAD
            text = productQuote.QuantityOpenPurchaseOrder.ToString("#,##0");
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 614, 0);

            //ORIGEN
            text = productQuote.ProductBrandName.ToString();
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 597, 0);

            //ENVASE
            text = productQuote.ProductPackagingName.ToString();
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 580, 0);

            //PRECIO
            text = Helper.RoundDecimal(productQuote.Price, 3).ToString("#,###0.000") + " USD/Kg + Impuestos";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 561, 0);

            //CONDICION DE PAGO
            text = productQuote.PaymentDeadlineName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 544, 0);

            //MONEDA
            text = productQuote.ExchangeTypeName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 526, 0);

            //VALIDEZ DEL PRECIO
            text = productQuote.ProductValidityOfPrice != null ? ((DateTime)productQuote.ProductValidityOfPrice).ToString("dd/MM/yyyy") : "";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 508, 0);

            //LUGAR DE ENTREGA
            text = !string.IsNullOrEmpty(productQuote.DeliveryAddress) ? productQuote.DeliveryAddress : productQuote.GeographicAreaName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 491, 0);

            //FECHAS ENTREGA
            text = !string.IsNullOrEmpty(productQuote.DatesDeliveryInput) ? productQuote.DatesDeliveryInput : "";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 473, 0);

            //DISPONIBILIDAD
            text = productQuote.AvailabilityDays != null ? productQuote.AvailabilityDays.ToString() : "";
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_LEFT, new Phrase(text, fontText), OFFSET_GRILLA, 456, 0);

            //OBSERVACIONES
            text = productQuote.UserObservations + Environment.NewLine + productQuote.Observations;
            ColumnText ct = new ColumnText(pageStamp.GetOverContent());
            ct.SetSimpleColumn(OFFSET_GRILLA, 448, 520, 230, 14, Element.ALIGN_JUSTIFIED);
            ct.AddElement(new Paragraph(new Phrase(text, fontText)));
            ct.Go();

            //FIRMA
            text = productQuote.UserFullName;
            ColumnText.ShowTextAligned(pageStamp.GetOverContent(), Element.ALIGN_RIGHT, new Phrase(text, fontText), 480, 95, 0); //90
        }


        public string UpdateCodeProductQuotePdf(ProductQuote productQuote)
        {
            string fileExport = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), productQuote.ProductQuotePDF);

            FileStream fs = new FileStream(fileExport, FileMode.Create, FileAccess.Write);

            PdfReader reader = new PdfReader(fileExport);
            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.BLACK);
            cb.SetFontAndSize(bf, 9);
            string text = "";
            //////////////////////////////////////////////////////////////////////////////
            cb.BeginText();

            text = "XXXXXXXXXXXXXXX ";
            cb.ShowTextAligned(Element.ALIGN_RIGHT, text, 550, 720, 0);

            cb.EndText();

            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();


            return "";
        }

        public string ProductQuoteToPdf(ProductQuote productQuote, ICollection<ProductDocument> productDocuments, string productQuotePdfTemplate)
        {
            string fileTemplate = Path.Combine(CommonHelper.MapPath("~/Documents/Templates"), productQuotePdfTemplate);
            string fileNameExport = string.Format("{0}_{1}.pdf", productQuote.ProductQuoteCode, Guid.NewGuid().ToString().Substring(0, 8));
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
                    StampDataIntoPdf(pageStamp, productQuote);
                    pageStamp.AlterContents();
                }

                PdfCopy.PageStamp pageStampFooter = copy.CreatePageStamp(page);
                StampFooterDataIntoPdf(pageStampFooter, productQuote.ProductQuotePDFFooter);
                pageStampFooter.AlterContents();

                copy.AddPage(page);
            }


            //Agregamos los documentos del Producto
            PdfReader readerProductDoc = null;
            if (productDocuments != null)
            {
                string fileDocument = "";
                foreach (ProductDocument item in productDocuments)
                {
                    fileDocument = Path.Combine(CommonHelper.MapPath("~/Documents/Products"), item.ProductDocumentPDF);
                    readerProductDoc = new PdfReader(fileDocument);

                    for (int i = 1; i <= readerProductDoc.NumberOfPages; i++)
                    {
                        page = copy.GetImportedPage(readerProductDoc, i);
                        copy.AddPage(page);
                    }

                    copy.FreeReader(readerProductDoc);
                    readerProductDoc.Close();
                }
            }


            copy.FreeReader(reader);
            document.Close();
            reader.Close();

            return fileNameExport;
        }

        public string ProductQuoteToSmallPdf(ProductQuote productQuote, string productQuoteSmallPdfTemplate)
        {
            string fileTemplate = Path.Combine(CommonHelper.MapPath("~/Documents/Templates"), productQuoteSmallPdfTemplate);
            string fileNameExport = string.Format("{0}_{1}_{2}.pdf", productQuote.ProductQuoteCode, "Rev08", Guid.NewGuid().ToString().Substring(0, 8));
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
                    StampDataIntoSmallPdf(pageStamp, productQuote);
                    pageStamp.AlterContents();
                }

                PdfCopy.PageStamp pageStampFooter = copy.CreatePageStamp(page);
                StampFooterDataIntoPdf(pageStampFooter, productQuote.ProductQuoteSmallPDFFooter);
                pageStampFooter.AlterContents();

                copy.AddPage(page);
            }

            copy.FreeReader(reader);
            document.Close();
            reader.Close();

            return fileNameExport;
        }

        public void DeleteProductQuotePdf(ProductQuote productQuote)
        {
            string pathFile = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), productQuote.ProductQuotePDF == null ? "" : productQuote.ProductQuotePDF);

            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }
        }

        public void DeleteProductQuoteSmallPdf(ProductQuote productQuote)
        {
            string pathFile = Path.Combine(CommonHelper.MapPath("~/Documents/Export"), productQuote.ProductQuoteSmallPDF == null ? "" : productQuote.ProductQuoteSmallPDF);

            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }

    }

}
