using GuessTheGameApi.Converter.Models.Domain;

namespace GuessTheGameApi.Domain.Interfaces
{
    public interface IJwtAndHashGenerator
    {
        public string HashPassword(string password);
        public Task<Credentials> RegisterUserAsync(string username, string password);
        public string GenerateJSONWebToken(Credentials userInfo);
    }
}
