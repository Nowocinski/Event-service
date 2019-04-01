using AutoMapper;
using Evento.Core.Domain;
using Evento.Core.Repositories;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Services.User.JwtToken;
using System;
using System.Threading.Tasks;

namespace Evento.Infrastructure.Services.User
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMapper _mapper;

        public UserService(IAccountRepository userRepository, IJwtHandler jwtHandler, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }

        public async Task<AccountDTO> GetAccountAsync(Guid UserId)
        {
            var user = await _userRepository.GetAsync(UserId);
            if (user == null)
                throw new Exception($"User with id: '{UserId}' does not exist.");

            return _mapper.Map<AccountDTO>(user);
        }

        public async Task RegisterAsync(Guid UserId, string Email, string Name, string Password, string Role = "user")
        {
            var user = await _userRepository.GetAsync(Email);
            if (user != null)
                throw new Exception($"User e-mail: '{Email}' already exists.");

            user = new Account(UserId, Role, Name, Email, Password);
            await _userRepository.AddAsync(user);
        }

        public async Task<TokenDTO> LoginAsync(string Email, string Password)
        {
            var user = await _userRepository.GetAsync(Email);
            if (user == null) 
                throw new Exception("Invalid credentials.");

            if (user.Password != Password)
                throw new Exception("Invalid credentials.");

            var jwt = _jwtHandler.CreateToken(user.Id, user.Role);

            return new TokenDTO
            {
                Token = jwt.Token,
                Expires = jwt.Expires,
                Role = user.Role
            };
        }
    }
}
