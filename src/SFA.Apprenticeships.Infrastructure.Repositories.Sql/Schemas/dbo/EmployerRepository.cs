namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
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
        private readonly ILogService _logger;

        public EmployerRepository(IGetOpenConnection getOpenConnection, IMapper mapper, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _logger = logger;
        }

        public DomainEmployer Get(int employerId)
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
                    new { EdsUrn = Convert.ToInt32(edsUrn) }).SingleOrDefault();

            return _mapper.Map<Employer, DomainEmployer>(employer);
        }

        public List<DomainEmployer> GetByIds(IEnumerable<int> employerIds)
        {
            var employers =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId IN @EmployerIds AND EmployerStatusTypeId != 2",
                    new { EmployerIds = employerIds }).ToList();

            return _mapper.Map<List<Employer>, List<DomainEmployer>>(employers);
        }

        public DomainEmployer Save(DomainEmployer entity)
        {
            var employer = _mapper.Map<DomainEmployer, Employer>(entity);
            if (employer.EmployerId == 0)
            {
                employer.EmployerStatusTypeId = 1;
                employer.EmployerId = (int)_getOpenConnection.Insert(employer);
            }
            else
            {
                var existingEmployer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId = @EmployerId",
                    new {employer.EmployerId }).Single();
                employer.TradingName = existingEmployer.TradingName;
                employer.CountyId = existingEmployer.CountyId;
                employer.LocalAuthorityId = existingEmployer.LocalAuthorityId;
                employer.GeocodeEasting = existingEmployer.GeocodeEasting;
                employer.GeocodeNorthing = existingEmployer.GeocodeNorthing;
                employer.PrimaryContact = existingEmployer.PrimaryContact;
                employer.NumberofEmployeesAtSite = existingEmployer.NumberofEmployeesAtSite;
                employer.NumberOfEmployeesInGroup = existingEmployer.NumberOfEmployeesInGroup;
                employer.OwnerOrgnistaion = existingEmployer.OwnerOrgnistaion;
                employer.CompanyRegistrationNumber = existingEmployer.CompanyRegistrationNumber;
                employer.TotalVacanciesPosted = existingEmployer.TotalVacanciesPosted;
                employer.BeingSupportedBy = existingEmployer.BeingSupportedBy;
                employer.LockedForSupportUntil = existingEmployer.LockedForSupportUntil;
                employer.EmployerStatusTypeId = existingEmployer.EmployerStatusTypeId;
                employer.DisableAllowed = existingEmployer.DisableAllowed;
                employer.TrackingAllowed = existingEmployer.TrackingAllowed;
                _getOpenConnection.UpdateSingle(employer);
            }

            return Get(employer.EmployerId);
        }
    }
}
