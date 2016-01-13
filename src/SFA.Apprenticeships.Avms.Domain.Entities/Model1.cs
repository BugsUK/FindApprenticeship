namespace SFA.Apprenticeships.Avms.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<AdditionalAnswer> AdditionalAnswers { get; set; }
        public virtual DbSet<AdditionalQuestion> AdditionalQuestions { get; set; }
        public virtual DbSet<AlertPreference> AlertPreferences { get; set; }
        public virtual DbSet<AlertType> AlertTypes { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationHistory> ApplicationHistories { get; set; }
        public virtual DbSet<ApplicationHistoryEvent> ApplicationHistoryEvents { get; set; }
        public virtual DbSet<ApplicationNextAction> ApplicationNextActions { get; set; }
        public virtual DbSet<ApplicationStatusType> ApplicationStatusTypes { get; set; }
        public virtual DbSet<ApplicationUnsuccessfulReasonType> ApplicationUnsuccessfulReasonTypes { get; set; }
        public virtual DbSet<ApplicationWithdrawnOrDeclinedReasonType> ApplicationWithdrawnOrDeclinedReasonTypes { get; set; }
        public virtual DbSet<ApprenticeshipFramework> ApprenticeshipFrameworks { get; set; }
        public virtual DbSet<ApprenticeshipFrameworkStatusType> ApprenticeshipFrameworkStatusTypes { get; set; }
        public virtual DbSet<ApprenticeshipOccupation> ApprenticeshipOccupations { get; set; }
        public virtual DbSet<ApprenticeshipOccupationStatusType> ApprenticeshipOccupationStatusTypes { get; set; }
        public virtual DbSet<ApprenticeshipType> ApprenticeshipTypes { get; set; }
        public virtual DbSet<AttachedDocument> AttachedDocuments { get; set; }
        public virtual DbSet<AttachedtoItemType> AttachedtoItemTypes { get; set; }
        public virtual DbSet<AuditRecord> AuditRecords { get; set; }
        public virtual DbSet<BackgroundSearchLog> BackgroundSearchLogs { get; set; }
        public virtual DbSet<CAFField> CAFFields { get; set; }
        public virtual DbSet<CAFFieldsFieldType> CAFFieldsFieldTypes { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<CandidateDisability> CandidateDisabilities { get; set; }
        public virtual DbSet<CandidateEthnicOrigin> CandidateEthnicOrigins { get; set; }
        public virtual DbSet<CandidateGender> CandidateGenders { get; set; }
        public virtual DbSet<CandidateHistory> CandidateHistories { get; set; }
        public virtual DbSet<CandidateHistoryEvent> CandidateHistoryEvents { get; set; }
        public virtual DbSet<CandidatePreference> CandidatePreferences { get; set; }
        public virtual DbSet<CandidateStatu> CandidateStatus { get; set; }
        public virtual DbSet<CandidateULNStatu> CandidateULNStatus { get; set; }
        public virtual DbSet<ContactPreferenceType> ContactPreferenceTypes { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<DisplayText> DisplayTexts { get; set; }
        public virtual DbSet<EducationResult> EducationResults { get; set; }
        public virtual DbSet<EducationResultLevel> EducationResultLevels { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<EmployerContact> EmployerContacts { get; set; }
        public virtual DbSet<EmployerHistory> EmployerHistories { get; set; }
        public virtual DbSet<EmployerHistoryEventType> EmployerHistoryEventTypes { get; set; }
        public virtual DbSet<EmployerSICCode> EmployerSICCodes { get; set; }
        public virtual DbSet<EmployerTrainingProviderStatu> EmployerTrainingProviderStatus { get; set; }
        public virtual DbSet<ExternalApplication> ExternalApplications { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<LocalAuthority> LocalAuthorities { get; set; }
        public virtual DbSet<LocalAuthorityGroup> LocalAuthorityGroups { get; set; }
        public virtual DbSet<LocalAuthorityGroupPurpose> LocalAuthorityGroupPurposes { get; set; }
        public virtual DbSet<LocalAuthorityGroupType> LocalAuthorityGroupTypes { get; set; }
        public virtual DbSet<MasterUPIN> MasterUPINs { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageEvent> MessageEvents { get; set; }
        public virtual DbSet<MIMEType> MIMETypes { get; set; }
        public virtual DbSet<NASSupportContact> NASSupportContacts { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonTitleType> PersonTitleTypes { get; set; }
        public virtual DbSet<PersonType> PersonTypes { get; set; }
        public virtual DbSet<PostcodeOutcode> PostcodeOutcodes { get; set; }
        public virtual DbSet<PrivilegeType> PrivilegeTypes { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderSite> ProviderSites { get; set; }
        public virtual DbSet<ProviderSiteFramework> ProviderSiteFrameworks { get; set; }
        public virtual DbSet<ProviderSiteHistory> ProviderSiteHistories { get; set; }
        public virtual DbSet<ProviderSiteHistoryEventType> ProviderSiteHistoryEventTypes { get; set; }
        public virtual DbSet<ProviderSiteLocalAuthority> ProviderSiteLocalAuthorities { get; set; }
        public virtual DbSet<ProviderSiteOffer> ProviderSiteOffers { get; set; }
        public virtual DbSet<ProviderSiteRelationship> ProviderSiteRelationships { get; set; }
        public virtual DbSet<ProviderSiteRelationshipType> ProviderSiteRelationshipTypes { get; set; }
        public virtual DbSet<ReportAgeRanx> ReportAgeRanges { get; set; }
        public virtual DbSet<ReportDefinition> ReportDefinitions { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<SavedSearchCriteria> SavedSearchCriterias { get; set; }
        public virtual DbSet<SavedSearchCriteriaSearchType> SavedSearchCriteriaSearchTypes { get; set; }
        public virtual DbSet<SavedSearchCriteriaVacancyPostedSince> SavedSearchCriteriaVacancyPostedSinces { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<SchoolAttended> SchoolAttendeds { get; set; }
        public virtual DbSet<SearchAuditRecord> SearchAuditRecords { get; set; }
        public virtual DbSet<SearchFramework> SearchFrameworks { get; set; }
        public virtual DbSet<SectorSuccessRate> SectorSuccessRates { get; set; }
        public virtual DbSet<SICCode> SICCodes { get; set; }
        public virtual DbSet<StakeHolder> StakeHolders { get; set; }
        public virtual DbSet<StakeHolderStatu> StakeHolderStatus { get; set; }
        public virtual DbSet<SubVacancy> SubVacancies { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<SystemParameter> SystemParameters { get; set; }
        public virtual DbSet<TermsAndCondition> TermsAndConditions { get; set; }
        public virtual DbSet<UniqueKeyRegister> UniqueKeyRegisters { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<VacancyHistory> VacancyHistories { get; set; }
        public virtual DbSet<VacancyHistoryEventType> VacancyHistoryEventTypes { get; set; }
        public virtual DbSet<VacancyLocation> VacancyLocations { get; set; }
        public virtual DbSet<VacancyLocationType> VacancyLocationTypes { get; set; }
        public virtual DbSet<VacancyOwnerRelationship> VacancyOwnerRelationships { get; set; }
        public virtual DbSet<VacancyOwnerRelationshipHistory> VacancyOwnerRelationshipHistories { get; set; }
        public virtual DbSet<VacancyProvisionRelationshipHistoryEventType> VacancyProvisionRelationshipHistoryEventTypes { get; set; }
        public virtual DbSet<VacancyProvisionRelationshipStatusType> VacancyProvisionRelationshipStatusTypes { get; set; }
        public virtual DbSet<VacancyReferralComment> VacancyReferralComments { get; set; }
        public virtual DbSet<VacancyReferralCommentsFieldType> VacancyReferralCommentsFieldTypes { get; set; }
        public virtual DbSet<VacancySearch> VacancySearches { get; set; }
        public virtual DbSet<VacancySearchAudit> VacancySearchAudits { get; set; }
        public virtual DbSet<VacancyStatusType> VacancyStatusTypes { get; set; }
        public virtual DbSet<VacancyTextField> VacancyTextFields { get; set; }
        public virtual DbSet<VacancyTextFieldValue> VacancyTextFieldValues { get; set; }
        public virtual DbSet<WageType> WageTypes { get; set; }
        public virtual DbSet<WatchedVacancy> WatchedVacancies { get; set; }
        public virtual DbSet<WorkExperience> WorkExperiences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalQuestion>()
                .HasMany(e => e.AdditionalAnswers)
                .WithRequired(e => e.AdditionalQuestion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AlertType>()
                .HasMany(e => e.AlertPreferences)
                .WithRequired(e => e.AlertType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.AdditionalAnswers)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.ApplicationHistories)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.SubVacancies)
                .WithOptional(e => e.Application)
                .HasForeignKey(e => e.AllocatedApplicationId);

            modelBuilder.Entity<ApplicationHistoryEvent>()
                .HasMany(e => e.ApplicationHistories)
                .WithRequired(e => e.ApplicationHistoryEvent)
                .HasForeignKey(e => e.ApplicationHistoryEventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationNextAction>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.ApplicationNextAction)
                .HasForeignKey(e => e.NextActionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationStatusType>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.ApplicationStatusType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUnsuccessfulReasonType>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.ApplicationUnsuccessfulReasonType)
                .HasForeignKey(e => e.UnsuccessfulReasonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationWithdrawnOrDeclinedReasonType>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.ApplicationWithdrawnOrDeclinedReasonType)
                .HasForeignKey(e => e.WithdrawnOrDeclinedReasonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipFramework>()
                .HasMany(e => e.CandidatePreferences)
                .WithOptional(e => e.ApprenticeshipFramework)
                .HasForeignKey(e => e.FirstFrameworkId);

            modelBuilder.Entity<ApprenticeshipFramework>()
                .HasMany(e => e.CandidatePreferences1)
                .WithOptional(e => e.ApprenticeshipFramework1)
                .HasForeignKey(e => e.SecondFrameworkId);

            modelBuilder.Entity<ApprenticeshipFramework>()
                .HasMany(e => e.SearchFrameworks)
                .WithRequired(e => e.ApprenticeshipFramework)
                .HasForeignKey(e => e.FrameworkId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipFramework>()
                .HasMany(e => e.ProviderSiteFrameworks)
                .WithRequired(e => e.ApprenticeshipFramework)
                .HasForeignKey(e => e.FrameworkId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipFramework>()
                .HasMany(e => e.VacancySearches)
                .WithRequired(e => e.ApprenticeshipFramework)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipFrameworkStatusType>()
                .HasMany(e => e.ApprenticeshipFrameworks)
                .WithRequired(e => e.ApprenticeshipFrameworkStatusType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipOccupation>()
                .HasMany(e => e.ApprenticeshipFrameworks)
                .WithRequired(e => e.ApprenticeshipOccupation)
                .HasForeignKey(e => e.ApprenticeshipOccupationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipOccupation>()
                .HasMany(e => e.ApprenticeshipFrameworks1)
                .WithOptional(e => e.ApprenticeshipOccupation1)
                .HasForeignKey(e => e.PreviousApprenticeshipOccupationId);

            modelBuilder.Entity<ApprenticeshipOccupation>()
                .HasMany(e => e.CandidatePreferences)
                .WithOptional(e => e.ApprenticeshipOccupation)
                .HasForeignKey(e => e.FirstOccupationId);

            modelBuilder.Entity<ApprenticeshipOccupation>()
                .HasMany(e => e.CandidatePreferences1)
                .WithOptional(e => e.ApprenticeshipOccupation1)
                .HasForeignKey(e => e.SecondOccupationId);

            modelBuilder.Entity<ApprenticeshipOccupation>()
                .HasMany(e => e.SectorSuccessRates)
                .WithRequired(e => e.ApprenticeshipOccupation)
                .HasForeignKey(e => e.SectorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipOccupationStatusType>()
                .HasMany(e => e.ApprenticeshipOccupations)
                .WithRequired(e => e.ApprenticeshipOccupationStatusType)
                .HasForeignKey(e => e.ApprenticeshipOccupationStatusTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipOccupationStatusType>()
                .HasMany(e => e.ApprenticeshipOccupations1)
                .WithRequired(e => e.ApprenticeshipOccupationStatusType1)
                .HasForeignKey(e => e.ApprenticeshipOccupationStatusTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprenticeshipType>()
                .HasMany(e => e.Vacancies)
                .WithOptional(e => e.ApprenticeshipType1)
                .HasForeignKey(e => e.ApprenticeshipType);

            modelBuilder.Entity<ApprenticeshipType>()
                .HasMany(e => e.VacancySearches)
                .WithRequired(e => e.ApprenticeshipType1)
                .HasForeignKey(e => e.ApprenticeshipType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AttachedDocument>()
                .HasMany(e => e.Applications)
                .WithOptional(e => e.AttachedDocument)
                .HasForeignKey(e => e.CVAttachmentId);

            modelBuilder.Entity<AttachedDocument>()
                .HasMany(e => e.VacancyOwnerRelationships)
                .WithOptional(e => e.AttachedDocument)
                .HasForeignKey(e => e.EmployerLogoAttachmentId);

            modelBuilder.Entity<AttachedtoItemType>()
                .HasMany(e => e.AuditRecords)
                .WithRequired(e => e.AttachedtoItemType1)
                .HasForeignKey(e => e.AttachedtoItemType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CAFFieldsFieldType>()
                .HasMany(e => e.CAFFields)
                .WithRequired(e => e.CAFFieldsFieldType)
                .HasForeignKey(e => e.Field)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Candidate>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.AlertPreferences)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.CAFFields)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.CandidateHistories)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.CandidatePreferences)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.EducationResults)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.ExternalApplications)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.SavedSearchCriterias)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.SchoolAttendeds)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.WatchedVacancies)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.WorkExperiences)
                .WithRequired(e => e.Candidate)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Candidate>()
                .HasMany(e => e.Messages)
                .WithMany(e => e.Candidates)
                .Map(m => m.ToTable("CandidateBroadcastMessage").MapLeftKey("CandidateId").MapRightKey("MessageId"));

            modelBuilder.Entity<CandidateDisability>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.CandidateDisability)
                .HasForeignKey(e => e.Disability)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateEthnicOrigin>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.CandidateEthnicOrigin)
                .HasForeignKey(e => e.EthnicOrigin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateGender>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.CandidateGender)
                .HasForeignKey(e => e.Gender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateHistoryEvent>()
                .HasMany(e => e.CandidateHistories)
                .WithRequired(e => e.CandidateHistoryEvent)
                .HasForeignKey(e => e.CandidateHistoryEventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateStatu>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.CandidateStatu)
                .HasForeignKey(e => e.CandidateStatusTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CandidateULNStatu>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.CandidateULNStatu)
                .HasForeignKey(e => e.UlnStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContactPreferenceType>()
                .HasMany(e => e.EmployerContacts)
                .WithRequired(e => e.ContactPreferenceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.Employers)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.LocalAuthorities)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.StakeHolders)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.ProviderSites)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<County>()
                .HasMany(e => e.VacancySearches)
                .WithRequired(e => e.County)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EducationResultLevel>()
                .HasMany(e => e.EducationResults)
                .WithRequired(e => e.EducationResultLevel)
                .HasForeignKey(e => e.Level)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employer>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Employer>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Employer>()
                .Property(e => e.OwnerOrgnistaion)
                .IsUnicode(false);

            modelBuilder.Entity<Employer>()
                .Property(e => e.CompanyRegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.VacancyOwnerRelationships)
                .WithRequired(e => e.Employer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.EmployerHistories)
                .WithRequired(e => e.Employer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employer>()
                .HasMany(e => e.EmployerSICCodes)
                .WithRequired(e => e.Employer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployerContact>()
                .HasMany(e => e.Employers)
                .WithRequired(e => e.EmployerContact)
                .HasForeignKey(e => e.PrimaryContact)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployerHistory>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<EmployerHistoryEventType>()
                .HasMany(e => e.EmployerHistories)
                .WithRequired(e => e.EmployerHistoryEventType)
                .HasForeignKey(e => e.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmployerTrainingProviderStatu>()
                .HasMany(e => e.Employers)
                .WithOptional(e => e.EmployerTrainingProviderStatu)
                .HasForeignKey(e => e.EmployerStatusTypeId);

            modelBuilder.Entity<EmployerTrainingProviderStatu>()
                .HasMany(e => e.ProviderSites)
                .WithOptional(e => e.EmployerTrainingProviderStatu)
                .HasForeignKey(e => e.TrainingProviderStatusTypeId);

            modelBuilder.Entity<EmployerTrainingProviderStatu>()
                .HasMany(e => e.Providers)
                .WithRequired(e => e.EmployerTrainingProviderStatu)
                .HasForeignKey(e => e.ProviderStatusTypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalAuthority>()
                .Property(e => e.CodeName)
                .IsFixedLength();

            modelBuilder.Entity<LocalAuthority>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.LocalAuthority)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalAuthorityGroup>()
                .HasMany(e => e.LocalAuthorityGroup1)
                .WithOptional(e => e.LocalAuthorityGroup2)
                .HasForeignKey(e => e.ParentLocalAuthorityGroupID);

            modelBuilder.Entity<LocalAuthorityGroup>()
                .HasMany(e => e.LocalAuthorities)
                .WithMany(e => e.LocalAuthorityGroups)
                .Map(m => m.ToTable("LocalAuthorityGroupMembership").MapLeftKey("LocalAuthorityGroupID").MapRightKey("LocalAuthorityID"));

            modelBuilder.Entity<LocalAuthorityGroupPurpose>()
                .HasMany(e => e.LocalAuthorityGroups)
                .WithRequired(e => e.LocalAuthorityGroupPurpose)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocalAuthorityGroupType>()
                .HasMany(e => e.LocalAuthorityGroups)
                .WithRequired(e => e.LocalAuthorityGroupType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MessageEvent>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.MessageEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MIMEType>()
                .HasMany(e => e.AttachedDocuments)
                .WithRequired(e => e.MIMEType1)
                .HasForeignKey(e => e.MIMEType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Organisation>()
                .Property(e => e.CodeName)
                .IsFixedLength();

            modelBuilder.Entity<Organisation>()
                .HasMany(e => e.StakeHolders)
                .WithRequired(e => e.Organisation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.Candidates)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.EmployerContacts)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.StakeHolders)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonTitleType>()
                .HasMany(e => e.People)
                .WithRequired(e => e.PersonTitleType)
                .HasForeignKey(e => e.Title)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonType>()
                .HasMany(e => e.People)
                .WithRequired(e => e.PersonType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostcodeOutcode>()
                .Property(e => e.Outcode)
                .IsFixedLength();

            modelBuilder.Entity<Provider>()
                .HasMany(e => e.ProviderSiteRelationships)
                .WithRequired(e => e.Provider)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Provider>()
                .HasMany(e => e.SectorSuccessRates)
                .WithRequired(e => e.Provider)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Provider>()
                .HasMany(e => e.Vacancies)
                .WithOptional(e => e.Provider)
                .HasForeignKey(e => e.ContractOwnerID);

            modelBuilder.Entity<ProviderSite>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<ProviderSite>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<ProviderSite>()
                .HasMany(e => e.ProviderSiteRelationships)
                .WithRequired(e => e.ProviderSite)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSite>()
                .HasMany(e => e.ProviderSiteHistories)
                .WithRequired(e => e.ProviderSite)
                .HasForeignKey(e => e.TrainingProviderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSite>()
                .HasMany(e => e.Vacancies)
                .WithOptional(e => e.ProviderSite)
                .HasForeignKey(e => e.DeliveryOrganisationID);

            modelBuilder.Entity<ProviderSite>()
                .HasMany(e => e.Vacancies1)
                .WithOptional(e => e.ProviderSite1)
                .HasForeignKey(e => e.VacancyManagerID);

            modelBuilder.Entity<ProviderSite>()
                .HasMany(e => e.VacancyOwnerRelationships)
                .WithRequired(e => e.ProviderSite)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSiteHistoryEventType>()
                .HasMany(e => e.ProviderSiteHistories)
                .WithRequired(e => e.ProviderSiteHistoryEventType)
                .HasForeignKey(e => e.EventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSiteRelationship>()
                .HasMany(e => e.ProviderSiteFrameworks)
                .WithRequired(e => e.ProviderSiteRelationship)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSiteRelationship>()
                .HasMany(e => e.ProviderSiteLocalAuthorities)
                .WithRequired(e => e.ProviderSiteRelationship)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProviderSiteRelationship>()
                .HasMany(e => e.VacancyOwnerRelationships)
                .WithMany(e => e.ProviderSiteRelationships)
                .Map(m => m.ToTable("RecruitmentAgentLinkedRelationships").MapLeftKey("ProviderSiteRelationshipID").MapRightKey("VacancyOwnerRelationshipID"));

            modelBuilder.Entity<ProviderSiteRelationshipType>()
                .HasMany(e => e.ProviderSiteRelationships)
                .WithRequired(e => e.ProviderSiteRelationshipType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleType>()
                .HasMany(e => e.ReportDefinitions)
                .WithRequired(e => e.RoleType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SavedSearchCriteria>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<SavedSearchCriteria>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<SavedSearchCriteria>()
                .HasMany(e => e.SearchFrameworks)
                .WithRequired(e => e.SavedSearchCriteria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SavedSearchCriteriaSearchType>()
                .HasMany(e => e.SavedSearchCriterias)
                .WithRequired(e => e.SavedSearchCriteriaSearchType)
                .HasForeignKey(e => e.SearchType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SavedSearchCriteriaVacancyPostedSince>()
                .HasMany(e => e.SavedSearchCriterias)
                .WithRequired(e => e.SavedSearchCriteriaVacancyPostedSince)
                .HasForeignKey(e => e.VacancyPostedSince)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SICCode>()
                .HasMany(e => e.EmployerSICCodes)
                .WithRequired(e => e.SICCode)
                .HasForeignKey(e => e.SICId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StakeHolder>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<StakeHolder>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<StakeHolderStatu>()
                .HasMany(e => e.StakeHolders)
                .WithRequired(e => e.StakeHolderStatu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubVacancy>()
                .Property(e => e.ILRNumber)
                .IsFixedLength();

            modelBuilder.Entity<UniqueKeyRegister>()
                .Property(e => e.KeyType)
                .IsFixedLength();

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.FAQs)
                .WithRequired(e => e.UserType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.Messages)
                .WithRequired(e => e.UserType)
                .HasForeignKey(e => e.RecipientType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.Messages1)
                .WithRequired(e => e.UserType1)
                .HasForeignKey(e => e.SenderType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserType>()
                .HasMany(e => e.TermsAndConditions)
                .WithRequired(e => e.UserType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Vacancy>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<Vacancy>()
                .Property(e => e.WeeklyWage)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.AdditionalQuestions)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.Applications)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.ExternalApplications)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.SubVacancies)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.Vacancy1)
                .WithOptional(e => e.Vacancy2)
                .HasForeignKey(e => e.MasterVacancyId);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.VacancyHistories)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.VacancyLocations)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.VacancyReferralComments)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.VacancySearches)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.VacancyTextFields)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vacancy>()
                .HasMany(e => e.WatchedVacancies)
                .WithRequired(e => e.Vacancy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyHistoryEventType>()
                .HasMany(e => e.VacancyHistories)
                .WithRequired(e => e.VacancyHistoryEventType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<VacancyLocation>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<VacancyOwnerRelationship>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<VacancyOwnerRelationship>()
                .HasMany(e => e.Vacancies)
                .WithRequired(e => e.VacancyOwnerRelationship)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyOwnerRelationship>()
                .HasMany(e => e.VacancyOwnerRelationshipHistories)
                .WithRequired(e => e.VacancyOwnerRelationship)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyOwnerRelationshipHistory>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<VacancyProvisionRelationshipHistoryEventType>()
                .HasMany(e => e.VacancyOwnerRelationshipHistories)
                .WithRequired(e => e.VacancyProvisionRelationshipHistoryEventType)
                .HasForeignKey(e => e.EventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyProvisionRelationshipStatusType>()
                .HasMany(e => e.VacancyOwnerRelationships)
                .WithRequired(e => e.VacancyProvisionRelationshipStatusType)
                .HasForeignKey(e => e.StatusTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyReferralCommentsFieldType>()
                .HasMany(e => e.VacancyReferralComments)
                .WithRequired(e => e.VacancyReferralCommentsFieldType)
                .HasForeignKey(e => e.FieldTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancySearch>()
                .Property(e => e.Latitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<VacancySearch>()
                .Property(e => e.Longitude)
                .HasPrecision(13, 10);

            modelBuilder.Entity<VacancySearch>()
                .Property(e => e.WeeklyWage)
                .HasPrecision(19, 4);

            modelBuilder.Entity<VacancyStatusType>()
                .HasMany(e => e.Vacancies)
                .WithRequired(e => e.VacancyStatusType)
                .HasForeignKey(e => e.VacancyStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyStatusType>()
                .HasMany(e => e.VacancySearches)
                .WithRequired(e => e.VacancyStatusType)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VacancyTextFieldValue>()
                .HasMany(e => e.VacancyTextFields)
                .WithRequired(e => e.VacancyTextFieldValue)
                .HasForeignKey(e => e.Field)
                .WillCascadeOnDelete(false);
        }
    }
}
