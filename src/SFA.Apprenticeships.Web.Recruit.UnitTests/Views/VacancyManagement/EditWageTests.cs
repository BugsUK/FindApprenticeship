namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyManagement
{
    using Builders;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Recruit.Views.VacancyManagement;

    [TestFixture]
    public class EditWageTests : ViewUnitTest
    {
        [Test]
        public void EditFixedWageChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.Custom).Build();

            var view = new EditWage().RenderAsHtml(viewModel);

            //Wage choices should not be visible
            view.GetElementbyId("wage-type-label").Should().BeNull();
            view.GetElementbyId("national-minimum-wage").Should().BeNull();
            view.GetElementbyId("apprenticeship-minimum-wage").Should().BeNull();
            var customWage = view.GetElementbyId("custom-wage-radio-label");
            customWage.Should().NotBeNull();
            customWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();

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

            //Wage choices should not be visible
            view.GetElementbyId("wage-type-label").Should().BeNull();
            view.GetElementbyId("national-minimum-wage").Should().BeNull();
            view.GetElementbyId("apprenticeship-minimum-wage").Should().BeNull();
            var customWage = view.GetElementbyId("custom-wage-radio-label");
            customWage.Should().NotBeNull();
            customWage.Attributes["class"].Value.Contains("hidden").Should().BeTrue();

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
    }
}