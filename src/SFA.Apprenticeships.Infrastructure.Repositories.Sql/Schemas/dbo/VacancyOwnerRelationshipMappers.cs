namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Domain.Entities.Providers;
    using Entities;
    using Vacancy;

    public class VacancyOwnerRelationshipMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ProviderSiteEmployerLink, VacancyOwnerRelationship>();
            //{
            //    EmployerId = employer.EmployerId, //TODO: SQL: Once EmployerRepo is done, uncomment
            //    //ContractHolderIsEmployer = ,  TODO: SQL: What is this?
            //    EmployerDescription = providerSiteEmployerLink.Description,
            //    EmployerLogoAttachmentId = null,
            //    EmployerWebsite = providerSiteEmployerLink.WebsiteUrl,
            //    //ManagerIsEmployer = , TODO: SQL: What is this?
            //    //NationwideAllowed = , TODO: SQL: What is this?
            //    //Notes = , TODO: SQL: What is this?
            //    ProviderSiteID = provider.ProviderSiteId
            //    //StatusTypeId = TODO: SQL: What is this?
            //};


            Mapper.CreateMap<VacancyOwnerRelationship, ProviderSiteEmployerLink>();
        }
    }
}
