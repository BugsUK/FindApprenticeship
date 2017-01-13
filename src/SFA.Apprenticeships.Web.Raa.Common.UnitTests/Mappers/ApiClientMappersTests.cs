namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using NUnit.Framework;
    using ApiGeoPoint = DAS.RAA.Api.Client.V1.Models.GeoPoint;
    using ApiPostalAddress = DAS.RAA.Api.Client.V1.Models.PostalAddress;
    using ApiWage = DAS.RAA.Api.Client.V1.Models.Wage;
    using ApiVacancy = DAS.RAA.Api.Client.V1.Models.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class ApiClientMappersTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new ApiClientMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void WageMappingTest()
        {
            var mappers = new ApiClientMappers();

            var apiWage = new ApiWage("NationalMinimum", null, null, null, null, "£120.00 - £208.50", "Weekly", 30);
            var apiVacancy = new ApiVacancy {Wage = apiWage};

            var vacancy = mappers.Map<ApiVacancy, Vacancy>(apiVacancy);
        }
    }
}