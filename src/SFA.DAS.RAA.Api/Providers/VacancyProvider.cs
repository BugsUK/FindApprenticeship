namespace SFA.DAS.RAA.Api.Providers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Exceptions;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;

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
                throw new ArgumentException("Please specify either a vacancyId, a vacancyReferenceNumber or a vacancyGuid.");
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
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "The requested vacancy has not been found" };
                    throw new HttpResponseException(message);
                }

                return vacancy;
            }
            catch (CustomException ex)
            {
                if (ex.Code == ErrorCodes.ProviderVacancyAuthorisation.Failed)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "You are not authorized to view or edit this vacancy" };
                    throw new HttpResponseException(message);
                }
                throw;
            }
        }
    }
}