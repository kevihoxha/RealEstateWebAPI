using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using RealEstateWebAPI.BLL.DTO;
    using RealEstateWebAPI.Common.ErrorHandeling;
    using RealEstateWebAPI.DAL.Entities;
    using RealEstateWebAPI.DAL.Repositories;
    using Serilog;
    using System.Collections.Generic;
    using System.Security.AccessControl;
    using System.Threading.Tasks;

    public class UsersService : ExceptionHandling, IUsersService
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
            return await HandleAsync(async () =>
            {
                var users = await _userRepository.GetAllUsersAsync();
                if (users != null)
                {
                    Log.Information($"Retrieved {users.Count()} users.");
                    return _mapper.Map<IEnumerable<UserDTO>>(users);
                }
                throw new CustomException("No users found.");
            });
        }
        /// <summary>
        /// Merr nje use me ane te Id asinkronisht.
        /// </summary>
        /// <param name="userId">Id e User qe do te kapi</param>
        /// <returns>Userin me Id specifike,ose null nese nuk e gjen</returns>
        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await HandleAsync(async () =>
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    Log.Information($"Retrieved user with ID: {user.UserId}, Username: {user.UserName}, Email: {user.Email}, Role: {user.Role}");
                    return _mapper.Map<UserDTO>(user);
                }
                throw new CustomException($"User with ID: {userId} not found.");
            });
        }
        /// <summary>
        /// Shton nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te shtoje</param>
        public async Task<int> AddUserAsync(UserDTO userDTO)
        {
            return await HandleAsync(async () =>
            {

                await ValidateUserDTOAsync(userDTO);
                userDTO.RoleId = 2;
                var user = _mapper.Map<User>(userDTO);
                byte[] salt;
                user.PasswordHash = PasswordHashing.HashPasword(userDTO.Password, out salt);
                user.PasswordSalt = salt;
                await _userRepository.AddUserAsync(user);
                if (user.UserId > 0)
                {
                    Log.Information($"User added successfully. UserID: {user.UserId}, Username: {user.UserName}, RoleID: {user.RoleId}");
                    return user.UserId;
                }
                throw new CustomException($"User with username '{user.UserName}' could not be added.");
            });
        }
        /// <summary>
        /// Modifikon nje User te ri asinkronisht.
        /// </summary>
        /// <param name="user">User qe do te modifikoje.</param>
        public async Task UpdateUserAsync(int userId, UserDTO userDTO)
        {
             await HandleAsync(async () =>
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    userDTO.UserId= userId;
                    userDTO.RoleId= 2;
                    _mapper.Map<UserDTO, User>(userDTO, user);
                    await _userRepository.UpdateUserAsync(user);
                    Log.Information($"User updated successfully. UserID: {user.UserId}, Username: {user.UserName}, RoleID: {user.RoleId}");
                }
                else
                {
                    throw new CustomException($"User with ID: { userId } does not exist.");
                }
            });
        }
        /// <summary>
        /// Fshin nje User asinkronisht.
        /// </summary>
        /// <param name="userId">Id e usierit qe do te fshihet.</param>
        public async Task DeleteUserAsync(int userId)
        {
             await HandleAsync(async () =>
             {
                 
                 var userToDelete = await _userRepository.GetUserByIdAsync(userId);
                if (userToDelete == null)
                {
                    throw new CustomException($"User with ID: {userId} not found.");
                }
                 var associatedProperties = await _propertyRepository.GetPropertiesByUserIdAsync(userId);
                 if (associatedProperties.Any())
                 {
                     throw new CustomException($"Cannot delete user with ID {userId} as it is associated with one or more properties.");

                 }
                 await _userRepository.DeleteUserAsync(userId);
                Log.Information($"User {userToDelete.UserName} with ID: {userToDelete.UserId} has been deleted.");
            });
        }
        /// <summary>
        /// Merr nje user me ane te Username asinkronisht.
        /// </summary>
        /// <param name="username">UserName e User qe do te kapi</param>
        /// <returns>Userin me UserName specifike,ose null nese nuk e gjen.</returns>
        public async Task<UserDTO> GetUserByUserNameAsync(string username)
        {
            return await HandleAsync(async () =>
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                if (user != null)
                {
                    Log.Information($"Successfully retrieved the user with username: {user.UserName}");
                    return _mapper.Map<UserDTO>(user);
                }
                throw new CustomException($"User with username: {username} not found.");
            });
        }
        /// <summary>
        /// Merr nje user me ane te Email asinkronisht.
        /// </summary>
        /// <param name="email">Email e User qe do te kapi</param>
        /// <returns>Userin me Email specifike,ose null nese nuk e gjen.</returns>
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            return await HandleAsync(async () =>
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    Log.Information($"Successfully retrieved the user with email: {user.Email}");
                    return _mapper.Map<UserDTO>(user);
                }
                throw new CustomException($"User with email: {email} not found.");

            });
        }
        /// <summary>
        /// Merr nje property me ane te userid asinkronisht.
        /// </summary>
        /// <param name="username">UserId e User qe do te kapi property te tij</param>
        /// <returns>Koleksion te properties qe ai user zoteron</returns>
        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByUserIdAsync(int userId)
        {
            return await HandleAsync(async () =>
            {
                var properties = await _propertyRepository.GetPropertiesByUserIdAsync(userId);
                if (properties.Count() > 0)
                {
                    Log.Information($"Successfully retrieved {properties.Count()} properties for user with ID: {userId}");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                throw new CustomException($"No properties found for user with ID: {userId}.");

            });
        }
        private async Task ValidateUserDTOAsync(UserDTO userDTO)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(userDTO.UserName);
            if (existingUser != null)
            {
                throw new CustomException($"User with username {userDTO.UserName} already exists");
            }

            existingUser = await _userRepository.GetUserByEmailAsync(userDTO.Email);
            if (existingUser != null)
            {
                throw new CustomException($"User with email {userDTO.Email} already exists");
            }
        }
    }
}
