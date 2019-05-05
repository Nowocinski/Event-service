using System;
using System.Threading.Tasks;
using Evento.Infrastructure.Commends;
using Evento.Infrastructure.Commends.Users;
using Evento.Infrastructure.Services.User;

namespace Evento.Infrastructure.Handlers.Users
{
    public class RegisterHandler : ICommandHandler<Register>
    {
        private readonly IUserService _userService;
        public RegisterHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task HandleAsync(Register command)
        {
            await _userService.RegisterAsync(Guid.NewGuid(), command.Email, command.Name, command.Password, command.Role);
        }
    }
}
