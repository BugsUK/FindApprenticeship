namespace SFA.Apprenticeship.Api.AvService.Mappers.Version51
{
    using System;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using DataContracts.Version51;

    // See Capgemini.LSC.Navms.MS.DataAccess.VacancyController for more default mappings.

    public class ApprenticeshipVacancyMapper : IApprenticeshipVacancyMapper
    {
        private const string Todo = "TODO";

        private readonly IAddressMapper _addressMapper;
        private readonly IVacancyDurationMapper _vacancyDurationMapper;

        public ApprenticeshipVacancyMapper(
            IAddressMapper addressMapper,
            IVacancyDurationMapper vacancyDurationMapper)
        {
            _addressMapper = addressMapper;
            _vacancyDurationMapper = vacancyDurationMapper;
        }

        public VacancyFullData MapToVacancyFullData(ApprenticeshipVacancy vacancy)
        {
            // TODO: API: review optional object properties here. Throw if null or allow null?
            return new VacancyFullData
            {
                VacancyLocationType = Todo,
                // TODO: map VacancyAddress
                VacancyAddress =  _addressMapper.MapToAddressData(vacancy.ProviderSiteEmployerLink?.Employer?.Address),
                ApprenticeshipFramework = $"{Todo}: via Framework Code {vacancy.FrameworkCodeName}",
                // TODO: what if date null?
                ClosingDate = vacancy.ClosingDate ?? DateTime.MinValue,
                ShortDescription = vacancy.ShortDescription,
                // TODO: what if ProviderSiteEmployerLink null?
                EmployerName = vacancy.ProviderSiteEmployerLink?.Employer?.Name,
                LearningProviderName = $"{Todo}: via UKPRN {vacancy.Ukprn}",
                // TODO: map number of positions when it is available ApprenticeshipVacancy.
                NumberOfPositions = 1,
                VacancyTitle = vacancy.Title,
                CreatedDateTime = vacancy.DateCreated,
                // TODO: longs versus int32s an issue for v6.0?
                VacancyReference = Convert.ToInt32(vacancy.VacancyReferenceNumber),
                // TODO: should map from vacancy.ApprenticeshipLevel.
                VacancyType = Todo,
                VacancyUrl = Todo,
                FullDescription = vacancy.LongDescription,
                SupplementaryQuestion1 = vacancy.FirstQuestion,
                SupplementaryQuestion2 = vacancy.SecondQuestion,
                ContactPerson = Todo,
                EmployerDescription = vacancy.ProviderSiteEmployerLink?.Description,
                ExpectedDuration = _vacancyDurationMapper.MapDurationToString(vacancy.Duration, vacancy.DurationType),
                FutureProspects = vacancy.FutureProspects,
                InterviewFromDate = DateTime.MinValue,
                LearningProviderDesc = $"{Todo}: via UKPRN {vacancy.Ukprn}",
                LearningProviderSectorPassRate = null,
                PersonalQualities = vacancy.PersonalQualities,
                // TODO: what if date null?
                PossibleStartDate = vacancy.PossibleStartDate ?? DateTime.MinValue,
                QualificationRequired = vacancy.DesiredQualifications,
                SkillsRequired = vacancy.DesiredSkills,
                TrainingToBeProvided = null,
                // WageType = WageType.Weekly, // WageType
                // Wage = -1m, // Wage
                // WageText = "TODO", // TODO: Wage + WageUnit + ?
                // WorkingWeek = "TODO", // TODO: WorkingWeek + HoursPerWeek?
                OtherImportantInformation = Todo,
                // TODO: what if ProviderSiteEmployerLink null?
                EmployerWebsite = vacancy.ProviderSiteEmployerLink?.WebsiteUrl,
                VacancyOwner = Todo,
                ContractOwner = Todo,
                DeliveryOrganisation = Todo,
                VacancyManager = Todo,
                IsDisplayRecruitmentAgency = null,
                IsSmallEmployerWageIncentive = false
            };
        }
    }
}
