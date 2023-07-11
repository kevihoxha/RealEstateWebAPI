using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RealEstateWebAPI.BLL.DTO;
    using RealEstateWebAPI.DAL.Entities;
    using RealEstateWebAPI.DAL.Repositories;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher _passwordHasher;

        public UsersService(IUserRepository userRepository, IMapper mapper, PasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> AddUserAsync(UserDTO userDTO)
        {
            string hashedPassword = _passwordHasher.HashPassword(userDTO.PasswordHash);
            var user = _mapper.Map<User>(userDTO);
            user.PasswordHash = hashedPassword;
            await _userRepository.AddUserAsync(user);
            return user.UserId;
        }

        public async Task UpdateUserAsync(int userId, UserDTO userDTO)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user != null)
            {
                _mapper.Map<UserDTO, User>(userDTO, user);
                await _userRepository.UpdateUserAsync(user);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<UserDTO> GetUserByUserNameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<UserDTO>(user);
           
        }
        public async Task<bool> VerifyPasswordAsync(string username, string password)
        {
            // Retrieve the user by username
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return false; // User not found
            }

            // Verify the password
            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }

    }


}
