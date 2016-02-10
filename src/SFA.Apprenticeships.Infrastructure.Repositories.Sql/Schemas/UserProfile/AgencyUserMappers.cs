namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile
{
    using AutoMapper;
    using Domain.Entities.Users;
    using Entities;
    using Vacancy;
    using AgencyUser = Domain.Entities.Users.AgencyUser;

    public class AgencyUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Role, AgencyUserRole>().ConvertUsing<DomainRoleToAgencyUserRoleConverter>();
            Mapper.CreateMap<AgencyUserRole, Role>().ConvertUsing<AgencyUserRoleToDomainRoleConverter>();
            Mapper.CreateMap<Team, AgencyUserTeam>().ConvertUsing<DomainTeamToAgencyUserTeamConverter>();
            Mapper.CreateMap<AgencyUserTeam, Team>().ConvertUsing<AgencyUserTeamToDomainTeamConverter>();

            Mapper.CreateMap<AgencyUser, Entities.AgencyUser>()
                .ForMember(destination => destination.AgencyUserId, opt => opt.MapFrom(source => source.EntityId))
                .ForMember(destination => destination.Role, opt => opt.MapFrom(source => source.Role))
                .ForMember(destination => destination.Team, opt => opt.MapFrom(source => source.Team));

            Mapper.CreateMap<Entities.AgencyUser, AgencyUser>()
                .ForMember(destination => destination.EntityId, opt => opt.MapFrom(source => source.AgencyUserId))
                .ForMember(destination => destination.Role, opt => opt.MapFrom(source => source.Role))
                .ForMember(destination => destination.Team, opt => opt.MapFrom(source => source.Team));
        }
    }

    public class DomainRoleToAgencyUserRoleConverter :
        ITypeConverter<Role, AgencyUserRole>
    {
        public AgencyUserRole Convert(ResolutionContext context)
        {
            var source = (Role)context.SourceValue;
            var result = new AgencyUserRole()
            {
                AgencyUserRoleId = int.Parse(source.Id),
                CodeName = source.CodeName,
                IsDefault = System.Convert.ToInt32(source.IsDefault),
                Name = source.Name
            };

            return result;
        }
    }

    public class AgencyUserRoleToDomainRoleConverter :
        ITypeConverter<AgencyUserRole, Role>
    {
        public Role Convert(ResolutionContext context)
        {
            var source = (AgencyUserRole)context.SourceValue;
            var result = new Role()
            {
                Id = source.AgencyUserRoleId.ToString(),
                CodeName = source.CodeName,
                IsDefault = System.Convert.ToBoolean(source.IsDefault),
                Name = source.Name
            };

            return result;
        }
    }

    public class DomainTeamToAgencyUserTeamConverter :
        ITypeConverter<Team, AgencyUserTeam>
    {
        public AgencyUserTeam Convert(ResolutionContext context)
        {
            var source = (Team)context.SourceValue;
            var result = new AgencyUserTeam()
            {
                AgencyUserTeamId = int.Parse(source.Id),
                CodeName = source.CodeName,
                IsDefault = System.Convert.ToInt32(source.IsDefault),
                Name = source.Name
            };

            return result;
        }
    }

    public class AgencyUserTeamToDomainTeamConverter :
        ITypeConverter<AgencyUserTeam, Team>
    {
        public Team Convert(ResolutionContext context)
        {
            var source = (AgencyUserTeam)context.SourceValue;
            var result = new Team()
            {
                Id = source.AgencyUserTeamId.ToString(),
                CodeName = source.CodeName,
                IsDefault = System.Convert.ToBoolean(source.IsDefault),
                Name = source.Name
            };

            return result;
        }
    }
}
