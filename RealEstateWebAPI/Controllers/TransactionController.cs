using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System.Net;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            return await HandleAsync<IEnumerable<TransactionDTO>>(async () =>
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(transactions);
            });
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<TransactionDTO>> GetTransactionById(int id)
        {
            return await HandleAsync<TransactionDTO>(async () =>
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                return Ok(transaction);
            });
        }

        [HttpPost("create")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction(TransactionDTO transactionRequest)
        {
            return await HandleAsync<TransactionDTO>(async () =>
            {
                var createdTransaction = await _transactionService.CreateTransactionAsync(transactionRequest);
                return Ok(createdTransaction);
            });
        }
        [HttpGet("{id}/invoice")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            // Retrieve the transaction by ID
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            // Generate the invoice PDF
            byte[] invoicePdf = InvoiceGenerator.GenerateInvoicePdf(transaction);

            // Save the PDF to a temporary file
            var tempFilePath = Path.GetTempFileName();
            System.IO.File.WriteAllBytes(tempFilePath, invoicePdf);

            // Return the PDF as a downloadable file
            var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", "invoice.pdf", true);
        }
    }
}
