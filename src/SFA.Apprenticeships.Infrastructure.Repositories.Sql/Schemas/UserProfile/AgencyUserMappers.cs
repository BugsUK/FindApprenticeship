namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using Vacancy;
    using AgencyUser = Domain.Entities.Raa.Users.AgencyUser;

    public class AgencyUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<AgencyUser, Entities.AgencyUser>();

            Mapper.CreateMap<Entities.AgencyUser, AgencyUser>();
        }
    }
}
