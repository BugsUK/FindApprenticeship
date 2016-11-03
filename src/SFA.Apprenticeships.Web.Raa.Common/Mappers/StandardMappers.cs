namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using ViewModels.Admin;
    using ViewModels.Provider;

    public class StandardMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<StandardSubjectAreaTierOne, StandardViewModel>()
                .ForMember(dest => dest.ApprenticeshipSectors, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipLevels, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}