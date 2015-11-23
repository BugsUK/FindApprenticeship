namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Web.Common.ViewModels;

    public class DateTimeToDateViewModelConverter : 
        ITypeConverter<DateTime?, DateViewModel>,
        ITypeConverter<DateTime, DateViewModel>
    {
        public DateViewModel Convert(ResolutionContext context)
        {
            var source = (DateTime?)context.SourceValue;
            var destination = new DateViewModel
            {
                Day = source?.Day,
                Month = source?.Month,
                Year = source?.Year
            };

            return destination;
        }
    }
}