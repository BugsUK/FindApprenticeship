namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;
    using Namespaces.Version50;

    [MessageContract]
    public class BulkVacancyUploadRequest : NavmsMessageHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public List<VacancyUploadData> Vacancies { get; set; }
    }
}
