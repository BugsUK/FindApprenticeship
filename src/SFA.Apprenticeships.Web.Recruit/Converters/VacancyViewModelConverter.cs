namespace SFA.Apprenticeships.Web.Recruit.Converters
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;

    public static class VacancyViewModelConverter
    {
        public static VacancyViewModel Convert(this ApprenticeshipVacancy apprenticeshipVacancy)
        {
            var vacancyViewModel = new VacancyViewModel
            {
                VacancyReferenceNumber = apprenticeshipVacancy.VacancyReferenceNumber,
                Ukprn = apprenticeshipVacancy.Ukprn,
                Title = apprenticeshipVacancy.Title,
                ShortDescription = apprenticeshipVacancy.ShortDescription,
                WorkingWeek = apprenticeshipVacancy.WorkingWeek,
                WeeklyWage = apprenticeshipVacancy.WeeklyWage,
                Duration = apprenticeshipVacancy.Duration,
                ClosingDate = apprenticeshipVacancy.ClosingDate,
                PossibleStartDate = apprenticeshipVacancy.PossibleStartDate,
                LongDescription = apprenticeshipVacancy.LongDescription,
                DesiredSkills = apprenticeshipVacancy.DesiredSkills,
                FutureProspects = apprenticeshipVacancy.FutureProspects,
                PersonalQualities = apprenticeshipVacancy.PersonalQualities,
                ThingsToConsider = apprenticeshipVacancy.ThingsToConsider,
                DesiredQualifications = apprenticeshipVacancy.DesiredQualifications,
                FirstQuestion = apprenticeshipVacancy.FirstQuestion,
                SecondQuestion = apprenticeshipVacancy.SecondQuestion,
                ApprenticeshipLevel = apprenticeshipVacancy.ApprenticeshipLevel,
                FrameworkCodeName = apprenticeshipVacancy.FrameworkCodeName,
                ProviderSiteEmployerLink = apprenticeshipVacancy.ProviderSiteEmployerLink.Convert(),
            };

            return vacancyViewModel;
        }
    }
}