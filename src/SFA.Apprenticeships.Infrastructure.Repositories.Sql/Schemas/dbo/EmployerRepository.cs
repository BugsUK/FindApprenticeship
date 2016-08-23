namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    using SFA.Apprenticeships.Application.Interfaces;
    using DomainEmployer = Domain.Entities.Raa.Parties.Employer;
    using Employer = Entities.Employer;

    public class EmployerRepository: IEmployerReadRepository, IEmployerWriteRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

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

            return MapEmployer(employer);
        }

        //TODO: temporary method. Remove after moving status checks to a higher tier
        public DomainEmployer GetByIdWithoutStatusCheck(int employerId)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId = @EmployerId",
                    new { EmployerId = employerId }).SingleOrDefault();

            return MapEmployer(employer);
        }

        public DomainEmployer GetByEdsUrn(string edsUrn)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EdsUrn = @EdsUrn AND EmployerStatusTypeId != 2",
                    new { EdsUrn = Convert.ToInt32(edsUrn) }).SingleOrDefault();

            return employer == null ? null : MapEmployer(employer);
        }

        public List<DomainEmployer> GetByIds(IEnumerable<int> employerIds)
        {
            List<Employer> employers = new List<Employer>();
            var splitEmployerIds = DbHelpers.SplitIds(employerIds);           
            foreach (int[] employersIds in splitEmployerIds)
            {
                var splitEmployer= _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId IN @EmployerIds AND EmployerStatusTypeId != 2",
                    new { EmployerIds = employersIds }).ToList();
                employers.AddRange(splitEmployer);
            }                                 
            return employers.Select(MapEmployer).ToList();
        }

        public IEnumerable<MinimalEmployerDetails> GetMinimalDetailsByIds(IEnumerable<int> employerIds)
        {
            var employers = new List<MinimalEmployerDetails>();
            var splitEmployerIds = DbHelpers.SplitIds(employerIds);
            foreach (var employersIds in splitEmployerIds)
            {
                var splitEmployer = _getOpenConnection.Query<MinimalEmployerDetails>("SELECT EmployerId, FullName FROM dbo.Employer WHERE EmployerId IN @EmployerIds AND EmployerStatusTypeId != 2",
                    new { EmployerIds = employersIds }).ToList();
                employers.AddRange(splitEmployer);
            }

            return employers;
        } 

        public DomainEmployer Save(DomainEmployer employer)
        {
            var dbEmployer = _mapper.Map<DomainEmployer, Employer>(employer);
            PopulateCountyId(employer, dbEmployer);

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

        private DomainEmployer MapEmployer(Employer employer)
        {
            var result = _mapper.Map<Employer, DomainEmployer>(employer);
            return MapCountyId(employer, result);
        }

        private void PopulateCountyId(DomainEmployer entity, Employer dbVacancyLocation)
        {
            if (!string.IsNullOrWhiteSpace(entity.Address?.County))
            {
                dbVacancyLocation.CountyId = _getOpenConnection.QueryCached<int>(_cacheDuration, @"
SELECT CountyId
FROM   dbo.County
WHERE  FullName = @CountyFullName",
                    new
                    {
                        CountyFullName = entity.Address.County
                    }).SingleOrDefault();
            }
        }

        private DomainEmployer MapCountyId(Employer dbEmployer, DomainEmployer result)
        {
            if (dbEmployer.CountyId > 0)
            {
                result.Address.County = _getOpenConnection.QueryCached<string>(_cacheDuration, @"
SELECT FullName
FROM   dbo.County
WHERE  CountyId = @CountyId",
                    new
                    {
                        CountyId = dbEmployer.CountyId
                    }).Single();
            }

            return result;
        }
    }
}
