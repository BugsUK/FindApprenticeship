namespace SFA.Apprenticeships.Application.Interfaces.ReferenceData
{
    using System.Collections.Generic;
    using Domain.Entities.ReferenceData;

    public interface IReferenceDataService
    {
        IEnumerable<Category> GetCategories();

        Category GetSubCategory(string subCategory);
    }
}
