namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version51
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class DivisionData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Code;

        [DataMember(IsRequired = true, Order = 2)]
        public string FullName;
    }
}
