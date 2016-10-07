namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System.Collections.Generic;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;

    public static class VacancyProviderTestHelper
    {
        public static VacancyProviderBuilder GetBasicVacancyProviderBuilder(string userName, params int[] vacancyReferenceNumbers)
        {
            var currentUserService = new Mock<ICurrentUserService>();
            currentUserService.Setup(cus => cus.CurrentUserName).Returns(userName);

            var postingService = new Mock<IVacancyPostingService>();

            postingService.Setup(ps => ps.GetVacancyLocations(It.IsAny<int>())).Returns(new List<VacancyLocation>());

            int total;

            postingService.Setup(ps => ps.GetWithStatus(It.IsAny<VacancySummaryByStatusQuery>(), out total))
                .Returns(new List<VacancySummary>());

            foreach (var vacancyReferenceNumber in vacancyReferenceNumbers)
            {
                var vacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber };
                postingService.Setup(ps => ps.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(vacancy);
                postingService.Setup(ps => ps.GetVacancyByReferenceNumber(vacancyReferenceNumber)).Returns(vacancy);
            }

            var vacancyLockingService = new Mock<IVacancyLockingService>();
            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()))
                .Returns(true);

            var providerService = new Mock<IProviderService>();
            providerService.Setup(ps => ps.GetVacancyParty(It.IsAny<int>(), true)).Returns(new VacancyParty());
            providerService.Setup(ps => ps.GetProviderSite(It.IsAny<int>()))
                .Returns(new ProviderSite { Address = new PostalAddress() });

            var employerService = new Mock<IEmployerService>();
            employerService.Setup(es => es.GetEmployer(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(new Employer { Address = new PostalAddress() });

            var mapper = new Mock<IMapper>();
            foreach (var vacancyReferenceNumber in vacancyReferenceNumbers)
            {
                mapper.Setup(m => m.Map<Vacancy, VacancyViewModel>(It.IsAny<Vacancy>())).Returns(new VacancyViewModel
                {
                    VacancyReferenceNumber = vacancyReferenceNumber,
                    NewVacancyViewModel = new NewVacancyViewModel(),
                    TrainingDetailsViewModel = new TrainingDetailsViewModel()
                });
            }

            var vacancyProviderBuilder =
                new VacancyProviderBuilder().With(employerService)
                    .With(providerService)
                    .With(vacancyLockingService)
                    .With(postingService)
                    .With(currentUserService)
                    .With(mapper);

            return vacancyProviderBuilder;
        }
    }
}