namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Users;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;

    public class AgencyUserRepository : IAgencyUserReadRepository, IAgencyUserWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        private readonly IGetOpenConnection _getOpenConnection;

        public AgencyUserRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public AgencyUser GetByUsername(string username)
        {
            var user = _getOpenConnection.Query<Entities.AgencyUser>("SELECT * FROM UserProfile.AgencyUser WHERE Username = @username", new { username = username }).SingleOrDefault();
            var result = _mapper.Map<Entities.AgencyUser, AgencyUser>(user);
            return result;
        }

        public AgencyUser Save(AgencyUser entity)
        {
            _logger.Debug("Called SQL DB to save AgencyUser with AgencyUserId={0}", entity.AgencyUserId);

            UpdateEntityTimestamps(entity);

            var dbEntity = _mapper.Map<AgencyUser, Entities.AgencyUser>(entity);

            if (dbEntity.AgencyUserGuid == Guid.Empty)
            {
                dbEntity.AgencyUserGuid = Guid.NewGuid();
            }

            // TODO: SQL: AG: consider generalising (and testing) SqlException handling below.

            try
            {
                var result = (int)_getOpenConnection.Insert(dbEntity);
                dbEntity.AgencyUserId = result;
            }
            catch (SqlException e)
            when (e.Number == 2601)
            {
                if (!_getOpenConnection.UpdateSingle(dbEntity))
                    throw new Exception("Failed to update record after failed insert", e);
            }

            _logger.Debug("Saved AgencyUser with AgencyUserId={0}", entity.AgencyUserId);

            var endResult = _mapper.Map<Entities.AgencyUser, AgencyUser>(dbEntity);
            endResult.Role = entity.Role;
            endResult.RegionalTeam = entity.RegionalTeam;

            return endResult;
        }

        private void UpdateEntityTimestamps(AgencyUser entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.CreatedDateTime == DateTime.MinValue)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = null;
            }
            else
            {
                entity.UpdatedDateTime = DateTime.UtcNow;
            }
        }
    }
}
