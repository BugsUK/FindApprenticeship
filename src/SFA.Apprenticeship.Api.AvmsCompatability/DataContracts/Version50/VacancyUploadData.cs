namespace SFA.Apprenticeship.Api.AvmsCompatability.DataContracts.Version50
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Common;

    [DataContract( Namespace = CommonNamespaces.ExternalInterfaces )]
    public class VacancyUploadData
    {       
        [DataMember( IsRequired = true, Order = 1)]
        public Guid VacancyId { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public int LearningProviderEdsUrn { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public int EmployerEdsUrn { get; set; }

        [DataMember( IsRequired = true, Order = 4)]
        public string Title { get; set; }

        [DataMember( IsRequired = true, Order = 5)]
        public string ShortDescription { get; set; }

        [DataMember( IsRequired = true, Order = 6)]
        public string LongDescription { get; set; }

        [DataMember(IsRequired = true, Order = 7)]
        public Decimal WeeklyWage { get; set; }

        [DataMember(IsRequired = true, Order = 8)]
        public string WorkingWeek { get; set; }

        [DataMember(IsRequired = false, Order = 9)]
        public string FutureProspects { get; set; }
        
        [DataMember(IsRequired = false, Order = 10)]
        public string ContactName { get; set; }

        //Page 2
        [DataMember(IsRequired = false, Order = 11)]
        public string EmployerAnonymousName { get; set; }

        [DataMember(IsRequired = false, Order = 12)]
        public byte[] EmployerImage { get; set; }

        [DataMember(IsRequired = false, Order = 13)]
        public string EmployerDescription { get; set; }

        [DataMember(IsRequired = false, Order = 14)]
        public string EmployerWebsite { get; set; }

        [DataMember(IsRequired = true, Order = 15)]
        public VacancyLocationType VacancyLocationType { get; set; }

        [DataMember(IsRequired = false, Order = 16)]
        public AddressData AddressDetails { get; set; }

        [DataMember(IsRequired = false, Order = 17)]
        public short NumberOfPositions { get; set; }

        [DataMember(IsRequired = false, Order = 18)]
        public List<MultisiteVacanciesData> MultisiteVacanciesDetails { get; set; }

        /// <summary>
        /// Apprenticeship framework ID
        /// </summary>
        [DataMember(IsRequired = true, Order = 19)]
        public string ApprenticeshipFramework { get; set; }

        /// <summary>  
        /// Apprenticeship = 1,
        /// AdvancedApprenticeship = 2,
        /// HigherApprenticeship = 3,
        /// </summary>
        [DataMember(IsRequired = true, Order = 20)]
        public VacancyType VacancyType { get; set; }

        [DataMember(IsRequired = false, Order = 21)]
        public string TrainingToBeProvided { get; set; }

        [DataMember(IsRequired = false, Order = 22)]
        public string ExpectedDurationOfApprenticeship { get; set; }

        //Page3
        [DataMember(IsRequired = false, Order = 23)]
        public string SkillsRequired { get; set; }

        [DataMember(IsRequired = false, Order = 24)]
        public string PersonalQualities { get; set; }

        [DataMember(IsRequired = false, Order = 25)]
        public string QualificationRequired { get; set; }

        [DataMember(IsRequired = false, Order = 26)]
        public string RealityCheck { get; set; }

        [DataMember(IsRequired = false, Order = 27)]
        public string OtherImportantInformation { get; set; }

        [DataMember(IsRequired = false, Order = 28)]
        public string SupplementaryQuestion1 { get; set; }

        [DataMember(IsRequired = false, Order = 29)]
        public string SupplementaryQuestion2 { get; set; }

        //Page 4
        [DataMember(IsRequired = true, Order = 30)]
        public DateTime ClosingDate { get; set; }

        [DataMember(IsRequired = true, Order = 31)]
        public DateTime InterviewStartDate { get; set; }

        [DataMember(IsRequired = true, Order = 32)]
        public DateTime PossibleStartDate { get; set; }

        /// <summary>
        /// valid values: 
        /// 0 - online
        /// 1 - offline
        /// </summary>
        [DataMember(IsRequired = true, Order = 33)]
        public ApplicationType ApplicationType { get; set; }

        [DataMember( IsRequired = false, Order = 34)]
        public string ApplicationInstructions { get; set; }

        [DataMember(IsRequired = false, Order = 35)]
        public string EmployerExternalApplicationWebsite { get; set; }

    }
}
