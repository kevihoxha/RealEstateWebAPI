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
    using Serilog;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                Log.Information("Got all Users");
                return _mapper.Map<IEnumerable<UserDTO>>(users);
                
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Getting Users");
                return Enumerable.Empty<UserDTO>();
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                Log.Information($"Got user will id {userId}");
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Geting a user");
                return new UserDTO();
            }
        }

        public async Task<int> AddUserAsync(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                byte[] salt;
                user.PasswordHash = PasswordHashing.HashPasword(userDTO.Password, out salt);
                user.PasswordSalt = salt;
                await _userRepository.AddUserAsync(user);
                Log.Information("User added succesfully");
                return user.UserId;
            }catch(Exception ex)
            {
                Log.Error(ex, "Error while trying to add user");
                return 0;
            }

        }

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
            }catch (Exception ex)
            {
                Log.Information(ex, "Error while updating user");
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {

                await _userRepository.DeleteUserAsync(userId);
            }catch(Exception ex)
            {
                Log.Information(ex, "Error while deleting user");
            }
        }

        public async Task<UserDTO> GetUserByUserNameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(username);
                Log.Information("Got the user by name");
                return _mapper.Map<UserDTO>(user);
            }catch  (Exception ex)
            {
                Log.Error(ex, "Error while getting a user bt name");
                return new UserDTO();
            }
           
        }

    }


}
