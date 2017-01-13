namespace SFA.Apprenticeships.Application.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.ReferenceData;

    public class ReferenceDataService : IReferenceDataService
    {
        private readonly IReferenceDataProvider _referenceDataProvider;
        private readonly IReferenceRepository _referenceRepository;

        public ReferenceDataService(IReferenceDataProvider referenceDataProvider, IReferenceRepository referenceRepository)
        {
            _referenceDataProvider = referenceDataProvider;
            _referenceRepository = referenceRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _referenceDataProvider.GetCategories();
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            return _referenceDataProvider.GetSubCategoryByName(subCategoryName);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return _referenceDataProvider.GetCategoryByName(categoryName);
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            return _referenceDataProvider.GetSubCategoryByCode(subCategoryCode);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            return _referenceDataProvider.GetCategoryByCode(categoryCode);
        }

        public IEnumerable<Category> GetFrameworks()
        {
            return _referenceDataProvider.GetFrameworks();
        }

        public IEnumerable<Sector> GetSectors()
        {
            return _referenceDataProvider.GetSectors();
        }

        public IList<ReleaseNote> GetReleaseNotes(DasApplication dasApplication)
        {
            return _referenceDataProvider.GetReleaseNotes(dasApplication);
        }

        public IEnumerable<County> GetCounties()
        {
            return _referenceDataProvider.GetCounties();
        }

        public County GetCounty(int countyId)
        {
            return _referenceDataProvider.GetCounty(countyId);
        }

        public County GetCounty(string countyName)
        {
            return _referenceDataProvider.GetCounty(countyName);
        }

        public IEnumerable<LocalAuthority> GetLocalAuthorities()
        {
            return _referenceDataProvider.GetLocalAuthorities();
        }

        public LocalAuthority GetLocalAuthority(int localAuthorityId)
        {
            return _referenceDataProvider.GetLocalAuthority(localAuthorityId);
        }

        public LocalAuthority GetLocalAuthority(string localAuthorityCodeName)
        {
            return _referenceDataProvider.GetLocalAuthority(localAuthorityCodeName);
        }
		
		public Standard CreateStandard(Standard standard)
        {
            return _referenceRepository.CreateStandard(standard);
        }

        public Sector CreateSector(Sector sector)
        {
            return _referenceRepository.CreateSector(sector);
        }

        public Standard GetStandard(int standardId)
        {
            return _referenceRepository.GetStandardById(standardId);
        }

        public Standard SaveStandard(Standard standard)
        {
            return _referenceRepository.UpdateStandard(standard);
        }

        public Sector GetSector(int sectorId)
        {
            return _referenceRepository.GetSectorById(sectorId);
        }

        public Sector SaveSector(Sector sector)
        {
            return _referenceRepository.UpdateSector(sector);
        }
    }
}
