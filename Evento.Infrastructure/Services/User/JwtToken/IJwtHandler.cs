using Evento.Infrastructure.DTO;
using System;

namespace Evento.Infrastructure.Services.User.JwtToken
{
    public interface IJwtHandler
    {
        JwtDTO CreateToken(Guid UserId, string Role);
    }
}
