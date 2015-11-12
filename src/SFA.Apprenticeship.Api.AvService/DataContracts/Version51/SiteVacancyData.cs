namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System.Runtime.Serialization;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class SiteVacancyData
    {
        [DataMember(IsRequired = true, Order = 1)]
        public AddressData AddressDetails;

        [DataMember(IsRequired = true, Order = 2)]
        public short NumberOfVacancies;

        [DataMember(IsRequired = false, Order = 3)]
        public string EmployerWebsite { get; set; }

    }
}
