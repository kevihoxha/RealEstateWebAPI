using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionRequest)
        {
            try
            {

                var transaction = _mapper.Map<Transaction>(transactionRequest);


                var createdTransaction = await _transactionRepository.AddTransactionAsync(transaction);


                var transactionDTO = _mapper.Map<TransactionDTO>(createdTransaction);

                return transactionDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create transaction.");
                throw;
            }
        }

        public async Task<TransactionDTO> GetTransactionByIdAsync(int transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            return _mapper.Map<TransactionDTO>(transaction);
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();
            return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
        }

    }
}
