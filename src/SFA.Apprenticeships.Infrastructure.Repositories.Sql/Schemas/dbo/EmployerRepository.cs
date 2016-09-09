namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Parties;
    using Domain.Raa.Interfaces.Repositories;

    using Application.Interfaces;
    using Domain.Entities.Raa.Reference;
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

        public DomainEmployer GetById(int employerId, bool currentOnly = true)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId = @EmployerId" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EmployerId = employerId }).SingleOrDefault();

            return MapEmployers(new []{employer}).Single();
        }

        public DomainEmployer GetByEdsUrn(string edsUrn, bool currentOnly = true)
        {
            var employer =
                _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EdsUrn = @EdsUrn" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EdsUrn = Convert.ToInt32(edsUrn) }).SingleOrDefault();

            return employer == null ? null : MapEmployers(new[] { employer }).Single();
        }

        public List<DomainEmployer> GetByIds(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            List<Employer> employers = new List<Employer>();
            var splitEmployerIds = DbHelpers.SplitIds(employerIds);           
            foreach (int[] employersIds in splitEmployerIds)
            {
                var splitEmployer= _getOpenConnection.Query<Employer>("SELECT * FROM dbo.Employer WHERE EmployerId IN @EmployerIds" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EmployerIds = employersIds }).ToList();
                employers.AddRange(splitEmployer);
            }                                 
            return MapEmployers(employers);
        }

        public IEnumerable<MinimalEmployerDetails> GetMinimalDetailsByIds(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            var employers = new List<MinimalEmployerDetails>();
            var splitEmployerIds = DbHelpers.SplitIds(employerIds);
            foreach (var employersIds in splitEmployerIds)
            {
                var splitEmployer = _getOpenConnection.Query<MinimalEmployerDetails>("SELECT EmployerId, FullName FROM dbo.Employer WHERE EmployerId IN @EmployerIds" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
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

        private List<DomainEmployer> MapEmployers(IReadOnlyCollection<Employer> employers)
        {
            var results = new List<DomainEmployer>(employers.Count);

            var countyIds = employers.Where(e => e.CountyId > 0).Select(e => e.CountyId).Distinct();
            var countyIdsMap = _getOpenConnection.QueryCached<County>(_cacheDuration, @"
SELECT *
FROM   dbo.County
WHERE  CountyId IN @countyIds",
                    new
                    {
                        countyIds
                    }).ToDictionary(c => c.CountyId, c => c.FullName);

            foreach (var employer in employers)
            {
                var result = _mapper.Map<Employer, DomainEmployer>(employer);
                if (countyIdsMap.ContainsKey(employer.CountyId))
                {
                    result.Address.County = countyIdsMap[employer.CountyId];
                }
                results.Add(result);
            }

            return results;
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
    }
}
