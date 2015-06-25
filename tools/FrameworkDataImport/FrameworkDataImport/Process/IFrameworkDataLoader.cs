namespace FrameworkDataImport.Process
{
    using System.Collections.Generic;
    using SFA.Apprenticeships.Domain.Entities.ReferenceData;

    public interface IFrameworkDataLoader
    {
        void Load(List<Category> categories);
    }
}