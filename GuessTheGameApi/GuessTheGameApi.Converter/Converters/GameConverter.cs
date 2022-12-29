using Model = GuessTheGameApi.Converter.Models.Domain;
using Dto = GuessTheGameApi.Converter.Models.Api;
using Entity = GuessTheGameApi.Converter.Models.DataAccess;

namespace GuessTheGameApi.Converter.Converters
{
    public interface IGameConverter
    {
        Model.Game FromDtoToModel(Dto.Game gameDto);
        Dto.Game FromModelToDto(Model.Game gameModel);
        Model.Game FromEntityToModel(Entity.Game gameEntity);
        Entity.Game FromModelToEntity(Model.Game gameModel);
    }

    public class GameConverter : IGameConverter
    {
        public Model.Game FromDtoToModel(Dto.Game gameDto)
        {
            var gameModel = new Model.Game();

            if (gameDto != null)
            {
                gameModel.Id = gameDto.Id;
                gameModel.Name = gameDto.Name;
                gameModel.IdUser = gameDto.IdUser;
                gameModel.ApiId = gameDto.ApiId;
            }

            return gameModel;
        }

        public Dto.Game FromModelToDto(Model.Game gameModel)
        {
            var gameDto = new Dto.Game();

            if (gameModel != null)
            {
                gameDto.Id = gameModel.Id;
                gameDto.Name = gameModel.Name;
                gameDto.IdUser = gameModel.IdUser;
                gameDto.ApiId = gameModel.ApiId;
            }

            return gameDto;
        }

        public Model.Game FromEntityToModel(Entity.Game gameEntity)
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

        }

        public Entity.Game FromModelToEntity(Model.Game gameModel)
        {
            var gameEntity = new Entity.Game();

            if (gameModel != null)
            {
                gameEntity.Id = gameModel.Id;
                gameEntity.Name = gameModel.Name;
                gameEntity.IdUser = gameModel.IdUser;
                gameEntity.ApiId = gameModel.ApiId;
            }

            return gameEntity;
        }
    }
}
