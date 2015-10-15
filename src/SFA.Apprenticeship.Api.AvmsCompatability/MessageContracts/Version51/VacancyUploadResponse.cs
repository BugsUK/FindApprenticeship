namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class VacancyUploadResponse: NavmsResponseHeader
    {
        [MessageBodyMember( Namespace = CommonNamespaces.ExternalInterfacesRel51 )]
        public List<VacancyUploadResultData> Vacancies { get; set; }
    }
}
