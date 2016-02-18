namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Domain.Entities.Providers;
    using Vacancy;

    public class ProviderSiteMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ProviderSite, Entities.ProviderSite>();

            Mapper.CreateMap<Entities.ProviderSite, ProviderSite>();
        }
    }
}
