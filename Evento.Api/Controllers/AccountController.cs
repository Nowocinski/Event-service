using Evento.Infrastructure.Commends.Users;
using Evento.Infrastructure.Services.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Evento.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/account - Pobranie danych konta
        [HttpGet]
        public async Task<ActionResult> GetAccount()
        {
            throw new NotImplementedException();
        }

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
            await _userService.RegisterAsync(Guid.NewGuid(), command.Email, command.Name, command.Password, command.Role);

            return Created("/account", null);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(Login command)
        {
            throw new NotImplementedException();
        }
    }
}
