namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Infrastructure.Common.Mappers;
    using ApiGeoPoint = DAS.RAA.Api.Client.V1.Models.GeoPoint;
    using ApiPostalAddress = DAS.RAA.Api.Client.V1.Models.PostalAddress;
    using ApiWage = DAS.RAA.Api.Client.V1.Models.Wage;
    using ApiVacancy = DAS.RAA.Api.Client.V1.Models.Vacancy;

    public class ApiClientMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApiGeoPoint, GeoPoint>();
            Mapper.CreateMap<ApiPostalAddress, PostalAddress>();
            Mapper.CreateMap<ApiWage, Wage>();
            Mapper.CreateMap<ApiVacancy, Vacancy>();
        }
    }
}