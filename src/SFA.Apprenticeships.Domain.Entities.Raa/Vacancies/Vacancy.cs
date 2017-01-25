// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Reference;

    public class Vacancy : VacancySummary, ICreatableEntity, IUpdatableEntity, ICloneable
    {
        public string AdditionalLocationInformation { get; set; }
        public string AdditionalLocationInformationComment { get; set; }
        public string ApprenticeshipLevelComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string ContactDetailsComment { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string CreatedByProviderUsername { get; set; }
        public string DesiredQualifications { get; set; }
        public string DesiredQualificationsComment { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredSkillsComment { get; set; }
        public string DurationComment { get; set; }
        public bool EditedInRaa { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerDescriptionComment { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }
        public string FirstQuestion { get; set; }
        public string FirstQuestionComment { get; set; }
        public string FrameworkCodeNameComment { get; set; }
        public string FutureProspects { get; set; }
        public string FutureProspectsComment { get; set; }
        public int LastEditedById { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocationAddressesComment { get; set; }
        public string LongDescription { get; set; }
        public string LongDescriptionComment { get; set; }
        public string NumberOfPositionsComment { get; set; }
        public string OfflineApplicationInstructions { get; set; }
        public string OfflineApplicationInstructionsComment { get; set; }
        public string OfflineApplicationUrl { get; set; }
        public string OfflineApplicationUrlComment { get; set; }
        public OfflineVacancyType? OfflineVacancyType { get; set; }
        public string OtherInformation { get; set; }
        public string OtherInformationComment { get; set; }
        public string PersonalQualities { get; set; }
        public string PersonalQualitiesComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string SecondQuestion { get; set; }
        public string SecondQuestionComment { get; set; }
        public string SectorCodeNameComment { get; set; }
        public string ShortDescriptionComment { get; set; }
        public string StandardIdComment { get; set; }
        public string ThingsToConsider { get; set; }
        public string ThingsToConsiderComment { get; set; }

        public string TitleComment { get; set; }
        public string TrainingProvided { get; set; }
        public string TrainingProvidedComment { get; set; }
        public VacancySource VacancySource { get; set; }
        public string WageComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string AnonymousEmployerDescriptionComment { get; set; }
        public string AnonymousEmployerReasonComment { get; set; }
        public string AnonymousAboutTheEmployerComment { get; set; }

        public FrameworkStatusType FrameworkStatus { get; set; }
        public FrameworkStatusType StandardStatus { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public object Clone()
        {
            return new Vacancy
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
                VacancyGuid = VacancyGuid,
                Title = Title,
                TitleComment = TitleComment,
                ShortDescription = ShortDescription,
                ShortDescriptionComment = ShortDescriptionComment,
                WorkingWeek = WorkingWeek,
                Wage = Wage,
                ExpectedDuration = ExpectedDuration,
                DurationType = DurationType,
                Duration = Duration,
                ClosingDate = ClosingDate,
                PossibleStartDate = PossibleStartDate,
                LongDescription = LongDescription,
                DesiredSkills = DesiredSkills,
                DesiredSkillsComment = DesiredSkillsComment,
                FutureProspects = FutureProspects,
                FutureProspectsComment = FutureProspectsComment,
                PersonalQualities = PersonalQualities,
                PersonalQualitiesComment = PersonalQualitiesComment,
                ThingsToConsider = ThingsToConsider,
                ThingsToConsiderComment = ThingsToConsiderComment,
                DesiredQualifications = DesiredQualifications,
                DesiredQualificationsComment = DesiredQualificationsComment,
                FirstQuestion = FirstQuestion,
                SecondQuestion = SecondQuestion,
                EmployerDescription = EmployerDescription,
                EmployerWebsiteUrl = EmployerWebsiteUrl,
                OfflineVacancy = OfflineVacancy,
                OfflineVacancyType = OfflineVacancyType,
                OfflineApplicationUrl = OfflineApplicationUrl,
                OfflineApplicationUrlComment = OfflineApplicationUrlComment,
                OfflineApplicationInstructions = OfflineApplicationInstructions,
                OfflineApplicationInstructionsComment = OfflineApplicationInstructionsComment,
                NoOfOfflineApplicants = NoOfOfflineApplicants,
                DateSubmitted = DateSubmitted,
                DateFirstSubmitted = DateFirstSubmitted,
                DateStartedToQA = DateStartedToQA,
                QAUserName = QAUserName,
                DateQAApproved = DateQAApproved,
                SubmissionCount = SubmissionCount,
                VacancyManagerId = VacancyManagerId,
                LastEditedById = LastEditedById,
                ParentVacancyId = ParentVacancyId,
                TrainingType = TrainingType,
                ApprenticeshipLevel = ApprenticeshipLevel,
                ApprenticeshipLevelComment = ApprenticeshipLevelComment,
                FrameworkCodeName = FrameworkCodeName,
                FrameworkStatus = FrameworkStatus,
                StandardStatus = StandardStatus,
                FrameworkCodeNameComment = FrameworkCodeNameComment,
                StandardId = StandardId,
                StandardIdComment = StandardIdComment,
                SectorCodeName = SectorCodeName,
                SectorCodeNameComment = SectorCodeNameComment,
                Status = Status,
                WageComment = WageComment,
                ClosingDateComment = ClosingDateComment,
                DurationComment = DurationComment,
                LongDescriptionComment = LongDescriptionComment,
                PossibleStartDateComment = PossibleStartDateComment,
                WorkingWeekComment = WorkingWeekComment,
                FirstQuestionComment = FirstQuestionComment,
                SecondQuestionComment = SecondQuestionComment,
                AdditionalLocationInformation = AdditionalLocationInformation,
                VacancyLocationType = VacancyLocationType,
                NumberOfPositions = NumberOfPositions,
                EmployerDescriptionComment = EmployerDescriptionComment,
                EmployerWebsiteUrlComment = EmployerWebsiteUrlComment,
                LocationAddressesComment = LocationAddressesComment,
                NumberOfPositionsComment = NumberOfPositionsComment,
                AdditionalLocationInformationComment = AdditionalLocationInformationComment,
                TrainingProvided = TrainingProvided,
                TrainingProvidedComment = TrainingProvidedComment,
                ContactName = ContactName,
                ContactNumber = ContactNumber,
                ContactEmail = ContactEmail,
                ContactDetailsComment = ContactDetailsComment,
                VacancyType = VacancyType,
                Address = Address,
                ContractOwnerId = ContractOwnerId,
                OriginalContractOwnerId = OriginalContractOwnerId,
                EditedInRaa = EditedInRaa,
                AnonymousEmployerReasonComment = AnonymousEmployerReasonComment,
                AnonymousEmployerDescriptionComment = AnonymousEmployerDescriptionComment,
                AnonymousAboutTheEmployerComment = AnonymousAboutTheEmployerComment
            };
        }
    }
}
