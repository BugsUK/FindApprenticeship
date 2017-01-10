namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using AutoMapper;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using DomainEmployer = Domain.Entities.Raa.Parties.Employer;
    using Employer = Entities.Employer;
    using DomainPostalAddress = Domain.Entities.Raa.Locations.PostalAddress;

    public class EmployerMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Employer, DomainEmployer>()
                .ForMember(v => v.EmployerGuid, opt => opt.Ignore())
                .ForMember(v => v.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(v => v.TradingName, opt => opt.MapFrom(src => src.TradingName))
                .ForMember(v => v.Address, opt => opt.Ignore())
                .ForMember(v => v.IsPositiveAboutDisability, opt => opt.MapFrom(src => src.DisableAllowed))
                .ForMember(v => v.EmployerStatus, opt => opt.MapFrom(src => (EmployerTrainingProviderStatuses)src.EmployerStatusTypeId))
                .AfterMap((v, av) =>
                {
                    av.Address = new DomainPostalAddress
                    {
                        AddressLine1 = v.AddressLine1,
                        AddressLine2 = v.AddressLine2,
                        AddressLine3 = v.AddressLine3,
                        AddressLine4 = v.AddressLine4,
                        AddressLine5 = v.AddressLine5,
                        Postcode = v.PostCode,
                        Town = v.Town,
                        CountyId = v.CountyId,
                        County = v.County,
                        LocalAuthorityId = v.LocalAuthorityId ?? 0,
                        LocalAuthorityCodeName = v.LocalAuthorityCodeName,
                        LocalAuthority = v.LocalAuthority,
                        GeoPoint = new GeoPoint
                        {
                            Latitude = (double) (v.Latitude ?? 0),
                            Longitude = (double) (v.Longitude ?? 0),
                            Easting = v.GeocodeEasting ?? 0,
                            Northing = v.GeocodeNorthing ?? 0
                        }
                    };
                });

            Mapper.CreateMap<DomainEmployer, Employer>()
                .ForMember(v => v.EdsUrn, opt => opt.MapFrom(src => Convert.ToInt32(src.EdsUrn)))
                .ForMember(v => v.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(v => v.TradingName, opt => opt.MapFrom(src => src.TradingName))
                .ForMember(v => v.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(v => v.AddressLine2, opt => opt.MapFrom(src => src.Address.AddressLine2))
                .ForMember(v => v.AddressLine3, opt => opt.MapFrom(src => src.Address.AddressLine3))
                .ForMember(v => v.AddressLine4, opt => opt.MapFrom(src => src.Address.AddressLine4))
                .ForMember(v => v.AddressLine5, opt => opt.MapFrom(src => src.Address.AddressLine5))
                .ForMember(v => v.PostCode, opt => opt.MapFrom(src => src.Address.Postcode))
                .ForMember(v => v.Town, opt => opt.MapFrom(src => src.Address.Town))
                .ForMember(v => v.Latitude, opt => opt.ResolveUsing<GeocodeToLatitudeConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.Longitude, opt => opt.ResolveUsing<GeocodeToLongitudeConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.CountyId, opt => opt.MapFrom(src => src.Address.CountyId))
                .ForMember(v => v.County, opt => opt.MapFrom(src => src.Address.County))
                .ForMember(v => v.LocalAuthorityId, opt => opt.MapFrom(src => src.Address.LocalAuthorityId))
                .ForMember(v => v.LocalAuthorityCodeName, opt => opt.MapFrom(src => src.Address.LocalAuthorityCodeName))
                .ForMember(v => v.LocalAuthority, opt => opt.MapFrom(src => src.Address.LocalAuthority))
                .ForMember(v => v.GeocodeEasting, opt => opt.ResolveUsing<GeocodeToEastingConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.GeocodeNorthing, opt => opt.ResolveUsing<GeocodeToNorthingConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.NumberofEmployeesAtSite, opt => opt.Ignore())
                .ForMember(v => v.NumberOfEmployeesInGroup, opt => opt.Ignore())
                .ForMember(v => v.OwnerOrgnistaion, opt => opt.Ignore())
                .ForMember(v => v.CompanyRegistrationNumber, opt => opt.Ignore())
                .ForMember(v => v.TotalVacanciesPosted, opt => opt.Ignore())
                .ForMember(v => v.BeingSupportedBy, opt => opt.Ignore())
                .ForMember(v => v.LockedForSupportUntil, opt => opt.Ignore())
                .ForMember(v => v.EmployerStatusTypeId, opt => opt.MapFrom(src => (int)src.EmployerStatus))
                .ForMember(v => v.DisableAllowed, opt => opt.MapFrom(src => src.IsPositiveAboutDisability))
                .ForMember(v => v.TrackingAllowed, opt => opt.Ignore());

            Mapper.CreateMap<VerifiedOrganisationSummary, DomainEmployer>()
                .ForMember(dest => dest.EmployerId, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerGuid, opt => opt.Ignore())
                .ForMember(dest => dest.EdsUrn, opt => opt.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.PrimaryContact, opt => opt.UseValue(Constants.UnspecifiedEmployerContact))
                .ForMember(dest => dest.IsPositiveAboutDisability, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerStatus, opt => opt.UseValue(EmployerTrainingProviderStatuses.Activated));
        }
    }

    public class GeocodeToLatitudeConverter : ValueResolver<GeoPoint, decimal>
    {
        protected override decimal ResolveCore(GeoPoint source)
        {
            if (source == null)
            {
                return 0;
            }

            return (decimal)source.Latitude;
        }
    }

    public class GeocodeToLongitudeConverter : ValueResolver<GeoPoint, decimal>
    {
        protected override decimal ResolveCore(GeoPoint source)
        {
            if (source == null)
            {
                return 0;
            }

            return (decimal)source.Longitude;
        }
    }

    public class GeocodeToEastingConverter : ValueResolver<GeoPoint, int>
    {
        protected override int ResolveCore(GeoPoint source)
        {
            if (source == null)
            {
                return 0;
            }

            return source.Easting;
        }
    }

    public class GeocodeToNorthingConverter : ValueResolver<GeoPoint, int>
    {
        protected override int ResolveCore(GeoPoint source)
        {
            if (source == null)
            {
                return 0;
            }

            return source.Northing;
        }
    }
}
