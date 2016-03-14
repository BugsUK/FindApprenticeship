namespace SFA.Apprenticeships.Application.Employer.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;

    public class EmployerMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VerifiedOrganisationSummary, Employer>()
                .ForMember(dest => dest.EmployerId, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerGuid, opt => opt.Ignore())
                .ForMember(dest => dest.EdsUrn, opt => opt.MapFrom(src => src.ReferenceNumber));
        }
    }
}