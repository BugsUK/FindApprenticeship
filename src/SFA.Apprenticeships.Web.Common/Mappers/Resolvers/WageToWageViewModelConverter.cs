namespace SFA.Apprenticeships.Web.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using ViewModels;

    public sealed class WageToWageViewModelConverter : ITypeConverter<Wage, WageViewModel>
    {
        public WageViewModel Convert(ResolutionContext context)
        {
            return new WageViewModel((Wage)context.SourceValue);
        }
    }
}
