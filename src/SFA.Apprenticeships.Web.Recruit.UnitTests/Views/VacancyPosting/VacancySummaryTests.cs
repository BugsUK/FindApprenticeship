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
                Wage = new WageViewModel() { Type = WageType.Custom, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null }
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
            view.GetElementbyId("vacancySummaryButton").InnerHtml.Should().Be("Save and continue");
            view.GetElementbyId("vacancySummaryButton").Attributes["value"].Value.Should().Be("VacancySummary");
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



        [Test]
        public void ShouldAlwaysShowChooseTextDescriptionRadio()
        {
            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Referred)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("preset-text").Should().NotBeNull();
        }

        [TestCase(WageType.ToBeAgreedUponAppointment)]
        [TestCase(WageType.CompetitiveSalary)]
        [TestCase(WageType.Unwaged)]
        public void ShouldShowTextDropdownAndReasonBoxWhenChooseTextIsSelected(WageType wageType)
        {
            var wage = new Wage(wageType, null, null, null, null, WageUnit.NotApplicable, null, null);

            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel(wage))
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("preset-text").Attributes["checked"].Should().Be("checked");
            view.GetElementbyId("Wage_PresetText").Should().NotBeNull();
            view.GetElementbyId("Wage_PresetText").ChildNodes.Any(x => x.Attributes.Contains("selected") && x.Attributes["selected"].ToString() == "selected");
            view.GetElementbyId("Wage_PresetText").ChildNodes.First(x => x.Attributes["selected"].ToString() == "selected").Attributes["value"].ToString().Should().Be(wageType.ToString());
            view.GetElementbyId("Wage_WageTypeReason").Should().NotBeNull();
        }

        [TestCase(WageType.Custom)]
        [TestCase(WageType.CustomRange)]
        public void ShouldShowFixedWageAndWageRangeRadiosWhenCustomWageIsSelected(WageType wageType)
        {
            var wage = new Wage(wageType, null, null, null, null, WageUnit.NotApplicable, null, null);

            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel(wage))
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Should().NotBeNull();
            view.GetElementbyId("custom-wage-range").Should().NotBeNull();

            if (wageType == WageType.Custom)
            {
                view.GetElementbyId("custom-wage-fixed").Attributes["checked"].ToString().Should().Be("checked");
            }
            else
            {
                view.GetElementbyId("custom-wage-range").Attributes["checked"].ToString().Should().Be("checked");
            }
        }

        [Test]
        public void ShouldShowFixedWageInputWhenFixedWageIsSelected()
        {
            var wage = new Wage(WageType.Custom, 300, null, null, null, WageUnit.Weekly, null, null);

            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel(wage))
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Attributes["checked"].ToString().Should().Be("checked");
            view.GetElementbyId("custom-wage-range").Attributes["checked"]?.ToString().Should().NotBe("checked");
            view.GetElementbyId("Wage_Amount").Should().NotBeNull();
            view.GetElementbyId("Wage_Amount").Attributes["value"].ToString().Should().Be("300");
            view.GetElementbyId("Wage_Unit").Should().NotBeNull();
            view.GetElementbyId("Wage_Unit").Attributes["value"].ToString().Should().Be(wage.Unit.ToString());
        }


        [Test]
        public void ShouldShowWageRangeInputsWhenWageRangeIsSelected()
        {
            var wage = new Wage(WageType.CustomRange, null, 200, 500, null, WageUnit.Weekly, null, null);

            var details = new VacancySummary();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel(wage))
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Attributes["checked"]?.ToString().Should().NotBe("checked");
            view.GetElementbyId("custom-wage-range").Attributes["checked"].ToString().Should().Be("checked");
            view.GetElementbyId("Wage_AmountLower").Should().NotBeNull();
            view.GetElementbyId("Wage_AmountLower").Attributes["value"].ToString().Should().Be("200");
            view.GetElementbyId("Wage_AmountUpper").Attributes["value"].ToString().Should().Be("500");
            view.GetElementbyId("Wage_Unit").Should().NotBeNull();
            view.GetElementbyId("Wage_Unit").Attributes["value"].ToString().Should().Be(wage.Unit.ToString());
        }


    }
}
