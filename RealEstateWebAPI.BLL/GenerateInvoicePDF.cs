
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
            MemoryStream memoryStream = new MemoryStream();

            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create a PDF graphics object for the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Arial", 12, XFontStyle.Regular);

            // Draw the content on the page
            gfx.DrawString($"Invoice for Transaction ID: {transaction.TransactionId}", font, XBrushes.Black, new XRect(50, 50, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
            gfx.DrawString($"Transaction Amount: ${transaction.SalePrice}", font, XBrushes.Black, new XRect(50, 70, page.Width.Point, page.Height.Point), XStringFormats.TopLeft);
            // Add more content as needed

            // Save the PDF document to the memory stream
            document.Save(memoryStream);

            // Close the PDF document
            document.Close();

            // Get the PDF document as a byte array
            byte[] pdfBytes = memoryStream.ToArray();

            return pdfBytes;
        }
    }
}

