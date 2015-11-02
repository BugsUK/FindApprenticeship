namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System;
    using System.Runtime.Serialization;

    [DataContract( Namespace = CommonNamespaces.ExternalInterfacesRel51 )]
    public class VacancyUploadData
    {       
        [DataMember( IsRequired = true, Order = 1)]
        public Guid VacancyId { get; set; }

        [DataMember( IsRequired = true, Order = 2)]
        public string Title { get; set; }

        [DataMember( IsRequired = true, Order = 3)]
        public string ShortDescription { get; set; }

        [DataMember( IsRequired = true, Order = 4)]
        public string LongDescription { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public EmployerData Employer { get; set; }

        [DataMember(IsRequired = true, Order = 6)]
        public VacancyData Vacancy { get; set; }

        [DataMember(IsRequired = true, Order = 7)]
        public ApplicationData Application { get; set; }

        [DataMember(IsRequired = true, Order = 8)]
        public ApprenticeshipData Apprenticeship { get; set; }

        //Page 4
        [DataMember(IsRequired = true, Order = 9)]
        public int? ContractedProviderUkprn { get; set; }

        [DataMember(IsRequired = true, Order = 10)]
        public int VacancyOwnerEdsUrn { get; set; }

        [DataMember(IsRequired = false, Order = 11)]
        public int? VacancyManagerEdsUrn { get; set; }

        [DataMember(IsRequired = false, Order = 12)]
        public int? DeliveryProviderEdsUrn { get; set; }

        [DataMember(IsRequired = false, Order = 13)]
        public bool? IsDisplayRecruitmentAgency { get; set; }

        [DataMember(IsRequired = true, Order = 14)]
        public bool IsSmallEmployerWageIncentive { get; set; }
    }
}
