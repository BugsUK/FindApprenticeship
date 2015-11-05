namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version50
{
    using System;
    using System.Runtime.Serialization;
    using Namespaces.Version50;

    [DataContract(Namespace = Namespace.Uri)]
    public class VacancyFullData : VacancySummaryData
    {
        [DataMember(IsRequired = true, Order = 15)]
        public string FullDescription { get; set; }

        [DataMember(IsRequired = true, Order = 16)]
        public string SupplementaryQuestion1 { get; set; }

        [DataMember(IsRequired = true, Order = 17)]
        public string SupplementaryQuestion2 { get; set; }

        [DataMember(IsRequired = true, Order = 18)]
        public string ContactPerson { get; set; }

        [DataMember(IsRequired = true, Order = 19)]
        public string EmployerDescription { get; set; }

        [DataMember(IsRequired = true, Order = 20)]
        public string ExpectedDuration { get; set; }

        [DataMember(IsRequired = true, Order = 21)]
        public string FutureProspects { get; set; }

        [DataMember(IsRequired = true, Order = 22)]
        public DateTime InterviewFromDate { get; set; }

        [DataMember(IsRequired = true, Order = 23)]
        public string LearningProviderDesc { get; set; }

        [DataMember(IsRequired = true, Order = 24)]
        public int? LearningProviderSectorPassRate { get; set; }

        [DataMember(IsRequired = true, Order = 25)]
        public string PersonalQualities { get; set; }

        [DataMember(IsRequired = true, Order = 26)]
        public DateTime PossibleStartDate { get; set; }

        [DataMember(IsRequired = true, Order = 27)]
        public string QualificationRequired { get; set; }

        [DataMember(IsRequired = true, Order = 28)]
        public string SkillsRequired { get; set; }

        [DataMember(IsRequired = true, Order = 29)]
        public string TrainingToBeProvided { get; set; }

        [DataMember(IsRequired = true, Order = 30)]
        public decimal WeeklyWage { get; set; }

        [DataMember(IsRequired = true, Order = 31)]
        public string WorkingWeek { get; set; }

        // removed as a duplicate
        //[DataMember(IsRequired = true, Order = 32)] 
        //public DateTime ClosingDate { get; set; }

        [DataMember(IsRequired = true, Order = 33)]
        public string OtherImportantInformation { get; set; }

        [DataMember(IsRequired = true, Order = 34)]
        public string EmployerWebsite { get; set; }
    }
}
