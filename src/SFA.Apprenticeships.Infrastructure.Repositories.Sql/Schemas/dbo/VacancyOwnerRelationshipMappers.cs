namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Domain.Entities.Providers;
    using Entities;
    using Vacancy;

    public class VacancyOwnerRelationshipMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ProviderSiteEmployerLink, VacancyOwnerRelationship>()
                .ForMember(vor => vor.StatusTypeId, opt => opt.UseValue(2)) //Active by default?
                .MapMemberFrom(vor => vor.EmployerId, psel => psel.Employer.EntityId)
                .MapMemberFrom(vor => vor.EmployerDescription, psel => psel.Description)
                .MapMemberFrom(vor => vor.EmployerWebsite, psel => psel.WebsiteUrl)
                .MapMemberFrom(vor => vor.VacancyOwnerRelationshipId, psel => psel.EntityId)
                .IgnoreMember(vor => vor.Notes)
                .IgnoreMember(vor => vor.ProviderSiteID) // DB lookup
                .IgnoreMember(vor => vor.ContractHolderIsEmployer)
                .IgnoreMember(vor => vor.ManagerIsEmployer)
                .IgnoreMember(vor => vor.NationwideAllowed)
                .IgnoreMember(vor => vor.EmployerLogoAttachmentId)
                ;

            Mapper.CreateMap<VacancyOwnerRelationship, ProviderSiteEmployerLink>()
                .MapMemberFrom(psel => psel.EntityId, vor => vor.VacancyOwnerRelationshipId)
                .MapMemberFrom(psel => psel.Description, vor => vor.EmployerDescription)
                .MapMemberFrom(psel => psel.WebsiteUrl, vor => vor.EmployerWebsite)
                .IgnoreMember(psel => psel.DateCreated)
                .IgnoreMember(psel => psel.Employer) // DB lookup
                .IgnoreMember(psel => psel.ProviderSiteErn) // DB lookup
                .IgnoreMember(psel => psel.DateUpdated)
                ;
        }
    }
}
