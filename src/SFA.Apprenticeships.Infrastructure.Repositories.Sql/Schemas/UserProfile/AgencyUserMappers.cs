namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using AutoMapper;
    using Domain.Entities.Raa.Users;
    using Infrastructure.Common.Mappers;
    using AgencyUser = Domain.Entities.Raa.Users.AgencyUser;

    public class AgencyUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<AgencyUser, Entities.AgencyUser>()
                .ForMember(u => u.RoleId, opt => opt.MapFrom(s => s.Role.Id));

            Mapper.CreateMap<Entities.AgencyUser, AgencyUser>()
                .ForMember(u => u.Role, opt => opt.ResolveUsing<RoleResolver>().FromMember(src => src.RoleId))
                .ForMember(u => u.Team, opt => opt.Ignore());
        }
    }

    public class RoleResolver : ValueResolver<string, Role>
    {
        protected override Role ResolveCore(string source)
        {
            return new Role {Id = source};
        }
    }
}
