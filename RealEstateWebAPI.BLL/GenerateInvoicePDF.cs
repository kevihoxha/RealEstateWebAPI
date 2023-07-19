
using PdfSharp.Drawing;
using RealEstateWebAPI.BLL.DTO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;


namespace RealEstateWebAPI.BLL
{
    public class InvoiceGenerator
    {
        /// <summary>
        /// Gjeneron nje Invoice si PDF per transaksionin e marre
        /// </summary>
        /// <param name="transaction">Transaksioni per te cilin duam invoice PDF</param>
        /// <returns>Invoice PDF si nje  byte array.</returns>
        public static byte[] GenerateInvoicePdf(TransactionDTO transaction)
        {
            // krijimi i nje memorie imagjinare ne RAM ku mund te shkruash dhe lexosh te dhena ne byte te ruajtura ne rradhe
            MemoryStream memoryStream = new MemoryStream();

            // Krijon nje dokument PDF
            PdfDocument document = new PdfDocument();

            // Shton nje faqe ne dokumentin PDF
            PdfPage page = document.AddPage();

            // Krijon nje objekt Grafik ne kete doc
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Krijimi i fontit te shkrimit
            XFont font = new XFont("Arial", 12, XFontStyle.Regular);

            // Shkrimi i te dhenave ne faqe
            gfx.DrawString($"Invoice for Transaction ID: {transaction.TransactionId}", font, XBrushes.Black, new XRect(50, 50, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
            gfx.DrawString($"Transaction Amount: ${transaction.SalePrice}", font, XBrushes.Black, new XRect(50, 70, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);

            // Ruan dokumentin ne memoryStream
            document.Save(memoryStream);

            // Mbyll dokumentin
            document.Close();

            // Merr dokumentin si nje vektor byte 
            byte[] pdfBytes = memoryStream.ToArray();
            // Kthen dokumentin
            return pdfBytes;
        }
    }
}

