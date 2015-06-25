namespace FrameworkDataImport.Process
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public interface IFrameworkDataComparer
    {
        void Compare(List<Category> categories);
    }
}