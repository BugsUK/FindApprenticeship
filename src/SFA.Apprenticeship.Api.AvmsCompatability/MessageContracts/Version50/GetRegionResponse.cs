namespace SFA.Apprenticeship.Api.AvmsCompatability.MessageContracts.Version50
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using DataContracts.Version50;

    [MessageContract]
    public class GetRegionResponse : NavmsResponseHeader
    {
        [MessageBodyMember(Namespace = CommonNamespaces.ExternalInterfaces)]
        public List<RegionData> Regions { get; set; }
    }
}
