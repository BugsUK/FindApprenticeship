namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using AutoMapper;
    using Domain.Entities.Users;
    using Vacancy;

    public class ProviderUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            // TODO: AG: remove the Guid / int mappers below?
            Mapper.CreateMap<Guid, int>().ConvertUsing<GuidToIntConverter>();
            Mapper.CreateMap<int, Guid>().ConvertUsing<IntToGuidConverter>();

            // ProviderUser -> Entities.ProviderUser
            Mapper.CreateMap<ProviderUser, Entities.ProviderUser>()
                .ForMember(destination => destination.ProviderUserId, opt =>
                    opt.MapFrom(source => source.EntityId))

                .ForMember(destination => destination.ProviderUserStatusId, opt =>
                    opt.MapFrom(source => (int)source.Status))

                .ForMember(destination => destination.EmailVerifiedDateTime, opt =>
                    opt.MapFrom(source => source.EmailVerifiedDate));

            // Entities.ProviderUser -> ProviderUser
            Mapper.CreateMap<Entities.ProviderUser, ProviderUser>()
                .ForMember(destination => destination.EntityId, opt =>
                    opt.MapFrom(source => source.ProviderId))

                .ForMember(destination => destination.Status, opt =>
                    opt.ResolveUsing<ProviderUserStatusResolver>()
                        .FromMember(source => source.ProviderUserStatusId))

                .ForMember(destination => destination.EmailVerifiedDate, opt =>
                    opt.MapFrom(source => source.EmailVerifiedDateTime));
        }
    }

    public class ProviderUserStatusResolver : ValueResolver<int, ProviderUserStatuses>
    {
        protected override ProviderUserStatuses ResolveCore(int providerUserStatusId)
        {
            switch (providerUserStatusId)
            {
                case 10:
                    return ProviderUserStatuses.Registered;
                case 20:
                    return ProviderUserStatuses.EmailVerified;
            }

            throw new ArgumentOutOfRangeException(
                nameof(providerUserStatusId),
                $"Unknown Provider User Status: {providerUserStatusId}");
        }
    }
}
