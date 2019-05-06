using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.Services.User;
using Evento.Infrastructure.Services.User.JwtToken;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Testowanie_prawidłowej_rejestracji_użytkownika()
        {
            // Arrange
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var userService = new UserService(accountRepositoryMock.Object,
                jwtHandlerMock.Object, mapperMock.Object);
            // Act
            await userService.RegisterAsync(Guid.NewGuid(), "test@test.com", "test", "secret", "test");
            // Assert
            accountRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Account>()), Times.Once);
        }

        [Fact]
        public async Task Testowanie_prawidłowej_pobierania_użytkownika()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new DataBaseContext(options);
            Account account = new Account(Guid.NewGuid(), "test", "test", "test@test.com", "secret");
            var accountRepositoryMock = new Mock<IAccountRepository>();
            var jwtHandlerMock = new Mock<IJwtHandler>();
            var mapperMock = new Mock<IMapper>();
            var userService = new UserService(accountRepositoryMock.Object,
                jwtHandlerMock.Object, mapperMock.Object);
            // Act
            accountRepositoryMock.Setup(x => x.GetAsync(account.Id)).ReturnsAsync(account);
            var accountDTO = await userService.GetAccountAsync(account.Id);
            // Assert
            accountRepositoryMock.Verify(x => x.GetAsync(account.Id), Times.Once);
        }
    }
}
