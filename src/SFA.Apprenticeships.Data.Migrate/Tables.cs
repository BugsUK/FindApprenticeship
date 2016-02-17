namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;

    public interface ITableSpec : ITableDetails
    {
        decimal BatchSizeMultiplier { get; }
        IEnumerable<ITableSpec> DependsOn { get; }
        Func<dynamic, dynamic, bool> Transform { get; }
    }

    public class AvmsToAvmsPlusTables
    {
        private ILogService _log;
        private TableSpecList _tables;

        public AvmsToAvmsPlusTables(ILogService log)
        {
            _log = log;

            _tables = new TableSpecList();

            // Reference / bottom level
            var VacancyProvisionRelationshipHistoryEventType = _tables.AddNew("VacancyProvisionRelationshipHistoryEventType", OwnedByAv);
            var VacancyProvisionRelationshipStatusType = _tables.AddNew("VacancyProvisionRelationshipStatusType", OwnedByAv);
            var MimeType                           = _tables.AddNew("MIMEType",                               OwnedByAv);
            var EmployerHistoryEventType           = _tables.AddNew("EmployerHistoryEventType",               OwnedByAv);
            var ProviderSiteHistoryEventType       = _tables.AddNew("ProviderSiteHistoryEventType",           OwnedByAv);
            var PersonTitleType                    = _tables.AddNew("PersonTitleType",                        OwnedByAv);
            var ContactPreferenceType              = _tables.AddNew("ContactPreferenceType",                  OwnedByAv);
            var VacancyHistoryEventType            = _tables.AddNew("VacancyHistoryEventType",           OwnedByAv);
            var PersonType                         = _tables.AddNew("PersonType",                        OwnedByAv);
            var SicCode                            = _tables.AddNew("SICCode",                           OwnedByAv);
            var Organisation                       = _tables.AddNew("Organisation",                      OwnedByAv);
            var StakeholderStatus                  = _tables.AddNew("StakeHolderStatus",                 OwnedByAv);
            var EmployerTrainingProviderStatus     = _tables.AddNew("EmployerTrainingProviderStatus",    OwnedByAv);
            var VacancyTextFieldValue              = _tables.AddNew("VacancyTextFieldValue",             OwnedByAv);
            var ApprenticeshipType                 = _tables.AddNew("ApprenticeshipType",                OwnedByAv);
            var ProviderSiteRelationshipType       = _tables.AddNew("ProviderSiteRelationshipType", "ProviderSiteRelationshipTypeID", OwnedByAv);
            var VacancyStatusType                  = _tables.AddNew("VacancyStatusType",                 OwnedByAv);
            var ApprenticeshipOccupationStatusType = _tables.AddNew("ApprenticeshipOccupationStatusType", OwnedByAv);
            var ApprenticeshipFrameworkStatusType  = _tables.AddNew("ApprenticeshipFrameworkStatusType", OwnedByAv);
            var VacancyReferralCommentsFieldType   = _tables.AddNew("VacancyReferralCommentsFieldType",  OwnedByAv);

            // Not in diagram
            var County         = _tables.AddNew("County",         OwnedByAv);
            var LocalAuthority = _tables.AddNew("LocalAuthority", OwnedByAv, County);

            // Other tables
            var AttachedDocument                  = _tables.AddNew("AttachedDocument", "AttachedDocumentId", 0.1m, OwnedByAv, MimeType);
            var EmployerContact                   = _tables.AddNew("EmployerContact",                 OwnedByAv, ContactPreferenceType, County, LocalAuthority);
            var Employer                          = _tables.AddNew("Employer",                        OwnedByAv, EmployerTrainingProviderStatus, EmployerContact, County, LocalAuthority);
            var EmployerHistory                   = _tables.AddNew("EmployerHistory",                 OwnedByAv, EmployerHistoryEventType, Employer, County, LocalAuthority);
            var ProviderSite                      = _tables.AddNew("ProviderSite",        "ProviderSiteID",            OwnedByAv, EmployerTrainingProviderStatus);
            var ProviderSiteHistory               = _tables.AddNew("ProviderSiteHistory", "TrainingProviderHistoryId", OwnedByAv, ProviderSiteHistoryEventType, ProviderSite);
            var EmployerSicCodes                  = _tables.AddNew("EmployerSICCodes",    "EmployerSICCodes",          OwnedByAv, Employer, SicCode);
            var Person                            = _tables.AddNew("Person",                          OwnedByAv, PersonTitleType, PersonType);
            var Stakeholder                       = _tables.AddNew("StakeHolder",         "StakeHolderID",                 OwnedByAv, Person, Organisation, StakeholderStatus, County, LocalAuthority);
            var VacancyHistory                    = _tables.AddNew("VacancyHistory",                  OwnedByAv, VacancyHistoryEventType);
            var VacancyTextField                  = _tables.AddNew("VacancyTextField",                OwnedByAv, VacancyTextFieldValue);
            var Provider                          = _tables.AddNew("Provider", "ProviderID",          TransformProvider, EmployerTrainingProviderStatus);
            var ApprenticeshipOccupation          = _tables.AddNew("ApprenticeshipOccupation",        OwnedByAv, ApprenticeshipOccupationStatusType);
            var ApprenticeshipFramework           = _tables.AddNew("ApprenticeshipFramework",         OwnedByAv, ApprenticeshipOccupation, ApprenticeshipFrameworkStatusType);
            var VacancyOwnerRelationship          = _tables.AddNew("VacancyOwnerRelationship",        OwnedByAv,
                ProviderSite, Employer, AttachedDocument, VacancyProvisionRelationshipStatusType);
            var Vacancy                           = _tables.AddNew("Vacancy",                         TransformVacancy,
                VacancyOwnerRelationship, ProviderSite, Provider, ApprenticeshipType, VacancyStatusType, ApprenticeshipFramework, County, LocalAuthority);
            var VacancyLocation                   = _tables.AddNew("VacancyLocation",                 OwnedByAv, Vacancy, County, LocalAuthority);
            var ProviderSiteRelationship          = _tables.AddNew("ProviderSiteRelationship",              "ProviderSiteRelationshipID",            OwnedByAv, Provider, ProviderSiteRelationshipType);
            var RecruitmentAgentLinkedRelationships = _tables.AddNew("RecruitmentAgentLinkedRelationships", "RecruitmentAgentLinkedRelationshipsID", OwnedByAv, VacancyOwnerRelationship, ProviderSiteRelationship);
            var SubVacancy                        = _tables.AddNew("SubVacancy",                      OwnedByAv, Vacancy);
            // var SectorSuccessRates = tables.AddNew("SectorSuccessRates", OwnedByAv); // TODO: Hard as link table with no primary key
            var AdditionalQuestion                = _tables.AddNew("AdditionalQuestion",              OwnedByAv, Vacancy);
            var VacancyReferralComments           = _tables.AddNew("VacancyReferralComments",    "VacancyReferralCommentsID",    OwnedByAv, Vacancy, VacancyReferralCommentsFieldType);
            var ProviderSiteLocalAuthority        = _tables.AddNew("ProviderSiteLocalAuthority", "ProviderSiteLocalAuthorityID", OwnedByAv, ProviderSiteRelationship);
            var ProviderSiteFramework             = _tables.AddNew("ProviderSiteFramework",      "ProviderSiteFrameworkID",      OwnedByAv, ProviderSiteRelationship, ApprenticeshipFramework);
            var ProviderSiteOffer                 = _tables.AddNew("ProviderSiteOffer",          "ProviderSiteOfferID",          OwnedByAv, ProviderSiteLocalAuthority, ProviderSiteFramework);
            var VacancyOwnerRelationshipHistory   = _tables.AddNew("VacancyOwnerRelationshipHistory", OwnedByAv, VacancyOwnerRelationship, VacancyProvisionRelationshipHistoryEventType);

            /* Other tables from diagram not required here
            var VacancySearch = tables.AddNew("VacancySearch", OwnedByAv);
            var SearchFrameworks                = tables.AddNew("SearchFrameworks", OwnedByAv);
            var SavedSearchCriteria = tables.AddNew("SavedSearchCriteria", OwnedByAv);
            var SavedSearchCriteriaSearchtype = tables.AddNew("SavedSearchCriteriaSearchtype", OwnedByAv);
            var SavedSearchCriteriaVacancyPostedSince = tables.AddNew("SavedSearchCriteriaVacancyPostedSince", OwnedByAv);
            */
        }

        public IEnumerable<ITableSpec> All { get { return _tables.All; } }

        private class TableSpecList
        {
            public List<TableSpec> All = new List<TableSpec>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tableName"></param>
            /// <param name="transform">Function taking (oldRecord, newRecord). "originalTarget" will be null if there is not yet a corresponding record in the target. "source" may be changed by this function
            /// to add or change fields. If this function then returns true AND "source" and "originalTarget" are then found to vary then the record will be inserted/updated, otherwise it will be left alone.</param>
            /// <param name="transform">Action taking (ourOriginal, av). ourOriginal is the record on our side (may be null), av is the record on their side. This action will be called after</param>
            /// <param name="dependsOn"></param>
            /// <returns></returns>
            public TableSpec AddNew(string tableName, Func<dynamic, dynamic, bool> transform, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, tableName + "Id", 1, transform, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string primaryKey, Func<dynamic, dynamic, bool> transform, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKey, 1, transform, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string primaryKey, decimal batchSizeMultiplier, Func<dynamic, dynamic, bool> transform, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKey, batchSizeMultiplier, transform, dependsOn);
                All.Add(table);
                return table;
            }
        }

        private class TableSpec : ITableSpec
        {
            private List<TableSpec> _dependsOn;
            public IEnumerable<ITableSpec> DependsOn { get { foreach (var i in _dependsOn) yield return i; } } // Safe read only enumeration

            public string Name { get; private set; }

            public Func<dynamic, dynamic, bool> Transform { get; private set; }

            public string PrimaryKey { get; private set; }

            public decimal BatchSizeMultiplier { get; private set; }

            public TableSpec(string name, string primaryKey, decimal batchSizeMultiplier, Func<dynamic, dynamic, bool> transform, params TableSpec[] dependsOn)
            {
                for (int i = 0; i < dependsOn.Length; i++)
                {
                    if (dependsOn[i] == null)
                        throw new ArgumentNullException($"dependsOn[{i}]");
                }

                Name = name;
                PrimaryKey = primaryKey;
                BatchSizeMultiplier = batchSizeMultiplier;
                Transform = transform;
                _dependsOn = new List<TableSpec>(dependsOn);
            }
        }

        public static bool OwnedByAv(dynamic source, dynamic originalTarget)
        {
            return true;
        }

        public bool TransformVacancy(dynamic oldRecord, dynamic newRecord)
        {
            if (newRecord.VacancyId > 1000000) // TODO
            {
                _log.Error($"Ignored Vacancy from AVMS with VacancyId = {newRecord.VacancyId}");
                return false;
            }

            newRecord.OtherImportantInformation = string.Join(" ", newRecord.OtherImportantInformation, newRecord.RealityCheck);

            // The old values in this field would not be recognised by our system (although they will probably have timed out)
            newRecord.BeingSupportedBy      = null;
            newRecord.LockedForSupportUntil = null;

            // We can't populate these for new vacancies and none of our systems used it (including FAA)
            // Therefore it is better to set it to null (etc) to out problems early
            // TODO: Check that new vacancies are really setting to null and not the vacancy owner site id (etc)
            newRecord.VacancyManagerID        = null;
            newRecord.DeliveryOrganisationID  = null;
            newRecord.ContractOwnerID         = null;
            newRecord.OriginalContractOwnerID = null;

            // Believed to be supported by FAA, so don't blank (TODO: Check)
            // newRecord.EmployerAnonymousName = null;
            // newRecord.VacancyManagerAnonymous = false;

            return true;
        }

        public static bool TransformProvider(dynamic oldRecord, dynamic newRecord)
        {
            newRecord.UPIN = null; // We can't always populate this, so always set it to null to prove it isn't used.
            return true;
        }

    }
}
