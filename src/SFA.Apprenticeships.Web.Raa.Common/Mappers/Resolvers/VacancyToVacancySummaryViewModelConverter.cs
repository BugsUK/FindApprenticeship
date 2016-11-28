namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using System;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

    /// <summary>
    /// This is meant to replace
    /// SFA.Apprenticeships.Web.Raa.Common.Converters.ApprenticeshipVacancyConverter.ConvertToVacancyViewModel
    /// 
    /// TODO: DI for automapper within Raa websites (manage and recruit), then replace usage of converter (above) with this mapping class
    /// </summary>
    internal sealed class VacancyToVacancySummaryViewModelConverter : ITypeConverter<VacancySummary, VacancySummaryViewModel>
    {
        public VacancySummaryViewModel Convert(ResolutionContext context)
        {
            var source = (VacancySummary)context.SourceValue;

            var destination = new VacancySummaryViewModel
            {
                VacancyId = source.VacancyId,
                VacancyReferenceNumber = source.VacancyReferenceNumber,
                VacancyType = source.VacancyType,
                Status = source.Status,
                Title = source.Title,
                VacancyOwnerRelationshipId = source.VacancyOwnerRelationshipId,
                EmployerName = source.EmployerName,
                Location = context.Engine.Map<PostalAddress, AddressViewModel>(source.Address),
                OfflineVacancy = source.OfflineVacancy,
                OfflineApplicationClickThroughCount = source.NoOfOfflineApplicants,
                ClosingDate = context.Engine.Map<DateTime?, DateViewModel>(source.ClosingDate),
                DateSubmitted = source.DateSubmitted,
                SubmissionCount = source.SubmissionCount,
                EmployerApprenticeshipLocation = source.EmployerApprenticeshipLocation,
                ParentVacancyId = source.ParentVacancyId,
                NewApplicationCount = source.NewApplicationCount,
                ApplicationCount = source.ApplicantCount,
                IsEmployerAnonymous = string.IsNullOrWhiteSpace(source.EmployerAnonymousName) && string.IsNullOrWhiteSpace(source.EmployerAnonymousReason)
            };

            return destination;
        }
    }
}
