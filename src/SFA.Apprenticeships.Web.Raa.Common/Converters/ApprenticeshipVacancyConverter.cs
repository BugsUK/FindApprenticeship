﻿using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;

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
                Status = apprenticeshipVacancy.Status,
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
                DesiredQualificationsComment = apprenticeshipVacancy.DesiredQualificationsComment,
                Status = apprenticeshipVacancy.Status
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
    }
}