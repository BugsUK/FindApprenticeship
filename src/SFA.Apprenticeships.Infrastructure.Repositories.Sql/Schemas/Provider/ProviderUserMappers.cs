namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using AutoMapper;
    using Infrastructure.Common.Mappers;

    public class ProviderUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            // ProviderUser -> Entities.ProviderUser
            Mapper.CreateMap<Domain.Entities.Raa.Users.ProviderUser, Entities.ProviderUser>()
                //.ForMember(destination => destination.PreferredProviderSiteId, opt =>
                //    opt.MapFrom(source => source.PreferredProviderSiteId))

                .ForMember(destination => destination.ProviderUserStatusId, opt =>
                    opt.MapFrom(source => (int)source.Status))

                .ForMember(destination => destination.EmailVerifiedDateTime, opt =>
                    opt.MapFrom(source => source.EmailVerifiedDate));

            // Entities.ProviderUser -> ProviderUser
            Mapper.CreateMap<Entities.ProviderUser, Domain.Entities.Raa.Users.ProviderUser>()
                /*
                .ForMember(destination => destination.Ukprn, opt =>
                    opt.MapFrom(source => Convert.ToString(source.Ukprn)))
                */

                .ForMember(destination => destination.Status, opt =>
                    opt.ResolveUsing<ProviderUserStatusResolver>()
                        .FromMember(source => source.ProviderUserStatusId))

                .ForMember(destination => destination.EmailVerifiedDate, opt =>
                    opt.MapFrom(source => source.EmailVerifiedDateTime));
        }
    }

    public class ProviderUserStatusResolver : ValueResolver<int, Domain.Entities.Raa.Users.ProviderUserStatus>
    {
        protected override Domain.Entities.Raa.Users.ProviderUserStatus ResolveCore(int providerUserStatusId)
        {
            switch (providerUserStatusId)
            {
                case 10:
                    return Domain.Entities.Raa.Users.ProviderUserStatus.Registered;
                case 20:
                    return Domain.Entities.Raa.Users.ProviderUserStatus.EmailVerified;
            }

            throw new ArgumentOutOfRangeException(
                nameof(providerUserStatusId),
                $"Unknown Provider User Status: {providerUserStatusId}");
        }
    }
}