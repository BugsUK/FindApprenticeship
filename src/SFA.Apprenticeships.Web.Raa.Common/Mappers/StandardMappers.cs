namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Common.Mappers;
    using ViewModels.Admin;

    public class StandardMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<StandardSubjectAreaTierOne, StandardViewModel>()
                .ForMember(dest => dest.ApprenticeshipSectors, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipLevels, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}