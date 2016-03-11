namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using SFA.Infrastructure.Interfaces;
    using DomainEmployer = Domain.Entities.Raa.Parties.Employer;
    using Employer = Entities.Employer;

    public class EmployerRepository: IEmployerReadRepository, IEmployerWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;

        public EmployerRepository(IGetOpenConnection getOpenConnection, IMapper mapper)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
        }

        public DomainEmployer GetById(int employerId)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId = @EmployerId AND EmployerStatusTypeId != 2",
                    new { EmployerId = employerId }).SingleOrDefault();

            return _mapper.Map<Employer, DomainEmployer>(employer);
        }

        public DomainEmployer GetByEdsUrn(string edsUrn)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EdsUrn = @EdsUrn AND EmployerStatusTypeId != 2",
                    new { EdsUrn = edsUrn }).SingleOrDefault();

            return _mapper.Map<Employer, DomainEmployer>(employer);
        }

        public List<DomainEmployer> GetByIds(IEnumerable<int> employerIds)
        {
            var employers =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId IN @EmployerIds AND EmployerStatusTypeId != 2",
                    new { EmployerIds = employerIds }).ToList();

            return _mapper.Map<List<Employer>, List<DomainEmployer>>(employers);
        }

        public DomainEmployer Save(DomainEmployer employer)
        {
            var dbEmployer = _mapper.Map<DomainEmployer, Employer>(employer);

            if (dbEmployer.EmployerId == 0)
            {
                dbEmployer.EmployerStatusTypeId = 1;
                dbEmployer.EmployerId = (int)_getOpenConnection.Insert(dbEmployer);
            }
            else
            {
                const string sql = "SELECT * FROM dbo.Employer WHERE EmployerId = @EmployerId";

                var sqlParams = new
                {
                    dbEmployer.EmployerId
                };

                var existingEmployer = _getOpenConnection.Query<Employer>(sql, sqlParams).Single();

                dbEmployer.TradingName = existingEmployer.TradingName;
                dbEmployer.CountyId = existingEmployer.CountyId;
                dbEmployer.LocalAuthorityId = existingEmployer.LocalAuthorityId;
                dbEmployer.GeocodeEasting = existingEmployer.GeocodeEasting;
                dbEmployer.GeocodeNorthing = existingEmployer.GeocodeNorthing;
                dbEmployer.PrimaryContact = existingEmployer.PrimaryContact;
                dbEmployer.NumberofEmployeesAtSite = existingEmployer.NumberofEmployeesAtSite;
                dbEmployer.NumberOfEmployeesInGroup = existingEmployer.NumberOfEmployeesInGroup;
                dbEmployer.OwnerOrgnistaion = existingEmployer.OwnerOrgnistaion;
                dbEmployer.CompanyRegistrationNumber = existingEmployer.CompanyRegistrationNumber;
                dbEmployer.TotalVacanciesPosted = existingEmployer.TotalVacanciesPosted;
                dbEmployer.BeingSupportedBy = existingEmployer.BeingSupportedBy;
                dbEmployer.LockedForSupportUntil = existingEmployer.LockedForSupportUntil;
                dbEmployer.EmployerStatusTypeId = existingEmployer.EmployerStatusTypeId;
                dbEmployer.DisableAllowed = existingEmployer.DisableAllowed;
                dbEmployer.TrackingAllowed = existingEmployer.TrackingAllowed;

                _getOpenConnection.UpdateSingle(dbEmployer);
            }

            return GetById(dbEmployer.EmployerId);
        }
    }
}
