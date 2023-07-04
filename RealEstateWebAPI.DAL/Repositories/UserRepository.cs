using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstateWebAPI.DAL.Repositories.UserRepository;

namespace RealEstateWebAPI.DAL.Repositories
{
        public class UserRepository : IUserRepository
        {
            private readonly AppDbContext _dbContext;

            public UserRepository(AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IEnumerable<User>> GetAllUsersAsync()
            {
                return await _dbContext.Users.ToListAsync();
            }

            public async Task<User> GetUserByIdAsync(int userId)
            {
                return await _dbContext.Users.FindAsync(userId);
            }

            public async Task AddUserAsync(User user)
            {
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }

            public async Task UpdateUserAsync(User user)
            {
                _dbContext.Entry(user).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            public async Task DeleteUserAsync(int userId)
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
