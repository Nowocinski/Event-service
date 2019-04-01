using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Extensions;
using Evento.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Evento.Infrastructure.Services.User.JwtToken
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _jwtSettings;

        public JwtHandler(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public JwtDTO CreateToken(Guid UserId, string Role)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserId.ToString()),        // Klucz użytkownika
                new Claim(JwtRegisteredClaimNames.UniqueName, UserId.ToString()), // Unikatowa nazwa użytkownika
                new Claim(ClaimTypes.Role, Role),                                 // Rola użytkownika
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),// Unikalny identyfikator tokena
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())      // Data wydania tokena
            };

            var expires = now.AddMinutes(_jwtSettings.ExpiryInMinutes);           // Data wyczerpania tokena

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Site, // Strony mogące korzystać z tokena
                claims: claims,            // Lista ról użytkownika
                notBefore: now,            // Blokowanie używania tokena z inną datą niż teraz
                expires: expires,          // Data wyczerpania
                signingCredentials: signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtDTO
            {
                Token = token,
                Expires = expires.ToTimestamp()
            };
        }
    }
}
