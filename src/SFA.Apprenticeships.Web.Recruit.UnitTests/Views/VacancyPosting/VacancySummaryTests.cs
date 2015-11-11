using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Raa.Common.Converters;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using System.Linq;
    using System.Web.Mvc;
    using FluentValidation;
    using Common.Validators;
    using Common.Validators.Extensions;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Raa.Common.Validators.Vacancy;
    using Recruit.Views.Shared;
    using Shared;

    [TestFixture]
    public class VacancySummaryTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new VacancySummary();

            var viewModel = new VacancySummaryViewModel
            {
                WageUnits = ApprenticeshipVacancyConverter.GetWageUnits(),
                DurationTypes = ApprenticeshipVacancyConverter.GetDurationTypes()
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
            var viewModelToValidate = new VacancySummaryViewModel
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
            };
            var validator = new VacancySummaryViewModelServerValidator();
            var results = validator.Validate(viewModelToValidate, ruleSet: RuleSets.ErrorsAndWarnings);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));
            var closingDateYearError = document.Errors.FirstOrDefault(n => n.FirstChild.Attributes["href"].Value == "#closingdate_year");
            var possibleStartDateError = document.Errors.FirstOrDefault(n => n.FirstChild.Attributes["href"].Value == "#possiblestartdate_year");

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
    }
}
