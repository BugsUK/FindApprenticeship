namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mappers
{
    using Candidate.Mappers;
    using Candidate.ViewModels.VacancySearch;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipVacancyDetailViewModelMapperTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new ApprenticeshipCandidateWebMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldNotAnonymiseEmployerNameIfEmployerHasNotOptedForAnonymity()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerName = "Acme Corp",
                IsEmployerAnonymous = false,
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.EmployerName);
        }

        [Test]
        public void ShouldAnonymiseEmployerNameIfEmployerHasOptedForAnonymity()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerName = "Acme Corp",
                AnonymousEmployerName = "Blue Chip Corp",
                IsEmployerAnonymous = true,
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerName.Should().Be(vacancyDetail.AnonymousEmployerName);
        }

        [Test]
        public void ShouldShowWageIfWageTypeIsWeekly()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                Wage = new Wage(WageType.LegacyWeekly, 101.19m, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);
            const string expectedWage = "£101.19";

            model.Should().NotBeNull();
            WagePresenter.GetDisplayAmount(model.Wage.Type, model.Wage.Amount, model.Wage.AmountLowerBound, model.Wage.AmountUpperBound, model.Wage.Text, model.Wage.HoursPerWeek, null).Should().Be(expectedWage);
        }

        [Test]
        public void ShouldShowWageDescriptionIfWageTypeIsText()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                Wage = new Wage(WageType.LegacyText, null, null, null, "Competitive", WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            WagePresenter.GetDisplayAmount(model.Wage.Type, model.Wage.Amount, model.Wage.AmountLowerBound, model.Wage.AmountUpperBound, model.Wage.Text, model.Wage.HoursPerWeek, null).Should()
                .Be("Competitive");
        }

        [Test]
        public void ShouldTryParseWageDescriptionIfWageTypeIsText()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                Wage = new Wage(WageType.LegacyText, null, null, null, "123.45678", WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            WagePresenter.GetDisplayAmount(model.Wage.Type, model.Wage.Amount, model.Wage.AmountLowerBound, model.Wage.AmountUpperBound, model.Wage.Text, model.Wage.HoursPerWeek, null).Should().Be("£123.46");
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "http://wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("http://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheEmployerWebsiteWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "https://wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().StartWith("https://");
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfEmployerWebsiteIsNotWellFormed()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                EmployerWebsite = "www.somedomain.co.uk / www.anotherdomain.co.uk",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.EmployerWebsite.Should().Be(vacancyDetail.EmployerWebsite);
            model.IsWellFormedEmployerWebsiteUrl.Should().Be(false);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItAlreadyHasIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "http://wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpIfItDoesntHaveIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("http://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnTheVacancyUrlWithHttpsIfItStartsWithIt()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "https://wwww.someweb.com",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().StartWith("https://");
            model.IsWellFormedVacancyUrl.Should().Be(true);
        }

        [Test]
        public void ShouldReturnRawStringIfVacancyUrlIsNotWellFormed()
        {
            var vacancyDetail = new ApprenticeshipVacancyDetail
            {
                VacancyUrl = "www.somedomain.co.uk / www.anotherdomain.co.uk",
                Wage = new Wage(WageType.Custom, null, null, null, null, WageUnit.NotApplicable, null, null)
            };

            var model = new ApprenticeshipCandidateWebMappers().Map<ApprenticeshipVacancyDetail, ApprenticeshipVacancyDetailViewModel>(vacancyDetail);

            model.Should().NotBeNull();
            model.VacancyUrl.Should().Be(vacancyDetail.VacancyUrl);
            model.IsWellFormedVacancyUrl.Should().Be(false);
        }
    }
}
