using Application.Interfaces;
using Application.Models.Request;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IUserRepository _usersRepository;
        private readonly AuthenticationServiceOptions _options;

        public AuthenticationService(IUserRepository usersRepository, IOptions<AuthenticationServiceOptions> options)
        {
            _usersRepository = usersRepository;
            _options = options.Value;
        }

        private async Task<Domain.Entities.User?> ValidateUser(string email, string password)
        {
            var user = await _usersRepository.GetByEmailAsync(email);

            if (user is null)
                return null;

            // Comparación directa de contraseñas en texto plano
            if (user.Password != password)
                return null;

            return user;
        }

        public async Task<string> AuthenticateAsync(AuthenticationRequestDTO authenticationRequestDTO)
        {
            // Validar credenciales
            var validatedUser = await ValidateUser(authenticationRequestDTO.Email, authenticationRequestDTO.Password);

            if (validatedUser is null)
                throw new UnauthorizedAccessException("Credenciales inválidas");

            // Crear token JWT
            var securityPassword = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_options.SecretForKey));

            var signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
            {
                new Claim("sub", validatedUser.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, validatedUser.Id.ToString()),
                new Claim(ClaimTypes.Email, validatedUser.Email),
                new Claim(ClaimTypes.Role, validatedUser.Rol.ToString()),
                new Claim("name", validatedUser.Name),
                new Claim("lastName", validatedUser.LastName)
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(24), // Token válido por 24 horas
                signature
            );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return tokenToReturn;
        }
    }

    public class AuthenticationServiceOptions
    {
        public const string Authentication = "Authentication";

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required]
        public string SecretForKey { get; set; } = string.Empty;
    }
}