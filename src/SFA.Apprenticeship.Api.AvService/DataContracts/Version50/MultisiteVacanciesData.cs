namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class MultisiteVacanciesData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public AddressData AddressDetails;

        [DataMember(IsRequired = true, Order = 2)]
        public int NumberOfVacancies;
    }
}
