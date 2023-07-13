using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.DAL;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using System.Collections.Generic;

namespace NUnitTesting
{
    [TestFixture]
    public class UsersServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IUsersService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UsersService(_userRepositoryMock.Object, _mapperMock.Object);
        }

        /// <summary>
        /// Teston metoden GetAllUsersAsync nga klasa UsersService.
        /// Duhet te ktheje te gjithe users.
        /// </summary>
        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User> { new User { UserId = 1, UserName = "user1" }, new User { UserId = 2, UserName = "user2" } };
            var userDTOs = new List<UserDTO> { new UserDTO { UserId = 1, UserName = "user1" }, new UserDTO { UserId = 2, UserName = "user2" } };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(userDTOs, result);
        }
        /// <summary>
        /// Teston metoden GetAllUsersAsync nga klasa UsersService.
        /// Duhet te kthehe numrin e sakte e users.
        /// </summary>
        [Test]
        public async Task GetAllUsersAsync_ReturnsNumberOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { UserId = 1, UserName = "user1" }, new User { UserId = 2, UserName = "user2" } };
            var userDTOs = new List<UserDTO> { new UserDTO { UserId = 1, UserName = "user1" }, new UserDTO { UserId = 2, UserName = "user2" } };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<UserDTO>>(users)).Returns(userDTOs);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(userDTOs.Count, 2);
        }
        /// <summary>
        /// Teston metoden GetUserByIdAsyncnga klasa UsersService.
        /// Duhet te ktheje nje UserDTO per nje user qe ekziston me ate ID.
        /// </summary>
        [Test]
        public async Task GetUserByIdAsync_ExistingUserId_ShouldReturnUserDTO()
        {
            // Arrange
            int userId = 1;
            var user = new User();
            var userDTO = new UserDTO();

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDTO);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.AreEqual(userDTO, result);
        }
        /// <summary>
        /// Teston metoden AddUserAsync per klasen UsersService.
        /// Duhet te ktheje user ID per UserDTO e sapo shtuar.
        /// </summary>
        [Test]
        public async Task AddUserAsync_ValidUserDTO_ShouldReturnUserId()
        {
            // Arrange
            var userDTO = new UserDTO { UserName = "john", Password = "password" };
            var user = new User { UserId = 1 };

            _mapperMock.Setup(mapper => mapper.Map<User>(userDTO)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(user));

            // Act
            var result = await _userService.AddUserAsync(userDTO);

            // Assert
            Assert.AreEqual(user.UserId, result);
        }
    }
}
