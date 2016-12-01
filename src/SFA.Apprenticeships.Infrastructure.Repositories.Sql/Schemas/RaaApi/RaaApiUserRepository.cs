namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.RaaApi
{
    using System;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.RaaApi;
    using Domain.Raa.Interfaces.Repositories;

    public class RaaApiUserRepository : IRaaApiUserRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        public RaaApiUserRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public RaaApiUser GetUser(Guid apiKey)
        {
            var raaApiUser =
                _getOpenConnection.Query<RaaApiUser>("SELECT * FROM [RaaApi].[User] WHERE PrimaryApiKey = @ApiKey OR SecondaryApiKey = @ApiKey",
                    new { ApiKey = apiKey }).SingleOrDefault();

            return raaApiUser ?? RaaApiUser.UnknownApiUser;
        }
    }
}