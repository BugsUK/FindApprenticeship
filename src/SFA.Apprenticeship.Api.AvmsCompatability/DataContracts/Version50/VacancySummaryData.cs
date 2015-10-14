namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = CommonNamespaces.ExternalInterfaces)]
    public class VacancySummaryData
    {
        
        [DataMember(IsRequired = true, Order = 1)]
        public string VacancyLocationType { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public AddressData VacancyAddress { get; set; }

        //[DataMember(IsRequired = true, Order = 3)]
        //public int VacancyId { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public string ApprenticeshipFramework { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public DateTime ClosingDate { get; set; }

        [DataMember(IsRequired = true, Order = 6)]
        public string ShortDescription { get; set; }

        [DataMember(IsRequired = true, Order = 7)]
        public string EmployerName { get; set; }

        [DataMember(IsRequired = true, Order = 8)]
        public string LearningProviderName { get; set; }

        [DataMember(IsRequired = true, Order = 9)]
        public int NumberOfPositions { get; set; }

        [DataMember(IsRequired = true, Order = 10)]
        public string VacancyTitle { get; set; }

        [DataMember(IsRequired = true, Order = 11)]
        public DateTime CreatedDateTime { get; set; }

        [DataMember(IsRequired = true, Order = 12)]
        public int VacancyReference { get; set; }

        [DataMember(IsRequired = true, Order = 13)]
        public string VacancyType { get; set; }

        [DataMember(IsRequired = true, Order = 14)]
        public string VacancyUrl { get; set; }
    }
}
