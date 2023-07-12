using AutoMapper;
using Microsoft.AspNet.Identity;
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

        // Add more tests for other methods in the UsersService class
    }
}
