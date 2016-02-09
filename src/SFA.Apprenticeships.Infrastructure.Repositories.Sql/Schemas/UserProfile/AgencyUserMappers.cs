namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using Domain.Entities.Users;
    using Vacancy;
    public class AgencyUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<AgencyUser, Entities.AgencyUser>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(source => source.EntityId));
        }
    }
}
