namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using DataContracts.Version51;

    public interface IReferenceDataProvider
    {
        List<CountyData> GetCounties();
    }
}