namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using System.Linq;
    using Common.Converters;
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    public class FurtherVacancyDetailsTests : ViewUnitTest
    {
        private WageViewModel dummyWageViewModel = new WageViewModel()
        {
            Type = WageType.Custom,
            Amount = null,
            AmountLowerBound = null,
            AmountUpperBound = null,
            Text = null,
            Unit = WageUnit.NotApplicable,
            HoursPerWeek = null
        };

        [Test]
        public void ApprenticeshipHeading()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("heading").Should().NotBeNull();
            view.GetElementbyId("heading").InnerText.Should().Contain("Enter further details");
        }

        [Test]
        public void TraineeshipHeading()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("heading").Should().NotBeNull();
            view.GetElementbyId("heading").InnerText.Should().Be("Enter opportunity details");
        }

        [Test]
        public void ApprenticeshipWorkingWeekLabel()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var workingWeek = view.GetElementbyId("WorkingWeek");
            workingWeek.Should().NotBeNull();
            var label = workingWeek.PreviousSibling;
            label.Should().NotBeNull();
            label.InnerText.Should().Be("Working week");
        }

        [Test]
        public void TraineeshipWorkingWeekLabel()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var workingWeek = view.GetElementbyId("WorkingWeek");
            workingWeek.Should().NotBeNull();
            var label = workingWeek.PreviousSibling;
            label.Should().NotBeNull();
            label.InnerText.Should().Be("Weekly hours");
        }

        [Test]
        public void ShouldHaveHoursPerWeekWhenApprenticeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .With(vm => vm.Wage, dummyWageViewModel)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var hoursPerWeek = view.GetElementbyId("Wage_HoursPerWeek");
            hoursPerWeek.Should().NotBeNull();
            hoursPerWeek.Attributes["type"].Value.Should().Be("tel");
        }

        [Test]
        public void ShouldNotHaveHoursPerWeekWhenTraineeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .With(vm => vm.Wage, dummyWageViewModel)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var hoursPerWeek = view.GetElementbyId("Wage_HoursPerWeek");
            hoursPerWeek.Should().NotBeNull();
            hoursPerWeek.Attributes["type"].Value.Should().Be("hidden");
        }
        
        [Test]
        public void ShouldHaveWageWhenApprenticeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .With(vm => vm.Wage, dummyWageViewModel)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("custom-wage").Should().NotBeNull();
            view.GetElementbyId("national-minimum-wage").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-minimum-wage").Should().NotBeNull();
            var wage = view.GetElementbyId("Wage_Amount");
            wage.Should().NotBeNull();
            wage.Attributes["type"].Value.Should().Be("tel");
            var wageUnit = view.GetElementbyId("Wage_Unit");
            wageUnit.Should().NotBeNull();
            wageUnit.Name.Should().Be("select");
        }

        [Test]
        public void ShouldHaveWageRegardlessOfWageTextValue()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .With(vm => vm.Wage, dummyWageViewModel)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("custom-wage").Should().NotBeNull();
            view.GetElementbyId("national-minimum-wage").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-minimum-wage").Should().NotBeNull();
            var wage = view.GetElementbyId("Wage_Amount");
            wage.Should().NotBeNull();
            wage.Attributes["type"].Value.Should().Be("tel");
            var wageUnit = view.GetElementbyId("Wage_Unit");
            wageUnit.Should().NotBeNull();
            wageUnit.Name.Should().Be("select");
        }

        [Test]
        public void ShouldNotHaveWageWhenTraineeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .With(vm => vm.Wage, dummyWageViewModel)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("custom-wage").Should().BeNull();
            view.GetElementbyId("national-minimum-wage").Should().BeNull();
            view.GetElementbyId("apprenticeship-minimum-wage").Should().BeNull();
            var wageType= view.GetElementbyId("Wage_Type");
            wageType.Should().NotBeNull();
            wageType.Attributes["type"].Value.Should().Be("hidden");
            var wage = view.GetElementbyId("Wage_Amount");
            wage.Should().NotBeNull();
            wage.Attributes["type"].Value.Should().Be("hidden");
            var wageUnit = view.GetElementbyId("Wage_Unit");
            wageUnit.Should().NotBeNull();
            wageUnit.Attributes["type"].Value.Should().Be("hidden");
        }

        [Test]
        public void ShouldNoDisplayLegacyExpectedDurationWhenVacancySourceIsRaa()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.VacancySource, VacancySource.Raa)
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var expectedDuration = view.GetElementbyId("ExpectedDuration");
            expectedDuration.Should().BeNull();
        }

        [Test]
        public void ShouldDisplayHoursPerWeek()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .With(vm => vm.Wage, new WageViewModel() { Type = WageType.Custom, Amount = null, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = 37.5m })
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var hoursPerWeek = view.GetElementbyId("Wage_HoursPerWeek");
            hoursPerWeek.Should().NotBeNull();
            hoursPerWeek.GetAttributeValue("value", "").Should().Be("37.5");
        }

        [Test]
        public void ShouldDisplayWageAmount()
        {
            //Arrange
            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .With(vm => vm.Wage, new WageViewModel() { Type = WageType.Custom, Amount = 123.45m, AmountLowerBound = null, AmountUpperBound = null, Text = null, Unit = WageUnit.NotApplicable, HoursPerWeek = null })
                .Create();
            var details = new FurtherVacancyDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var wage = view.GetElementbyId("Wage_Amount");
            wage.Should().NotBeNull();
            wage.GetAttributeValue("value", "").Should().Be("123.45");
        }

        [Test]
        public void ShouldAlwaysShowChooseTextDescriptionRadio()
        {
            var details = new FurtherVacancyDetails();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Referred)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("preset-text").Should().NotBeNull();
        }

        [TestCase(PresetText.ToBeAgreedUponAppointment)]
        [TestCase(PresetText.CompetitiveSalary)]
        [TestCase(PresetText.Unwaged)]
        public void ShouldShowTextDropdownAndReasonBoxWhenChooseTextIsSelected(PresetText wageType)
        {
            var details = new FurtherVacancyDetails();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel() { Type = WageType.LegacyText, PresetText = wageType })
                .With(v => v.WageTextPresets, ApprenticeshipVacancyConverter.GetWageTextPresets())
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("preset-text").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("Wage_PresetText").Should().NotBeNull();
            view.GetElementbyId("Wage_PresetText").ChildNodes.Any(x => x.Attributes.Contains("selected") && x.Attributes["selected"].Value == "selected").Should().BeTrue();
            view.GetElementbyId("Wage_PresetText").ChildNodes.Single(x => x.Attributes.Contains("selected") && x.Attributes["selected"].Value == "selected").Attributes["value"].Value.Should().Be(((int)wageType).ToString());
            view.GetElementbyId("Wage_WageTypeReason").Should().NotBeNull();
        }

        [TestCase(CustomWageType.Fixed)]
        [TestCase(CustomWageType.Ranged)]
        public void ShouldShowFixedWageAndWageRangeRadiosWhenCustomWageIsSelected(CustomWageType wageType)
        {
            var details = new FurtherVacancyDetails();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel() { Type = WageType.Custom,  CustomType = wageType })
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Should().NotBeNull();
            view.GetElementbyId("custom-wage-range").Should().NotBeNull();

            if (wageType == CustomWageType.Fixed)
            {
                view.GetElementbyId("custom-wage-fixed").Attributes["checked"].Value.Should().Be("checked");
            }
            else
            {
                view.GetElementbyId("custom-wage-range").Attributes["checked"].Value.Should().Be("checked");
            }
        }

        [Test]
        public void ShouldShowFixedWageInputWhenFixedWageIsSelected()
        {
            var details = new FurtherVacancyDetails();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.WageUnits, ApprenticeshipVacancyConverter.GetWageUnits())
                .With(v => v.Wage, new WageViewModel() {Type = WageType.Custom, CustomType = CustomWageType.Fixed, Amount = 300, Unit = WageUnit.Weekly})
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("custom-wage-range").Attributes["checked"]?.Value.Should().NotBe("checked");
            view.GetElementbyId("Wage_Amount").Should().NotBeNull();
            view.GetElementbyId("Wage_Amount").Attributes["value"].Value.Should().Be("300");
            view.GetElementbyId("Wage_Unit").Should().NotBeNull();
            view.GetElementbyId("Wage_Unit")
                .ChildNodes.Any(cn => cn.Attributes["value"].Value == WageUnit.Weekly.ToString()).Should().BeTrue();
        }


        [Test]
        public void ShouldShowWageRangeInputsWhenWageRangeIsSelected()
        {
            var details = new FurtherVacancyDetails();

            var viewModel = new Fixture().Build<FurtherVacancyDetailsViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.Wage, new WageViewModel() {Type = WageType.Custom, CustomType = CustomWageType.Ranged, AmountLowerBound = 200, AmountUpperBound = 500, Unit = WageUnit.Weekly })
                .With(v => v.WageUnits, ApprenticeshipVacancyConverter.GetWageUnits())
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("custom-wage").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("custom-wage-fixed").Attributes["checked"]?.Value.Should().NotBe("checked");
            view.GetElementbyId("custom-wage-range").Attributes["checked"].Value.Should().Be("checked");
            view.GetElementbyId("Wage_AmountLowerBound").Should().NotBeNull();
            view.GetElementbyId("Wage_AmountLowerBound").Attributes["value"].Value.Should().Be("200");
            view.GetElementbyId("Wage_AmountUpperBound").Attributes["value"].Value.Should().Be("500");
            view.GetElementbyId("Wage_RangeUnit").Should().NotBeNull();
            view.GetElementbyId("Wage_RangeUnit")
                .ChildNodes.Any(cn => cn.Attributes["value"].Value == WageUnit.Weekly.ToString()).Should().BeTrue();
        }
    }
}