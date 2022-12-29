using GuessTheGameApi.Converter.Models.Domain;

namespace GuessTheGameApi.Domain.Interfaces
{
    public interface IGameDomain
    {
        public Task<GameApiResult.Result> CallApiAsync();
        public Task<IEnumerable<Game>> GetGamesAsync();
        public Task<IEnumerable<CurrentLevel>> GetCurrentLevelsAsync();
        public Task<Game> CreateGameAsync(Game game);
        public Task<CurrentLevel> UpdateCurrentLevelAsync(int score, int combo, int idUser);
        public Task<Credentials> GetCredentialsAsync(string username, string password);
        public Task<Credentials> RegisterUserAsync(string username, string password);
        public Task<CurrentLevel> CreateCurrentLevelAsync(int idUSer);
    }
}
