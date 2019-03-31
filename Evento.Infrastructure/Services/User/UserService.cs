using Evento.Core.Domain;
using Evento.Core.Repositories;
using System;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository _userRepository;

        public UserService(IAccountRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterAsync(Guid UserId, string Email, string Name, string Password, string Role = "user")
        {
            var user = await _userRepository.GetAsync(Email);
            if (user != null)
                throw new Exception($"User e-mail: '{Email}' already exists.");

            user = new Account(UserId, Role, Name, Email, Password);
            await _userRepository.AddAsync(user);
        }

        public async Task LoginAsync(string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user == null || user.Password != Password)
                throw new Exception("Invalid credentials.");
        }
    }
}
