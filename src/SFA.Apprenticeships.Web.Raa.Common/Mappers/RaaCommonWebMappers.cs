namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Users;
    using Infrastructure.Common.Mappers;
    using Infrastructure.Presentation;
    using Resolvers;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;
    using Web.Common.ViewModels.Locations;

    public class RaaCommonWebMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Domain.Entities.Raa.Locations.GeoPoint, GeoPointViewModel>();
            Mapper.CreateMap<PostalAddress, AddressViewModel>()
                .ForMember(dest => dest.Uprn, opt => opt.Ignore());
            Mapper.CreateMap<AddressViewModel, PostalAddress>()
                .ForMember(dest => dest.PostalAddressId, opt => opt.Ignore())
                .ForMember(dest => dest.AddressLine5, opt => opt.Ignore())
                .ForMember(dest => dest.Town, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationSourceCode, opt => opt.Ignore())
                .ForMember(dest => dest.ValidationSourceKeyValue, opt => opt.Ignore())
                .ForMember(dest => dest.DateValidated, opt => opt.Ignore())
                .ForMember(dest => dest.County, opt => opt.Ignore())
                .ForMember(dest => dest.GeoPoint, opt => opt.Ignore());
            Mapper.CreateMap<Employer, EmployerViewModel>();
            Mapper.CreateMap<VacancyParty, VacancyPartyViewModel>()
                .ForMember(dest => dest.IsEmployerLocationMainApprenticeshipLocation, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositions, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerDescriptionComment, opt => opt.Ignore())
                .ForMember(dest => dest.EmployerWebsiteUrlComment, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfPositionsComment, opt => opt.Ignore())
                .ForMember(dest => dest.Employer, opt => opt.Ignore());
            Mapper.CreateMap<VacancyLocation, VacancyLocationAddressViewModel>();
            Mapper.CreateMap<VacancyLocationAddressViewModel, VacancyLocation>()
                .ForMember(dest => dest.VacancyId, opt => opt.Ignore())
                .ForMember(dest => dest.VacancyLocationId, opt => opt.Ignore());

            Mapper.CreateMap<DateTime?, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();
            Mapper.CreateMap<DateTime, DateViewModel>().ConvertUsing<DateTimeToDateViewModelConverter>();

            Mapper.CreateMap<Vacancy, VacancyDatesViewModel>().ConvertUsing<VacancyToVacancyDatesViewModelConverter>();

            Mapper.CreateMap<Vacancy, NewVacancyViewModel>()
                .ForMember(dest => dest.VacancyGuid, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore())
                .ForMember(dest => dest.Ukprn, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerParty, opt => opt.Ignore())
                .ForMember(dest => dest.LocationAddresses, opt => opt.Ignore());

            Mapper.CreateMap<Vacancy, TrainingDetailsViewModel>()
                .ForMember(dest => dest.SectorsAndFrameworks, opt => opt.Ignore())
                .ForMember(dest => dest.Standards, opt => opt.Ignore())
                .ForMember(dest => dest.Sectors, opt => opt.Ignore())
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());

            Mapper.CreateMap<Vacancy, FurtherVacancyDetailsViewModel>().ConvertUsing<VacancyToFurtherVacancyDetailsViewModelConverter>();

            Mapper.CreateMap<Vacancy, VacancyRequirementsProspectsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<Vacancy, VacancyQuestionsViewModel>()
                .ForMember(dest => dest.ComeFromPreview, opt => opt.Ignore());
            Mapper.CreateMap<Vacancy, VacancyViewModel>().ConvertUsing<VacancyToVacancyViewModelConverter>();
            Mapper.CreateMap<Vacancy, VacancySummaryViewModel>().ConvertUsing<VacancyToVacancySummaryViewModelConverter>();

            //Applications
            Mapper.CreateMap<Address, AddressViewModel>();
            Mapper.CreateMap<Domain.Entities.Locations.GeoPoint, GeoPointViewModel>();

            Mapper.CreateMap<Vacancy, ApplicationVacancyViewModel>()
                .ForMember(v => v.EmployerName, opt => opt.Ignore());

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
                .ForMember(v => v.ApplicationSelection, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, ApplicationSelectionViewModel>(src)))
                .ForMember(v => v.Vacancy, opt => opt.Ignore())
                .ForMember(v => v.ApplicantDetails, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, ApplicantDetailsViewModel>(src)))
                .ForMember(v => v.AboutYou, opt => opt.MapFrom(src => Map<AboutYou, AboutYouViewModel>(src.CandidateInformation.AboutYou)))
                .ForMember(v => v.Education, opt => opt.MapFrom(src => Map<Education, EducationViewModel>(src.CandidateInformation.EducationHistory)))
                .ForMember(v => v.Qualifications, opt => opt.MapFrom(src => Map<IList<Qualification>, IList<QualificationViewModel>>(src.CandidateInformation.Qualifications)))
                .ForMember(v => v.WorkExperience, opt => opt.MapFrom(src => Map<IList<WorkExperience>, IList<WorkExperienceViewModel>>(src.CandidateInformation.WorkExperience)))
                .ForMember(v => v.TrainingCourses, opt => opt.MapFrom(src => Map<IList<TrainingCourse>, IList<TrainingCourseViewModel>>(src.CandidateInformation.TrainingCourses)))
                .ForMember(v => v.VacancyQuestionAnswers, opt => opt.MapFrom(src => Map<ApprenticeshipApplicationDetail, VacancyQuestionAnswersViewModel>(src)));

            Mapper.CreateMap<ApprenticeshipApplicationDetail, ApplicationSelectionViewModel>()
                .ForMember(v => v.ApplicationId, opt => opt.MapFrom(src => src.EntityId))
                .ForMember(v => v.VacancyReferenceNumber, opt => opt.Ignore())
                .ForMember(v => v.FilterType, opt => opt.Ignore())
                .ForMember(v => v.PageSize, opt => opt.Ignore())
                .ForMember(v => v.PageSizes, opt => opt.Ignore())
                .ForMember(v => v.CurrentPage, opt => opt.Ignore());
        }
    }
}