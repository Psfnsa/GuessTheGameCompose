using Model = GuessTheGameApi.Converter.Models.Domain;
using Dto = GuessTheGameApi.Converter.Models.Api;
using Entity = GuessTheGameApi.Converter.Models.DataAccess;

namespace GuessTheGameApi.Converter.Converters
{
    public interface ICurrentLevelConverter
    {
        Model.CurrentLevel FromDtoToModel(Dto.CurrentLevel currentLevelDto);
        Dto.CurrentLevel FromModelToDto(Model.CurrentLevel currentLevelModel);
        Model.CurrentLevel FromEntityToModel(Entity.CurrentLevel currentLevelEntity);
        Entity.CurrentLevel FromModelToEntity(Model.CurrentLevel currentLevelModel);
    }

    public class CurrentLevelConverter : ICurrentLevelConverter
    {
        private readonly IGameConverter _gameConverter;

        public CurrentLevelConverter(IGameConverter gameConverter)
        {
            _gameConverter = gameConverter;
        }

        public Model.CurrentLevel FromDtoToModel(Dto.CurrentLevel currentLevelDto)
        {
            var listGamesTemp = new List<Model.Game>();
            var currentLevelModel = new Model.CurrentLevel();

            if (currentLevelDto != null)
            {
                if (currentLevelDto.ListGames != null)
                {
                    currentLevelDto.ListGames.ForEach(game => listGamesTemp.Add(_gameConverter.FromDtoToModel(game)));
                }

                currentLevelModel.Id = currentLevelDto.Id;
                currentLevelModel.Combo = currentLevelDto.Combo;
                currentLevelModel.Score = currentLevelDto.Score;
                currentLevelModel.IdUser = currentLevelDto.IdUser;
                currentLevelModel.ListGames = listGamesTemp;
            }

            return currentLevelModel;
        }

        public Dto.CurrentLevel FromModelToDto(Model.CurrentLevel currentLevelModel)
        {
            var listGamesTemp = new List<Dto.Game>();
            var currentLevelDto = new Dto.CurrentLevel();

            if (currentLevelModel != null)
            {
                if (currentLevelModel.ListGames != null)
                {
                    currentLevelModel.ListGames.ForEach(game => listGamesTemp.Add(_gameConverter.FromModelToDto(game)));
                }

                currentLevelDto.Id = currentLevelModel.Id;
                currentLevelDto.Combo = currentLevelModel.Combo;
                currentLevelDto.Score = currentLevelModel.Score;
                currentLevelDto.IdUser = currentLevelModel.IdUser;
                currentLevelDto.ListGames = listGamesTemp;
            }

            return currentLevelDto;
        }

        public Model.CurrentLevel FromEntityToModel(Entity.CurrentLevel currentLevelEntity)
        {
            var listGamesTemp = new List<Model.Game>();
            var currentLevelModel = new Model.CurrentLevel();

            if (currentLevelEntity != null)
            {
                if (currentLevelEntity.ListGames != null)
                {
                    currentLevelEntity.ListGames.ForEach(game => listGamesTemp.Add(_gameConverter.FromEntityToModel(game)));
                }

                currentLevelModel.Id = currentLevelEntity.Id;
                currentLevelModel.Combo = currentLevelEntity.Combo;
                currentLevelModel.Score = currentLevelEntity.Score;
                currentLevelModel.IdUser = currentLevelEntity.IdUser;
                currentLevelModel.ListGames = listGamesTemp;
            }

            return currentLevelModel;
        }

        public Entity.CurrentLevel FromModelToEntity(Model.CurrentLevel currentLevelModel)
        {
            var currentLevelEntity = new Entity.CurrentLevel();
            var listGamesTemp = new List<Entity.Game>();

            if (currentLevelModel != null)
            {
                if (currentLevelModel.ListGames != null)
                {
                    currentLevelModel.ListGames.ForEach(game => listGamesTemp.Add(_gameConverter.FromModelToEntity(game)));
                }

                currentLevelEntity.Id = currentLevelModel.Id;
                currentLevelEntity.Combo = currentLevelModel.Combo;
                currentLevelEntity.Score = currentLevelModel.Score;
                currentLevelEntity.IdUser = currentLevelModel.IdUser;
                currentLevelEntity.ListGames = listGamesTemp;
            }

            return currentLevelEntity;
        }
    }
}
