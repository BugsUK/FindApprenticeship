﻿namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Users;
    using Infrastructure.Presentation;
    using Raa.Common.Mappers;
    using Raa.Common.ViewModels.Application;

    using Domain.Entities.Communication;
    using ViewModels.Home;

    public class RecruitMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<Vacancy, VacancyApplicationsViewModel>()
                .ForMember(v => v.VacancyApplicationsSearch, opt => opt.Ignore())
                .ForMember(v => v.EmployerName, opt => opt.Ignore())
                .ForMember(v => v.EmployerGeoPoint, opt => opt.Ignore())
                .ForMember(v => v.NewApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.ViewedApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.SuccessfulApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.UnsuccessfulApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.ApplicationSummaries, opt => opt.Ignore());
            
            Mapper.CreateMap<ApplicationSummary, ApplicationSummaryViewModel>()
                .ForMember(v => v.ApplicantName, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()));

            Mapper.CreateMap<ContactMessageViewModel, ProviderContactMessage>()
                .ForMember(c => c.Type, opt => opt.UseValue(ContactMessageTypes.ContactUs))
                .ForMember(c => c.EntityId, opt => opt.Ignore())
                .ForMember(c => c.UserId, opt => opt.Ignore())
                .ForMember(c => c.DateCreated, opt => opt.Ignore())
                .ForMember(c => c.DateUpdated, opt => opt.Ignore());
        }
    }
}