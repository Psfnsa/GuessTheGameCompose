using GuessTheGameApi.Converter.Converters;
using GuessTheGameApi.Converter.Models.Domain;
using GuessTheGameApi.DataAccess.Repositories;
using Newtonsoft.Json;

namespace GuessTheGameApi.Domain.Interfaces
{
    public class GameDomain : IGameDomain
    {
        bool existResult = false;
        Random random = new Random();
        GameApiResult gamesApiResult = new GameApiResult();
        IEnumerable<Game> listGames = new List<Game>();

        private readonly IGameRepository _gameRepository;
        private readonly IGameConverter _gameConverter;
        private readonly ICurrentLevelConverter _currentLevelConverter;
        private readonly ICredentialsConverter _credentialsConverter;

        static HttpClient client = new HttpClient();

        public GameDomain(IGameRepository gameRepository, IGameConverter gameConverter, ICurrentLevelConverter currentLevelConverter, ICredentialsConverter credentialsConverter)
        {
            _gameRepository = gameRepository;
            _gameConverter = gameConverter;
            _currentLevelConverter = currentLevelConverter;
            _credentialsConverter = credentialsConverter;
        }

        public async Task<GameApiResult.Result> CallApiAsync()
        {
            if (!existResult)
            {
                client.DefaultRequestHeaders.Add("User-Agent", "User-Agent-Here");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var stringTask = client.GetStringAsync("https://www.gamespot.com/api/games/?api_key=4e0e4fe96df6e1cb3210d771dbc868f830faea01&format=json");
                var result = await stringTask;
                listGames = await GetGamesAsync();
                gamesApiResult = JsonConvert.DeserializeObject<GameApiResult>(result);
            }
            int randomNumber = 0;
            bool existGame = true;

            //check if game exist in list, if exist don't send it to the site
            while (existGame)
            {
                randomNumber = random.Next(0, gamesApiResult.results.Length);

                if (listGames.ToList().Count == 0)
                {
                    break;
                }

                if (!listGames.Where(g => g.Id == gamesApiResult.results[randomNumber].id).Any())
                {
                    break;
                }
            }

            var gameResult = gamesApiResult.results[randomNumber];

            return gameResult;
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            var listGames = await _gameRepository.GetGamesAsync();
            var listGamesModel = listGames.Select(g => _gameConverter.FromEntityToModel(g));

            return listGamesModel;
        }

        public async Task<IEnumerable<CurrentLevel>> GetCurrentLevelsAsync()
        {
            var listCurrentLevels = await _gameRepository.GetCurrentLevelsAsync();
            var listCurrentLevelsModel = listCurrentLevels.Select(cl => _currentLevelConverter.FromEntityToModel(cl));

            return listCurrentLevelsModel;
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            var gameResulted = await _gameRepository.CreateGameAsync(_gameConverter.FromModelToEntity(game));
            var gameModel = _gameConverter.FromEntityToModel(gameResulted);

            return gameModel;
        }

        public async Task<CurrentLevel> UpdateCurrentLevelAsync(int score, int combo, int idUser)
        {
            var currentLevel = await _gameRepository.UpdateCurrentLevelAsync(score, combo, idUser);
            var currentLevelModel = _currentLevelConverter.FromEntityToModel(currentLevel);

            return currentLevelModel;
        }

        public async Task<Credentials> GetCredentialsAsync(string username, string password)
        {
            var credentials = await _gameRepository.GetCredentialsAsync(username, password);
            var crendentialsModel = _credentialsConverter.FromEntityToModel(credentials);
            return crendentialsModel;
        }

        public async Task<Credentials> RegisterUserAsync(string username, string password)
        {
            var credentialsEntity = await _gameRepository.RegisterUserAsync(username, password);
            var credentialsModel = _credentialsConverter.FromEntityToModel(credentialsEntity);

            return credentialsModel;
        }

        public async Task<CurrentLevel> CreateCurrentLevelAsync(int idUSer)
        {
            var currentLevelEntity = await _gameRepository.CreateCurrentLevelAsync(idUSer);

            var currentLevelModel = _currentLevelConverter.FromEntityToModel(currentLevelEntity);

            return currentLevelModel;
        }
    }
}
