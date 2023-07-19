﻿using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<Transaction> _transactions;

        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _transactions = _dbContext.Transactions;
        }
        /// <summary>
        /// Shton nje Transaction te ri asinkronisht.
        /// </summary>
        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            _transactions.Add(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        /// <summary>
        /// Merr nje Transaction me ane te Id asinkronisht.
        /// </summary>
        public async Task<Transaction> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactions.FindAsync(transactionId);
        }

        /// <summary>
        /// Merr te gjithe Transactions asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Transactions.</returns>
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactions.ToListAsync();
        }


    }
}
