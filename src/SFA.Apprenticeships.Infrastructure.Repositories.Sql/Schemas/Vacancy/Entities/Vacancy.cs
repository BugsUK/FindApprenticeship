namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Vacancy")]
    public class Vacancy
    {
        public int VacancyId { get; set; }

        public Guid VacancyGuid { get; set; }

        public int VacancyOwnerRelationshipId { get; set; }

        public int VacancyTypeId { get; set; }

        public int VacancyReferenceNumber { get; set; }

        public string ContactName { get; set; }

        public int VacancyStatusId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string AddressLine4 { get; set; }

        public string AddressLine5 { get; set; }

        public string Town { get; set; }

        public int CountyId { get; set; }

        public string PostCode { get; set; }

        public int? LocalAuthorityId { get; set; }

        public int? GeocodeEasting { get; set; }

        public int? GeocodeNorthing { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? ApprenticeshipFrameworkId { get; set; }

        //public string FrameworkIdComment { get; set; }

        public int? StandardId { get; set; }

        //public string StandardIdComment { get; set; }

        [Required]
        public string Title { get; set; }

        //public string TitleComment { get; set; }

        public int? ApprenticeshipType { get; set; } // equivalent to LevelCode (old-new schema)

        //public string LevelCodeComment { get; set; }

        public string ShortDescription { get; set; }

        // public string ShortDescriptionComment { get; set; }

        public string Description { get; set; } // equivalent to LongDescription (old-new schema)

        //public string LongDescriptionComment { get; set; }

        [Column(TypeName = "money")]
        public decimal? WeeklyWage { get; set; } // equivalent (more or less) to WageValue

        public int WageType { get; set; } // equivalent to WageTypeCode

        public string WageText { get; set; }

        // public string WageIntervalCode { get; set; }

        // public string WageComment { get; set; }

        public int WageUnitId { get; set; }

        [Column(TypeName = "smallint")]
        public short? NumberOfPositions { get; set; } // was int

        // public string NumberOfPositionsComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ApplicationClosingDate { get; set; } // was ClosingDate

        // public string ClosingDateComment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? InterviewsFromDate { get; set; } // was AV_InterviewStartDate

        [Column(TypeName = "datetime2")]
        public DateTime? ExpectedStartDate { get; set; } // was PossibleStartDate

        // public string PossibleStartDateComment { get; set; }

        public int? DurationValue { get; set; } //Was short

        public string ExpectedDuration { get; set; }

        //[StringLength(1)]
        public int DurationTypeId { get; set; }

        //public string DurationComment { get; set; }

        public string WorkingWeek { get; set; } // was WorkingWeekText

        //public string WorkingWeekComment { get; set; }

        public int? NumberOfViews { get; set; }

        public string EmployerAnonymousName { get; set; }

        public string EmployerDescription { get; set; }

        //public string EmployerDescriptionComment { get; set; }

        public string EmployersWebsite { get; set; } // was EmployerWebsite -> change to employerwebsite?

        public int? MaxNumberofApplications { get; set; }

        public bool? ApplyOutsideNAVMS { get; set; }        // Equivalent to IsDirectApplication

        public string EmployersApplicationInstructions { get; set; } // Equivalent to DirectApplicationInstructions

        // public string DirectApplicationInstructionsComment { get; set; }

        public string EmployersRecruitmentWebsite { get; set; } // equivalent to DirectApplicationUrl

        // public string DirectApplicationUrlComment { get; set; }

        // public string EmployerWebsiteUrlComment { get; set; }

        public string BeingSupportedBy { get; set; }

        public DateTime? LockedForSupportUntil { get; set; }

        public int? NoOfOfflineApplicants { get; set; } // What's the difference with NoOfOfflineSystemApplicants?

        public int? MasterVacancyId { get; set; }   // equivalent to ParentVacancyId

        public int? VacancyLocationTypeId { get; set; } // equivalent to VacancyLocationTypeCode

        public int? NoOfOfflineSystemApplicants { get; set; } // equivalent to OfflineApplicationClickThroughCount

        public int? VacancyManagerID { get; set; } // Probably equivalent to ManagerVacancyPartyId, or VacancyManagerID

        public int? DeliveryOrganisationID { get; set; } // Probably equivalent to DeliveryProviderVacancyPartyId

        public int? ContractOwnerID { get; set; } // Probably equivalent to OwnerVacancyPartyId

        public bool SmallEmployerWageIncentive { get; set; }

        public int? OriginalContractOwnerId { get; set; } //Probably equivalent to OriginalContractOwnerVacancyPartyId

        public bool VacancyManagerAnonymous { get; set; }

        //[Required]
        //[StringLength(1)]
        //public string VacancyTypeCode { get; set; }  // In AVMS this is merged with apprenticeship type


        // public int EmployerVacancyPartyId { get; set; }

        // public int OwnerVacancyPartyId { get; set; } -> probably goes to ContractOwnerID

        // public int ManagerVacancyPartyId { get; set; } -> probably goes to VacancyManagerID

        // public int DeliveryProviderVacancyPartyId { get; set; } -> probably goes to DeliveryOrganisationID

        // public int ContractOwnerVacancyPartyId { get; set; } -> probably goes to ContractOwnerID

        // public int? OriginalContractOwnerVacancyPartyId { get; set; } -> probably goes to OriginalContractOwnerId


        //[Required]
        //[StringLength(1)]
        //public string TrainingTypeCode { get; set; } // Framework or standard. Not considered in AVMS

        public int TrainingTypeId { get; set; }

        public decimal? HoursPerWeek { get; set; } // Not considered in AVMS

        public string AdditionalLocationInformation { get; set; }

        // public string AdditionalLocationInformationComment { get; set; }

        // public string DesiredSkills { get; set; } // Get from VacancyTextField table

        // public string DesiredSkillsComment { get; set; }

        // public string FutureProspects { get; set; }  // Get from VacancyTextField table

        // public string FutureProspectsComment { get; set; }

        // public string PersonalQualities { get; set; }  // Get from VacancyTextField table

        // public string PersonalQualitiesComment { get; set; }

        // public string ThingsToConsider { get; set; }  // Get from VacancyTextField table

        // public string ThingsToConsiderComment { get; set; }

        // public string DesiredQualifications { get; set; }  // Get from VacancyTextField table

        // public string DesiredQualificationsComment { get; set; }

        // public string FirstQuestion { get; set; } // Get from AdditionalQuestion table

        // public string FirstQuestionComment { get; set; }

        // public string SecondQuestion { get; set; } // Get from AdditionalQuestion table

        // public string SecondQuestionComment { get; set; }

        public string QAUserName { get; set; } // Not considered in AVMS

        // public string LocationAddressesComment { get; set; }

        public int SubmissionCount { get; set; } // Not considered in AVMS

        public DateTime? StartedToQADateTime { get; set; }  // Not considered in AVMS

        // public Guid LastEditedById { get; set; } // Can be inferred from VacancyHistory?

        // public Guid VacancyManagerId { get; set; } -> probably goes to VacancyManagerID

        // public string TrainingProvided { get; set; } // Get from VacancyTextField table

        // public string TrainingProvidedComment { get; set; }

        public string ContactNumber { get; set; } // Added to AVMS

        public string ContactEmail { get; set; } // Added to AVMS

        // public string ContactDetailsComment { get; set; } // Not considered in AVMS

        public bool EditedInRaa { get; set; }

        public int? SectorId { get; set; }
    }
}
