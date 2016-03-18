﻿namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels.Locations;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Infrastructure.Presentation;
    using ApprenticeshipLevel = Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLevel;
    using WageUnit = Domain.Entities.Vacancies.WageUnit;

    //TODO move type specific properties into sub classes, remove unused properties
    [Serializable]
    public abstract class VacancyDetailViewModel : ViewModelBase
    {
        protected VacancyDetailViewModel()
        {
        }

        protected VacancyDetailViewModel(string message) : base(message)
        {
        }

        #region Controls

        public string Distance { get; set; }

        #endregion

        #region Vacancy

        public int Id { get; set; }

        public VacancyStatuses VacancyStatus { get; set; }

        public string VacancyReference { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int NumberOfPositions { get; set; }

        public string FullDescription { get; set; }

        public string SubCategory { get; set; }

        public string VacancyType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime ClosingDate { get; set; }

        public LegacyWageType WageType { get; set; }

        public string Wage { get; set; }

        public WageUnit WageUnit { get; set; }

        public string WorkingWeek { get; set; }

        #endregion

        #region Employer

        public string EmployerName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public bool IsWellFormedEmployerWebsiteUrl { get; set; }

        public string ExpectedDuration { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        public AddressViewModel VacancyAddress { get; set; }

        public string SupplementaryQuestion1 { get; set; }

        public string SupplementaryQuestion2 { get; set; }

        public bool IsRecruitmentAgencyAnonymous { get; set; }

        public string RecruitmentAgency { get; set; }

        public bool IsEmployerAnonymous { get; set; }

        public bool IsPositiveAboutDisability { get; set; }

        #endregion

        #region Provider

        public string ProviderName { get; set; }

        public string ProviderDescription { get; set; }

        public string Contact { get; set; }

        public int? ProviderSectorPassRate { get; set; }

        public string TrainingToBeProvided { get; set; }

        public bool IsNasProvider { get; set; }

        #endregion

        #region Candidate

        public string PersonalQualities { get; set; }

        public string QualificationRequired { get; set; }

        public string SkillsRequired { get; set; }

        public string FutureProspects { get; set; }

        public string RealityCheck { get; set; }

        public string OtherInformation { get; set; }

        public bool ApplyViaEmployerWebsite { get; set; }

        public string ApplicationInstructions { get; set; }

        public bool IsWellFormedVacancyUrl { get; set; }

        public string VacancyUrl { get; set; }

        public ApplicationStatuses? CandidateApplicationStatus { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime? DateApplied { get; set; }

        [UIHint("Date")]
        [DataType(DataType.DateTime)]
        public DateTime PostedDate { get; set; }

        #endregion
    }
}