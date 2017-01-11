namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using DAS.RAA.Api.Client.V1.Models;
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using ApiGeoPoint = DAS.RAA.Api.Client.V1.Models.GeoPoint;
    using ApiPostalAddress = DAS.RAA.Api.Client.V1.Models.PostalAddress;
    using ApiWage = DAS.RAA.Api.Client.V1.Models.Wage;
    using ApiVacancy = DAS.RAA.Api.Client.V1.Models.Vacancy;
    using ApiWageUpdate = DAS.RAA.Api.Client.V1.Models.WageUpdate;
    using GeoPoint = Domain.Entities.Raa.Locations.GeoPoint;
    using PostalAddress = Domain.Entities.Raa.Locations.PostalAddress;
    using Vacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using Wage = Domain.Entities.Vacancies.Wage;
    using WageUpdate = Domain.Entities.Raa.Vacancies.WageUpdate;

    public class ApiClientMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApiGeoPoint, GeoPoint>();
            Mapper.CreateMap<ApiPostalAddress, PostalAddress>();
            Mapper.CreateMap<ApiWage, Wage>();
            Mapper.CreateMap<ApiVacancy, Vacancy>();
            Mapper.CreateMap<WageUpdate, ApiWageUpdate>();

            Mapper.CreateMap<EmployerProviderSiteLink, VacancyOwnerRelationship>()
                .ForMember(dest => dest.VacancyOwnerRelationshipId, opt => opt.MapFrom(src => src.EmployerProviderSiteLinkId))
                .ForMember(dest => dest.VacancyOwnerRelationshipGuid, opt => opt.Ignore())
                .ForMember(dest => dest.StatusType, opt => opt.UseValue(VacancyOwnerRelationshipStatusTypes.Live));
        }
    }
}