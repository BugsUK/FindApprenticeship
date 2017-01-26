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
        private readonly EmployerProviderSiteLinkRequestValidator _employerProviderSiteLinkValidator = new EmployerProviderSiteLinkRequestValidator();

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

        public EmployerProviderSiteLink LinkEmployer(EmployerProviderSiteLinkRequest employerProviderSiteLinkRequest, int edsUrn, string ukprn)
        {
            //Doing validation here rather than on object as the object requires populating before validation
            employerProviderSiteLinkRequest.EmployerEdsUrn = edsUrn;

            var validationResult = _employerProviderSiteLinkValidator.Validate(employerProviderSiteLinkRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Employer employer = null;
            try
            {
                employer = _getByEdsUrnStrategy.Get(employerProviderSiteLinkRequest.EmployerEdsUrn.ToString());
                if (employer == null)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerNotFoundFormat, employerProviderSiteLinkRequest.EmployerEdsUrn)));
                }
            }
            catch (CustomException ex)
            {
                _logService.Warn($"Error when linking to employer with EDSURN {employerProviderSiteLinkRequest.EmployerEdsUrn}", ex);

                if (ex.Code == Apprenticeships.Application.Employer.ErrorCodes.InvalidAddress)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerAddressNotValid, employerProviderSiteLinkRequest.EmployerEdsUrn)));
                }
                else if (ex.Code == Apprenticeships.Infrastructure.EmployerDataService.ErrorCodes.GetByReferenceNumberFailed)
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerGetByReferenceNumberFailed, employerProviderSiteLinkRequest.EmployerEdsUrn)));
                }
                else
                {
                    validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerUnknownError, employerProviderSiteLinkRequest.EmployerEdsUrn)));
                }
            }
            catch (Exception ex)
            {
                _logService.Warn($"Error when linking to employer with EDSURN {employerProviderSiteLinkRequest.EmployerEdsUrn}", ex);
                validationResult.Errors.Add(new ValidationFailure("EmployerEdsUrn", string.Format(EmployerProviderSiteLinkMessages.EmployerUnknownError, employerProviderSiteLinkRequest.EmployerEdsUrn)));
            }

            ProviderSite providerSite = null;
            if(employerProviderSiteLinkRequest.ProviderSiteEdsUrn.HasValue)
            {
                providerSite = _providerSiteReadRepository.GetByEdsUrn(employerProviderSiteLinkRequest.ProviderSiteEdsUrn.ToString());
                if (providerSite == null)
                {
                    validationResult.Errors.Add(new ValidationFailure("ProviderSiteEdsUrn", string.Format(EmployerProviderSiteLinkMessages.ProviderSiteNotFoundEdsUrnFormat, employerProviderSiteLinkRequest.ProviderSiteEdsUrn)));
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

            var vacancyOwnerRelationship = _getVacancyOwnerRelationshipStrategy.GetVacancyOwnerRelationship(providerSite.ProviderSiteId, employer.EmployerId, false);
            vacancyOwnerRelationship.EmployerDescription = employerProviderSiteLinkRequest.EmployerDescription;
            vacancyOwnerRelationship.EmployerWebsiteUrl = employerProviderSiteLinkRequest.EmployerWebsiteUrl;
            vacancyOwnerRelationship.StatusType = VacancyOwnerRelationshipStatusTypes.Active;

            vacancyOwnerRelationship = _vacancyOwnerRelationshipWriteRepository.Save(vacancyOwnerRelationship);

            var employerProviderSiteLink = new EmployerProviderSiteLink
            {
                EmployerProviderSiteLinkId = vacancyOwnerRelationship.VacancyOwnerRelationshipId,
                ProviderSiteId = providerSite.ProviderSiteId,
                ProviderSiteEdsUrn = Convert.ToInt32(providerSite.EdsUrn),
                EmployerId = employer.EmployerId,
                EmployerEdsUrn = Convert.ToInt32(employer.EdsUrn),
                EmployerDescription = vacancyOwnerRelationship.EmployerDescription,
                EmployerWebsiteUrl = vacancyOwnerRelationship.EmployerWebsiteUrl,
            };

            return employerProviderSiteLink;
        }
    }
}