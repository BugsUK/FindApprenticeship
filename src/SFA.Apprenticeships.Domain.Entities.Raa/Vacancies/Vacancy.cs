// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;
    using Locations;

    public class Vacancy : ICreatableEntity, IUpdatableEntity, ICloneable
    {
        public object Clone()
        {
            return new Vacancy
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                VacancyGuid = VacancyGuid,
                Title = Title,
                TitleComment = TitleComment,
                ShortDescription = ShortDescription,
                ShortDescriptionComment = ShortDescriptionComment,
                WorkingWeek = WorkingWeek,
                HoursPerWeek = HoursPerWeek,
                WageType = WageType,
                Wage = Wage,
                WageUnit = WageUnit,
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
                OwnerPartyId = OwnerPartyId,
                EmployerDescription = EmployerDescription,
                EmployerWebsiteUrl = EmployerWebsiteUrl,
                OfflineVacancy = OfflineVacancy,
                OfflineApplicationUrl = OfflineApplicationUrl,
                OfflineApplicationUrlComment = OfflineApplicationUrlComment,
                OfflineApplicationInstructions = OfflineApplicationInstructions,
                OfflineApplicationInstructionsComment = OfflineApplicationInstructionsComment,
                OfflineApplicationClickThroughCount = OfflineApplicationClickThroughCount,
                DateSubmitted = DateSubmitted,
                DateFirstSubmitted = DateFirstSubmitted,
                DateStartedToQA = DateStartedToQA,
                QAUserName = QAUserName,
                DateQAApproved = DateQAApproved,
                SubmissionCount = SubmissionCount,
                VacancyManagerId = VacancyManagerId,
                LastEditedById = LastEditedById,
                ParentVacancyReferenceNumber = ParentVacancyReferenceNumber,
                TrainingType = TrainingType,
                ApprenticeshipLevel = ApprenticeshipLevel,
                ApprenticeshipLevelComment = ApprenticeshipLevelComment,
                FrameworkCodeName = FrameworkCodeName,
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
                IsEmployerLocationMainApprenticeshipLocation = IsEmployerLocationMainApprenticeshipLocation,
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
                Address = Address
            };
        }

        public int VacancyId { get; set; }
        public long VacancyReferenceNumber { get; set; }
        public Guid VacancyGuid { get; set; }
        public string Title { get; set; }
        public string TitleComment { get; set; }
        public string ShortDescription { get; set; }
        public string ShortDescriptionComment { get; set; }
        public string WorkingWeek { get; set; }
        public decimal? HoursPerWeek { get; set; }
        public WageType WageType { get; set; }
        public decimal? Wage { get; set; }
        public WageUnit WageUnit { get; set; }
        public DurationType DurationType { get; set; }
        public int? Duration { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? PossibleStartDate { get; set; }
        public string LongDescription { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredSkillsComment { get; set; }
        public string FutureProspects { get; set; }
        public string FutureProspectsComment { get; set; }
        public string PersonalQualities { get; set; }
        public string PersonalQualitiesComment { get; set; }
        public string ThingsToConsider { get; set; }
        public string ThingsToConsiderComment { get; set; }
        public string DesiredQualifications { get; set; }
        public string DesiredQualificationsComment { get; set; }
        public string FirstQuestion { get; set; }
        public string SecondQuestion { get; set; }
        public int OwnerPartyId { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public bool? OfflineVacancy { get; set; }
        public string OfflineApplicationUrl { get; set; }
        public string OfflineApplicationUrlComment { get; set; }
        public string OfflineApplicationInstructions { get; set; }
        public string OfflineApplicationInstructionsComment { get; set; }
        public int OfflineApplicationClickThroughCount { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? DateFirstSubmitted { get; set; }
        public DateTime? DateStartedToQA { get; set; }
        public string QAUserName { get; set; }
        public DateTime? DateQAApproved { get; set; }
        public int SubmissionCount { get; set; }
        //Id if the Provider User who created the vacancy
        public int VacancyManagerId { get; set; }
        public int LastEditedById { get; set; }
        public long ParentVacancyReferenceNumber { get; set; }
        public TrainingType TrainingType { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string ApprenticeshipLevelComment { get; set; }
        public string FrameworkCodeName { get; set; }
        public string FrameworkCodeNameComment { get; set; }
        public int? StandardId { get; set; }
        public string StandardIdComment { get; set; }
        public string SectorCodeName { get; set; }
        public string SectorCodeNameComment { get; set; }
        public VacancyStatus Status { get; set; }
        public string WageComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string DurationComment { get; set; }
        public string LongDescriptionComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string FirstQuestionComment { get; set; }
        public string SecondQuestionComment { get; set; }
        public string AdditionalLocationInformation { get; set; }
        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public int? NumberOfPositions { get; set; }
        public string EmployerDescriptionComment { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }
        public string LocationAddressesComment { get; set; }
        public string NumberOfPositionsComment { get; set; }
        public string AdditionalLocationInformationComment { get; set; }
        public string TrainingProvided { get; set; }
        public string TrainingProvidedComment { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactDetailsComment { get; set; }
        public VacancyType VacancyType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public PostalAddress Address { get; set; }
    }
}
