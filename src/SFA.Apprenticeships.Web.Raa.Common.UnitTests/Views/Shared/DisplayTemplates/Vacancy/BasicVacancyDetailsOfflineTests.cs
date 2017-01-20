namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using HtmlAgilityPack;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using System.Collections.Generic;
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
                .With(vm => vm.VacancyLocationType, VacancyLocationType.SpecificLocation)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.LocationAddresses, null)
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);
            AssertOfflineUrls(view, false, false);
            AssertOfflineDisplay(view, false, true);
        }

        [Test]
        public void SingleDifferentLocationOfflineVacancy()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyLocationType, VacancyLocationType.MultipleLocations)
                .With(vm => vm.OfflineVacancy, true)
                .With(vm => vm.LocationAddresses, new List<VacancyLocationAddressViewModel> { new VacancyLocationAddressViewModel() })
                .Create();
            var details = new BasicVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            AssertOfflineSelected(view);
            AssertOfflineUrls(view, false, false);
            AssertOfflineDisplay(view, false, true);
        }

        [TestCase(null)]
        [TestCase(OfflineVacancyType.Unknown)]
        [TestCase(OfflineVacancyType.SingleUrl)]
        public void MultiLocationOfflineVacancy(OfflineVacancyType offlineVacancyType)
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyLocationType, VacancyLocationType.MultipleLocations)
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
            AssertOfflineUrls(view, true, false);
            AssertOfflineDisplay(view, false, true);
        }

        [Test]
        public void MultiLocationOfflineVacancyMultiUrls()
        {
            //Arrange
            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyLocationType, VacancyLocationType.MultipleLocations)
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
            AssertOfflineUrls(view, false, true);
            AssertOfflineDisplay(view, true, false);

            var multipleUrlsTable = view.GetElementbyId("multiple-offline-application-urls-table");
            multipleUrlsTable.Should().NotBeNull();
            for (int i = 0; i < viewModel.LocationAddresses.Count; i++)
            {
                var locationId = view.GetElementbyId($"LocationAddresses_{i}__VacancyLocationId");
                locationId.Should().NotBeNull();
                var locationPositions = view.GetElementbyId($"LocationAddresses_{i}__NumberOfPositions");
                locationPositions.Should().NotBeNull();
                var locationAddress = view.GetElementbyId($"locationaddresses_{i}__offlineapplicationurl");
                locationAddress.Should().NotBeNull();
            }
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

        private static void AssertOfflineUrls(HtmlDocument view, bool multipleUrlsVisible, bool singleUrlVisible)
        {
            var multipleUrlsButton = view.GetElementbyId("multiple-offline-application-urls-button");
            multipleUrlsButton.Should().NotBeNull();
            multipleUrlsButton.InnerText.Should().Be("enter a different web address for each vacancy location");
            var multipleUrlsParagraph = view.GetElementbyId("multiple-offline-application-urls-para");
            multipleUrlsParagraph.Should().NotBeNull();
            if (multipleUrlsVisible)
            {
                multipleUrlsButton.ParentNode.Attributes["style"].Should().BeNull();
                multipleUrlsParagraph.Attributes["style"].Should().BeNull();
            }
            else
            {
                multipleUrlsButton.ParentNode.Attributes["style"].Value.Should().Be("display: none;");
                multipleUrlsParagraph.Attributes["style"].Value.Should().Be("display: none;");
            }
            var singleUrlButton = view.GetElementbyId("single-offline-application-url-button");
            singleUrlButton.Should().NotBeNull();
            singleUrlButton.InnerText.Should().Be("use the same web address for all vacancy locations");
            var singleUrlParagraph = view.GetElementbyId("single-offline-application-url-para");
            singleUrlParagraph.Should().NotBeNull();
            if (singleUrlVisible)
            {
                singleUrlButton.ParentNode.Attributes["style"].Should().BeNull();
                singleUrlParagraph.Attributes["style"].Should().BeNull();
            }
            else
            {
                singleUrlButton.ParentNode.Attributes["style"].Value.Should().Be("display: none;");
                singleUrlParagraph.Attributes["style"].Value.Should().Be("display: none;");
            }
        }

        private static void AssertOfflineDisplay(HtmlDocument view, bool multipleUrlsVisible, bool singleUrlVisible)
        {
            var multipleUrlsTable = view.GetElementbyId("multiple-offline-application-urls-table");
            multipleUrlsTable.Should().NotBeNull();
            var multipleUrlsDiv = view.GetElementbyId("multiple-offline-application-urls-div");
            multipleUrlsDiv.Should().NotBeNull();
            if (multipleUrlsVisible)
            {
                multipleUrlsDiv.Attributes["style"].Should().BeNull();
            }
            else
            {
                multipleUrlsDiv.Attributes["style"].Value.Should().Be("display: none;");
            }
            var singleUrlInput = view.GetElementbyId("apprenticeship-offline-application-url");
            singleUrlInput.Should().NotBeNull();
            var singleUrlDiv = view.GetElementbyId("single-offline-application-url-div");
            singleUrlDiv.Should().NotBeNull();
            if (singleUrlVisible)
            {
                singleUrlDiv.Attributes["style"].Should().BeNull();
            }
            else
            {
                singleUrlDiv.Attributes["style"].Value.Should().Be("display: none;");
            }
        }
    }
}