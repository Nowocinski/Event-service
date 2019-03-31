﻿using System;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services.User
{
    public interface IUserService
    {
        Task RegisterAsync(Guid UserId, string Email, string Name, string Password, string Role = "user");
        Task LoginAsync(string Email, string Password);
    }
}
