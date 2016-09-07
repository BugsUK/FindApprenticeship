﻿namespace SFA.Apprenticeships.Domain.Entities.Vacancies
{
    using System.Text.RegularExpressions;

    public class VacancyHelper
    {
        private static readonly Regex VacancyReferenceNumberRegex = new Regex(@"^(VAC)?(\d*?\d{6})$", RegexOptions.IgnoreCase);

        public static bool IsVacancyReference(string value)
        {
            return !string.IsNullOrEmpty(value) && VacancyReferenceNumberRegex.IsMatch(value);
        }

        public static bool TryGetVacancyReference(string value, out string vacancyReference)
        {
            vacancyReference = null;

            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            var match = VacancyReferenceNumberRegex.Match(value);
            
            if (match.Success)
            {
                vacancyReference = match.Groups[2].Value;
                int vacancyReferenceNumber;
                if (!string.IsNullOrEmpty(vacancyReference) && int.TryParse(vacancyReference, out vacancyReferenceNumber))
                {
                    vacancyReference = vacancyReferenceNumber.ToString();
                }
            }

            return match.Success;
        }

        public static bool TryGetVacancyReferenceNumber(string value, out int vacancyReferenceNumber)
        {
            vacancyReferenceNumber = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;
            
            var match = VacancyReferenceNumberRegex.Match(value);
            
            if (match.Success)
            {
                var vacancyReference = match.Groups[2].Value;
                if (!string.IsNullOrEmpty(vacancyReference) && int.TryParse(vacancyReference, out vacancyReferenceNumber))
                {
                    return true;
                }
            }

            return false;
        }
    }
}