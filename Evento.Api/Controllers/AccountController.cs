using Evento.Infrastructure.Commends;
using Evento.Infrastructure.Commends.Users;
using Evento.Infrastructure.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Evento.Api.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICommandDispatcher _commandDispatcher;

        public AccountController(IUserService userService, ICommandDispatcher commandDispatcher)
        {
            _userService = userService;
            _commandDispatcher = commandDispatcher;
        }

        // GET: api/account - Pobranie danych konta
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAccount()
            => Json(await _userService.GetAccountAsync(UserId));

        // GET: api/account/tickets - Sprawdzenie biletów użytkownika
        [HttpGet("tickets")]
        public async Task<ActionResult> GetTickets()
        {
            throw new NotImplementedException();
        }

        // POST: api/account/register - Rejestracja
        [HttpPost("register")]
        public async Task<ActionResult> Post([FromBody] Register command)
        {
            await _commandDispatcher.DispatchAsync(command);

            return Created("/account", null);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));
    }
}
