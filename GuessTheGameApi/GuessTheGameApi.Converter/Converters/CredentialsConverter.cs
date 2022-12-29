using Model = GuessTheGameApi.Converter.Models.Domain;
using Dto = GuessTheGameApi.Converter.Models.Api;
using Entity = GuessTheGameApi.Converter.Models.DataAccess;

namespace GuessTheGameApi.Converter.Converters
{
    public interface ICredentialsConverter
    {
        Model.Credentials FromDtoToModel(Dto.Credentials credentialsDto);
        Dto.Credentials FromModelToDto(Model.Credentials credentialsModel);
        Model.Credentials FromEntityToModel(Entity.Credentials credentialsEntity);
        Entity.Credentials FromModelToEntity(Model.Credentials credentialsModel);
    }

    public class CredentialsConverter : ICredentialsConverter
    {
        public Model.Credentials FromDtoToModel(Dto.Credentials credentialsDto)
        {
            var credentialsModel = new Model.Credentials();

            if (credentialsDto != null)
            {
                credentialsModel.Id = credentialsDto.Id;
                credentialsModel.Username = credentialsDto.Username;
                credentialsModel.Password = credentialsDto.Password;
            }

            return credentialsModel;
        }

        public Dto.Credentials FromModelToDto(Model.Credentials credentialsModel)
        {
            var credentialsDto = new Dto.Credentials();

            if (credentialsModel != null)
            {
                credentialsDto.Id = credentialsModel.Id;
                credentialsDto.Username = credentialsModel.Username;
                credentialsDto.Password = credentialsModel.Password;
            }

            return credentialsDto;
        }

        public Model.Credentials FromEntityToModel(Entity.Credentials credentialsEntity)
        {
            var credentialsModel = new Model.Credentials();

            if (credentialsEntity != null)
            {
                credentialsModel.Id = credentialsEntity.Id;
                credentialsModel.Username = credentialsEntity.Username;
                credentialsModel.Password = credentialsEntity.Password;

            }

            return credentialsModel;
        }

        public Entity.Credentials FromModelToEntity(Model.Credentials credentialsModel)
        {
            var credentialsEntity = new Entity.Credentials();
            
            if (credentialsModel != null)
            {
                credentialsEntity.Id = credentialsModel.Id;
                credentialsEntity.Username = credentialsModel.Username;
                credentialsEntity.Password = credentialsModel.Password;
            }

            return credentialsEntity;
        }
    }
}
