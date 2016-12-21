namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Application.Interfaces;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using VacancyOwnerRelationship = Domain.Entities.Raa.Parties.VacancyOwnerRelationship;

    public class VacancyOwnerRelationshipRepository : IVacancyOwnerRelationshipReadRepository, IVacancyOwnerRelationshipWriteRepository
    {
        private enum VacancyOwnerRelationshipStatusTypes
        {
            Deleted = 3,
            Live = 4
        };

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public VacancyOwnerRelationshipRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public VacancyOwnerRelationship GetByProviderSiteAndEmployerId(int providerSiteId, int employerId, bool liveOnly = true)
        {
            var sql = @"
                SELECT * FROM dbo.VacancyOwnerRelationship
                WHERE ProviderSiteID = @ProviderSiteId
                AND EmployerId = @EmployerId";

            if (liveOnly)
            {
                sql += " AND StatusTypeId = @StatusTypeId";
            }

            var sqlParams = new
            {
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                StatusTypeId = VacancyOwnerRelationshipStatusTypes.Live
            };

            var vacancyOwnerRelationship = _getOpenConnection.Query<Entities.VacancyOwnerRelationship>(sql, sqlParams).SingleOrDefault();

            return _mapper.Map<Entities.VacancyOwnerRelationship, VacancyOwnerRelationship>(vacancyOwnerRelationship);
        }

        public IEnumerable<VacancyOwnerRelationship> GetByIds(IEnumerable<int> vacancyOwnerRelationshipIds, bool currentOnly = true)
        {
            var vacancyOwnerRelationshipIdsArray = vacancyOwnerRelationshipIds.Distinct().ToArray();

            _logger.Debug("Calling database to get vacancy parties with Ids={0}", string.Join(", ", vacancyOwnerRelationshipIdsArray));

            string sql = @"
                SELECT *
                FROM   dbo.VacancyOwnerRelationship
                WHERE  VacancyOwnerRelationshipId IN @VacancyOwnerRelationshipIds
" + (currentOnly ? "AND StatusTypeId = @StatusTypeId" : "");

            List<Entities.VacancyOwnerRelationship> vacancyOwnerRelationships = new List<Entities.VacancyOwnerRelationship>();
            var splitVacancyOwnerRelationshipIdsArray = DbHelpers.SplitIds(vacancyOwnerRelationshipIdsArray);
            foreach (int[] ids in splitVacancyOwnerRelationshipIdsArray)
            {
                var sqlParams = new
                {
                    VacancyOwnerRelationshipIds = ids,
                    StatusTypeId = VacancyOwnerRelationshipStatusTypes.Live
                };
                var vacancyParties = _getOpenConnection.Query<Entities.VacancyOwnerRelationship>(sql, sqlParams);
                vacancyOwnerRelationships.AddRange(vacancyParties);
            }

            return _mapper.Map<IEnumerable<Entities.VacancyOwnerRelationship>, IEnumerable<VacancyOwnerRelationship>>(vacancyOwnerRelationships);
        }

        public IEnumerable<VacancyOwnerRelationship> GetByProviderSiteId(int providerSiteId)
        {
            const string sql = @"
                SELECT * FROM dbo.VacancyOwnerRelationship
                WHERE ProviderSiteID = @ProviderSiteId
                AND StatusTypeId = @StatusTypeId";

            var sqlParams = new
            {
                ProviderSiteID = providerSiteId,
                StatusTypeId = VacancyOwnerRelationshipStatusTypes.Live
            };

            var vacancyParties = _getOpenConnection.Query<Entities.VacancyOwnerRelationship>(sql, sqlParams);

            return _mapper.Map<IEnumerable<Entities.VacancyOwnerRelationship>, IEnumerable<VacancyOwnerRelationship>>(vacancyParties);
        }

        public VacancyOwnerRelationship Save(VacancyOwnerRelationship vacancyOwnerRelationship)
        {
            var dbVacancyOwnerRelationship = _mapper.Map<VacancyOwnerRelationship, Entities.VacancyOwnerRelationship>(vacancyOwnerRelationship);

            dbVacancyOwnerRelationship.StatusTypeId = (int)VacancyOwnerRelationshipStatusTypes.Live;
            dbVacancyOwnerRelationship.EditedInRaa = true;

            if (dbVacancyOwnerRelationship.VacancyOwnerRelationshipId == 0)
            {
                dbVacancyOwnerRelationship.VacancyOwnerRelationshipId = (int)_getOpenConnection.Insert(dbVacancyOwnerRelationship);
            }
            else
            {
                const string sql = @"
                    SELECT * FROM dbo.VacancyOwnerRelationship
                    WHERE VacancyOwnerRelationshipId = @VacancyOwnerRelationshipId
                    AND StatusTypeId = @StatusTypeId";

                var sqlParams = new
                {
                    dbVacancyOwnerRelationship.VacancyOwnerRelationshipId,
                    dbVacancyOwnerRelationship.StatusTypeId
                };

                var existingVacancyOwnerRelationship = _getOpenConnection.Query<Entities.VacancyOwnerRelationship>(sql, sqlParams).Single();

                dbVacancyOwnerRelationship.ContractHolderIsEmployer = existingVacancyOwnerRelationship.ContractHolderIsEmployer;
                dbVacancyOwnerRelationship.ManagerIsEmployer = existingVacancyOwnerRelationship.ManagerIsEmployer;
                dbVacancyOwnerRelationship.Notes = existingVacancyOwnerRelationship.Notes;
                dbVacancyOwnerRelationship.EmployerLogoAttachmentId = existingVacancyOwnerRelationship.EmployerLogoAttachmentId;
                dbVacancyOwnerRelationship.NationWideAllowed = existingVacancyOwnerRelationship.NationWideAllowed;

                _getOpenConnection.UpdateSingle(dbVacancyOwnerRelationship);
            }

            return GetByIds(new[] { dbVacancyOwnerRelationship.VacancyOwnerRelationshipId }).Single();
        }

        public bool IsADeletedVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            const string sql = @"
                SELECT StatusTypeId FROM dbo.VacancyOwnerRelationship
                WHERE ProviderSiteID = @ProviderSiteId
                AND EmployerId = @EmployerId";

            var sqlParams = new
            {
                ProviderSiteId = providerSiteId,
                EmployerId = employerId
            };

            return _getOpenConnection.Query<Entities.VacancyOwnerRelationship>(sql, sqlParams).SingleOrDefault()?.StatusTypeId == (int)VacancyOwnerRelationshipStatusTypes.Deleted;
        }

        public void ResurrectVacancyOwnerRelationship(int providerSiteId, int employerId)
        {
            const string sql = @"
                UPDATE dbo.VacancyOwnerRelationship SET StatusTypeId = @StatusTypeId
                WHERE ProviderSiteID = @ProviderSiteId
                AND EmployerId = @EmployerId";

            var sqlParams = new
            {
                ProviderSiteId = providerSiteId,
                EmployerId = employerId,
                StatusTypeId = VacancyOwnerRelationshipStatusTypes.Live
            };

            _getOpenConnection.MutatingQuery<object>(sql, sqlParams);

        }
    }
}