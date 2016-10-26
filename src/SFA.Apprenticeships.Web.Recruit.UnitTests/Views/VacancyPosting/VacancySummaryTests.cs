using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Raa.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using System.Linq;
    using System.Web.Mvc;
    using FluentValidation;
    using Common.Validators;
    using Common.Validators.Extensions;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Ploeh.AutoFixture;
    using Raa.Common.Validators.Vacancy;
    using Recruit.Views.Shared;
    using Shared;
    using VacancySummary = Recruit.Views.VacancyPosting.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    public class VacancySummaryTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new VacancySummary();

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                WageUnits = ApprenticeshipVacancyConverter.GetWageUnits(),
                WageTextPresets = ApprenticeshipVacancyConverter.GetWageTextPresets(),
                DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes(VacancyType.Apprenticeship)
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancySummaryAndExit").Should().NotBeNull("Should exists a save and exit button");
        }

        [TestCase(16, false)]
        [TestCase(2016, true)]
        public void DateYearValidation(int year, bool expectValid)
        {
            //Arrange
            var view = new ValidationSummary();
            var viewModel = new ModelStateDictionary();
            var viewModelToValidate = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel
                    {
                        Day = 1,
                        Month = 2,
                        Year = year
                    },
                    PossibleStartDate = new DateViewModel
                    {
                        Day = 1,
                        Month = 2,
                        Year = year
                    }
                },
                Wage = new WageViewModel() { Type = WageType.Custom, Classification = WageClassification.Custom, CustomType = CustomWageType.Fixed, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null }
            };
            var validator = new VacancySummaryViewModelServerValidator();
            var results = validator.Validate(viewModelToValidate, ruleSet: RuleSets.ErrorsAndWarnings);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));
            var closingDateYearError = document.Errors.FirstOrDefault(n => n.FirstChild.Attributes["href"].Value == "#vacancydatesviewmodel_closingdate_year");
            var possibleStartDateError = document.Errors.FirstOrDefault(n => n.FirstChild.Attributes["href"].Value == "#vacancydatesviewmodel_possiblestartdate_year");

            //Assert
            if (expectValid)
            {
                closingDateYearError.Should().BeNull();
                possibleStartDateError.Should().BeNull();
            }
            else
            {
                closingDateYearError.Should().NotBeNull();
                possibleStartDateError.Should().NotBeNull();
            }
        }

        [Test]
        public void ShouldShowSaveAndContinueToPreviewButtonWhenEditingRejectedVacancy()
        {
            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Referred)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancySummaryButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("vacancySummaryButton").InnerHtml.Should().Be("Save and return to Preview");
            view.GetElementbyId("vacancySummaryButton").Attributes["value"].Value.Should().Be("VacancySummaryAndPreview");
        }

        [Test]
        public void ShouldShowSaveButtonWhenEditingDraftVacancy()
        {
            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancySummaryButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("vacancySummaryButton").InnerHtml.Should().Be("Save and return to Preview");
            view.GetElementbyId("vacancySummaryButton").Attributes["value"].Value.Should().Be("VacancySummaryAndPreview");
        }



        [Test]
        public void ShouldShowSaveAndCancelButtonWhenEditingDates()
        {
            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Live)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("vacancySummaryButton").Should().BeNull("Should exists a save button");
        }
    }
}
