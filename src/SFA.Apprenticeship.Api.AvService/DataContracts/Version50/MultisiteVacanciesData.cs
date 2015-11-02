namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class MultisiteVacanciesData
    {
        [DataMember(IsRequired = true, Order = 1)] 
        public AddressData AddressDetails;

        [DataMember(IsRequired = true, Order = 2)]
        public int NumberOfVacancies;
    }
}
