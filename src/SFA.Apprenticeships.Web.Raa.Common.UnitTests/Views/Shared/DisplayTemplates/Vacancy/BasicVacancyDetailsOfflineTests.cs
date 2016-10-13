namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using System.Collections.Generic;
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using HtmlAgilityPack;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    [TestFixture]
    public class BasicVacancyDetailsOfflineTests : ViewUnitTest
    {
        [Test]
        public void SingleLocationOfflineVacancy()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.IsEmployerLocationMainApprenticeshipLocation, true)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.LocationAddresses, null)
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);

            view.GetElementbyId("multiple-offline-application-urls-button").Should().BeNull();
            view.GetElementbyId("single-offline-application-url-button").Should().BeNull();
            view.GetElementbyId("apprenticeship-offline-application-url").Should().NotBeNull();
        }

        [Test]
        public void SingleDifferentLocationOfflineVacancy()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.LocationAddresses, new List<VacancyLocationAddressViewModel> {new VacancyLocationAddressViewModel()})
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);

            view.GetElementbyId("multiple-offline-application-urls-button").Should().BeNull();
            view.GetElementbyId("single-offline-application-url-button").Should().BeNull();
            view.GetElementbyId("apprenticeship-offline-application-url").Should().NotBeNull();
        }

        [TestCase(null)]
        [TestCase(OfflineVacancyType.Unknown)]
        [TestCase(OfflineVacancyType.SingleUrl)]
        public void MultiLocationOfflineVacancy(OfflineVacancyType offlineVacancyType)
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.OfflineVacancyType, offlineVacancyType)
                .With(vm => vm.LocationAddresses, new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel(),
                    new VacancyLocationAddressViewModel(),
                    new VacancyLocationAddressViewModel()
                })
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);

            var multipleUrlsButton = view.GetElementbyId("multiple-offline-application-urls-button");
            multipleUrlsButton.Should().NotBeNull();
            multipleUrlsButton.InnerText.Should().Be("enter a different web address for each vacancy location");
            view.GetElementbyId("single-offline-application-url-button").Should().BeNull();
            view.GetElementbyId("apprenticeship-offline-application-url").Should().NotBeNull();
        }

        [Test]
        public void MultiLocationOfflineVacancyMultiUrls()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.OfflineVacancyType, OfflineVacancyType.MultiUrl)
                .With(vm => vm.LocationAddresses, new List<VacancyLocationAddressViewModel>
                {
                    new VacancyLocationAddressViewModel(),
                    new VacancyLocationAddressViewModel(),
                    new VacancyLocationAddressViewModel()
                })
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);

            view.GetElementbyId("multiple-offline-application-urls-button").Should().BeNull();
            var singleUrlButton = view.GetElementbyId("single-offline-application-url-button");
            singleUrlButton.Should().NotBeNull();
            singleUrlButton.InnerText.Should().Be("use the same web address for all vacancy locations");

            //Single offline application url input should not be visible
            view.GetElementbyId("apprenticeship-offline-application-url").Should().BeNull();
            //Instead url table should be visible
            var multipleUrlsTable = view.GetElementbyId("multiple-apprenticeship-offline-application-urls-table");
            multipleUrlsTable.Should().NotBeNull();
        }

        private static void AssertOfflineSelected(HtmlDocument view)
        {
            var onlineInput = view.GetElementbyId("apprenticeship-online-vacancy");
            onlineInput.Attributes["checked"].Should().BeNull();
            onlineInput.Attributes["value"].Value.Should().Be("False");
            var offlineInput = view.GetElementbyId("apprenticeship-offline-vacancy");
            offlineInput.Attributes["checked"].Value.Should().Be("checked");
            offlineInput.Attributes["value"].Value.Should().Be("True");
        }
    }
}