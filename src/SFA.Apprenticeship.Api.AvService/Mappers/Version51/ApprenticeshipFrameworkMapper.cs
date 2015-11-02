namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.ReferenceData;
    using DataContracts.Version50;

    public class ApprenticeshipFrameworkMapper
    {
        public IEnumerable<ApprenticeshipFrameworkAndOccupationData> FromCategories(IEnumerable<Category> categories)
        {
            return categories.Select(category => new ApprenticeshipFrameworkAndOccupationData());
        }
    }
}
