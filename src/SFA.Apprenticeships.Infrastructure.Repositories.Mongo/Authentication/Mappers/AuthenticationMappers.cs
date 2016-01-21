namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Authentication.Mappers
{
    using Domain.Entities.Users;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class AuthenticationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseUserCredentialsMappers();
        }

        private void InitialiseUserCredentialsMappers()
        {
            Mapper.CreateMap<MongoUserCredentials, UserCredentials>();
            Mapper.CreateMap<UserCredentials, MongoUserCredentials>();
        }
    }
}