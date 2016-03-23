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
                    return LegacyWageType.LegacyWeekly;

                case "Text":
                    return LegacyWageType.LegacyText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(source),
                        $"Unknown Wage Type received from NAS Gateway Service: \"{source}\"");
            }
        }
    }
}
