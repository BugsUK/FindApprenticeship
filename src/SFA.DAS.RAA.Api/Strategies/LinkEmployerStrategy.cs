namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using System.Linq;
    using System.Security;
    using Apprenticeships.Application.Employer.Strategies;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Provider.Strategies;
    using Apprenticeships.Domain.Entities.Exceptions;
    using Apprenticeships.Domain.Entities.Raa.Parties;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Constants;
    using FluentValidation;
    using FluentValidation.Results;
    using Models;
    using Validators;

    public class LinkEmployerStrategy : ILinkEmployerStrategy
    {
        private readonly EmployerProviderSiteLinkValidator _employerProviderSiteLinkValidator = new EmployerProviderSiteLinkValidator();

        private readonly ILogService _logService;

        private readonly IGetByEdsUrnStrategy _getByEdsUrnStrategy;
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IGetOwnedProviderSitesStrategy _getOwnedProviderSitesStrategy;
        private readonly IGetVacancyOwnerRelationshipStrategy _getVacancyOwnerRelationshipStrategy;
        private readonly IVacancyOwnerRelationshipWriteRepository _vacancyOwnerRelationshipWriteRepository;

        public LinkEmployerStrategy(IGetByEdsUrnStrategy getByEdsUrnStrategy, IProviderReadRepository providerReadRepository, IProviderSiteReadRepository providerSiteReadRepository, IGetOwnedProviderSitesStrategy getOwnedProviderSitesStrategy, IGetVacancyOwnerRelationshipStrategy getVacancyOwnerRelationshipStrategy, IVacancyOwnerRelationshipWriteRepository vacancyOwnerRelationshipWriteRepository, ILogService logService)
        {
            _getByEdsUrnStrategy = getByEdsUrnStrategy;
            _providerReadRepository = providerReadRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _getOwnedProviderSitesStrategy = getOwnedProviderSitesStrategy;
            _getVacancyOwnerRelationshipStrategy = getVacancyOwnerRelationshipStrategy;
            _vacancyOwnerRelationshipWriteRepository = vacancyOwnerRelationshipWriteRepository;
            _logService = logService;
        }

        public EmployerProviderSiteLink LinkEmployer(EmployerProviderSiteLink employerProviderSiteLink, int edsUrn, string ukprn)
        {
            //Doing validation here rather than on object as the object requires populating before validation
            employerProviderSiteLink.EmployerEdsUrn = employerProviderSiteLink.EmployerEdsUrn ?? edsUrn;

            var validationResult = _employerProviderSiteLinkValidator.Validate(employerProviderSiteLink);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Employer employer = null;
            try
            {
                employer = _getByEdsUrnStrategy.Get(employerProviderSiteLink.EmployerEdsUrn.ToString());
                if (employer == null)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerNotFoundFormat, employerProviderSiteLink.EmployerEdsUrn)));
                }
                //TODO: Validate geocoding for employer. Use Google as a fallback
            }
            catch (CustomException ex)
            {
                _logService.Warn($"Error when linking to employer with EDSURN {employerProviderSiteLink.EmployerEdsUrn}", ex);

                if (ex.Code == Apprenticeships.Application.Employer.ErrorCodes.InvalidAddress)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerAddressNotValid, employerProviderSiteLink.EmployerEdsUrn)));
                }
                else if (ex.Code == Apprenticeships.Infrastructure.EmployerDataService.ErrorCodes.GetByReferenceNumberFailed)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerGetByReferenceNumberFailed, employerProviderSiteLink.EmployerEdsUrn)));
                }
                else
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerUnknownError, employerProviderSiteLink.EmployerEdsUrn)));
                }
            }
            catch (Exception ex)
            {
                _logService.Warn($"Error when linking to employer with EDSURN {employerProviderSiteLink.EmployerEdsUrn}", ex);
                validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerUnknownError, employerProviderSiteLink.EmployerEdsUrn)));
            }

            ProviderSite providerSite = null;
            if (employerProviderSiteLink.ProviderSiteId.HasValue)
            {
                providerSite = _providerSiteReadRepository.GetById(employerProviderSiteLink.ProviderSiteId.Value);
                if (employer == null)
                {
                    validationResult.Errors.Add(new ValidationFailure("ProviderSiteId", string.Format(EmployerProviderSiteLinkMessages.ProviderSiteNotFoundIdFormat, employerProviderSiteLink.ProviderSiteId)));
                }
            }
            else if(employerProviderSiteLink.ProviderSiteEdsUrn.HasValue)
            {
                providerSite = _providerSiteReadRepository.GetByEdsUrn(employerProviderSiteLink.ProviderSiteEdsUrn.ToString());
                if (employer == null)
                {
                    validationResult.Errors.Add(new ValidationFailure("ProviderSiteEdsUrn", string.Format(EmployerProviderSiteLinkMessages.ProviderSiteNotFoundEdsUrnFormat, employerProviderSiteLink.ProviderSiteEdsUrn)));
                }
            }
            if (employer == null || providerSite == null)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var provider = _providerReadRepository.GetByUkprn(ukprn, false);
            if (provider == null)
            {
                throw new SecurityException(EmployerProviderSiteLinkMessages.UnauthorizedProviderSiteAccess);
            }
            var ownedProviderSites = _getOwnedProviderSitesStrategy.GetOwnedProviderSites(provider.ProviderId);
            if (ownedProviderSites.All(ps => ps.ProviderSiteId != providerSite.ProviderSiteId))
            {
                throw new SecurityException(EmployerProviderSiteLinkMessages.UnauthorizedProviderSiteAccess);
            }

            //Yuk. Taken from existing RAA code. Should be dealt with by returning the status of the VOR and checking that
            if (_getVacancyOwnerRelationshipStrategy.IsADeletedVacancyOwnerRelationship(providerSite.ProviderSiteId, employer.EmployerId))
            {
                _getVacancyOwnerRelationshipStrategy.ResurrectVacancyOwnerRelationship(providerSite.ProviderSiteId, employer.EmployerId);
            }

            var vacancyOwnerRelationship = _getVacancyOwnerRelationshipStrategy.GetVacancyOwnerRelationship(providerSite.ProviderSiteId, employer.EmployerId, false);
            vacancyOwnerRelationship.EmployerDescription = employerProviderSiteLink.EmployerDescription;
            vacancyOwnerRelationship.EmployerWebsiteUrl = new UriBuilder(employerProviderSiteLink.EmployerWebsiteUrl).ToString();

            vacancyOwnerRelationship = _vacancyOwnerRelationshipWriteRepository.Save(vacancyOwnerRelationship);

            employerProviderSiteLink.ProviderSiteId = providerSite.ProviderSiteId;
            employerProviderSiteLink.ProviderSiteEdsUrn = Convert.ToInt32(providerSite.EdsUrn);
            employerProviderSiteLink.EmployerId = employer.EmployerId;
            employerProviderSiteLink.EmployerEdsUrn = Convert.ToInt32(employer.EdsUrn);
            employerProviderSiteLink.EmployerDescription = vacancyOwnerRelationship.EmployerDescription;
            employerProviderSiteLink.EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsiteUrl;

            return employerProviderSiteLink;
        }
    }
}