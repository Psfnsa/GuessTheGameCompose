using GuessTheGameApi.Converter.Models.Domain;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GuessTheGameApi.Domain.Interfaces
{
    public class JwtAndHashGenerator : IJwtAndHashGenerator
    {
        private readonly IConfiguration _config;
        private readonly IGameDomain _gameDomain;

        public JwtAndHashGenerator(IConfiguration config, IGameDomain gameDomain)
        {
            _config = config;
            _gameDomain = gameDomain;
        }

        public string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _config["Salt:Key"].Select(b => Convert.ToByte(b)).ToArray(),//we need to convert in byte[]
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public async Task<Credentials> RegisterUserAsync(string username, string password)
        {
            var user = await _gameDomain.RegisterUserAsync(username, password);

            return user;
        }

        public string GenerateJSONWebToken(Credentials userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
