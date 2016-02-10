namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using System;
    using System.Linq;
    using Common;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
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

        public AgencyUser Get(string username)
        {
            var user = _getOpenConnection.Query<Entities.AgencyUser>("SELECT * FROM UserProfile.AgencyUser WHERE Username = @Username", new { Username  = username }).SingleOrDefault();
            if (user != null)
            {
                var role = _getOpenConnection.Query<Entities.AgencyUserRole>("SELECT * FROM UserProfile.AgencyUserRole WHERE AgencyUserRole.AgencyUserRoleId = @id", new { id = user.RoleId }).SingleOrDefault();
                user.Role = role;
                var team = _getOpenConnection.Query<Entities.AgencyUserTeam>("SELECT * FROM UserProfile.AgencyUserTeam WHERE AgencyUserTeam.AgencyUserTeamId = @id", new { id = user.TeamId }).SingleOrDefault();
                user.Team = team;
            }

            var result = _mapper.Map<Entities.AgencyUser, AgencyUser>(user);
            return result;
        }

        public AgencyUser Save(AgencyUser entity)
        {
            //_logger.Debug("Called SQL DB to save AgencyUser with Id={0}", entity.EntityId);

            //UpdateEntityTimestamps(entity);

            //var dbEntity = _mapper.Map<AgencyUser, Entities.AgencyUser>(entity);

            //try
            //{
            //    var result = (int)_getOpenConnection.Insert(dbEntity);
            //    dbEntity.Id = result;
            //}
            //catch (Exception ex)
            //{
            //    // TODO: Detect key violation

            //    if (!_getOpenConnection.UpdateSingle(dbEntity))
            //        throw new Exception("Failed to update record after failed insert", ex);
            //}

            //_logger.Debug("Saved provider to SQL DB with UKPRN={0}", entity.EntityId);

            //return _mapper.Map<Entities.AgencyUser, AgencyUser>(dbEntity);
            throw new NotImplementedException();
        }

        private void UpdateEntityTimestamps(AgencyUser entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateCreated == DateTime.MinValue)
            {
                entity.DateCreated = DateTime.UtcNow;
                entity.DateUpdated = null;
            }
            else
            {
                entity.DateUpdated = DateTime.UtcNow;
            }
        }
    }
}
