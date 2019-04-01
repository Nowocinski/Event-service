using System;
using System.Threading.Tasks;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Evento.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataBaseContext _context;

        public AccountRepository(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Account> GetAsync(Guid Id)
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == Id);
            return await Task.FromResult(user);
        }

        public async Task<Account> GetAsync(string Email)
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Email == Email);
            return await Task.FromResult(user);
        }

        public async Task AddAsync(Account User)
        {
            _context.Accounts.Add(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Account User)
        {
            _context.Accounts.Update(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Account User)
        {
            _context.Accounts.Remove(User);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
