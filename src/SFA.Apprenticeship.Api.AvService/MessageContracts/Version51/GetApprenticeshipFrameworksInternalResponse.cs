namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version51
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version51;

    [MessageContract]
    public class GetApprenticeshipFrameworksInternalResponse : NavmsInternalResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
        public List<ApprenticeshipFrameworkAndOccupationData> ApprenticeshipFrameworks { get; set; }
    }
}
