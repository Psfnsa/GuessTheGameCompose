using GuessTheGameApi.Converter.Models.DataAccess;
using GuessTheGameApi.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace GuessTheGameApi.DataAccess.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GuessTheGameDBContext _context;

        public GameRepository(GuessTheGameDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await _context.Game.ToListAsync();
        }        
        
        public async Task<IEnumerable<CurrentLevel>> GetCurrentLevelsAsync()
        {
            return await _context.CurrentLevel.ToListAsync();
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            _context.Game.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public  async Task<CurrentLevel> UpdateCurrentLevelAsync(int score, int combo, int idUser)
        {
            CurrentLevel currentLevel = new CurrentLevel();
            var listGames = await GetGamesAsync();
            var listCurrentLevels = await GetCurrentLevelsAsync();

            for (int i = 0; i < listCurrentLevels.Count(); i++)
            {
                currentLevel = listCurrentLevels.ElementAt(i);

                if (idUser == currentLevel.IdUser)
                {
                    currentLevel.Score = score;
                    currentLevel.Combo = combo;
                    currentLevel.ListGames = listGames.ToList();
                }
            }
            await _context.SaveChangesAsync();

            return currentLevel;
        }

        public async Task<Credentials> GetCredentialsAsync(string username, string password)
        {

            var listCredentials = await _context.Credentials.ToListAsync();
            Credentials credentials = null;

            if (listCredentials.Where(cr => cr.Username == username && cr.Password == password).Any())
            {
                credentials = listCredentials.Where(cr => cr.Username == username && cr.Password == password).First();
            }

            return credentials;
        }        
        
        public async Task<IEnumerable<Credentials>> GetListCredentialsAsync()
        {
            var listCredentials = await _context.Credentials.ToListAsync();

            return listCredentials;
        }

        public async Task<Credentials> RegisterUserAsync(string username, string password)
        {
            Credentials credentials = new Credentials()
            {
                Username = username,
                Password = password,
            };

            var listCredentials = await GetListCredentialsAsync();

            if (!listCredentials.Where(cr => cr.Username == username).Any())
            {
                _context.Credentials.Add(credentials);

                await _context.SaveChangesAsync();
            }

            if (listCredentials.Where(cr => cr.Username == username).Any())
            {
                new Credentials()
                {
                    Username = "0",
                    Password = "0",
                };
            }

            return credentials;
        }

        public async Task<CurrentLevel> CreateCurrentLevelAsync(int idUSer)
        {
            var currentLevel = new CurrentLevel()
            {
                Combo = 0,
                Score = 0,
                IdUser = idUSer,
            };

            _context.Add(currentLevel);
            await _context.SaveChangesAsync();

            return currentLevel;
        }

    }
}
