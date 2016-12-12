﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.RaaApi
{
    using System;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.RaaApi;
    using Domain.Raa.Interfaces.Repositories;

    public class RaaApiUserRepository : IRaaApiUserRepository
    {
        public static readonly string SelectSql = "SELECT PrimaryApiKey, SecondaryApiKey, UserTypeId AS UserType, ReferencedEntityId, ReferencedEntityGuid, ReferencedEntitySurrogateId FROM [RaaApi].[User] WHERE PrimaryApiKey = @ApiKey OR SecondaryApiKey = @ApiKey";

        private readonly IGetOpenConnection _getOpenConnection;

        public RaaApiUserRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public RaaApiUser GetUser(Guid apiKey)
        {
            var raaApiUser = _getOpenConnection.Query<RaaApiUser>(SelectSql, new { ApiKey = apiKey }).SingleOrDefault();

            return raaApiUser ?? RaaApiUser.UnknownApiUser;
        }
    }
}