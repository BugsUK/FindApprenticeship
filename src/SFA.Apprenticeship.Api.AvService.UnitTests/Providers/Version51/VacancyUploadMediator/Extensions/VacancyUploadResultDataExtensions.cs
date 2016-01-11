namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers.Version51.VacancyUploadMediator.Extensions
{
    using Common;
    using DataContracts.Version51;
    using FluentAssertions;

    public static class VacancyUploadResultDataExtensions
    {
        public static void ShouldBeValid(this VacancyUploadResultData vacancy)
        {
            vacancy.Status.Should().Be(VacancyUploadResult.Success);
            vacancy.ErrorCodes.Should().NotBeNull();
            vacancy.ErrorCodes.Count.Should().Be(0);
        }
    }
}