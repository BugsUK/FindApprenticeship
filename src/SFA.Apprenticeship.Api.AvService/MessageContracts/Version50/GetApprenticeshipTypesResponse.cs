namespace SFA.Apprenticeship.Api.AvService.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class GetApprenticeshipTypesResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces)]
        public List<ApprenticeshipTypeData> ApprenticeshipTypes { get; set; }
    }
}
