namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Mappers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;

    public class WageTypeResolver : ValueResolver<string, WageType>
    {
        protected override WageType ResolveCore(string source)
        {
            switch (source)
            {
                case "Weekly":
                    return WageType.LegacyWeekly;

                case "Text":
                    return WageType.LegacyText;

                default:
                    throw new ArgumentOutOfRangeException(nameof(source),
                        $"Unknown Wage Type received from NAS Gateway Service: \"{source}\"");
            }
        }
    }
}
