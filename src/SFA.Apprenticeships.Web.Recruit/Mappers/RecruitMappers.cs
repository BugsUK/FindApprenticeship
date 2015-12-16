namespace SFA.Apprenticeships.Web.Recruit.Mappers
{
    using System;
    using System.Collections.Generic;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
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
                .ForMember(v => v.ApplicantName, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()));

            Mapper.CreateMap<ApprenticeshipVacancy, ApplicationVacancyViewModel>();

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApplicantDetailsViewModel>()
                .ForMember(v => v.Name, opt => opt.MapFrom(src => new Name(src.CandidateDetails.FirstName, src.CandidateDetails.MiddleNames, src.CandidateDetails.LastName).GetDisplayText()))
                .ForMember(v => v.Address, opt => opt.MapFrom(src => Map<Address, AddressViewModel>(src.CandidateDetails.Address)))
                .ForMember(v => v.DateOfBirth, opt => opt.MapFrom(src => src.CandidateDetails.DateOfBirth))
                .ForMember(v => v.PhoneNumber, opt => opt.MapFrom(src => src.CandidateDetails.PhoneNumber))
                .ForMember(v => v.EmailAddress, opt => opt.MapFrom(src => src.CandidateDetails.EmailAddress))
                .ForMember(v => v.DisabilityStatus, opt => opt.MapFrom(src => src.CandidateInformation.DisabilityStatus));

            Mapper.CreateMap<AboutYou, AboutYouViewModel>();
            Mapper.CreateMap<Education, EducationViewModel>();
            Mapper.CreateMap<Qualification, QualificationViewModel>();
            Mapper.CreateMap<WorkExperience, WorkExperienceViewModel>();
            Mapper.CreateMap<TrainingCourse, TrainingCourseViewModel>();

            Mapper.CreateMap<ApprenticeshipApplicationDetail, VacancyQuestionAnswersViewModel>()
                .ForMember(v => v.FirstQuestionAnswer, opt => opt.MapFrom(src => src.AdditionalQuestion1Answer))
                .ForMember(v => v.SecondQuestionAnswer, opt => opt.MapFrom(src => src.AdditionalQuestion2Answer))
                .ForMember(v => v.AnythingWeCanDoToSupportYourInterviewAnswer, opt => opt.MapFrom(src => src.CandidateInformation.AboutYou.Support));

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>()
                .ForMember(v => v.Vacancy, opt => opt.Ignore())
                .ForMember(v => v.ApplicantDetails, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, ApplicantDetailsViewModel>(src)))
                .ForMember(v => v.AboutYou, opt => opt.MapFrom(src => Map<AboutYou, AboutYouViewModel>(src.CandidateInformation.AboutYou)))
                .ForMember(v => v.Education, opt => opt.MapFrom(src => Map<Education, EducationViewModel>(src.CandidateInformation.EducationHistory)))
                .ForMember(v => v.Qualifications, opt => opt.MapFrom(src => Map<IList<Qualification>, IList<QualificationViewModel>>(src.CandidateInformation.Qualifications)))
                .ForMember(v => v.WorkExperience, opt => opt.MapFrom(src => Map<IList<WorkExperience>, IList<WorkExperienceViewModel>>(src.CandidateInformation.WorkExperience)))
                .ForMember(v => v.TrainingCourses, opt => opt.MapFrom(src => Map<IList<TrainingCourse>, IList<TrainingCourseViewModel>>(src.CandidateInformation.TrainingCourses)))
                .ForMember(v => v.VacancyQuestionAnswers, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, VacancyQuestionAnswersViewModel>(src)));
        }
    }
}