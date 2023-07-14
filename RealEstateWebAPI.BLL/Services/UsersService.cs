﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using RealEstateWebAPI.BLL.DTO;
    using RealEstateWebAPI.Common.ErrorHandeling;
    using RealEstateWebAPI.DAL.Entities;
    using RealEstateWebAPI.DAL.Repositories;
    using Serilog;
    using System.Collections.Generic;
    using System.Security.AccessControl;
    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyRepository _propertyRepository;

        public UsersService(IUserRepository userRepository, IMapper mapper, IPropertyRepository propertyRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _propertyRepository = propertyRepository;
        }
        /// <summary>
        /// Merr te gjithe users asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion UserDTOs</returns>
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
                var users = await _userRepository.GetAllUsersAsync();
                if (users != null)
                {
                    Log.Information("Got all Users");
                    return _mapper.Map<IEnumerable<UserDTO>>(users);
                }
                Log.Error("Error Getting Users");
                throw new CustomException("Couldnt get users");  
        }
        /// <summary>
        /// Merr nje use me ane te Id asinkronisht.
        /// </summary>
        /// <param name="userId">Id e User qe do te kapi</param>
        /// <returns>Userin me Id specifike,ose null nese nuk e gjen</returns>
        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    Log.Information($"Got user with ID: {userId}");
                    return _mapper.Map<UserDTO>(user);
                }

                Log.Error($"User with ID: {userId} not found");
                throw new CustomException("User not found");
            }
            catch (CustomException ex)
            {
                Log.Error(ex, "CustomException in GetUserByIdAsync");
                throw; 
            }
        }



        /// <summary>
        /// Shton nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te shtoje</param>
        public async Task<int> AddUserAsync(UserDTO userDTO)
        {
            try
            {
                if(await _userRepository.GetUserByUsernameAsync(userDTO.UserName) != null)
                {
                    throw new CustomException($"User with username {userDTO.UserName} already exists");
                }
                if (await _userRepository.GetUserByEmailAsync(userDTO.Email) != null)
                {
                    throw new CustomException($"User with email {userDTO.Email} already exists");
                }

                var user = _mapper.Map<User>(userDTO);
                byte[] salt;
                user.PasswordHash = PasswordHashing.HashPasword(userDTO.Password, out salt);
                user.PasswordSalt = salt;
                await _userRepository.AddUserAsync(user);
                Log.Information("User added succesfully");
                return user.UserId;
            }
            catch (CustomException)
            {
                throw;
            }

        }
        /// <summary>
        /// Modifikon nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te modifikoje.</param>
        public async Task UpdateUserAsync(int userId, UserDTO userDTO)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    _mapper.Map<UserDTO, User>(userDTO, user);
                    await _userRepository.UpdateUserAsync(user);
                    Log.Information("User updated succesfully");
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex, "Error while updating user");
            }
        }
        /// <summary>
        /// Fshin nje User asinkronisht.
        /// </summary>
        /// <param name="userId">Id e usierit qe do te fshihet.</param>
        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                var userToDelete = await _userRepository.GetUserByIdAsync(userId);
                if (userToDelete == null)
                {
                    Log.Error("User not found");
                    throw new CustomException("User not found");
                }
                await _userRepository.DeleteUserAsync(userId);
                Log.Information($"User {userToDelete.UserName} got deleted");
            }
            catch (CustomException )
            {
                throw;
            }
        }
        /// <summary>
        /// Merr nje user me ane te Username asinkronisht.
        /// </summary>
        /// <param name="username">UserName e User qe do te kapi</param>
        /// <returns>Userin me UserName specifike,ose null nese nuk e gjen.</returns>
        public async Task<UserDTO> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user != null)
                {
                    Log.Information($"Got the user {user.UserName} by name");
                    return _mapper.Map<UserDTO>(user);
                }
                Log.Error("Couldnt get this user");
            }
            catch (Exception ex)
            {

            }
            return new UserDTO();

        }
        /// <summary>
        /// Merr nje user me ane te Email asinkronisht.
        /// </summary>
        /// <param name="email">Email e User qe do te kapi</param>
        /// <returns>Userin me Email specifike,ose null nese nuk e gjen.</returns>
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    Log.Information($"Got the user {user.Email} by email");
                    return _mapper.Map<UserDTO>(user);
                }
                Log.Error("Couldnt get this user");
            }
            catch (Exception ex)
            {

            }
            return new UserDTO();

        }
        /// <summary>
        /// Merr nje property me ane te userid asinkronisht.
        /// </summary>
        /// <param name="username">UserId e User qe do te kapi property te tij</param>
        /// <returns>Koleksion te properties qe ai user zoteron</returns>
        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByUserIdAsync(int userId)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesByUserIdAsync(userId);
                if (properties != null)
                {
                    Log.Information("Got Properties by userId");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                Log.Error("Couldn't get Properties by UserId");
                throw new CustomException("UserId not found");
            }
            catch (CustomException ex)
            {
                throw; // Re-throw the same exception to be caught by the middleware
            }
        }


    }


}
