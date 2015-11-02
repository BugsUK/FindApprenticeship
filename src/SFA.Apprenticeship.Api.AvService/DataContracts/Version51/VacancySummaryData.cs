namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfacesRel51)]
    public class VacancySummaryData
    {        
        [DataMember]
        public string VacancyLocationType { get; set; }

        [DataMember]
        public AddressData VacancyAddress { get; set; }

        //[DataMember]
        //public int VacancyId { get; set; }

        [DataMember]
        public string ApprenticeshipFramework { get; set; }

        [DataMember]
        public DateTime ClosingDate { get; set; }

        [DataMember]
        public string ShortDescription { get; set; }

        [DataMember]
        public string EmployerName { get; set; }

        [DataMember]
        public string LearningProviderName { get; set; }

        [DataMember]
        public int NumberOfPositions { get; set; }

        [DataMember]
        public string VacancyTitle { get; set; }

        [DataMember]
        public DateTime CreatedDateTime { get; set; }

        [DataMember]
        public int VacancyReference { get; set; }

        [DataMember]
        public string VacancyType { get; set; }

        [DataMember]
        public string VacancyUrl { get; set; }
    }
}
