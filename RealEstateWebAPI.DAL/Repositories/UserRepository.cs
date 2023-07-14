using iText.Commons.Utils;
using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstateWebAPI.DAL.Repositories.UserRepository;

namespace RealEstateWebAPI.DAL.Repositories
{/// <summary>
/// Repository qe meaxhon te dhenat per User.
/// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<User> _users;
        /// <summary>
        /// Konstruktori me parametra per  <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _users = _dbContext.Users;
        }
        /// <summary>
        /// Merr te gjithe users asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Userash.</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _users.ToListAsync();
        }
        /// <summary>
        /// Merr nje use me ane te Id asinkronisht.
        /// </summary>
        /// <param name="userId">Id e User qe do te kapi</param>
        /// <returns>Userin me Id specifike,ose null nese nuk e gjen</returns>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _users.FindAsync(userId);
        }
        /// <summary>
        /// Merr nje use me ane te Username asinkronisht.
        /// </summary>
        /// <param name="username">UserName e User qe do te kapi</param>
        /// <returns>Userin me UserName specifike,ose null nese nuk e gjen.</returns>
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _users.SingleOrDefaultAsync(u => u.UserName == username);
        }
        /// <summary>
        /// Merr nje use me ane te Email asinkronisht.
        /// </summary>
        /// <param name="email">Email e User qe do te kapi</param>
        /// <returns>Userin me Email specifike,ose null nese nuk e gjen.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.SingleOrDefaultAsync(u => u.Email == email);
        }
        /// <summary>
        /// Shton nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te shtoje</param>
        public async Task AddUserAsync(User user)
        {
            _users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Modifikon nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te modifikoje.</param>
        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Fshin nje User asinkronisht.
        /// </summary>
        /// <param name="userId">Id e usierit qe do te fshihet.</param>
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _users.FindAsync(userId);
            if (user != null)
            {
                _users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
       
    }
}
