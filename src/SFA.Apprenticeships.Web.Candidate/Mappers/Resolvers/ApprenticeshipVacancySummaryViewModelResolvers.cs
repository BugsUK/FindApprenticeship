namespace SFA.Apprenticeships.Web.Candidate.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Presentation;

    public class ApprenticeshipVacancySummaryViewModelResolvers
    {
        public class WageResolver : ValueResolver<ApprenticeshipSearchResponse, string>
        {
            protected override string ResolveCore(ApprenticeshipSearchResponse source)
            {
                return WagePresenter.GetDisplayAmountWithFrequencyPostfix(source.Wage.Type, source.Wage.Amount, source.Wage.Text, source.Wage.Unit, source.Wage.HoursPerWeek);
            }
        }
    }
}