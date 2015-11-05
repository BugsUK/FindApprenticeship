namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces.Version51;

    [MessageContract]
    public class VacancyUploadResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri)]
        public List<VacancyUploadResultData> Vacancies { get; set; }
    }
}
