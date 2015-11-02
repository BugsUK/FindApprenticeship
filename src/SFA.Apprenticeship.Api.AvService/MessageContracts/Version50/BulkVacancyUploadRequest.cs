namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using AvService;
    using DataContracts.Version50;

    [MessageContract]
    public class BulkVacancyUploadRequest: NavmsMessageHeader
    {
        [MessageBodyMember(Namespace=CommonNamespaces.ExternalInterfaces, Order=1)]
        public List<VacancyUploadData> Vacancies { get; set; } 
    }
}
