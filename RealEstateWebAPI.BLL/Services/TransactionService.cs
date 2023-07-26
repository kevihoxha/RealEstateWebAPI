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
        private readonly IPropertyRepository _propertyRepository;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IPropertyRepository propertyRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
        }
        /// <summary>
        /// Shton nje Transaction te ri asinkronisht.
        /// </summary>
        /// 
        public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionRequest)
        {
            return await HandleAsync(async () =>
            {
                var property = await _propertyRepository.GetPropertyByIdAsync(transactionRequest.PropertyId);
                if (property == null)
                {
                    throw new CustomException($"Property with ID: {transactionRequest.PropertyId} not found.");
                }
                transactionRequest.SalePrice = property.Price;
                var transaction = _mapper.Map<Transaction>(transactionRequest);
                var createdTransaction = await _transactionRepository.AddTransactionAsync(transaction);
                var transactionDTO = _mapper.Map<TransactionDTO>(createdTransaction);
                Log.Information($"Transaction added successfully. Transaction ID: {transactionDTO.TransactionId}, Amount: {transactionDTO.SalePrice}, Date: {transactionDTO.TransactionDate}");
                return transactionDTO;
            });
        }
        /// <summary>
        /// Merr nje Transaction me ane te Id asinkronisht.
        /// </summary>
        public async Task<TransactionDTO> GetTransactionByIdAsync(int transactionId)
        {
            return await HandleAsync(async () =>
            {
                var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
                if (transaction != null)
                {
                    Log.Information($"Retrieved transaction with ID: {transaction.TransactionId}, Amount: {transaction.SalePrice}, Date: {transaction.TransactionDate}");
                    return _mapper.Map<TransactionDTO>(transaction);
                }
                throw new CustomException($"Transaction with ID: {transactionId} not found.");
            });
        }
        /// <summary>
        /// Merr te gjithe Transactions asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Transactions.</returns>
        public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
        {
            return await HandleAsync(async () =>
            {
                var transactions = await _transactionRepository.GetAllTransactionsAsync();
                if (transactions != null)
                {
                    Log.Information($"Retrieved {transactions.Count()} transactions.");
                    return _mapper.Map<IEnumerable<TransactionDTO>>(transactions);
                }
                throw new CustomException("No Transactions Found");
            });
        }

    }
}
