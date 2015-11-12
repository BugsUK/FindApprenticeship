namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;
    using Namespaces.Version51;

    [MessageContract]
    public class VacancyUploadInternalRequest : NavmsInternalMessageHeader
    {
        [MessageBodyMember(Namespace = Namespace.Uri, Order = 1)]
        public int ClientMajorVersion { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri, Order = 2)]
        public int ClientMinorVersion { get; set; }

        [MessageBodyMember(Namespace = Namespace.Uri, Order = 3)]
        public List<VacancyUploadData> Vacancies { get; set; }

    }
}
