using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<AppDbContext> _dbContextMock;
        private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            // Set up the DbContext mock and UserRepository
            _dbContextMock = new Mock<AppDbContext>();
            _userRepository = new UserRepository(_dbContextMock.Object);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { UserId = 1, UserName = "User1" },
                new User { UserId = 2, UserName = "User2" }
            };

            var userDbSetMock = MockDbSet(users);
            _dbContextMock.Setup(db => db.Users).Returns(userDbSetMock.Object);

            // Act
            var result = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        // Write similar test methods for other UserRepository methods

        // ...

        private static Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return dbSetMock;
        }
    }
}
}
