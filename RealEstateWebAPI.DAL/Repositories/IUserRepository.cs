﻿using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Repositories
{
    public interface IUserRepository
    {

            Task<IEnumerable<User>> GetAllUsersAsync();
            Task<User> GetUserByIdAsync(int userId);
            Task AddUserAsync(User user);
            Task UpdateUserAsync(User user);
            Task DeleteUserAsync(int userId);
        }
    }

