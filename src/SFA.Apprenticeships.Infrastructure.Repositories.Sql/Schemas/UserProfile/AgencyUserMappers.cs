namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using Vacancy;
    using AgencyUser = Domain.Entities.Users.AgencyUser;

    public class AgencyUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<AgencyUser, Entities.AgencyUser>()
                .ForMember(destination => destination.AgencyUserId, opt => opt.MapFrom(source => source.EntityId));

            Mapper.CreateMap<Entities.AgencyUser, AgencyUser>()
                .ForMember(destination => destination.EntityId, opt => opt.MapFrom(source => source.AgencyUserId));
        }
    }
}
