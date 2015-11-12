namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.Shared
{
    using System.Web.Mvc;
    using Common.Validators.Extensions;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Recruit.Views.Shared;

    [TestFixture]
    public class ValidationSummaryTests : ViewUnitTest
    {
        [Test]
        public void NoErrorsOrWarnings()
        {
            //Arrange
            var view = new ValidationSummary();
            var viewModel = new ModelStateDictionary();
            var viewModelToValidate = new ValidationSummaryViewModel
            {
                ErrorIfZero = 1,
                ErrorIfNull = "NotNull",
                WarningIfZero = 1,
                WarningIfNull = "NotNull"
            };
            var validator = new ValidationSummaryViewModelValidator();
            var results = validator.Validate(viewModelToValidate);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));

            //Assert
            document.ErrorsClass.Should().StartWith("validation-summary-valid");
            document.ErrorsClass.Should().NotContain("-errors");
            document.WarningsClass.Should().StartWith("validation-summary-valid");
            document.WarningsClass.Should().NotContain("-warnings");
            document.Errors.Count.Should().Be(0);
            document.Warnings.Count.Should().Be(0);
        }

        [Test]
        public void ErrorsAndWarnings()
        {
            //Arrange
            var view = new ValidationSummary();
            var viewModel = new ModelStateDictionary();
            var viewModelToValidate = new ValidationSummaryViewModel();
            var validator = new ValidationSummaryViewModelValidator();
            var results = validator.Validate(viewModelToValidate);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));

            //Assert
            document.ErrorsClass.Should().StartWith("validation-summary-errors");
            document.ErrorsClass.Should().NotContain("-valid");
            document.WarningsClass.Should().StartWith("validation-summary-warnings");
            document.WarningsClass.Should().NotContain("-valid");
            document.Errors.Count.Should().Be(2);
            document.Warnings.Count.Should().Be(2);
        }

        [Test]
        public void ParentChildNoErrorsOrWarnings()
        {
            //Arrange
            var view = new ValidationSummary();
            var viewModel = new ModelStateDictionary();
            var viewModelToValidate = new ParentViewModel
            {
                ErrorIfZero = 1,
                WarningIfZero = 1,
                Child = new ChildViewModel
                {
                    ErrorIfNull = "NotNull",
                    WarningIfNull = "NotNull"
                }
            };
            var validator = new ParentViewModelValidator();
            var results = validator.Validate(viewModelToValidate);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));

            //Assert
            document.ErrorsClass.Should().StartWith("validation-summary-valid");
            document.ErrorsClass.Should().NotContain("-errors");
            document.WarningsClass.Should().StartWith("validation-summary-valid");
            document.WarningsClass.Should().NotContain("-warnings");
            document.Errors.Count.Should().Be(0);
            document.Warnings.Count.Should().Be(0);
        }

        [Test]
        public void ParentChildErrorsAndWarnings()
        {
            //Arrange
            var view = new ValidationSummary();
            var viewModel = new ModelStateDictionary();
            var viewModelToValidate = new ParentViewModel
            {
                Child = new ChildViewModel()
            };
            var validator = new ParentViewModelValidator();
            var results = validator.Validate(viewModelToValidate);
            results.AddToModelStateWithSeverity(viewModel, string.Empty);

            //Act
            var document = new ValidationSummaryDocument(view.RenderAsHtml(viewModel));

            //Assert
            document.ErrorsClass.Should().StartWith("validation-summary-errors");
            document.ErrorsClass.Should().NotContain("-valid");
            document.WarningsClass.Should().StartWith("validation-summary-warnings");
            document.WarningsClass.Should().NotContain("-valid");
            document.Errors.Count.Should().Be(2);
            document.Warnings.Count.Should().Be(2);
        }
    }
}