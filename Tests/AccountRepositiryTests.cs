using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AccountRepositiryTests
    {
        [Fact]
        public async Task Poprawne_dodanie_nowego_użytkownika_do_bazy_danych()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                      .UseInMemoryDatabase(Guid.NewGuid().ToString())
                      .Options;
            var context = new DataBaseContext(options);

            Account account = new Account(Guid.NewGuid(), "test", "test", "test@test.com", "secret");
            IAccountRepository accountRepository = new AccountRepository(context);
            // Act
            await accountRepository.AddAsync(account);
            Account existAccount = await accountRepository.GetAsync(account.Id);
            // Assert
            Assert.Equal(account, existAccount);
        }
    }
}
