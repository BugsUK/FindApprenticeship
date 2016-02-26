namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mappers
{
    using Candidate.Mappers;
    using Common.ViewModels.Candidate;
    using Domain.Entities.Candidates;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipCandidateWebMappersTests
    {
        [Test]
        public void MonitoringInformationViewModelShouldNotFailWhenDisablilityOrGenderAreNull()
        {
            var mapper = new ApprenticeshipCandidateWebMappers();
            var monitoringInformationViewModel = new MonitoringInformationViewModel();
            var monitoringInformation = mapper.Map<MonitoringInformationViewModel, MonitoringInformation>(monitoringInformationViewModel);
            monitoringInformation.Should().NotBeNull();
            monitoringInformation.DisabilityStatus.Should().BeNull();
            monitoringInformation.Gender.Should().BeNull();
            monitoringInformation.Ethnicity.Should().Be(null);
        }

        [Test]
        public void MonitoringInformationViewModelShouldParseValuesAsExpected()
        {
            var mapper = new ApprenticeshipCandidateWebMappers();
            var monitoringInformationViewModel = new MonitoringInformationViewModel
            {
                AnythingWeCanDoToSupportYourInterview = "Anything?",
                RequiresSupportForInterview = true,
                DisabilityStatus = (int?) DisabilityStatus.PreferNotToSay,
                Ethnicity = 2,
                Gender = (int?) Gender.PreferNotToSay
            };
            var monitoringInformation = mapper.Map<MonitoringInformationViewModel, MonitoringInformation>(monitoringInformationViewModel);
            monitoringInformation.DisabilityStatus.Should().Be(DisabilityStatus.PreferNotToSay);
            monitoringInformation.Gender.Should().Be(Gender.PreferNotToSay);
            monitoringInformation.Ethnicity.Should().Be(2);
        }
    }
}
