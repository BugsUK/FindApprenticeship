namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    public class VacancyToVacancyDatesViewModelConverter :
        ITypeConverter<Vacancy, VacancyDatesViewModel>
    {
        public VacancyDatesViewModel Convert(ResolutionContext context)
        {
            var source = (Vacancy)context.SourceValue;

            var destination = new VacancyDatesViewModel
            {
                VacancyStatus = source.Status,
                ClosingDate = context.Engine.Map<DateTime?, DateViewModel>(source.ClosingDate),
                PossibleStartDate = context.Engine.Map<DateTime?, DateViewModel>(source.PossibleStartDate),
                ClosingDateComment = source.ClosingDateComment,
                PossibleStartDateComment = source.PossibleStartDateComment
            };


            return destination;
        }
    }
}