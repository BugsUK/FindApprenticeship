/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Data
:r ".\Scripts\Data\dbo.County.Upsert.sql"
:r ".\Scripts\Data\dbo.EmployerTrainingProviderStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.ContactPreferenceType.Upsert.sql"
:r ".\Scripts\Data\dbo.PersonType.Upsert.sql"
:r ".\Scripts\Data\dbo.PersonTitleType.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthority.Upsert.sql"
:r ".\Scripts\Data\dbo.ProviderSiteRelationshipType.Upsert.sql"
:r ".\Scripts\Data\Provider.ProviderUserStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyLocationType.Upsert.sql"
:r ".\Scripts\Data\dbo.WageUnit.Upsert.sql"
:r ".\Scripts\Data\dbo.WageType.Upsert.sql"
:r ".\Scripts\Data\dbo.DurationType.Upsert.sql"
:r ".\Scripts\Data\Reference.EducationLevel.Upsert.sql" -- Needs to be before ApprenticeshipType
:r ".\Scripts\Data\dbo.ApprenticeshipType.Upsert.sql"
:r ".\Scripts\Data\dbo.TrainingType.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyStatusType.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyTextFieldValue.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApprenticeshipOccupationStatusType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApprenticeshipFrameworkStatusType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApprenticeshipOccupation.Upsert.sql"
:r ".\Scripts\Data\dbo.ApprenticeshipFramework.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyHistoryEventType.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyReferralCommentsFieldType.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancyProvisionRelationshipStatusType.Upsert.sql"
:r ".\Scripts\Data\Sync.SyncParams.Upsert.sql"
:r ".\Scripts\Data\Reference.StandardSector.Upsert.sql"
:r ".\Scripts\Data\Reference.Standard.Upsert.sql"
:r ".\Scripts\Data\dbo.Person.Upsert.sql"
:r ".\Scripts\Data\dbo.EmployerContact.Upsert.sql"
:r ".\Scripts\Data\dbo.SystemParameters.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthorityGroupType.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthorityGroupPurpose.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthorityGroup.Upsert.sql"
:r ".\Scripts\Data\dbo.LocalAuthorityGroupMembership.Upsert.sql"
:r ".\Scripts\Data\dbo.ApplicationHistoryEvent.Upsert.sql"
:r ".\Scripts\Data\dbo.ApplicationNextAction.Upsert.sql"
:r ".\Scripts\Data\dbo.ApplicationStatusType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApplicationUnsuccessfulReasonType.Upsert.sql"
:r ".\Scripts\Data\dbo.ApplicationWithdrawnOrDeclinedReasonType.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateDisability.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateEthnicOrigin.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateGender.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateHistoryEvent.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.CandidateULNStatus.Upsert.sql"
:r ".\Scripts\Data\dbo.PostcodeOutcode.Upsert.sql"
:r ".\Scripts\Data\dbo.VacancySource.Upsert.sql"
:r ".\Scripts\Data\dbo.Organisation.Upsert.sql"
:r ".\Scripts\Data\dbo.InitialData.AttachedToItemType.sql"
:r ".\Scripts\Data\dbo.InitialData.ExternalServices.sql"
:r ".\Scripts\Data\dbo.InitialData.InterfaceErrorGroupType.sql"
:r ".\Scripts\Data\dbo.InitialData.InterfaceErrorType.sql"
:r ".\Scripts\Data\dbo.InitialData.MessageStatus.sql"
:r ".\Scripts\Data\dbo.RegionalTeams.Upsert.sql"
:r ".\Scripts\Data\dbo.RegionalTeamMappings.Upsert.sql"
:r ".\Scripts\EnableSnapshotIsolation.sql"
