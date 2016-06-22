namespace SFA.Apprenticeships.Data.Migrate
{
    using SFA.Infrastructure.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SFA.Apprenticeships.Application.Interfaces;

    public interface ITableSpec : ITableDetails
    {
        decimal BatchSizeMultiplier { get; }
        IEnumerable<ITableSpec> DependsOn { get; }

        /// <summary>
        /// Table, OldRecordFromTarget, NewRecordFromSource
        /// OldRecordFromTarget will be null for inserts
        /// NewRecordFromSource will always be set as this is not called for deletes.
        /// Changes can be made to NewRecord to effect a transform 
        /// </summary>
        Action<ITableSpec, dynamic, dynamic> Transform { get; }

        /// <summary>
        /// Table, OldRecordFromTarget, NewRecordFromSource, Operation => Whether to apply the change
        /// OldRecordFromTarget will be null for inserts
        /// NewRecordFromSource will be null for deletes
        /// </summary>
        Func<ITableSpec, dynamic, dynamic, Operation, bool> CanMutate { get; }
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
                var VacancyProvisionRelationshipHistoryEventType = _tables.AddNew("VacancyProvisionRelationshipHistoryEventType", Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var VacancyProvisionRelationshipStatusType = _tables.AddNew("VacancyProvisionRelationshipStatusType", Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var MimeType                           = _tables.AddNew("MIMEType",                               Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var EmployerHistoryEventType           = _tables.AddNew("EmployerHistoryEventType",               Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ProviderSiteHistoryEventType       = _tables.AddNew("ProviderSiteHistoryEventType",           Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var PersonTitleType                    = _tables.AddNew("PersonTitleType",                        Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ContactPreferenceType              = _tables.AddNew("ContactPreferenceType",                  Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var VacancyHistoryEventType            = _tables.AddNew("VacancyHistoryEventType",                Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var PersonType                         = _tables.AddNew("PersonType",                             Unchanged, OwnedByAVUnlessNegativeNoDeletes);

                // See EmployerSicCodes below
                // var SicCode                         = _tables.AddNew("SICCode",                                OwnedByAv, DeleteShouldNeverHappen);

                var EmployerTrainingProviderStatus     = _tables.AddNew("EmployerTrainingProviderStatus",         Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var VacancyTextFieldValue              = _tables.AddNew("VacancyTextFieldValue",                  Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ApprenticeshipType                 = _tables.AddNew("ApprenticeshipType",                     Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ProviderSiteRelationshipType       = _tables.AddNew("ProviderSiteRelationshipType", new string[] { "ProviderSiteRelationshipTypeID" }, Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var VacancyStatusType                  = _tables.AddNew("VacancyStatusType",                      Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ApprenticeshipOccupationStatusType = _tables.AddNew("ApprenticeshipOccupationStatusType",     Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var ApprenticeshipFrameworkStatusType  = _tables.AddNew("ApprenticeshipFrameworkStatusType",      Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var VacancyReferralCommentsFieldType   = _tables.AddNew("VacancyReferralCommentsFieldType",       Unchanged, OwnedByAVUnlessNegativeNoDeletes);

                // Not in diagram
                var County         = _tables.AddNew("County",         Unchanged, OwnedByAVUnlessNegativeNoDeletes);
                var LocalAuthority = _tables.AddNew("LocalAuthority", Unchanged, OwnedByAVUnlessNegativeNoDeletes, County);

                // Other tables
                var AttachedDocument                  = _tables.AddNew("AttachedDocument",          0.1m, TransformAttachedDocument, OwnedByAVUnlessNegative, MimeType);
                var Person                            = _tables.AddNew("Person",                          AnonymisePerson,           OwnedByAVUnlessNegative, PersonTitleType, PersonType);
                var EmployerContact                   = _tables.AddNew("EmployerContact",                 AnonymiseEmployerContact,  OwnedByAVUnlessNegative, ContactPreferenceType, County, LocalAuthority, Person);
                var Employer                          = _tables.AddNew("Employer",                        TransformEmployer,         OwnedByAVUnlessNegative, EmployerTrainingProviderStatus, EmployerContact, County, LocalAuthority);
                var EmployerHistory                   = _tables.AddNew("EmployerHistory",                 Unchanged,                 OwnedByAVUnlessNegative, EmployerHistoryEventType, Employer, County, LocalAuthority);
                var ProviderSite                      = _tables.AddNew("ProviderSite",        new string[] { "ProviderSiteID" },            AnonymiseProviderSite,        OwnedByAVUnlessNegativeNoDeletes, EmployerTrainingProviderStatus, LocalAuthority);
                var ProviderSiteHistory               = _tables.AddNew("ProviderSiteHistory", new string[] { "TrainingProviderHistoryId" }, AnonymiseProviderSiteHistory, OwnedByAVUnlessNegativeNoDeletes, ProviderSiteHistoryEventType, ProviderSite);

                // This isn't really needed, plus records can be deleted (outside of archiving activity) which we don't support yet (also, records get deleted and re-added by the AVMS GUI when editing)
                // var EmployerSicCodes               = _tables.AddNew("EmployerSICCodes",    new string[] { "EmployerSICCodes" },          OwnedByAv, DeleteShouldNeverHappen,         Employer, SicCode);

                var Provider                          = _tables.AddNew("Provider",            new string[] { "ProviderID" },                TransformProvider, OwnedByAVUnlessNegativeNoDeletes, EmployerTrainingProviderStatus);
                var ApprenticeshipOccupation          = _tables.AddNew("ApprenticeshipOccupation",        Unchanged, OwnedByAVUnlessNegativeNoDeletes, ApprenticeshipOccupationStatusType);
                var ApprenticeshipFramework           = _tables.AddNew("ApprenticeshipFramework",         Unchanged, OwnedByAVUnlessNegativeNoDeletes, ApprenticeshipOccupation, ApprenticeshipFrameworkStatusType);

                var VacancyOwnerRelationship          = _tables.AddNew("VacancyOwnerRelationship",        Unchanged, CanMutateVacancyOwnerRelationship,
                    ProviderSite, Employer, AttachedDocument, VacancyProvisionRelationshipStatusType);
                var VacancyOwnerRelationshipHistory   = _tables.AddNew("VacancyOwnerRelationshipHistory", Unchanged, CanMutateVacancyOwnerRelationshipHistory, VacancyOwnerRelationship, VacancyProvisionRelationshipHistoryEventType);

                var ProviderSiteRelationship          = _tables.AddNew("ProviderSiteRelationship",   new string[] { "ProviderSiteRelationshipID" },   Unchanged, OwnedByAVUnlessNegativeNoDeletes, Provider, ProviderSite, ProviderSiteRelationshipType);
                var RecruitmentAgentLinkedRelationships = _tables.AddNew("RecruitmentAgentLinkedRelationships", new string[] { "VacancyOwnerRelationshipID", "ProviderSiteRelationshipID" }, Unchanged, OwnedByAVUnlessNegativeNoDeletes, VacancyOwnerRelationship, ProviderSiteRelationship);
                var SectorSuccessRates                = _tables.AddNew("SectorSuccessRates",         new string[] { "ProviderID", "SectorID" },       Unchanged, OwnedByAVUnlessNegativeNoDeletes, Provider, ApprenticeshipOccupation);

                // Seems to be related to applications rather than vacancies
                // var SubVacancy                     = _tables.AddNew("SubVacancy",                                                                  OwnedByAv, Vacancy);

                var ProviderSiteLocalAuthority        = _tables.AddNew("ProviderSiteLocalAuthority", new string[] { "ProviderSiteLocalAuthorityID" }, Unchanged, OwnedByAVUnlessNegativeNoDeletes, ProviderSiteRelationship);
                var ProviderSiteFramework             = _tables.AddNew("ProviderSiteFramework",      new string[] { "ProviderSiteFrameworkID" },      Unchanged, OwnedByAVUnlessNegativeAllowDeletes, ProviderSiteRelationship, ApprenticeshipFramework);

                // This isn't really needed, plus records can be deleted (outside of archiving activity) which we don't support yet (also, records get deleted and re-added by the AVMS GUI when editing)
                // var ProviderSiteOffer              = _tables.AddNew("ProviderSiteOffer",          new string[] { "ProviderSiteOfferID" },          OwnedByAv, DoDelete,                ProviderSiteLocalAuthority, ProviderSiteFramework);

                if (includeVacancy)
                {
                    // These take a while so don't always do them
                    var Vacancy                 = _tables.AddNew("Vacancy",                                                         0.2m, TransformVacancy, CanMutateVacancy,
                        VacancyOwnerRelationship, ProviderSite, Provider, ApprenticeshipType, VacancyStatusType, ApprenticeshipFramework, County, LocalAuthority);
                    var VacancyHistory          = _tables.AddNew("VacancyHistory",                                                  2.0m, Unchanged, CanMutateVacancyChild,   Vacancy, VacancyHistoryEventType);
                    var VacancyTextField        = _tables.AddNew("VacancyTextField",                                                2.0m, Unchanged, CanMutateVacancyChild,   Vacancy, VacancyTextFieldValue);
                    var VacancyLocation         = _tables.AddNew("VacancyLocation",                                                       Unchanged, CanMutateVacancyChild,   Vacancy, County, LocalAuthority);
                    var AdditionalQuestion      = _tables.AddNew("AdditionalQuestion",                                              2.0m, Unchanged, CanMutateVacancyChild,   Vacancy);
                    var VacancyReferralComments = _tables.AddNew("VacancyReferralComments", new string[] { "VacancyReferralCommentsID" }, 2.0m, Unchanged, CanMutateVacancyChild,   Vacancy, VacancyReferralCommentsFieldType);
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
            public TableSpec AddNew(string tableName, Action<ITableSpec, dynamic, dynamic> transform, Func<ITableSpec, dynamic, dynamic, Operation, bool> canMutate, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, new string[] { tableName + "Id" }, 1, transform, canMutate, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string[] primaryKeys, Action<ITableSpec, dynamic, dynamic> transform, Func<ITableSpec, dynamic, dynamic, Operation, bool> canMutate, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKeys, 1, transform, canMutate, dependsOn);
                All.Add(table);
                return table;
            }

            public TableSpec AddNew(string tableName, string[] primaryKeys, decimal batchSizeMultiplier, Action<ITableSpec, dynamic, dynamic> transform, Func<ITableSpec, dynamic, dynamic, Operation, bool> canMutate, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, primaryKeys, batchSizeMultiplier, transform, canMutate, dependsOn);
                All.Add(table);
                return table;
            }
            public TableSpec AddNew(string tableName, decimal batchSizeMultiplier, Action<ITableSpec, dynamic, dynamic> transform, Func<ITableSpec, dynamic, dynamic, Operation, bool> canMutate, params TableSpec[] dependsOn)
            {
                var table = new TableSpec(tableName, new string[] { tableName + "Id" }, batchSizeMultiplier, transform, canMutate, dependsOn);
                All.Add(table);
                return table;
            }
        }

        private class TableSpec : ITableSpec
        {
            private List<TableSpec> _dependsOn;
            public IEnumerable<ITableSpec> DependsOn { get { foreach (var i in _dependsOn) yield return i; } } // Safe read only enumeration

            public string Name { get; private set; }

            public Action<ITableSpec, dynamic, dynamic> Transform { get; private set; }

            public Func<ITableSpec, dynamic, dynamic, Operation, bool> CanMutate { get; private set; }

            public IEnumerable<string> PrimaryKeys { get; private set; }

            public IEnumerable<string> ErrorKeys => PrimaryKeys;

            public bool IdentityInsert => true;

            public decimal BatchSizeMultiplier { get; private set; }

            public TableSpec(string name, string[] primaryKeys, decimal batchSizeMultiplier, Action<ITableSpec, dynamic, dynamic> transform, Func<ITableSpec, dynamic, dynamic, Operation, bool> canMutate, params TableSpec[] dependsOn)
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
                CanMutate = canMutate;
                _dependsOn = new List<TableSpec>(dependsOn);
            }
        }

        #region ShouldApplyChange methods

        /// <summary>
        /// Tables that are in AV and where records are normally owned by them.
        /// We never edit these records.
        /// </summary>
        public bool OwnedByAVUnlessNegative(ITableSpec table, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            var primaryKeys = Keys.GetPrimaryKeys((IDictionary<string, object>)(oldRecordFromTarget ?? newRecordFromSource), table);

            if (primaryKeys.First() < 0)
            {
                if (operation == Operation.Delete)
                {
                    // Migrate will identify these for deletion as they are not on the source system
                    return false;
                }
                else
                {
                    // If Migrate is trying to insert or update them then are assertion that only we create negative ids is wrong
                    _log.Error($"AVMS somehow has record in {table.Name} for {primaryKeys}");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Tables that are in AV and where records are normally owned by them.
        /// As far as we know they are never deleted and (although this might not have been fully analysed), deletion might fail because we might be using them.
        /// Generally used for reference data.
        /// </summary>
        public bool OwnedByAVUnlessNegativeNoDeletes(ITableSpec table, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            var primaryKeys = Keys.GetPrimaryKeys((IDictionary<string, object>)(oldRecordFromTarget ?? newRecordFromSource), table);

            if (primaryKeys.First() < 0)
            {
                if (operation == Operation.Delete)
                {
                    // Migrate will identify these for deletion as they are not on the source system
                    return false;
                }
                else
                {
                    // If Migrate is trying to insert or update them then are assertion that only we create negative ids is wrong
                    _log.Error($"AVMS somehow has record in {table.Name} for {primaryKeys}");
                    return false;
                }
            }
            else
            {
                if (operation == Operation.Delete)
                {
                    // If Migrate is trying to delete its own records then the assumption it never does is wrong
                    _log.Error($"AVMS unexpectedly trying to delete {table.Name} for {primaryKeys}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Tables that are in AV and where records are normally owned by them.
        /// We know they delete records from here and am pretty sure we don't use these tables so deletion wouldn't cause us any problems.
        /// </summary>
        public bool OwnedByAVUnlessNegativeAllowDeletes(ITableSpec table, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            var primaryKeys = Keys.GetPrimaryKeys((IDictionary<string, object>)(oldRecordFromTarget ?? newRecordFromSource), table);

            if (primaryKeys.First() < 0)
            {
                if (operation == Operation.Delete)
                {
                    // Migrate will identify these for deletion as they are not on the source system
                    return false;
                }
                else
                {
                    // If Migrate is trying to insert or update them then are assertion that only we create negative ids is wrong
                    _log.Error($"AVMS somehow has record in {table.Name} for {primaryKeys}");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool CanMutateVacancyOwnerRelationship(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            // Deletes could potentially be a problem, but since using a VacancyOwnerRelationship in a Vacancy results in editing of the EmployerDescription and setting of EditedInRaa, all is good

            if (oldRecordFromTarget != null && oldRecordFromTarget.EditedInRaa)
            {
                _log.Info($"Ignored {operation} to VacancyOwnerRelationship from AVMS with VacancyOwnerRelationshipId = {oldRecordFromTarget.VacancyOwnerRelationshipId} because EditedInRaa=1");
                return false;
            }

            return true;
        }

        public bool CanMutateVacancyOwnerRelationshipHistory(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            var record = oldRecordFromTarget ?? newRecordFromSource;
            return !_avmsSyncRepository.IsVacancyOwnerRelationshipOwnedByTargetDatabase(record.VacancyOwnerRelationshipId);
        }

        public bool CanMutateVacancy(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            if (operation == Operation.Delete)
            {
                // dbo.Application records (which include VacancyId) are not being deleted to deleting Vacancy will fail sometimes
                // Also it is probably desirable to keep Vacancy records for the moment, HOWEVER note that the child records are being deleted to
                // handle delete/re-add scenarios so the information will be incomplete.
                return false;
            }

            if (oldRecordFromTarget != null && oldRecordFromTarget.EditedInRaa)
            {
                _log.Info($"Ignored change to Vacancy from AVMS with VacancyId = {oldRecordFromTarget.VacancyId} because EditedInRaa=1");
                return false;
            }

            return true;
        }

        public bool CanMutateVacancyChild(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecordFromSource, Operation operation)
        {
            var record = oldRecordFromTarget ?? newRecordFromSource;
            return !_avmsSyncRepository.IsVacancyOwnedByTargetDatabase(record.VacancyId);
        }

        #endregion

        #region Normal transformations

        public void Unchanged(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {

        }

        public void TransformVacancy(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (oldRecord == null)
            {
                newRecord.VacancyGuid = (object)Guid.NewGuid(); // Need to manually box object (possible Dapper bug)
            }
            else
            {
                newRecord.VacancyGuid = oldRecord.VacancyGuid;
            }

            // TODO: Well, this is great, but it still won't be set for uploaded vacancies.
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
            // Required by unsuccessful candidates report and now correctly set in RAA so keeping
            //newRecord.ContractOwnerID         = null;
            //newRecord.OriginalContractOwnerId = null;

            // Believed to be supported by FAA, so don't blank (TODO: Check)
            // newRecord.EmployerAnonymousName = null;
            // newRecord.VacancyManagerAnonymous = false;

            AnonymiseVacancy(tableSpec, oldRecord, newRecord);
        }

        public void TransformEmployer(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            // The old values in this field would not be recognised by our system (although they will probably have timed out)
            newRecord.BeingSupportedBy = null;
            newRecord.LockedForSupportUntil = null;
        }

        public void TransformProvider(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            newRecord.UPIN = null; // We can't always populate this, so always set it to null to prove it isn't used.
        }


        public void TransformAttachedDocument(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            //newRecord.Attachment = new byte[0]; // These are rather large and causing timeouts
        }

        #endregion




        #region Anonymisation transformations

        // This is based on the anonymisation script used to create the static anonymised database
        // The following were anonymised there, but not here because they are not being migrated:
        // Candidate, Stakeholder, VacancySearch, ApplicationUnsuccessfulReasonType, CAFFields, NASSupportContact, Application, AdditionalAnswer

        public void AnonymisePerson(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecord)
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
        }

        public void AnonymiseEmployerContact(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecord)
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
        }

        public void AnonymiseProviderSite(ITableSpec tableSpec, dynamic oldRecordFromTarget, dynamic newRecord)
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
        }

        public void AnonymiseProviderSiteHistory(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                newRecord.Comment = "[Training provider history]";
            }
        }


        public void AnonymiseVacancy(ITableSpec tableSpec, dynamic oldRecord, dynamic newRecord)
        {
            if (_migrateConfig.AnonymiseData)
            {
                var anon = _avmsSyncRepository.GetAnonymousDetails((int)newRecord.VacancyId);

                newRecord.ContactName = $"{anon.GivenName} {anon.Surname}";
            }
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
