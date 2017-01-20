namespace SFA.DAS.RAA.Api.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Exceptions;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Constants;

    public class VacancyProvider : IVacancyProvider
    {
        private readonly IGetVacancyStrategies _getVacancyStrategies;

        public VacancyProvider(IGetVacancyStrategies getVacancyStrategies)
        {
            _getVacancyStrategies = getVacancyStrategies;
        }

        public Vacancy Get(int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            if (!vacancyId.HasValue && !vacancyReferenceNumber.HasValue && !vacancyGuid.HasValue)
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
                else if (vacancyReferenceNumber.HasValue)
                {
                    vacancy = _getVacancyStrategies.GetVacancyByReferenceNumber(vacancyReferenceNumber.Value);
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