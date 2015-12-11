namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using System;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Common.Mappers;
    using Infrastructure.Presentation;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;

    public class RecruitMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<GeoPoint, GeoPointViewModel>();

            Mapper.CreateMap<ApprenticeshipVacancy, VacancyApplicationsViewModel>()
                .ForMember(v => v.EmployerName, opt => opt.MapFrom(src => src.ProviderSiteEmployerLink.Employer.Name))
                .ForMember(v => v.EmployerGeoPoint, opt => opt.MapFrom(src => Map<GeoPoint, GeoPointViewModel>(src.ProviderSiteEmployerLink.Employer.Address.GeoPoint)))
                .ForMember(v => v.RejectedApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.UnresolvedApplicationsCount, opt => opt.Ignore())
                .ForMember(v => v.ApplicationSummaries, opt => opt.Ignore());
            
            Mapper.CreateMap<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>()
                .ForMember(v => v.ApplicantName, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()))
                .ForMember(v => v.ApplicantLocation, opt => opt.MapFrom(src => src.CandidateDetails.Address.GetCityOrTownDisplayText()))
                .ForMember(v => v.ApplicantGeoPoint, opt => opt.MapFrom(src => Map<GeoPoint, GeoPointViewModel>(src.CandidateDetails.Address.GeoPoint)))
                .ForMember(v => v.Distance, opt => opt.Ignore());

            Mapper.CreateMap<ApprenticeshipVacancy, ApplicationVacancyViewModel>();

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApplicantDetailsViewModel>()
                .ForMember(v => v.Name, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()))
                .ForMember(v => v.Address, opt => opt.MapFrom(src => Map<Address, AddressViewModel>(src.CandidateDetails.Address)))
                .ForMember(v => v.DateOfBirth, opt => opt.MapFrom(src => src.CandidateDetails.DateOfBirth))
                .ForMember(v => v.PhoneNumber, opt => opt.MapFrom(src => src.CandidateDetails.PhoneNumber))
                .ForMember(v => v.EmailAddress, opt => opt.MapFrom(src => src.CandidateDetails.EmailAddress))
                .ForMember(v => v.Distance, opt => opt.Ignore())
                .ForMember(v => v.DisabilityStatus, opt => opt.MapFrom(src => src.CandidateInformation.DisabilityStatus));

            Mapper.CreateMap<ApprenticeshipApplicationDetail, VacancyQuestionAnswersViewModel>()
                .ForMember(v => v.FirstQuestionAnswer, opt => opt.MapFrom(src => src.AdditionalQuestion1Answer))
                .ForMember(v => v.SecondQuestionAnswer, opt => opt.MapFrom(src => src.AdditionalQuestion2Answer))
                .ForMember(v => v.AnythingWeCanDoToSupportYourInterviewAnswer, opt => opt.MapFrom(src => src.CandidateInformation.AboutYou.Support));

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>()
                .ForMember(v => v.Vacancy, opt => opt.Ignore())
                .ForMember(v => v.ApplicantDetails, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, ApplicantDetailsViewModel>(src)))
                .ForMember(v => v.VacancyQuestionAnswers, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, VacancyQuestionAnswersViewModel>(src)));
        }
    }
}