using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using SFA.Apprenticeships.Application.ReferenceData;
using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
using SFA.Apprenticeships.Domain.Entities.ReferenceData;

namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Domain.Raa.Interfaces.Repositories;

    public class ReferenceDataProvider  : IReferenceDataProvider
    {
        private IReferenceRepository _referenceRepository;

        public ReferenceDataProvider(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Category GetSubCategoryByName(string subCategoryName)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryByName(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Category GetSubCategoryByCode(string subCategoryCode)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sector> GetSectors()
        {
            throw new NotImplementedException();
        }
    }
}
