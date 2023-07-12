﻿using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController( ITransactionService transactionService)
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
            return await HandleAsync <TransactionDTO>(async () =>
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                return Ok(transaction);
            });
        }

        [HttpPost("create")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction(TransactionDTO transactionRequest)
        {
            return await HandleAsync <TransactionDTO> (async () =>
            {
                var createdTransaction = await _transactionService.CreateTransactionAsync(transactionRequest);
                return Ok(createdTransaction);
            });
        }
    }
}
