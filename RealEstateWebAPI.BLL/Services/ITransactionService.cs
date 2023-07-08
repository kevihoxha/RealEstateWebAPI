using RealEstateWebAPI.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    public interface ITransactionService
    {
        Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionRequest);
        Task<TransactionDTO> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync();
    }
}
