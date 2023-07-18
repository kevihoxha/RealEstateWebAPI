using AutoMapper;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using Serilog;

namespace RealEstateWebAPI.BLL.Services
{
    public class TransactionService : ExceptionHandling, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Shton nje Transaction te ri asinkronisht.
        /// </summary>
        public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionRequest)
        {
            return await HandleAsync<TransactionDTO>(async () =>
            {
                var transaction = _mapper.Map<Transaction>(transactionRequest);
                var createdTransaction = await _transactionRepository.AddTransactionAsync(transaction);
                var transactionDTO = _mapper.Map<TransactionDTO>(createdTransaction);
                Log.Information("Transaction added succesfully");
                return transactionDTO;
            });
        }
        /// <summary>
        /// Merr nje Transaction me ane te Id asinkronisht.
        /// </summary>
        public async Task<TransactionDTO> GetTransactionByIdAsync(int transactionId)
        {
            return await HandleAsync<TransactionDTO>(async () =>
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
                if (transaction != null)
                {
                    Log.Information($"Got transaction with ID: {transactionId}");
                    return _mapper.Map<TransactionDTO>(transaction);
                }
                throw new CustomException("Transaction not found");
            });
        }
        /// <summary>
        /// Merr te gjithe Transactions asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Transactions.</returns>
        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            return await HandleAsync<IEnumerable<TransactionDTO>>(async () =>
            {
                var transactions = await _transactionRepository.GetAllTransactionsAsync();
                if (transactions != null)
                {
                    Log.Information("Got all Transactions");
                    return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
                }
                throw new CustomException("Couldnt get Transactions");
            });
        }

    }
}
