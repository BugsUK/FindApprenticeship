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
                .ForMember(v => v.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(v => v.Address, opt => opt.Ignore())
                .ForMember(v => v.IsPositiveAboutDisability, opt => opt.MapFrom(src => src.DisableAllowed))
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
                        Town = v.Town
                    };

                    if (v.Latitude.HasValue && v.Longitude.HasValue)
                    {
                        av.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint
                        {
                            Latitude = (double)v.Latitude.Value,
                            Longitude = (double)v.Longitude.Value
                        };
                    }
                });

            Mapper.CreateMap<DomainEmployer, Employer>()
                .ForMember(v => v.EdsUrn, opt => opt.MapFrom(src => Convert.ToInt32(src.EdsUrn)))
                .ForMember(v => v.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(v => v.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(v => v.AddressLine2, opt => opt.MapFrom(src => src.Address.AddressLine2))
                .ForMember(v => v.AddressLine3, opt => opt.MapFrom(src => src.Address.AddressLine3))
                .ForMember(v => v.AddressLine4, opt => opt.MapFrom(src => src.Address.AddressLine4))
                .ForMember(v => v.AddressLine5, opt => opt.MapFrom(src => src.Address.AddressLine5))
                .ForMember(v => v.PostCode, opt => opt.MapFrom(src => src.Address.Postcode))
                .ForMember(v => v.Town, opt => opt.MapFrom(src => src.Address.Town))
                .ForMember(v => v.Latitude, opt => opt.ResolveUsing<GeocodeToLatitudeConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.Longitude, opt => opt.ResolveUsing<GeocodeToLongitudeConverter>().FromMember(src => src.Address.GeoPoint))
                .ForMember(v => v.CountyId, opt => opt.Ignore())
                .ForMember(v => v.LocalAuthorityId, opt => opt.Ignore())
                .ForMember(v => v.GeocodeEasting, opt => opt.Ignore())
                .ForMember(v => v.GeocodeNorthing, opt => opt.Ignore())
                .ForMember(v => v.NumberofEmployeesAtSite, opt => opt.Ignore())
                .ForMember(v => v.NumberOfEmployeesInGroup, opt => opt.Ignore())
                .ForMember(v => v.OwnerOrgnistaion, opt => opt.Ignore())
                .ForMember(v => v.CompanyRegistrationNumber, opt => opt.Ignore())
                .ForMember(v => v.TotalVacanciesPosted, opt => opt.Ignore())
                .ForMember(v => v.BeingSupportedBy, opt => opt.Ignore())
                .ForMember(v => v.LockedForSupportUntil, opt => opt.Ignore())
                .ForMember(v => v.EmployerStatusTypeId, opt => opt.Ignore())
                .ForMember(v => v.DisableAllowed, opt => opt.MapFrom((src => src.IsPositiveAboutDisability)))
                .ForMember(v => v.TrackingAllowed, opt => opt.Ignore());

            Mapper.CreateMap<VerifiedOrganisationSummary, DomainEmployer>()
                .ForMember(dest => dest.EmployerId, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerGuid, opt => opt.Ignore())
                .ForMember(dest => dest.EdsUrn, opt => opt.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.PrimaryContact, opt => opt.UseValue(Constants.UnspecifiedEmployerContact))
                .ForMember(dest => dest.IsPositiveAboutDisability, opt => opt.Ignore());
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
}
