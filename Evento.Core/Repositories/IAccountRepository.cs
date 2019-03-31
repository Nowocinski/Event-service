using Evento.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Evento.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAsync(Guid Id);
        Task<Account> GetAsync(string Email);
        Task AddAsync(Account User);
        Task UpdateAsync(Account User);
        Task DeleteAsync(Account User);
    }
}
