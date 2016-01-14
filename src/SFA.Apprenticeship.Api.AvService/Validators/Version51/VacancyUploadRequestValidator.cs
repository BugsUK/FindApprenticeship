namespace SFA.Apprenticeship.Api.AvService.Validators.Version51
{
    using Common;
    using DataContracts.Version51;
    using FluentValidation;
    using MessageContracts.Version51;

    // TODO: US872: AG: REFACTOR: move classes individual files.

    public class VacancyUploadRequestValidator : AbstractValidator<VacancyUploadRequest>
    {
        public VacancyUploadRequestValidator()
        {
            RuleFor(request => request.Vacancies)
                .SetCollectionValidator(new VacancyUploadDataValidator())
                .When(request => request.Vacancies != null && request.Vacancies.Count > 0);
        }
    }

    public class VacancyUploadDataValidator : AbstractValidator<VacancyUploadData>
    {
        public const int MaxVacancyTitleLength = 200;
        public const int MaxVacancyShortDescriptionLength = 512;

        public VacancyUploadDataValidator()
        {
            RuleFor(vacancy => vacancy.Title)
                .NotEmpty()
                .WithError(ApiErrors.VacancyTitleIsMandatory)
                .Length(1, MaxVacancyTitleLength)
                .WithError(ApiErrors.VacancyTitleIsTooLong);

            RuleFor(vacancy => vacancy.ShortDescription)
                .NotEmpty()
                .WithError(ApiErrors.VacancyShortDescriptionIsMandatory)
                .Length(1, MaxVacancyShortDescriptionLength)
                .WithError(ApiErrors.VacancyShortDescriptionIsTooLong);

            RuleFor(vacancy => vacancy.LongDescription)
                .NotEmpty()
                .WithError(ApiErrors.VacancyLongDescriptionIsMandatory);

            RuleFor(vacancy => vacancy.DeliveryProviderEdsUrn)
                .InclusiveBetween(1, int.MaxValue)
                .WithError(ApiErrors.DeliveryProviderEdsUrnIsMandatory);

            RuleFor(vacancy => vacancy.VacancyManagerEdsUrn)
                .InclusiveBetween(1, int.MaxValue)
                .WithError(ApiErrors.VacancyManagerEdsUrnIsMandatory);

            RuleFor(vacancy => vacancy.VacancyOwnerEdsUrn)
                .InclusiveBetween(1, int.MaxValue)
                .WithError(ApiErrors.VacancyOwnerEdsUrnIsMandatory);

            /*
            RuleFor(vacancy => vacancy.Employer)
                .SetValidator(new EmployerDataValidator());
            */

            /*
            RuleFor(vacancy => vacancy.Application)
                .SetValidator(new ApplicationDataValidator());

            RuleFor(vacancy => vacancy.Apprenticeship)
                .SetValidator(new ApprenticeshipDataValidator());

            RuleFor(vacancy => vacancy.Vacancy)
                .SetValidator(new VacancyDataValidator());
            */
        }
    }

    public class ApplicationDataValidator : AbstractValidator<ApplicationData>
    {
    }

    public class ApprenticeshipDataValidator : AbstractValidator<ApprenticeshipData>
    {
    }
    public class EmployerDataValidator : AbstractValidator<EmployerData>
    {
        // public const int MaxEmployerContactName = 200;

        public EmployerDataValidator()
        {
            /*
            RuleFor(employer => employer.ContactName)
                .Length(0, MaxEmployerContactName)
                .When(employer => !string.IsNullOrWhiteSpace(employer.ContactName))
                .WithError(ApiErrors.Error10022);
            */
        }
    }

    public class VacancyDataValidator : AbstractValidator<VacancyData>
    {
    }
}
