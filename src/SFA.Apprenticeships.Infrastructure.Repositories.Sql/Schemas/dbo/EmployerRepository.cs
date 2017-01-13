namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Repositories;
    using Application.Interfaces;
    using Domain.Raa.Interfaces.Repositories.Models;
    using DomainEmployer = Domain.Entities.Raa.Parties.Employer;
    using Employer = Entities.Employer;

    public class EmployerRepository: IEmployerReadRepository, IEmployerWriteRepository
    {
        private const string BasicQuery =
@"SELECT e.*, c.FullName as County, la.CodeName as LocalAuthorityCodeName, la.FullName as LocalAuthority FROM dbo.Employer e 
LEFT JOIN dbo.County c ON e.CountyId = c.CountyId 
LEFT JOIN dbo.LocalAuthority la ON e.LocalAuthorityId = la.LocalAuthorityId ";

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IMapper _mapper;
        private readonly IReferenceRepository _referenceRepository;

        public EmployerRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IReferenceRepository referenceRepository)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _referenceRepository = referenceRepository;
        }

        public DomainEmployer GetById(int employerId, bool currentOnly = true)
        {
            var employer =
                _getOpenConnection.Query<Employer>(BasicQuery + "WHERE EmployerId = @EmployerId" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EmployerId = employerId }).SingleOrDefault();

            return MapEmployers(new []{employer}).Single();
        }

        public DomainEmployer GetByEdsUrn(string edsUrn, bool currentOnly = true)
        {
            var employer =
                _getOpenConnection.Query<Employer>(BasicQuery + "WHERE EdsUrn = @EdsUrn" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EdsUrn = Convert.ToInt32(edsUrn) }).SingleOrDefault();

            return employer == null ? null : MapEmployers(new[] { employer }).Single();
        }

        public List<DomainEmployer> GetByIds(IEnumerable<int> employerIds, bool currentOnly = true)
        {
            List<Employer> employers = new List<Employer>();
            var splitEmployerIds = DbHelpers.SplitIds(employerIds);           
            foreach (int[] employersIds in splitEmployerIds)
            {
                var splitEmployer= _getOpenConnection.Query<Employer>(BasicQuery + "WHERE EmployerId IN @EmployerIds" + (currentOnly ? " AND EmployerStatusTypeId != 2" : ""),
                    new { EmployerIds = employersIds }).ToList();
                employers.AddRange(splitEmployer);
            }                                 
            return MapEmployers(employers);
        }

        public IEnumerable<DomainEmployer> Search(EmployerSearchParameters searchParameters)
        {
            var sql = BasicQuery + "WHERE ";
            var and = "";

            if (!string.IsNullOrEmpty(searchParameters.Id))
            {
                sql += "EmployerId = @Id ";
                and = "AND ";
            }
            if (!string.IsNullOrEmpty(searchParameters.EdsUrn))
            {
                sql += and + "EDSURN = @EdsUrn ";
                and = "AND ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                sql += and + "(FullName LIKE '%' + @Name + '%' OR TradingName LIKE '%' + @Name + '%') ";
                and = "AND ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Location))
            {
                sql += and + "(Town LIKE '%' + @Location + '%' OR PostCode LIKE '%' + @Location + '%') ";
            }
            sql += "ORDER BY FullName";

            var employers = _getOpenConnection.Query<Employer>(sql, searchParameters).ToList();

            return MapEmployers(employers);
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
                const string sql = BasicQuery + "WHERE EmployerId = @EmployerId";

                var sqlParams = new
                {
                    dbEmployer.EmployerId
                };

                var existingEmployer = _getOpenConnection.Query<Employer>(sql, sqlParams).Single();

                if (!dbEmployer.LocalAuthorityId.HasValue || dbEmployer.LocalAuthorityId.Value == 0)
                {
                    dbEmployer.LocalAuthorityId = existingEmployer.LocalAuthorityId;
                }
                dbEmployer.PrimaryContact = existingEmployer.PrimaryContact;
                dbEmployer.NumberofEmployeesAtSite = existingEmployer.NumberofEmployeesAtSite;
                dbEmployer.NumberOfEmployeesInGroup = existingEmployer.NumberOfEmployeesInGroup;
                dbEmployer.OwnerOrgnistaion = existingEmployer.OwnerOrgnistaion;
                dbEmployer.CompanyRegistrationNumber = existingEmployer.CompanyRegistrationNumber;
                dbEmployer.TotalVacanciesPosted = existingEmployer.TotalVacanciesPosted;
                dbEmployer.BeingSupportedBy = existingEmployer.BeingSupportedBy;
                dbEmployer.LockedForSupportUntil = existingEmployer.LockedForSupportUntil;
                dbEmployer.DisableAllowed = existingEmployer.DisableAllowed;
                dbEmployer.TrackingAllowed = existingEmployer.TrackingAllowed;

                _getOpenConnection.UpdateSingle(dbEmployer);
            }

            return GetById(dbEmployer.EmployerId);
        }

        private List<DomainEmployer> MapEmployers(IReadOnlyCollection<Employer> employers)
        {
            var results = new List<DomainEmployer>(employers.Count);

            var countyIdsMap = _referenceRepository.GetCounties().ToDictionary(c => c.CountyId, c => c.FullName);

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

        private void PopulateCountyId(DomainEmployer entity, Employer dbEmployer)
        {
            if (dbEmployer.CountyId != 0) return;
            var county = _referenceRepository.GetCountyByName(entity.Address.County);
            if(county != null)
                dbEmployer.CountyId = county.CountyId;
        }
    }
}
