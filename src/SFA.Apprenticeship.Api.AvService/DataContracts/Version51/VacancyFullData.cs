namespace SFA.Apprenticeship.Api.AvService.DataContracts.Version51
{
    using System;
    using System.Runtime.Serialization;
    using Common;
    using Namespaces.Version51;

    [DataContract(Namespace = Namespace.Uri)]
    public class VacancyFullData : VacancySummaryData
    {
        [DataMember]
        public string FullDescription { get; set; }

        [DataMember]
        public string SupplementaryQuestion1 { get; set; }

        [DataMember]
        public string SupplementaryQuestion2 { get; set; }

        [DataMember]
        public string ContactPerson { get; set; }

        [DataMember]
        public string EmployerDescription { get; set; }

        [DataMember]
        public string ExpectedDuration { get; set; }

        [DataMember]
        public string FutureProspects { get; set; }

        [DataMember]
        public DateTime InterviewFromDate { get; set; }

        [DataMember]
        public string LearningProviderDesc { get; set; }

        [DataMember]
        public int? LearningProviderSectorPassRate { get; set; }

        [DataMember]
        public string PersonalQualities { get; set; }

        [DataMember]
        public DateTime PossibleStartDate { get; set; }

        [DataMember]
        public string QualificationRequired { get; set; }

        [DataMember]
        public string SkillsRequired { get; set; }

        [DataMember]
        public string TrainingToBeProvided { get; set; }

        [DataMember]
        public WageType WageType { get; set; } // Release 5.1

        [DataMember]
        public decimal? Wage { get; set; } // Changed from Weekly wage in Release 5.1

        [DataMember]
        public string WageText { get; set; } // Release 5.1

        [DataMember]
        public string WorkingWeek { get; set; }

        [DataMember]
        public string OtherImportantInformation { get; set; }

        [DataMember]
        public string EmployerWebsite { get; set; }

        [DataMember]
        public string VacancyOwner { get; set; } // Release 5.1

        [DataMember]
        public string ContractOwner { get; set; } // Release 5.1

        [DataMember]
        public string DeliveryOrganisation { get; set; } // Release 5.1

        [DataMember]
        public string VacancyManager { get; set; } // Release 5.1

        [DataMember]
        public bool? IsDisplayRecruitmentAgency { get; set; } // Release 5.1

        [DataMember]
        public bool IsSmallEmployerWageIncentive { get; set; } // Release 5.1
    }
}
