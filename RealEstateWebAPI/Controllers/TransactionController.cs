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
        /// <summary>
        ///pasi kalon middlewares per authentikim dhe autorizim ,  Merr te gjith transaksionet
        /// </summary>
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
        /// <summary>
        ///pasi kalon middlewares per authentikim dhe autorizim ,  Merr user me id specifike
        /// </summary>
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
        /// <summary>
        ///pasi kalon middlewares per authentikim dhe autorizim ,  Krijon transaksion
        /// </summary>
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
        /// <summary>
        ///pasi kalon middlewares per authentikim dhe autorizim ,  krijon downloadable pdf te invoice te transaksionit
        /// </summary>
        [HttpGet("{id}/invoice")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            // Merr trasnaksionin me ID specifike
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            // Gjeneron nje invoice si PDF
            byte[] invoicePdf = InvoiceGenerator.GenerateInvoicePdf(transaction);

            // E ruan kete PDF ne temporaty file
            var tempFilePath = Path.GetTempFileName();
            System.IO.File.WriteAllBytes(tempFilePath, invoicePdf);

            // Kthen PDF si nje Downloadable file
            var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", "invoice.pdf", true);
        }
    }
}
