namespace SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship
{
    using System;
    using System.Collections.Generic;
    using Locations;

    public class ApprenticeshipVacancy : Vacancy, ICloneable
    {
        /// <summary>
        /// This is a SHALLOW clone, only
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new ApprenticeshipVacancy(this);
        }

        public ApprenticeshipVacancy() { }

        private ApprenticeshipVacancy(ApprenticeshipVacancy previousVacancy)
        {
            VacancyReferenceNumber = previousVacancy.VacancyReferenceNumber;
            Ukprn = previousVacancy.Ukprn;
            Title = previousVacancy.Title;
            TitleComment = previousVacancy.TitleComment;
            ShortDescription = previousVacancy.ShortDescription;
            ShortDescriptionComment = previousVacancy.ShortDescriptionComment;
            WorkingWeek = previousVacancy.WorkingWeek;
            HoursPerWeek = previousVacancy.HoursPerWeek;
            WageType = previousVacancy.WageType;
            Wage = previousVacancy.Wage;
            WageUnit = previousVacancy.WageUnit;
            DurationType = previousVacancy.DurationType;
            Duration = previousVacancy.Duration;
            ClosingDate = previousVacancy.ClosingDate;
            InterviewStartDate = previousVacancy.InterviewStartDate;
            PossibleStartDate = previousVacancy.PossibleStartDate;
            LongDescription = previousVacancy.LongDescription;
            DesiredSkills = previousVacancy.DesiredSkills;
            DesiredSkillsComment = previousVacancy.DesiredSkillsComment;
            FutureProspects = previousVacancy.FutureProspects;
            FutureProspectsComment = previousVacancy.FutureProspectsComment;
            PersonalQualities = previousVacancy.PersonalQualities;
            PersonalQualitiesComment = previousVacancy.PersonalQualitiesComment;
            ThingsToConsider = previousVacancy.ThingsToConsider;
            ThingsToConsiderComment = previousVacancy.ThingsToConsiderComment;
            DesiredQualifications = previousVacancy.DesiredQualifications;
            DesiredQualificationsComment = previousVacancy.DesiredQualificationsComment;
            FirstQuestion = previousVacancy.FirstQuestion;
            SecondQuestion = previousVacancy.SecondQuestion;
            ProviderSiteEmployerLink = previousVacancy.ProviderSiteEmployerLink;
            OfflineVacancy = previousVacancy.OfflineVacancy;
            OfflineApplicationUrl = previousVacancy.OfflineApplicationUrl;
            OfflineApplicationUrlComment = previousVacancy.OfflineApplicationUrlComment;
            OfflineApplicationInstructions = previousVacancy.OfflineApplicationInstructions;
            OfflineApplicationInstructionsComment = previousVacancy.OfflineApplicationInstructionsComment;
            OfflineApplicationClickThroughCount = previousVacancy.OfflineApplicationClickThroughCount;
            DateSubmitted = previousVacancy.DateSubmitted;
            DateFirstSubmitted = previousVacancy.DateFirstSubmitted;
            DateStartedToQA = previousVacancy.DateStartedToQA;
            QAUserName = previousVacancy.QAUserName;
            DateQAApproved = previousVacancy.DateQAApproved;
            SubmissionCount = previousVacancy.SubmissionCount;
            VacancyManagerId = previousVacancy.VacancyManagerId;
            LastEditedById = previousVacancy.LastEditedById;
            ParentVacancyReferenceNumber = previousVacancy.ParentVacancyReferenceNumber;
            TrainingType = previousVacancy.TrainingType;
            ApprenticeshipLevel = previousVacancy.ApprenticeshipLevel;
            ApprenticeshipLevelComment = previousVacancy.ApprenticeshipLevelComment;
            FrameworkCodeName = previousVacancy.FrameworkCodeName;
            FrameworkCodeNameComment = previousVacancy.FrameworkCodeNameComment;
            StandardId = previousVacancy.StandardId;
            StandardIdComment = previousVacancy.StandardIdComment;
            Status = previousVacancy.Status;
            WageComment = previousVacancy.WageComment;
            ClosingDateComment = previousVacancy.ClosingDateComment;
            DurationComment = previousVacancy.DurationComment;
            LongDescriptionComment = previousVacancy.LongDescriptionComment;
            PossibleStartDateComment = previousVacancy.PossibleStartDateComment;
            WorkingWeekComment = previousVacancy.WorkingWeekComment;
            FirstQuestionComment = previousVacancy.FirstQuestionComment;
            SecondQuestionComment = previousVacancy.SecondQuestionComment;
            AdditionalLocationInformation = previousVacancy.AdditionalLocationInformation;
            LocationAddresses = previousVacancy.LocationAddresses;
            IsEmployerLocationMainApprenticeshipLocation = previousVacancy.IsEmployerLocationMainApprenticeshipLocation;
            NumberOfPositions = previousVacancy.NumberOfPositions;
            EmployerDescriptionComment = previousVacancy.EmployerDescriptionComment;
            EmployerWebsiteUrlComment = previousVacancy.EmployerWebsiteUrlComment;
            LocationAddressesComment = previousVacancy.LocationAddressesComment;
            NumberOfPositionsComment = previousVacancy.NumberOfPositionsComment;
            AdditionalLocationInformationComment = previousVacancy.AdditionalLocationInformationComment;
        }

        public TrainingType TrainingType { get; set; }
        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        public string ApprenticeshipLevelComment { get; set; }
        public string FrameworkCodeName { get; set; }
        public string FrameworkCodeNameComment { get; set; }
        public int? StandardId { get; set; }
        public string StandardIdComment { get; set; }
        public ProviderVacancyStatuses Status { get; set; }
        public string WageComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string DurationComment { get; set; }
        public string LongDescriptionComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string FirstQuestionComment { get; set; }
        public string SecondQuestionComment { get; set; }
        public string AdditionalLocationInformation { get; set; }
        public List<VacancyLocationAddress> LocationAddresses { get; set; }
        public bool? IsEmployerLocationMainApprenticeshipLocation { get; set; }
        public int? NumberOfPositions { get; set; }
        public string EmployerDescriptionComment { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }
        public string LocationAddressesComment { get; set; }
        public string NumberOfPositionsComment { get; set; }
        public string AdditionalLocationInformationComment { get; set; }
    }
}
