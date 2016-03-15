namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using SFA.Infrastructure.Interfaces;

    public class VacancyPartyRepository : IVacancyPartyReadRepository, IVacancyPartyWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly ILogService _logger;
        
        public VacancyPartyRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public VacancyParty Get(int vacancyPartyId)
        {
            var vacancyParty =
                _getOpenConnection.Query<VacancyOwnerRelationship>("SELECT * FROM dbo.VacancyOwnerRelationship WHERE VacancyOwnerRelationshipId = @VacancyOwnerRelationshipId AND StatusTypeId = 4",
                    new { VacancyOwnerRelationshipId = vacancyPartyId }).SingleOrDefault();

            return _mapper.Map<VacancyOwnerRelationship, VacancyParty>(vacancyParty);
        }

        public VacancyParty Get(int providerSiteId, int employerId)
        {
            var vacancyParty =
                _getOpenConnection.Query<VacancyOwnerRelationship>("SELECT * FROM dbo.VacancyOwnerRelationship WHERE ProviderSiteID = @ProviderSiteId AND EmployerId = @employerId AND StatusTypeId = 4",
                    new { ProviderSiteId = providerSiteId, EmployerId = employerId }).SingleOrDefault();

            return _mapper.Map<VacancyOwnerRelationship, VacancyParty>(vacancyParty);
        }

        public IEnumerable<VacancyParty> GetByIds(IEnumerable<int> vacancyPartyIds)
        {
            var vacancyPartyIdsArray = vacancyPartyIds as int[] ?? vacancyPartyIds.ToArray();
            _logger.Debug("Calling database to get vacancy parties with Ids={0}", string.Join(", ", vacancyPartyIdsArray));

            var vacancyParties =
                _getOpenConnection.Query<VacancyOwnerRelationship>("SELECT * FROM dbo.VacancyOwnerRelationship WHERE VacancyOwnerRelationshipId IN @VacancyPartyIds AND StatusTypeId = 4",
                    new { VacancyPartyIds = vacancyPartyIdsArray });

            return _mapper.Map<IEnumerable<VacancyOwnerRelationship>, IEnumerable<VacancyParty>>(vacancyParties);
        }

        public IEnumerable<VacancyParty> GetForProviderSite(int providerSiteId)
        {
            var vacancyParties =
                _getOpenConnection.Query<VacancyOwnerRelationship>("SELECT * FROM dbo.VacancyOwnerRelationship WHERE ProviderSiteID = @ProviderSiteId AND StatusTypeId = 4",
                    new { ProviderSiteID = providerSiteId });

            return _mapper.Map<IEnumerable<VacancyOwnerRelationship>, IEnumerable<VacancyParty>>(vacancyParties);
        }
        
        public VacancyParty Save(VacancyParty entity)
        {
            var vacancyOwnerRelationship = _mapper.Map<VacancyParty, VacancyOwnerRelationship>(entity);
            vacancyOwnerRelationship.StatusTypeId = 4;
            vacancyOwnerRelationship.EditedInRaa = true;
            if (vacancyOwnerRelationship.VacancyOwnerRelationshipId == 0)
            {
                vacancyOwnerRelationship.VacancyOwnerRelationshipId = (int)_getOpenConnection.Insert(vacancyOwnerRelationship);
            }
            else
            {
                var existingVacancyOwnerRelationship =
                    _getOpenConnection.Query<VacancyOwnerRelationship>(
                        "SELECT * FROM dbo.VacancyOwnerRelationship WHERE VacancyOwnerRelationshipId = @VacancyOwnerRelationshipId AND StatusTypeId = 4",
                        new {vacancyOwnerRelationship.VacancyOwnerRelationshipId}).Single();
                vacancyOwnerRelationship.ContractHolderIsEmployer = existingVacancyOwnerRelationship.ContractHolderIsEmployer;
                vacancyOwnerRelationship.ManagerIsEmployer = existingVacancyOwnerRelationship.ManagerIsEmployer;
                vacancyOwnerRelationship.Notes = existingVacancyOwnerRelationship.Notes;
                vacancyOwnerRelationship.EmployerLogoAttachmentId = existingVacancyOwnerRelationship.EmployerLogoAttachmentId;
                vacancyOwnerRelationship.NationWideAllowed = existingVacancyOwnerRelationship.NationWideAllowed;
                _getOpenConnection.UpdateSingle(vacancyOwnerRelationship);
            }

            return Get(vacancyOwnerRelationship.VacancyOwnerRelationshipId);
        }
    }
}