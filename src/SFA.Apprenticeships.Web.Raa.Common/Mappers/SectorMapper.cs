namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Common.Mappers;
    using ViewModels.Admin;

    public class SectorMapper : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<SectorViewModel, Sector>()
                .ForMember(dest => dest.Standards, opt => opt.Ignore());
            Mapper.CreateMap<Sector, SectorViewModel>()
                .ForMember(dest => dest.ApprenticeshipOccupations, opt => opt.Ignore());
        }
    }
}