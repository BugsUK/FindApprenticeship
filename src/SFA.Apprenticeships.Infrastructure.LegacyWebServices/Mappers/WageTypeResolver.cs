namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class WageTypeResolver : ValueResolver<string, LegacyWageType>
    {
        protected override LegacyWageType ResolveCore(string source)
        {
            switch (source)
            {
                case "Weekly":
                    return LegacyWageType.Weekly;

                case "Text":
                    return LegacyWageType.Text;

                default:
                    throw new ArgumentOutOfRangeException("source",
                        string.Format("Unknown Wage Type received from NAS Gateway Service: \"{0}\"", source));
            }
        }
    }
}