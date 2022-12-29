using Xunit;
using Moq;
using GuessTheGameApi.Converter.Models.DataAccess;
using GuessTheGameApi.Domain.Interfaces;
using GuessTheGameApi.DataAccess.Repositories;
using GuessTheGameApi.Converter.Converters;
using Entity = GuessTheGameApi.Converter.Models.DataAccess;
using Model = GuessTheGameApi.Converter.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuessTheGameApi.UnitTesting
{
    public class DomainUnitTest
    {
        public Mock<ICurrentLevelConverter> currentLevelConverter;
        public Mock<ICredentialsConverter> credentialsConverter;
        public Mock<IGameConverter> gameConverter;

        public DomainUnitTest()
        {
            currentLevelConverter = new Mock<ICurrentLevelConverter>();
            credentialsConverter = new Mock<ICredentialsConverter>();
            gameConverter = new Mock<IGameConverter>();
        }
    

        [Fact]
        public async Task GetGamesUnitTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object, gameConverter.Object, 
                currentLevelConverter.Object, credentialsConverter.Object);

            gameConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.Game>())).Returns<Entity.Game>(gameEntity =>
            {
                var gameModel = new Model.Game();

                if (gameEntity != null)
                {
                    gameModel.Id = gameEntity.Id;
                    gameModel.Name = gameEntity.Name;
                    gameModel.IdUser = gameEntity.IdUser;
                    gameModel.ApiId = gameEntity.ApiId;
                }

                return gameModel;
            });

            gameRepository.Setup(g => g.GetGamesAsync()).Returns(Task.FromResult( new List<Game>() {
                new Game { Id = 1, ApiId = 1, IdUser = 1, Name = "Test 1" },
                new Game { Id = 2, ApiId = 2, IdUser = 1, Name = "Test 2" },
                new Game { Id = 3, ApiId = 3, IdUser = 1, Name = "Test 3" },
                new Game { Id = 4, ApiId = 4, IdUser = 1, Name = "Test 4" },
            }.AsEnumerable()));

            var result = await gameDomain.GetGamesAsync();
            Assert.Equal(4, result.Count());
        }  

        [Fact]
        public async Task GetCurrentLevelsUnitTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            currentLevelConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.CurrentLevel>())).Returns<Entity.CurrentLevel>(currentLevelEntity =>
            {
                var currentLevelModel = new Model.CurrentLevel();

                if (currentLevelEntity != null)
                {
                    currentLevelModel.Id = currentLevelEntity.Id;
                    currentLevelModel.Combo = currentLevelEntity.Combo;
                    currentLevelModel.IdUser = currentLevelEntity.IdUser;
                    currentLevelModel.Score = currentLevelEntity.Score;
                    currentLevelModel.ListGames = new List<Model.Game>() {
                        new Model.Game { Id = 1, ApiId = 1, IdUser = 1, Name = "Test 1" },
                        new Model.Game { Id = 2, ApiId = 2, IdUser = 1, Name = "Test 2" },
                        new Model.Game { Id = 3, ApiId = 3, IdUser = 1, Name = "Test 3" },
                        new Model.Game { Id = 4, ApiId = 4, IdUser = 1, Name = "Test 4" },
                    };
                }

                return currentLevelModel;
            });

            gameRepository.Setup(g => g.GetCurrentLevelsAsync()).Returns(Task.FromResult( new List<Entity.CurrentLevel>() {
                new Entity.CurrentLevel() { Id = 1, Score = 10, Combo = 3, IdUser = 1, ListGames = new List<Entity.Game>() },
                new Entity.CurrentLevel() { Id = 1, Score = 10, Combo = 3, IdUser = 1, ListGames = new List<Entity.Game>() },
                }.AsEnumerable()));

            var result = await gameDomain.GetCurrentLevelsAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateGameUniTest()
        {
            var gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            var gameModel = new Model.Game()
            {
                Id = 1,
                ApiId = 1,
                IdUser = 1,
                Name = "Test 7"
            };

            gameConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.Game>())).Returns<Entity.Game>(gEntity =>
            {
                var gModel = new Model.Game();

                if (gEntity != null)
                {
                    gModel.Id = gEntity.Id;
                    gModel.Name = gEntity.Name;
                    gModel.IdUser = gEntity.IdUser;
                    gModel.ApiId = gEntity.ApiId;
                }

                return gModel;
            });            
            
            gameConverter.Setup(g => g.FromModelToEntity(It.IsAny<Model.Game>())).Returns<Model.Game>(gModel =>
            {
                var gEntity = new Entity.Game();

                if (gModel != null)
                {
                    gEntity.Id = gModel.Id;
                    gEntity.Name = gModel.Name;
                    gEntity.IdUser = gModel.IdUser;
                    gEntity.ApiId = gModel.ApiId;
                }

                return gEntity;
            });

            gameRepository.Setup(g => g.CreateGameAsync(It.IsAny<Entity.Game>())).Returns<Entity.Game>(gEntity => {
                return Task.FromResult(gEntity);
                });

            var result = await gameDomain.CreateGameAsync(gameModel);
            Assert.Equal(result.Name, gameModel.Name);
        }

        [Fact]
        public async Task UpdateCurrentLevelUniTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            currentLevelConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.CurrentLevel>())).Returns<Entity.CurrentLevel>(currentLevelEntity =>
            {
                var currentLevelModel = new Model.CurrentLevel();

                if (currentLevelEntity != null)
                {
                    currentLevelModel.Id = currentLevelEntity.Id;
                    currentLevelModel.Combo = currentLevelEntity.Combo;
                    currentLevelModel.IdUser = currentLevelEntity.IdUser;
                    currentLevelModel.Score = currentLevelEntity.Score;
                    currentLevelModel.ListGames = new List<Model.Game>() {
                        new Model.Game { Id = 1, ApiId = 1, IdUser = 1, Name = "Test 1" },
                        new Model.Game { Id = 2, ApiId = 2, IdUser = 1, Name = "Test 2" },
                        new Model.Game { Id = 3, ApiId = 3, IdUser = 1, Name = "Test 3" },
                        new Model.Game { Id = 4, ApiId = 4, IdUser = 1, Name = "Test 4" },
                    };
                }

                return currentLevelModel;
            });

            gameRepository.Setup(g => g.UpdateCurrentLevelAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int, int>((score, combo, idUser )=>
            {
                return Task.FromResult(new CurrentLevel()
                {
                    Combo = combo,
                    IdUser = idUser,
                    Score = score,
                });
            });

            var result = await gameDomain.UpdateCurrentLevelAsync(100, 100, 1);
            Assert.Equal(100, result.Combo);
        }

        [Fact]
        public async Task GetCredentialsUniTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            credentialsConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.Credentials>())).Returns<Entity.Credentials>(credentialsEntity =>
            {
                var credentialsModel = new Model.Credentials();

                if (credentialsEntity != null)
                {
                    credentialsModel.Id = credentialsEntity.Id;
                    credentialsModel.Username = credentialsEntity.Username;
                    credentialsModel.Password = credentialsEntity.Password;
                }

                return credentialsModel;
            });

            gameRepository.Setup(g => g.GetCredentialsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((username, password) =>
                {
                    return Task.FromResult(new Credentials()
                    {
                        Username = username,
                        Password = password,
                    });
                });

            var result = await gameDomain.GetCredentialsAsync("marius", "12345");
            Assert.Equal("12345", result.Password);
        }        
        
        [Fact]
        public async Task RegisterUserUniTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            credentialsConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.Credentials>())).Returns<Entity.Credentials>(credentialsEntity =>
            {
                var credentialsModel = new Model.Credentials();

                if (credentialsEntity != null)
                {
                    credentialsModel.Id = credentialsEntity.Id;
                    credentialsModel.Username = credentialsEntity.Username;
                    credentialsModel.Password = credentialsEntity.Password;
                }

                return credentialsModel;
            });

            gameRepository.Setup(g => g.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((username, password) =>
                {
                    return Task.FromResult(new Credentials()
                    {
                        Username = username,
                        Password = password,
                    });
                });

            var result = await gameDomain.RegisterUserAsync("marius", "12345");
            Assert.Equal("marius", result.Username);
        }

        [Fact]
        public async Task CreateCurrentLevelUniTest()
        {
            Mock<IGameRepository> gameRepository = new Mock<IGameRepository>();
            GameDomain gameDomain = new GameDomain(gameRepository.Object,
                gameConverter.Object, currentLevelConverter.Object, credentialsConverter.Object);

            currentLevelConverter.Setup(g => g.FromEntityToModel(It.IsAny<Entity.CurrentLevel>())).Returns<Entity.CurrentLevel>(currentLevelEntity =>
            {
                var currentLevelModel = new Model.CurrentLevel();

                if (currentLevelEntity != null)
                {
                    currentLevelModel.Id = currentLevelEntity.Id;
                    currentLevelModel.Combo = currentLevelEntity.Combo;
                    currentLevelModel.IdUser = currentLevelEntity.IdUser;
                    currentLevelModel.Score = currentLevelEntity.Score;
                    currentLevelModel.ListGames = new List<Model.Game>() {
                        new Model.Game { Id = 1, ApiId = 1, IdUser = 1, Name = "Test 1" },
                        new Model.Game { Id = 2, ApiId = 2, IdUser = 1, Name = "Test 2" },
                        new Model.Game { Id = 3, ApiId = 3, IdUser = 1, Name = "Test 3" },
                        new Model.Game { Id = 4, ApiId = 4, IdUser = 1, Name = "Test 4" },
                    };
                }

                return currentLevelModel;
            });

            gameRepository.Setup(g => g.CreateCurrentLevelAsync(It.IsAny<int>()))
                .Returns<int>(idUser =>
                {
                    return Task.FromResult(new CurrentLevel()
                    {
                        Combo = 100,
                        IdUser = idUser,
                        Score = 1001,
                    });
                });

            var result = await gameDomain.CreateCurrentLevelAsync(5);
            Assert.Equal(5, result.IdUser);
        }
    }
}
