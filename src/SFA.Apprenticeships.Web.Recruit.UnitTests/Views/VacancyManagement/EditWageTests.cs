namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyManagement
{
    using Builders;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Recruit.Views.VacancyManagement;

    /// <summary>
    /// See https://skillsfundingagency.atlassian.net/wiki/display/FAA/Edit+wage+of+Live+vacancy for wireframes these tests are based on
    /// </summary>
    [TestFixture]
    public class EditWageTests : ViewUnitTest
    {
        [Test]
        public void EditFixedWageChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.Custom).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Current wage description should be null
            view.GetElementbyId("current-wage-header").Should().BeNull();
            view.GetElementbyId("current-wage-name").Should().BeNull();
            view.GetElementbyId("vacancy-wage-header").Should().BeNull();
            view.GetElementbyId("vacancy-wage").Should().BeNull();

            //Wage choices should not be visible
            view.GetElementbyId("wage-type-label").Should().BeNull();
            var nationalMinimumWage = view.GetElementbyId("national-minimum-wage-radio-label");
            nationalMinimumWage.Should().NotBeNull();
            nationalMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            var apprenticeshipMinimumWage = view.GetElementbyId("apprenticeship-minimum-wage-radio-label");
            apprenticeshipMinimumWage.Should().NotBeNull();
            apprenticeshipMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            view.GetElementbyId("custom-wage-radio-label").Should().BeNull();

            var customWagePanel = view.GetElementbyId("custom-wage-panel");
            customWagePanel.Should().NotBeNull();

            var customWageLabel = view.GetElementbyId("custom-wage-label");
            customWageLabel.Should().NotBeNull();
            customWageLabel.InnerText.Should().Be("Keep as fixed wage?");

            var customWageFixed = view.GetElementbyId("custom-wage-fixed");
            customWageFixed.Should().NotBeNull();
            customWageFixed.Attributes["checked"].Should().NotBeNull();
            var customWageWageFixedLabelText = view.GetElementbyId("custom-wage-fixed-label-text");
            customWageWageFixedLabelText.Should().NotBeNull();
            customWageWageFixedLabelText.InnerText.Should().Be("Yes");

            var customWageRange = view.GetElementbyId("custom-wage-range");
            customWageRange.Should().NotBeNull();
            customWageRange.Attributes["checked"].Should().BeNull();
            var customWageRangeLabelText = view.GetElementbyId("custom-wage-range-label-text");
            customWageRangeLabelText.Should().NotBeNull();
            customWageRangeLabelText.InnerText.Should().Be("No, change to wage range");

            var customWageFixedHelpText = view.GetElementbyId("custom-wage-fixed-help-text");
            customWageFixedHelpText.Should().NotBeNull();
            customWageFixedHelpText.InnerText.Should().Be("Enter a new increased wage");
            var customWageFixedHintText = view.GetElementbyId("custom-wage-fixed-hint-text");
            customWageFixedHintText.Should().BeNull();

            var customWageRangeHelpText = view.GetElementbyId("custom-wage-range-help-text");
            customWageRangeHelpText.Should().BeNull();
            var customWageRangeHintText = view.GetElementbyId("custom-wage-range-hint-text");
            customWageRangeHintText.Should().NotBeNull();
            customWageRangeHintText.InnerText.Should().Be("The minimum amount in the wage range must not be less than the original fixed wage");

            var amountInput = view.GetElementbyId("Amount");
            amountInput.Should().NotBeNull();
            amountInput.Attributes["value"].Value.Should().Be(viewModel.Amount.ToString());

            var amountLowerBoundInput = view.GetElementbyId("AmountLowerBound");
            amountLowerBoundInput.Should().NotBeNull();
            amountLowerBoundInput.Attributes["value"].Value.Should().Be(viewModel.Amount.ToString());

            var amountUpperBoundInput = view.GetElementbyId("AmountUpperBound");
            amountUpperBoundInput.Should().NotBeNull();
            amountUpperBoundInput.Attributes["value"].Value.Should().Be("");
        }

        [Test]
        public void EditWageRangeChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.CustomRange).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Current wage description should be null
            view.GetElementbyId("current-wage-header").Should().BeNull();
            view.GetElementbyId("current-wage-name").Should().BeNull();
            view.GetElementbyId("vacancy-wage-header").Should().BeNull();
            view.GetElementbyId("vacancy-wage").Should().BeNull();

            //Wage choices should not be visible
            view.GetElementbyId("wage-type-label").Should().BeNull();
            var nationalMinimumWage = view.GetElementbyId("national-minimum-wage-radio-label");
            nationalMinimumWage.Should().NotBeNull();
            nationalMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            var apprenticeshipMinimumWage = view.GetElementbyId("apprenticeship-minimum-wage-radio-label");
            apprenticeshipMinimumWage.Should().NotBeNull();
            apprenticeshipMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            view.GetElementbyId("custom-wage-radio-label").Should().BeNull();

            var customWagePanel = view.GetElementbyId("custom-wage-panel");
            customWagePanel.Should().NotBeNull();

            var customWageLabel = view.GetElementbyId("custom-wage-label");
            customWageLabel.Should().NotBeNull();
            customWageLabel.InnerText.Should().Be("Keep as wage range?");

            var customWageFixed = view.GetElementbyId("custom-wage-fixed");
            customWageFixed.Should().NotBeNull();
            customWageFixed.Attributes["checked"].Should().BeNull();
            var customWageWageFixedLabelText = view.GetElementbyId("custom-wage-fixed-label-text");
            customWageWageFixedLabelText.Should().NotBeNull();
            customWageWageFixedLabelText.InnerText.Should().Be("No, change to fixed wage");

            var customWageRange = view.GetElementbyId("custom-wage-range");
            customWageRange.Should().NotBeNull();
            customWageRange.Attributes["checked"].Should().NotBeNull();
            var customWageRangeLabelText = view.GetElementbyId("custom-wage-range-label-text");
            customWageRangeLabelText.Should().NotBeNull();
            customWageRangeLabelText.InnerText.Should().Be("Yes");

            var customWageFixedHelpText = view.GetElementbyId("custom-wage-fixed-help-text");
            customWageFixedHelpText.Should().BeNull();
            var customWageFixedHintText = view.GetElementbyId("custom-wage-fixed-hint-text");
            customWageFixedHintText.Should().NotBeNull();
            customWageFixedHintText.InnerText.Should().Be("The wage must not be less than the minimum amount set in the original wage range");

            var customWageRangeHelpText = view.GetElementbyId("custom-wage-range-help-text");
            customWageRangeHelpText.Should().NotBeNull();
            customWageRangeHelpText.InnerText.Should().Be("Enter increased wage range");
            var customWageRangeHintText = view.GetElementbyId("custom-wage-range-hint-text");
            customWageRangeHintText.Should().NotBeNull();
            customWageRangeHintText.InnerText.Should().Be("You can increase one or both of these figures");

            var amountInput = view.GetElementbyId("Amount");
            amountInput.Should().NotBeNull();
            amountInput.Attributes["value"].Value.Should().Be(viewModel.AmountLowerBound.ToString());

            var amountLowerBoundInput = view.GetElementbyId("AmountLowerBound");
            amountLowerBoundInput.Should().NotBeNull();
            amountLowerBoundInput.Attributes["value"].Value.Should().Be(viewModel.AmountLowerBound.ToString());

            var amountUpperBoundInput = view.GetElementbyId("AmountUpperBound");
            amountUpperBoundInput.Should().NotBeNull();
            amountUpperBoundInput.Attributes["value"].Value.Should().Be(viewModel.AmountUpperBound.ToString());
        }

        [Test]
        public void EditNationalMinimumWageChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.NationalMinimum).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Current wage description should be visible
            var currentWageHeader = view.GetElementbyId("current-wage-header");
            currentWageHeader.Should().NotBeNull();
            currentWageHeader.InnerText.Should().Be("Current wage");
            var currentWageName = view.GetElementbyId("current-wage-name");
            currentWageName.Should().NotBeNull();
            currentWageName.InnerText.Should().Be("National Minimum Wage");
            var vacancyWageHeader = view.GetElementbyId("vacancy-wage-header");
            vacancyWageHeader.Should().NotBeNull();
            vacancyWageHeader.InnerText.Should().Be("Wage displayed");
            var vacancyWage = view.GetElementbyId("vacancy-wage");
            vacancyWage.Should().NotBeNull();
            vacancyWage.InnerText.Should().Contain("&#163;120.00 - &#163;208.50 (based on 30 paid hours per week)");

            //Wage choices should not be visible
            view.GetElementbyId("wage-type-label").Should().BeNull();
            var nationalMinimumWage = view.GetElementbyId("national-minimum-wage-radio-label");
            nationalMinimumWage.Should().NotBeNull();
            nationalMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            var apprenticeshipMinimumWage = view.GetElementbyId("apprenticeship-minimum-wage-radio-label");
            apprenticeshipMinimumWage.Should().NotBeNull();
            apprenticeshipMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            view.GetElementbyId("custom-wage-radio-label").Should().BeNull();

            var customWagePanel = view.GetElementbyId("custom-wage-panel");
            customWagePanel.Should().NotBeNull();

            var customWageLabel = view.GetElementbyId("custom-wage-label");
            customWageLabel.Should().NotBeNull();
            customWageLabel.InnerText.Should().Be("Select a custom wage option");

            var customWageFixed = view.GetElementbyId("custom-wage-fixed");
            customWageFixed.Should().NotBeNull();
            customWageFixed.Attributes["checked"].Should().BeNull();
            var customWageWageFixedLabelText = view.GetElementbyId("custom-wage-fixed-label-text");
            customWageWageFixedLabelText.Should().NotBeNull();
            customWageWageFixedLabelText.InnerText.Should().Be("Fixed wage");

            var customWageRange = view.GetElementbyId("custom-wage-range");
            customWageRange.Should().NotBeNull();
            customWageRange.Attributes["checked"].Should().BeNull();
            var customWageRangeLabelText = view.GetElementbyId("custom-wage-range-label-text");
            customWageRangeLabelText.Should().NotBeNull();
            customWageRangeLabelText.InnerText.Should().Be("Wage range");

            var customWageFixedHelpText = view.GetElementbyId("custom-wage-fixed-help-text");
            customWageFixedHelpText.Should().BeNull();
            var customWageFixedHintText = view.GetElementbyId("custom-wage-fixed-hint-text");
            customWageFixedHintText.Should().NotBeNull();
            customWageFixedHintText.InnerText.Should().Be("The new increased wage must be more than &#163;208.50");

            var customWageRangeHelpText = view.GetElementbyId("custom-wage-range-help-text");
            customWageRangeHelpText.Should().BeNull();
            var customWageRangeHintText = view.GetElementbyId("custom-wage-range-hint-text");
            customWageRangeHintText.Should().NotBeNull();
            customWageRangeHintText.InnerText.Should().Be("The minimum amount in the wage range must be more than &#163;208.50");

            var amountInput = view.GetElementbyId("Amount");
            amountInput.Should().NotBeNull();
            amountInput.Attributes["value"].Value.Should().Be("");

            var amountLowerBoundInput = view.GetElementbyId("AmountLowerBound");
            amountLowerBoundInput.Should().NotBeNull();
            amountLowerBoundInput.Attributes["value"].Value.Should().Be("");

            var amountUpperBoundInput = view.GetElementbyId("AmountUpperBound");
            amountUpperBoundInput.Should().NotBeNull();
            amountUpperBoundInput.Attributes["value"].Value.Should().Be("");
        }

        [Test]
        public void EditApprenticeMinimumWageChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.ApprenticeshipMinimum).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Current wage description should be visible
            var currentWageHeader = view.GetElementbyId("current-wage-header");
            currentWageHeader.Should().NotBeNull();
            currentWageHeader.InnerText.Should().Be("Current wage");
            var currentWageName = view.GetElementbyId("current-wage-name");
            currentWageName.Should().NotBeNull();
            currentWageName.InnerText.Should().Be("National minimum wage for apprentices");
            var vacancyWageHeader = view.GetElementbyId("vacancy-wage-header");
            vacancyWageHeader.Should().NotBeNull();
            vacancyWageHeader.InnerText.Should().Be("Wage displayed");
            var vacancyWage = view.GetElementbyId("vacancy-wage");
            vacancyWage.Should().NotBeNull();
            vacancyWage.InnerText.Should().Contain("&#163;102.00 (based on 30 paid hours per week)");

            //Wage choices should be visible
            var wageTypeLabel = view.GetElementbyId("wage-type-label");
            wageTypeLabel.Should().NotBeNull();
            wageTypeLabel.InnerText.Should().Be("Change wage type");
            var nationalMinimumWage = view.GetElementbyId("national-minimum-wage-radio-label");
            nationalMinimumWage.Should().NotBeNull();
            nationalMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeFalse();
            var apprenticeshipMinimumWage = view.GetElementbyId("apprenticeship-minimum-wage-radio-label");
            apprenticeshipMinimumWage.Should().NotBeNull();
            apprenticeshipMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();
            var customWage = view.GetElementbyId("custom-wage-radio-label");
            customWage.Should().NotBeNull();
            customWage.Attributes["class"].Value.Contains("hidden").Should().BeFalse();

            var customWagePanel = view.GetElementbyId("custom-wage-panel");
            customWagePanel.Should().NotBeNull();

            var customWageLabel = view.GetElementbyId("custom-wage-label");
            customWageLabel.Should().NotBeNull();
            customWageLabel.InnerText.Should().Be("Select an option");

            var customWageFixed = view.GetElementbyId("custom-wage-fixed");
            customWageFixed.Should().NotBeNull();
            customWageFixed.Attributes["checked"].Should().BeNull();
            var customWageWageFixedLabelText = view.GetElementbyId("custom-wage-fixed-label-text");
            customWageWageFixedLabelText.Should().NotBeNull();
            customWageWageFixedLabelText.InnerText.Should().Be("Fixed wage");

            var customWageRange = view.GetElementbyId("custom-wage-range");
            customWageRange.Should().NotBeNull();
            customWageRange.Attributes["checked"].Should().BeNull();
            var customWageRangeLabelText = view.GetElementbyId("custom-wage-range-label-text");
            customWageRangeLabelText.Should().NotBeNull();
            customWageRangeLabelText.InnerText.Should().Be("Wage range");

            var customWageFixedHelpText = view.GetElementbyId("custom-wage-fixed-help-text");
            customWageFixedHelpText.Should().BeNull();
            var customWageFixedHintText = view.GetElementbyId("custom-wage-fixed-hint-text");
            customWageFixedHintText.Should().NotBeNull();
            customWageFixedHintText.InnerText.Should().Be("The new increased wage must be more than &#163;102.00");

            var customWageRangeHelpText = view.GetElementbyId("custom-wage-range-help-text");
            customWageRangeHelpText.Should().BeNull();
            var customWageRangeHintText = view.GetElementbyId("custom-wage-range-hint-text");
            customWageRangeHintText.Should().NotBeNull();
            customWageRangeHintText.InnerText.Should().Be("The minimum amount in the wage range must be more than &#163;102.00");

            var amountInput = view.GetElementbyId("Amount");
            amountInput.Should().NotBeNull();
            amountInput.Attributes["value"].Value.Should().Be("");

            var amountLowerBoundInput = view.GetElementbyId("AmountLowerBound");
            amountLowerBoundInput.Should().NotBeNull();
            amountLowerBoundInput.Attributes["value"].Value.Should().Be("");

            var amountUpperBoundInput = view.GetElementbyId("AmountUpperBound");
            amountUpperBoundInput.Should().NotBeNull();
            amountUpperBoundInput.Attributes["value"].Value.Should().Be("");
        }

        [Test]
        public void EditUnwagedChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.Unwaged).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Current wage description should be visible
            var currentWageHeader = view.GetElementbyId("current-wage-header");
            currentWageHeader.Should().NotBeNull();
            currentWageHeader.InnerText.Should().Be("Current wage");
            var currentWageName = view.GetElementbyId("current-wage-name");
            currentWageName.Should().NotBeNull();
            currentWageName.InnerText.Should().Be("Unwaged");
            view.GetElementbyId("vacancy-wage-header").Should().BeNull();
            view.GetElementbyId("vacancy-wage").Should().BeNull();

            //Wage choices should be visible
            var wageTypeLabel = view.GetElementbyId("wage-type-label");
            wageTypeLabel.Should().NotBeNull();
            wageTypeLabel.InnerText.Should().Be("Change wage type");
            var nationalMinimumWage = view.GetElementbyId("national-minimum-wage-radio-label");
            nationalMinimumWage.Should().NotBeNull();
            nationalMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeFalse();
            var apprenticeshipMinimumWage = view.GetElementbyId("apprenticeship-minimum-wage-radio-label");
            apprenticeshipMinimumWage.Should().NotBeNull();
            apprenticeshipMinimumWage.Attributes["class"].Value.Contains("hidden").Should().BeFalse();
            var customWage = view.GetElementbyId("custom-wage-radio-label");
            customWage.Should().NotBeNull();
            customWage.Attributes["class"].Value.Contains("hidden").Should().BeFalse();

            var customWagePanel = view.GetElementbyId("custom-wage-panel");
            customWagePanel.Should().NotBeNull();

            var customWageLabel = view.GetElementbyId("custom-wage-label");
            customWageLabel.Should().NotBeNull();
            customWageLabel.InnerText.Should().Be("Select an option");

            var customWageFixed = view.GetElementbyId("custom-wage-fixed");
            customWageFixed.Should().NotBeNull();
            customWageFixed.Attributes["checked"].Should().BeNull();
            var customWageWageFixedLabelText = view.GetElementbyId("custom-wage-fixed-label-text");
            customWageWageFixedLabelText.Should().NotBeNull();
            customWageWageFixedLabelText.InnerText.Should().Be("Fixed wage");

            var customWageRange = view.GetElementbyId("custom-wage-range");
            customWageRange.Should().NotBeNull();
            customWageRange.Attributes["checked"].Should().BeNull();
            var customWageRangeLabelText = view.GetElementbyId("custom-wage-range-label-text");
            customWageRangeLabelText.Should().NotBeNull();
            customWageRangeLabelText.InnerText.Should().Be("Wage range");

            var customWageFixedHelpText = view.GetElementbyId("custom-wage-fixed-help-text");
            customWageFixedHelpText.Should().BeNull();
            var customWageFixedHintText = view.GetElementbyId("custom-wage-fixed-hint-text");
            customWageFixedHintText.Should().NotBeNull();
            customWageFixedHintText.InnerText.Should().Be("The new increased wage must be more than &#163;102.00");

            var customWageRangeHelpText = view.GetElementbyId("custom-wage-range-help-text");
            customWageRangeHelpText.Should().BeNull();
            var customWageRangeHintText = view.GetElementbyId("custom-wage-range-hint-text");
            customWageRangeHintText.Should().NotBeNull();
            customWageRangeHintText.InnerText.Should().Be("The minimum amount in the wage range must be more than &#163;102.00");

            var amountInput = view.GetElementbyId("Amount");
            amountInput.Should().NotBeNull();
            amountInput.Attributes["value"].Value.Should().Be("");

            var amountLowerBoundInput = view.GetElementbyId("AmountLowerBound");
            amountLowerBoundInput.Should().NotBeNull();
            amountLowerBoundInput.Attributes["value"].Value.Should().Be("");

            var amountUpperBoundInput = view.GetElementbyId("AmountUpperBound");
            amountUpperBoundInput.Should().NotBeNull();
            amountUpperBoundInput.Attributes["value"].Value.Should().Be("");
        }
    }
}