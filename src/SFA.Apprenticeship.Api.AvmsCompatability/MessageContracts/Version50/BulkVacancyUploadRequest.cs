namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using AvmsCompatability;
    using DataContracts.Version50;

    [MessageContract]
    public class BulkVacancyUploadRequest: NavmsMessageHeader
    {
        [MessageBodyMember(Namespace=CommonNamespaces.ExternalInterfaces, Order=1)]
        public List<VacancyUploadData> Vacancies { get; set; } 
    }
}
