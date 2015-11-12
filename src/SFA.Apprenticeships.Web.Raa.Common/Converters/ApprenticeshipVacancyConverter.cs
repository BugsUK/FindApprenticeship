using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    public static class ApprenticeshipVacancyConverter
    {
        public static VacancySummaryViewModel ConvertToVacancySummaryViewModel(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var vacancyViewModel = new VacancySummaryViewModel
            {
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                HoursPerWeek = apprenticeshipVacancy.HoursPerWeek,
                WageType = apprenticeshipVacancy.WageType,
                Wage = apprenticeshipVacancy.Wage,
                WageUnit = apprenticeshipVacancy.WageUnit,
                WageUnits = GetWageUnits(),
                DurationType = apprenticeshipVacancy.DurationType,
                DurationTypes = GetDurationTypes(),
                Duration = apprenticeshipVacancy.Duration,
                ClosingDate = new DateViewModel(apprenticeshipVacancy.ClosingDate),
                PossibleStartDate = new DateViewModel(apprenticeshipVacancy.PossibleStartDate),
                LongDescription = apprenticeshipVacancy.LongDescription,
                WageComment = apprenticeshipVacancy.WageComment,
                ClosingDateComment = apprenticeshipVacancy.ClosingDateComment,
                DurationComment = apprenticeshipVacancy.DurationComment,
                LongDescriptionComment = apprenticeshipVacancy.LongDescriptionComment,
                PossibleStartDateComment = apprenticeshipVacancy.PossibleStartDateComment,
                WorkingWeekComment = apprenticeshipVacancy.WorkingWeekComment
            };

            return vacancyViewModel;
        }

        public static List<SelectListItem> GetWageUnits()
        {
            var wageUnits =
                Enum.GetValues(typeof(WageUnit))
                    .Cast<WageUnit>()
                    .Where(al => al != WageUnit.NotApplicable)
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString() })
                    .ToList();

            return wageUnits;
        }

        public static List<SelectListItem> GetDurationTypes()
        {
            var durationTypes =
                Enum.GetValues(typeof(DurationType))
                    .Cast<DurationType>()
                    .Where(al => al != DurationType.Unknown)
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString() })
                    .ToList();

            return durationTypes;
        }

        public static VacancyRequirementsProspectsViewModel ConvertToVacancyRequirementsProspectsViewModel(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var vacancyViewModel = new VacancyRequirementsProspectsViewModel
            {
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                DesiredSkills = apprenticeshipVacancy.DesiredSkills,
                DesiredSkillsComment = apprenticeshipVacancy.DesiredSkillsComment,
                FutureProspects = apprenticeshipVacancy.FutureProspects,
                FutureProspectsComment = apprenticeshipVacancy.FutureProspectsComment,
                PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                PersonalQualitiesComment = apprenticeshipVacancy.PersonalQualitiesComment,
                ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                ThingsToConsiderComment = apprenticeshipVacancy.ThingsToConsiderComment,
                DesiredQualifications = apprenticeshipVacancy.DesiredQualifications,
                DesiredQualificationsComment = apprenticeshipVacancy.DesiredQualificationsComment
            };

            return vacancyViewModel;
        }

        public static VacancyQuestionsViewModel ConvertToVacancyQuestionsViewModel(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var vacancyViewModel = new VacancyQuestionsViewModel
            {
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                FirstQuestion = apprenticeshipVacancy.FirstQuestion,
                SecondQuestion = apprenticeshipVacancy.SecondQuestion,
                FirstQuestionComment = apprenticeshipVacancy.FirstQuestionComment,
                SecondQuestionComment = apprenticeshipVacancy.SecondQuestionComment
            };

            return vacancyViewModel;
        }

        public static VacancyViewModel ConvertToVacancyViewModel(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            //TODO: Automap
            var vacancyViewModel = new VacancyViewModel
            {
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                NewVacancyViewModel = new NewVacancyViewModel
                {
                    VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                    Ukprn = apprenticeshipVacancy.Ukprn,
                    Title = apprenticeshipVacancy.Title,
                    ShortDescription = apprenticeshipVacancy.ShortDescription,
                    TrainingType = apprenticeshipVacancy.TrainingType,
                    ApprenticeshipLevel = apprenticeshipVacancy.ApprenticeshipLevel,
                    FrameworkCodeName = apprenticeshipVacancy.FrameworkCodeName,
                    StandardId = apprenticeshipVacancy.StandardId,
                    ProviderSiteEmployerLink = apprenticeshipVacancy.ProviderSiteEmployerLink.Convert(),
                    OfflineVacancy = apprenticeshipVacancy.OfflineVacancy,
                    OfflineApplicationUrl = apprenticeshipVacancy.OfflineApplicationUrl,
                    OfflineApplicationInstructions = apprenticeshipVacancy.OfflineApplicationInstructions
                },
                VacancySummaryViewModel = new VacancySummaryViewModel
                {
                    WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                    HoursPerWeek = apprenticeshipVacancy.HoursPerWeek,
                    WageType = apprenticeshipVacancy.WageType,
                    Wage = apprenticeshipVacancy.Wage,
                    WageUnit = apprenticeshipVacancy.WageUnit,
                    DurationType = apprenticeshipVacancy.DurationType,
                    Duration = apprenticeshipVacancy.Duration,
                    ClosingDate = new DateViewModel
                    {
                        Day = apprenticeshipVacancy.ClosingDate?.Day,
                        Month = apprenticeshipVacancy.ClosingDate?.Month,
                        Year = apprenticeshipVacancy.ClosingDate?.Year
                    },
                    PossibleStartDate = new DateViewModel
                    {
                        Day = apprenticeshipVacancy.PossibleStartDate?.Day,
                        Month = apprenticeshipVacancy.PossibleStartDate?.Month,
                        Year = apprenticeshipVacancy.PossibleStartDate?.Year
                    },
                    LongDescription = apprenticeshipVacancy.LongDescription
                },
                VacancyRequirementsProspectsViewModel = new VacancyRequirementsProspectsViewModel
                {
                    DesiredSkills = apprenticeshipVacancy.DesiredSkills,
                    FutureProspects = apprenticeshipVacancy.FutureProspects,
                    PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                    ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                    DesiredQualifications = apprenticeshipVacancy.DesiredQualifications
                },
                VacancyQuestionsViewModel = new VacancyQuestionsViewModel
                {
                    FirstQuestion = apprenticeshipVacancy.FirstQuestion,
                    SecondQuestion = apprenticeshipVacancy.SecondQuestion
                },
                // Ukprn = apprenticeshipVacancy.Ukprn,
                // Title = apprenticeshipVacancy.Title,
                //ShortDescription = apprenticeshipVacancy.ShortDescription,
                // WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                // HoursPerWeek = apprenticeshipVacancy.HoursPerWeek,
                // WageType = apprenticeshipVacancy.WageType,
                // Wage = apprenticeshipVacancy.Wage,
                // WageUnit = apprenticeshipVacancy.WageUnit,
                // DurationType = apprenticeshipVacancy.DurationType,
                // Duration = apprenticeshipVacancy.Duration,
                // ClosingDate = apprenticeshipVacancy.ClosingDate,
                // PossibleStartDate = apprenticeshipVacancy.PossibleStartDate,
                // LongDescription = apprenticeshipVacancy.LongDescription,
                // DesiredSkills = apprenticeshipVacancy.DesiredSkills,
                // FutureProspects = apprenticeshipVacancy.FutureProspects,
                // PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                // ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                // DesiredQualifications = apprenticeshipVacancy.DesiredQualifications,
                // FirstQuestion = apprenticeshipVacancy.FirstQuestion,
                // SecondQuestion = apprenticeshipVacancy.SecondQuestion,
                // ApprenticeshipLevel = apprenticeshipVacancy.ApprenticeshipLevel,
                Status = apprenticeshipVacancy.Status,
                // FrameworkCodeName = apprenticeshipVacancy.FrameworkCodeName,
                // ProviderSiteEmployerLink = apprenticeshipVacancy.ProviderSiteEmployerLink.Convert(),
                // OfflineVacancy = apprenticeshipVacancy.OfflineVacancy,
                // OfflineApplicationUrl = apprenticeshipVacancy.OfflineApplicationUrl,
                // OfflineApplicationInstructions = apprenticeshipVacancy.OfflineApplicationInstructions
            };

            return vacancyViewModel;
        }

        public static NewVacancyViewModel ConvertToNewVacancyViewModel(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var vacancyViewModel = new NewVacancyViewModel
            {
                TrainingType = apprenticeshipVacancy.TrainingType,
                ApprenticeshipLevel = apprenticeshipVacancy.ApprenticeshipLevel,
                ApprenticeshipLevelComment = apprenticeshipVacancy.ApprenticeshipLevelComment,
                FrameworkCodeName = apprenticeshipVacancy.FrameworkCodeName,
                FrameworkCodeNameComment = apprenticeshipVacancy.FrameworkCodeNameComment,
                StandardId = apprenticeshipVacancy.StandardId,
                ShortDescription = apprenticeshipVacancy.ShortDescription,
                ShortDescriptionComment = apprenticeshipVacancy.ShortDescriptionComment,
                Title = apprenticeshipVacancy.Title,
                TitleComment = apprenticeshipVacancy.TitleComment,
                Ukprn = apprenticeshipVacancy.Ukprn,
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                ProviderSiteEmployerLink = apprenticeshipVacancy.ProviderSiteEmployerLink.Convert(),
                OfflineVacancy = apprenticeshipVacancy.OfflineVacancy,
                OfflineApplicationUrl = apprenticeshipVacancy.OfflineApplicationUrl,
                OfflineApplicationUrlComment = apprenticeshipVacancy.OfflineApplicationUrlComment,
                OfflineApplicationInstructions = apprenticeshipVacancy.OfflineApplicationInstructions,
                OfflineApplicationInstructionsComment = apprenticeshipVacancy.OfflineApplicationInstructionsComment
            };

            return vacancyViewModel;
        }
    }
}