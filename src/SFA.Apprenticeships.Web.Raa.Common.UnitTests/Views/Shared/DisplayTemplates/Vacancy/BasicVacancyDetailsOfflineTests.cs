namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using System.Collections.Generic;
    using Common.Views.Shared.DisplayTemplates.Vacancy;
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
        }

        [Test]
        public void MultiLocationOfflineVacancy()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.IsEmployerLocationMainApprenticeshipLocation, false)
                .With(vm => vm.OfflineVacancy, true)
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

            var multipleUrlLink = view.GetElementbyId("multiple-offline-application-urls-button");
            multipleUrlLink.Should().NotBeNull();
            multipleUrlLink.InnerText.Should().Be("enter a different web address for each vacancy location");
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