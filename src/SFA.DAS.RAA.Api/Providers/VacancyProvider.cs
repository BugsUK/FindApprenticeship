namespace SFA.DAS.RAA.Api.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Exceptions;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Constants;
    using Models;

    public class VacancyProvider : IVacancyProvider
    {
        private readonly IGetVacancyStrategies _getVacancyStrategies;

        public VacancyProvider(IGetVacancyStrategies getVacancyStrategies)
        {
            _getVacancyStrategies = getVacancyStrategies;
        }

        public Vacancy Get(VacancyIdentifier vacancyIdentifier)
        {
            var vacancyId = vacancyIdentifier.Id;
            var vacancyReference = vacancyIdentifier.Reference;
            var vacancyGuid = vacancyIdentifier.Guid;

            if (!vacancyId.HasValue && !string.IsNullOrEmpty(vacancyReference) && !vacancyGuid.HasValue)
            {
                throw new ArgumentException(VacancyMessages.MissingVacancyIdentifier);
            }

            try
            {
                Vacancy vacancy;

                if (vacancyId.HasValue)
                {
                    vacancy = _getVacancyStrategies.GetVacancyById(vacancyId.Value);
                }
                else if (!string.IsNullOrEmpty(vacancyReference))
                {
                    int vacancyReferenceNumber;
                    if (VacancyHelper.TryGetVacancyReferenceNumber(vacancyReference, out vacancyReferenceNumber))
                    {
                        vacancy = _getVacancyStrategies.GetVacancyByReferenceNumber(vacancyReferenceNumber);
                    }
                    else
                    {
                        throw new ArgumentException(VacancyMessages.InvalidVacancyReference);
                    }
                }
                else
                {
                    vacancy = _getVacancyStrategies.GetVacancyByGuid(vacancyGuid.Value);
                }

                if (vacancy == null)
                {
                    throw new KeyNotFoundException(VacancyMessages.VacancyNotFound);
                }

                return vacancy;
            }
            catch (CustomException ex)
            {
                if (ex.Code == ErrorCodes.ProviderVacancyAuthorisation.Failed)
                {
                    throw new SecurityException(VacancyMessages.UnauthorizedVacancyAccess);
                }
                throw;
            }
        }
    }
}