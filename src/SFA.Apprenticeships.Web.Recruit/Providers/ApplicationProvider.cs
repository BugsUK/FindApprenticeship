namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using System;
    using System.Linq;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Logging;
    using Application.Interfaces.VacancyPosting;
    using Common.ViewModels;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using ViewModels.Application;

    public class ApplicationProvider : IApplicationProvider
    {
        private readonly IConfigurationService _configurationService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public ApplicationProvider(IConfigurationService configurationService, IVacancyPostingService vacancyPostingService, IApplicationService applicationService, IMapper mapper, ILogService logService)
        {
            _configurationService = configurationService;
            _vacancyPostingService = vacancyPostingService;
            _applicationService = applicationService;
            _mapper = mapper;
            _logService = logService;
        }

        public VacancyApplicationsViewModel GetVacancyApplicationsViewModel(long vacancyReferenceNumber)
        {
            var vacancy = _vacancyPostingService.GetVacancy(vacancyReferenceNumber);
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyApplicationsViewModel>(vacancy);

            //TODO: Store geopoints for employers
            if (viewModel.EmployerGeoPoint.Latitude == 0 || viewModel.EmployerGeoPoint.Longitude == 0)
            {
                //Coventry
                viewModel.EmployerGeoPoint.Latitude = 52.4009991288043;
                viewModel.EmployerGeoPoint.Longitude = -1.50812239495425;
            }

            var applications = _applicationService.GetSubmittedApplicationSummaries((int) vacancyReferenceNumber);
            var applicationSummaryViewModels = applications.Select(a => _mapper.Map<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>(a)).ToList();

            //TODO: This distance calculation should be done in a new elastic index of applications which will also form the basis of a filter and sorting mechanism
            foreach (var applicationSummaryViewModel in applicationSummaryViewModels)
            {
                applicationSummaryViewModel.Distance = Distance(applicationSummaryViewModel.ApplicantGeoPoint.Latitude,
                    applicationSummaryViewModel.ApplicantGeoPoint.Longitude, viewModel.EmployerGeoPoint.Latitude,
                    viewModel.EmployerGeoPoint.Longitude);
            }

            viewModel.ApplicationSummaries = new PageableViewModel<ApplicationSummaryViewModel>
            {
                Page = applicationSummaryViewModels
            };

            return viewModel;
        }

        private static double Distance(double lat1, double lon1, double lat2, double lon2, char unit = 'M')
        {
            var theta = lon1 - lon2;
            var dist = Math.Sin(Deg2Rad(lat1)) * Math.Sin(Deg2Rad(lat2)) + Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) * Math.Cos(Deg2Rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2Deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        private static double Deg2Rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double Rad2Deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}