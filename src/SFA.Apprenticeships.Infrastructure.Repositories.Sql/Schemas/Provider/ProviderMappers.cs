namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using AutoMapper;
    using Vacancy;
    using Domain = Domain.Entities.Raa.Users;
    using Database = Entities;

    // TODO: SQL: AG: need to understand MapperEngine usage here (and in AgencyUserMappers).
    public class ProviderUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            // ProviderUser -> Entities.ProviderUser
            Mapper.CreateMap<Domain.ProviderUser, Database.ProviderUser>()
                //.ForMember(destination => destination.PreferredProviderSiteId, opt =>
                //    opt.MapFrom(source => source.PreferredProviderSiteId))

                .ForMember(destination => destination.ProviderUserStatusId, opt =>
                    opt.MapFrom(source => (int)source.Status))

                .ForMember(destination => destination.EmailVerifiedDateTime, opt =>
                    opt.MapFrom(source => source.EmailVerifiedDate));

            // Entities.ProviderUser -> ProviderUser
            Mapper.CreateMap<Database.ProviderUser, Domain.ProviderUser>()
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

    public class ProviderUserStatusResolver : ValueResolver<int, Domain.ProviderUserStatus>
    {
        protected override Domain.ProviderUserStatus ResolveCore(int providerUserStatusId)
        {
            switch (providerUserStatusId)
            {
                case 10:
                    return Domain.ProviderUserStatus.Registered;
                case 20:
                    return Domain.ProviderUserStatus.EmailVerified;
            }

            throw new ArgumentOutOfRangeException(
                nameof(providerUserStatusId),
                $"Unknown Provider User Status: {providerUserStatusId}");
        }
    }
}