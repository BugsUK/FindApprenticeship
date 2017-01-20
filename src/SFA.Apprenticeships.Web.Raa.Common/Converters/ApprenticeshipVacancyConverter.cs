namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Infrastructure.Presentation;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    public static class ApprenticeshipVacancyConverter
    {
        public static FurtherVacancyDetailsViewModel ConvertToVacancySummaryViewModel(this Vacancy vacancy)
        {
            var vacancyViewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                WorkingWeek = vacancy.WorkingWeek,
                //this is set via automapper
                //Wage = new WageViewModel(vacancy.Wage),
                WageUnits = GetWageUnits(),
                WageTextPresets = GetWageTextPresets(),
                DurationType = vacancy.DurationType,
                DurationTypes = GetDurationTypes(vacancy.VacancyType),
                Duration = vacancy.Duration,
                ExpectedDuration = vacancy.ExpectedDuration,
                Status = vacancy.Status,
                VacancyDatesViewModel = new VacancyDatesViewModel { 
                    ClosingDate = new DateViewModel(vacancy.ClosingDate),
                    PossibleStartDate = new DateViewModel(vacancy.PossibleStartDate),
                    ClosingDateComment = vacancy.ClosingDateComment,
                    PossibleStartDateComment = vacancy.PossibleStartDateComment,
                    VacancyStatus = vacancy.Status
                },
                LongDescription = vacancy.LongDescription,
                WageComment = vacancy.WageComment,
                DurationComment = vacancy.DurationComment,
                LongDescriptionComment = vacancy.LongDescriptionComment,
                WorkingWeekComment = vacancy.WorkingWeekComment,
                VacancyType = vacancy.VacancyType,
                VacancySource = vacancy.VacancySource
            };

            return vacancyViewModel;
        }

        public static List<SelectListItem> GetWageUnits()
        {
            var wageUnits =
                Enum.GetValues(typeof(WageUnit))
                    .Cast<WageUnit>()
                    .Where(al => al != WageUnit.NotApplicable)
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString().ToLower() })
                    .ToList();

            return wageUnits;
        }

        public static List<SelectListItem> GetWageTextPresets()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() { Value = ((int)PresetText.CompetitiveSalary).ToString(), Text = WagePresenter.CompetitiveSalaryText},
                new SelectListItem() { Value = ((int)PresetText.ToBeAgreedUponAppointment).ToString(), Text = WagePresenter.ToBeAGreedUponAppointmentText},
                new SelectListItem() { Value = ((int)PresetText.Unwaged).ToString(), Text = WagePresenter.UnwagedText}
            };
        }

        public static List<SelectListItem> GetDurationTypes(VacancyType vacancyType)
        {
            var durationTypes =
                Enum.GetValues(typeof(DurationType))
                    .Cast<DurationType>()
                    .Where(al => al != DurationType.Unknown && (vacancyType != VacancyType.Traineeship || al != DurationType.Years))
                    .Select(al => new SelectListItem { Value = al.ToString(), Text = al.ToString().ToLower() })
                    .ToList();

            return durationTypes;
        }

        public static VacancyRequirementsProspectsViewModel ConvertToVacancyRequirementsProspectsViewModel(this Vacancy vacancy)
        {
            var vacancyViewModel = new VacancyRequirementsProspectsViewModel
            {
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                DesiredSkills = vacancy.DesiredSkills,
                DesiredSkillsComment = vacancy.DesiredSkillsComment,
                FutureProspects = vacancy.FutureProspects,
                FutureProspectsComment = vacancy.FutureProspectsComment,
                PersonalQualities = vacancy.PersonalQualities,
                PersonalQualitiesComment = vacancy.PersonalQualitiesComment,
                ThingsToConsider = vacancy.ThingsToConsider,
                ThingsToConsiderComment = vacancy.ThingsToConsiderComment,
                DesiredQualifications = vacancy.DesiredQualifications,
                DesiredQualificationsComment = vacancy.DesiredQualificationsComment,
                OtherInformation = vacancy.OtherInformation,
                OtherInformationComment = vacancy.OtherInformationComment,
                Status = vacancy.Status,
                VacancyType = vacancy.VacancyType,
                VacancySource = vacancy.VacancySource
            };

            return vacancyViewModel;
        }

        public static VacancyQuestionsViewModel ConvertToVacancyQuestionsViewModel(this Vacancy vacancy)
        {
            var vacancyViewModel = new VacancyQuestionsViewModel
            {
                VacancyReferenceNumber = vacancy.VacancyReferenceNumber,
                FirstQuestion = vacancy.FirstQuestion,
                SecondQuestion = vacancy.SecondQuestion,
                FirstQuestionComment = vacancy.FirstQuestionComment,
                SecondQuestionComment = vacancy.SecondQuestionComment,
                Status = vacancy.Status,
                VacancyType = vacancy.VacancyType
            };

            return vacancyViewModel;
        }
    }
}