namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class DivisionData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Code;

        [DataMember(IsRequired = true, Order = 2)]
        public string FullName;
    }
}
