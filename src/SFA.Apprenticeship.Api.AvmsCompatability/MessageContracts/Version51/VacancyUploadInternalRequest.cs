namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class VacancyUploadInternalRequest: NavmsInternalMessageHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51, Order = 1)]
        public int ClientMajorVersion { get; set; }

        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51, Order = 2)]
        public int ClientMinorVersion { get; set; }

        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51, Order = 3)]
        public List<VacancyUploadData> Vacancies { get; set; }

    }
}
