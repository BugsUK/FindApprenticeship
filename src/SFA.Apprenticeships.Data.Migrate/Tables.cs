﻿namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ITableSpec : ITableDetails
    {
        decimal BatchSizeMultiplier { get; }
        IEnumerable<ITableSpec> DependsOn { get; }
        Func<ITableSpec, dynamic, dynamic, bool> Transform { get; }
        Func<ITableSpec, dynamic, bool> ShouldDelete { get; }
    }

    public class AvmsToAvmsPlusTables
    {
        private ILogService _log;
        private IAvmsSyncRespository _avmsSyncRepository;
        private IMigrateConfiguration _migrateConfig;

        private TableSpecList _tables;

        private const int ApprenticeshipTypeUnknown = 0;
        private const int ApprenticeshipTypeTraineeship = 4;
        private const int VacancyTypeUnknown = 0;
        private const int VacancyTypeApprenticeship = 1;
        private const int VacancyTypeTraineeship = 2;

        public AvmsToAvmsPlusTables(ILogService log, IMigrateConfiguration migrateConfig, IAvmsSyncRespository avmsSyncRepository, bool includeVacancy = true)
        {
            _log = log;
            _migrateConfig = migrateConfig;
            _avmsSyncRepository = avmsSyncRepository;

            _tables = new TableSpecList();

            {
                // Reference / bottom level
                var VacancyProvisionRelationshipHistoryEventType = _tables.AddNew("VacancyProvisionRelationshipHistoryEventType", OwnedByAv, DeleteShouldNeverHappen);
                var VacancyProvisionRelationshipStatusType = _tables.AddNew("VacancyProvisionRelationshipStatusType", OwnedByAv, DeleteShouldNeverHappen);
                var MimeType                           = _tables.AddNew("MIMEType",                               OwnedByAv, DeleteShouldNeverHappen);
                var EmployerHistoryEventType           = _tables.AddNew("EmployerHistoryEventType",               OwnedByAv, DeleteShouldNeverHappen);
                var ProviderSiteHistoryEventType       = _tables.AddNew("ProviderSiteHistoryEventType",           OwnedByAv, DeleteShouldNeverHappen);
                var PersonTitleType                    = _tables.AddNew("PersonTitleType",                        OwnedByAv, DeleteShouldNeverHappen);
                var ContactPreferenceType              = _tables.AddNew("ContactPreferenceType",                  OwnedByAv, DeleteShouldNeverHappen);
                var VacancyHistoryEventType            = _tables.AddNew("VacancyHistoryEventType",                OwnedByAv, DeleteShouldNeverHappen);
                var PersonType                         = _tables.AddNew("PersonType",                             OwnedByAv, DeleteShouldNeverHappen);

                // See EmployerSicCodes below
                // var SicCode                         = _tables.AddNew("SICCode",                                OwnedByAv, DeleteShouldNeverHappen);

                var EmployerTrainingProviderStatus     = _tables.AddNew("EmployerTrainingProviderStatus",         OwnedByAv, DeleteShouldNeverHappen);
                var VacancyTextFieldValue              = _tables.AddNew("VacancyTextFieldValue",                  OwnedByAv, DeleteShouldNeverHappen);
                var ApprenticeshipType                 = _tables.AddNew("ApprenticeshipType",                     OwnedByAv, DeleteShouldNeverHappen);
                var ProviderSiteRelationshipType       = _tables.AddNew("ProviderSiteRelationshipType", new string[] { "ProviderSiteRelationshipTypeID" }, OwnedByAv, DeleteShouldNeverHappen);
                var VacancyStatusType                  = _tables.AddNew("VacancyStatusType",                      OwnedByAv, DeleteShouldNeverHappen);
                var ApprenticeshipOccupationStatusType = _tables.AddNew("ApprenticeshipOccupationStatusType",     OwnedByAv, DeleteShouldNeverHappen);
                var ApprenticeshipFrameworkStatusType  = _tables.AddNew("ApprenticeshipFrameworkStatusType",      OwnedByAv, DeleteShouldNeverHappen);
                var VacancyReferralCommentsFieldType   = _tables.AddNew("VacancyReferralCommentsFieldType",       OwnedByAv, DeleteShouldNeverHappen);

                // Not in diagram
                var County         = _tables.AddNew("County",         OwnedByAv, DeleteShouldNeverHappen);
                var LocalAuthority = _tables.AddNew("LocalAuthority", OwnedByAv, DeleteShouldNeverHappen, County);

                // Other tables
                var AttachedDocument                  = _tables.AddNew("AttachedDocument",          0.1m, TransformAttachedDocument, DeleteShouldNeverHappen, MimeType);
                var Person                            = _tables.AddNew("Person",                          AnonymisePerson,          IgnoreDelete,            PersonTitleType, PersonType);
                var EmployerContact                   = _tables.AddNew("EmployerContact",                 AnonymiseEmployerContact, DeleteShouldNeverHappen, ContactPreferenceType, County, LocalAuthority, Person);
                var Employer                          = _tables.AddNew("Employer",                        TransformEmployer,        DeleteShouldNeverHappen, EmployerTrainingProviderStatus, EmployerContact, County, LocalAuthority);
                var EmployerHistory                   = _tables.AddNew("EmployerHistory",                 OwnedByAv,                DeleteShouldNeverHappen, EmployerHistoryEventType, Employer, County, LocalAuthority);
                var ProviderSite                      = _tables.AddNew("ProviderSite",        new string[] { "ProviderSiteID" },            AnonymiseProviderSite,        DeleteShouldNeverHappen, EmployerTrainingProviderStatus, LocalAuthority);
                var ProviderSiteHistory               = _tables.AddNew("ProviderSiteHistory", new string[] { "TrainingProviderHistoryId" }, AnonymiseProviderSiteHistory, DeleteShouldNeverHappen, ProviderSiteHistoryEventType, ProviderSite);

                // This isn't really needed, plus records can be deleted (outside of archiving activity) which we don't support yet (also, records get deleted and re-added by the AVMS GUI when editing)
                // var EmployerSicCodes               = _tables.AddNew("EmployerSICCodes",    new string[] { "EmployerSICCodes" },          OwnedByAv, DeleteShouldNeverHappen,         Employer, SicCode);

                var Provider                          = _tables.AddNew("Provider",            new string[] { "ProviderID" },                TransformProvider, DeleteShouldNeverHappen, EmployerTrainingProviderStatus);
                var ApprenticeshipOccupation          = _tables.AddNew("ApprenticeshipOccupation",        OwnedByAv, DeleteShouldNeverHappen, ApprenticeshipOccupationStatusType);
                var ApprenticeshipFramework           = _tables.AddNew("ApprenticeshipFramework",         OwnedByAv, DeleteShouldNeverHappen, ApprenticeshipOccupation, ApprenticeshipFrameworkStatusType);

                var VacancyOwnerRelationship          = _tables.AddNew("VacancyOwnerRelationship",        CanMutateVacancyOwnerRelationship, DeleteShouldNeverHappen,
                    ProviderSite, Employer, AttachedDocument, VacancyProvisionRelationshipStatusType);
                var VacancyOwnerRelationshipHistory   = _tables.AddNew("VacancyOwnerRelationshipHistory", CanMutateVacancyOwnerRelationshipHistory, DeleteShouldNeverHappen, VacancyOwnerRelationship, VacancyProvisionRelationshipHistoryEventType);

                var ProviderSiteRelationship          = _tables.AddNew("ProviderSiteRelationship",   new string[] { "ProviderSiteRelationshipID" },   OwnedByAv, DeleteShouldNeverHappen, Provider, ProviderSite, ProviderSiteRelationshipType);
                var RecruitmentAgentLinkedRelationships = _tables.AddNew("RecruitmentAgentLinkedRelationships", new string[] { "VacancyOwnerRelationshipID", "ProviderSiteRelationshipID" }, OwnedByAv, DeleteShouldNeverHappen, VacancyOwnerRelationship, ProviderSiteRelationship);
                var SectorSuccessRates                = _tables.AddNew("SectorSuccessRates",         new string[] { "ProviderID", "SectorID" },       OwnedByAv, DeleteShouldNeverHappen, Provider, ApprenticeshipOccupation);

                // Seems to be related to applications rather than vacancies
                // var SubVacancy                     = _tables.AddNew("SubVacancy",                                                                  OwnedByAv, Vacancy);

                var ProviderSiteLocalAuthority        = _tables.AddNew("ProviderSiteLocalAuthority", new string[] { "ProviderSiteLocalAuthorityID" }, OwnedByAv, DeleteShouldNeverHappen, ProviderSiteRelationship);
                var ProviderSiteFramework             = _tables.AddNew("ProviderSiteFramework",      new string[] { "ProviderSiteFrameworkID" },      OwnedByAv, DeleteShouldNeverHappen, ProviderSiteRelationship, ApprenticeshipFramework);

                // This isn't really needed, plus records can be deleted (outside of archiving activity) which we don't support yet (also, records get deleted and re-added by the AVMS GUI when editing)
                // var ProviderSiteOffer              = _tables.AddNew("ProviderSiteOffer",          new string[] { "ProviderSiteOfferID" },          OwnedByAv, DoDelete,                ProviderSiteLocalAuthority, ProviderSiteFramework);

                if (includeVacancy)
                {
                    // These take a while so don't always do them
                    var Vacancy                 = _tables.AddNew("Vacancy",                                                         0.2m, TransformVacancy, DeleteShouldNeverHappen,
                        VacancyOwnerRelationship, ProviderSite, Provider, ApprenticeshipType, VacancyStatusType, ApprenticeshipFramework, County, LocalAuthority);
                    var VacancyHistory          = _tables.AddNew("VacancyHistory",                                                  2.0m, NotOwnedByVacancyOwner, IgnoreDelete,   Vacancy, VacancyHistoryEventType);
                    var VacancyTextField        = _tables.AddNew("VacancyTextField",                                                2.0m, NotOwnedByVacancyOwner, IgnoreDelete,   Vacancy, VacancyTextFieldValue);
                    var VacancyLocation         = _tables.AddNew("VacancyLocation",                                                       NotOwnedByVacancyOwner, IgnoreDelete,   Vacancy, County, LocalAuthority);
                    var AdditionalQuestion      = _tables.AddNew("AdditionalQuestion",                                              2.0m, NotOwnedByVacancyOwner, IgnoreDelete,   Vacancy);
                    var VacancyReferralComments = _tables.AddNew("VacancyReferralComments", new string[] { "VacancyReferralCommentsID" }, 2.0m, NotOwnedByVacancyOwner, IgnoreDelete,   Vacancy, VacancyReferralCommentsFieldType);
                }

                /*if (false) // TODO: Stakeholder
                {
                    var Organisation      = _tables.AddNew("Organisation",                                        OwnedByAv);
                    var StakeholderStatus = _tables.AddNew("StakeHolderStatus",                                   OwnedByAv);
                    var Stakeholder       = _tables.AddNew("StakeHolder",       new string[] { "StakeHolderID" }, OwnedByAv, Person, Organisation, StakeholderStatus, County, LocalAuthority);
                }*/

                /* Other tables from diagram not required here
                var VacancySearch = tables.AddNew("VacancySearch", OwnedByAv);
                var SearchFrameworks                = tables.AddNew("SearchFrameworks", OwnedByAv);
                var SavedSearchCriteria = tables.AddNew("SavedSearchCriteria", OwnedByAv);
                var SavedSearchCriteriaSearchtype = tables.AddNew("SavedSearchCriteriaSearchtype", OwnedByAv);
                var SavedSearchCriteriaVacancyPostedSince = tables.AddNew("SavedSearchCriteriaVacancyPostedSince", OwnedByAv);
                */
            }


            /*
select top 10 * from VacancyProvisionRelationshipHistoryEventType
select top 10 * from VacancyProvisionRelationshipStatusType
select top 10 * from MIMEType
select top 10 * from EmployerHistoryEventType
select top 10 * from ProviderSiteHistoryEventType
select top 10 * from PersonTitleType
select top 10 * from ContactPreferenceType
select top 10 * from VacancyHistoryEventType
select top 10 * from PersonType
select top 10 * from SICCode
select top 10 * from EmployerTrainingProviderStatus
select top 10 * from VacancyTextFieldValue
select top 10 * from ApprenticeshipType
select top 10 * from ProviderSiteRelationshipType
select top 10 * from VacancyStatusType
select top 10 * from ApprenticeshipOccupationStatusType
select top 10 * from ApprenticeshipFrameworkStatusType
select top 10 * from VacancyReferralCommentsFieldType
select top 10 * from County
select top 10 * from LocalAuthority
select top 10 * from AttachedDocument
select top 10 * from Person
select top 10 * from EmployerContact
select top 10 * from Employer
select top 10 * from EmployerHistory
select top 10 * from ProviderSite
select top 10 * from ProviderSiteHistory
select top 10 * from EmployerSICCodes
select top 10 * from Provider
select top 10 * from ApprenticeshipOccupation
select top 10 * from ApprenticeshipFramework
select top 10 * from VacancyOwnerRelationship
select top 10 * from VacancyOwnerRelationshipHistory
select top 10 * from ProviderSiteRelationship
select top 10 * from RecruitmentAgentLinkedRelationships
select top 10 * from SectorSuccessRates
select top 10 * from SubVacancy
select top 10 * from ProviderSiteLocalAuthority
select top 10 * from ProviderSiteFramework
select top 10 * from ProviderSiteOffer
select top 10 * from Vacancy
select top 10 * from VacancyHistory
select top 10 * from VacancyTextField
select top 10 * from VacancyLocation
select top 10 * from AdditionalQuestion
select top 10 * from VacancyReferralComments
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
            /// <param name="transform">Function taking (oldRecord, newRecord). "oldRecord" will be null if there is not yet a corresponding record in the target. "newRecord" may be changed by this function
            /// to add or change fields. If this function then returns true AND "source" and "originalTarget" are then found to vary then the record will be inserted/updated, otherwise it will be left alone.</param>
            /// <param name="dependsOn"></param>
            /// <returns></returns>
            public TableSpec AddNew(string tableName, Func<ITableSpec, dynamic, dynamic, bool> transform, Func<ITableSpec, dynamic, bool> shouldDelete, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, new string[] { tableName + "Id" }, 1, transform, shouldDelete, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string[] primaryKeys, Func<ITableSpec, dynamic, dynamic, bool> transform, Func<ITableSpec, dynamic, bool> shouldDelete, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKeys, 1, transform, shouldDelete, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string[] primaryKeys, decimal batchSizeMultiplier, Func<ITableSpec, dynamic, dynamic, bool> transform, Func<ITableSpec, dynamic, bool> shouldDelete, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKeys, batchSizeMultiplier, transform, shouldDelete, dependsOn);
                All.Add(table);
                return table;
            }
            public TableSpec AddNew(string tableName, decimal batchSizeMultiplier, Func<ITableSpec, dynamic, dynamic, bool> transform, Func<ITableSpec, dynamic, bool> shouldDelete, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, new string[] { tableName + "Id" }, batchSizeMultiplier, transform, shouldDelete, dependsOn);
                All.Add(table);
                return table;
            }
        }

        private class TableSpec : ITableSpec
        {
            private List<TableSpec> _dependsOn;
            public IEnumerable<ITableSpec> DependsOn { get { foreach (var i in _dependsOn) yield return i; } } // Safe read only enumeration

            public string Name { get; private set; }

            public Func<ITableSpec, dynamic, dynamic, bool> Transform { get; private set; }

            public Func<ITableSpec, dynamic, bool> ShouldDelete { get; private set; }

            public IEnumerable<string> PrimaryKeys { get; private set; }

            public decimal BatchSizeMultiplier { get; private set; }

            public TableSpec(string name, string[] primaryKeys, decimal batchSizeMultiplier, Func<ITableSpec, dynamic, dynamic, bool> transform, Func<ITableSpec, dynamic, bool> shouldDelete, params TableSpec[] dependsOn)
            {
                for (int i = 0; i < dependsOn.Length; i++)
                {
                    if (dependsOn[i] == null)
                        throw new ArgumentNullException($"dependsOn[{i}]");
                }

                Name = name;
                PrimaryKeys = primaryKeys;
                BatchSizeMultiplier = batchSizeMultiplier;
                Transform = transform;
                _dependsOn = new List<TableSpec>(dependsOn);
            }
        }

        public static bool OwnedByAv(ITableSpec tableSpec, dynamic source, dynamic originalTarget)
        {
            return true;
        }

        public bool TransformVacancy(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (oldRecord != null && oldRecord.EditedInRaa)
            {
                _log.Warn($"Ignored change to Vacancy from AVMS with VacancyId = {newRecord.VacancyId}");
                return false;
            }

            if (oldRecord == null)
            {
                newRecord.VacancyGuid = (object)Guid.NewGuid(); // Need to manually box object (possible Dapper bug)
            }

            var vacancyTypeId = VacancyTypeUnknown;
            var apprenticeshipType = newRecord.ApprenticeshipType;
            if (apprenticeshipType != ApprenticeshipTypeUnknown)
            {
                vacancyTypeId = apprenticeshipType == ApprenticeshipTypeTraineeship ? VacancyTypeTraineeship : VacancyTypeApprenticeship;
            }
            newRecord.VacancyTypeId = (object) vacancyTypeId;

            //newRecord.OtherImportantInformation = string.Join(" ", newRecord.OtherImportantInformation, newRecord.RealityCheck); // TODO: This must be in vacancytextfield instead

            // The old values in this field would not be recognised by our system (although they will probably have timed out)
            newRecord.BeingSupportedBy      = null;
            newRecord.LockedForSupportUntil = null;

            // We can't populate these for new vacancies and none of our systems used it (including FAA)
            // Therefore it is better to set it to null (etc) to out problems early
            // TODO: Check that new vacancies are really setting to null and not the vacancy owner site id (etc)
            newRecord.VacancyManagerID        = null;
            newRecord.DeliveryOrganisationID  = null;
            newRecord.ContractOwnerID         = null;
            newRecord.OriginalContractOwnerId = null;

            // Believed to be supported by FAA, so don't blank (TODO: Check)
            // newRecord.EmployerAnonymousName = null;
            // newRecord.VacancyManagerAnonymous = false;

            AnonymiseVacancy(tableSpec, oldRecord, newRecord);

            return true;
        }

        public bool TransformEmployer(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            // The old values in this field would not be recognised by our system (although they will probably have timed out)
            newRecord.BeingSupportedBy = null;
            newRecord.LockedForSupportUntil = null;

            return true;
        }

        public bool TransformProvider(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            newRecord.UPIN = null; // We can't always populate this, so always set it to null to prove it isn't used.
            return true;
        }

        public bool NotOwnedByVacancyOwner(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            return !_avmsSyncRepository.IsVacancyOwnedByTargetDatabase(newRecord.VacancyId);
        }

        public bool CanMutateVacancyOwnerRelationship(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (oldRecord != null && oldRecord.EditedInRaa)
            {
                _log.Warn($"Ignored change to VacancyOwnerRelationship from AVMS with VacancyOwnerRelationshipId = {newRecord.VacancyOwnerRelationshipId}");
                return false;
            }

            return true;
        }

        public bool TransformAttachedDocument(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            //newRecord.Attachment = new byte[0]; // These are rather large and causing timeouts
            return true;
        }

        public bool CanMutateVacancyOwnerRelationshipHistory(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            return !_avmsSyncRepository.IsVacancyOwnerRelationshipOwnedByTargetDatabase(newRecord.VacancyOwnerRelationshipId);
        }


        public bool DeleteShouldNeverHappen(ITableSpec tableSpec, dynamic oldRecord)
        {
            _log.Error($"Delete shouldn't happen on {tableSpec.Name} ({string.Join(", ", Keys.GetPrimaryKeys((IDictionary<string,object>)oldRecord, tableSpec).Select(k => k.ToString()))})");
            return false;
        }

        public bool IgnoreDelete(ITableSpec tableSpec, dynamic oldRecord)
        {
            _log.Info($"Ignored delete on {tableSpec.Name} ({string.Join(", ", Keys.GetPrimaryKeys((IDictionary<string, object>)oldRecord, tableSpec).Select(k => k.ToString()))})");
            return false;
        }

        #region Anonymisation

        // This is based on the anonymisation script used to create the static anonymised database
        // The following were anonymised there, but not here because they are not being migrated:
        // Candidate, Stakeholder, VacancySearch, ApplicationUnsuccessfulReasonType, CAFFields, NASSupportContact, Application, AdditionalAnswer

        public bool AnonymisePerson(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                int id = newRecord.PersonId;
                var anon = _avmsSyncRepository.GetAnonymousDetails(id);

                newRecord.Title = (object)_avmsSyncRepository.GetPersonTitleTypeIdsByTitleFullName().GetValueOrDefault(anon.TitleWithoutDot, 6); // Need to manually box (possible Dapper bug)
                newRecord.OtherTitle = (newRecord.Title == 6) ? anon.TitleWithoutDot : "";
                newRecord.FirstName = anon.GivenName;
                newRecord.MiddleNames = (id % 3 == 0) ? "" : anon.MothersMaiden; // TODO - better
                newRecord.Surname = anon.Surname;
                newRecord.LandlineNumber = anon.TelephoneNumber + "1"; // TODO
                newRecord.MobileNumber = anon.TelephoneNumber;
                newRecord.Email = anon.EmailAddress;
            }

            return true;
        }

        public bool AnonymiseEmployerContact(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                int id = newRecord.EmployerContactId;
                var anon1 = _avmsSyncRepository.GetAnonymousDetails(id);
                var anon2 = _avmsSyncRepository.GetAnonymousDetails(id + 1);

                // Don't think we're using this, so might as well anonymise it
                newRecord.AddressLine1 = anon1.StreetAddress;
                newRecord.AddressLine2 = "";
                newRecord.AddressLine3 = "";
                newRecord.AddressLine4 = "";
                newRecord.AddressLine5 = null;
                newRecord.Town = anon1.City;
                // TODO: CountyId
                newRecord.PostCode = anon1.ZipCode;
                // TODO: LocalAuthorityId

                newRecord.FaxNumber = anon1.TelephoneNumber; // TODO
                newRecord.AlternatePhoneNumber = anon2.TelephoneNumber;
            }

            return true;
        }

        public bool AnonymiseProviderSite(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                int id = newRecord.ProviderSiteID;
                var anon1 = _avmsSyncRepository.GetAnonymousDetails(id);
                var anon2 = _avmsSyncRepository.GetAnonymousDetails(id + 1);

                newRecord.ContactDetailsAsARecruitmentAgency = $"{anon1.GivenName} {anon1.Surname} on {anon1.TelephoneNumber}";
                newRecord.ContactDetailsForEmployer = $"{anon1.GivenName} on {anon1.TelephoneNumber}";
                newRecord.ContactDetailsForCandidate = $"{anon1.GivenName} on {anon2.TelephoneNumber}";
            }

            return true;
        }

        public bool AnonymiseProviderSiteHistory(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                newRecord.Comment = "[Training provider history]";
            }

            return true;
        }


        public bool AnonymiseVacancy(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                var anon = _avmsSyncRepository.GetAnonymousDetails((int)newRecord.VacancyId);

                newRecord.ContactName = $"{anon.GivenName} {anon.Surname}";
            }

            return true;
        }

        #endregion
    }

    public static class IDictionaryExtensions
    {
        public static V GetValueOrDefault<K, V>(this IDictionary<K,V> dict, K key, V defaultValue)
        {
            V value;
            if (dict.TryGetValue(key, out value))
                return value;
            else
                return defaultValue;
        }
    }
}
