CREATE PROCEDURE [dbo].[uspVacancyManagerSelectByVacancyManagerId]
@VacancyManagerId INT
AS
BEGIN       
	SET NOCOUNT ON    
	 
	SELECT ' ' AS 'ApprenticeshipFrameworkId' , 
			[VacancyOwnerRelationship].[ContractHolderisEmployer] AS 'ContractHolderisEmployer',
			[VacancyOwnerRelationship].[EmployerId] AS 'EmployerId',
			[VacancyOwnerRelationship].[ManagerisEmployer] AS 'ManagerisEmployer',
			ProviderSiteRelationship.ProviderSiteID AS 'TrainingProviderId',
			[VacancyOwnerRelationship].[VacancyOwnerRelationshipId] AS 'VacancyManagerId',
			IsNull([VacancyOwnerRelationship].[Notes],'') AS 'Notes',
			[VacancyOwnerRelationship].[StatusTypeId] as VacancyStatusTypeId,
            IsNull([VacancyOwnerRelationship].[EmployerDescription], '')  AS 'EmployerDescription',
            IsNull([VacancyOwnerRelationship].[EmployerWebsite],'') AS 'EmployerWebsite',
            [VacancyOwnerRelationship].[EmployerLogoAttachmentId] AS 'EmployerLogoAttachmentId',
            isnull([VacancyOwnerRelationship].[NationWideAllowed],'false') AS 'NationWideAllowed',     
            (
			 CASE WHEN EXISTS (
			 Select Vacancystatusid 
			 FROM VACANCY 
			 WHERE VACANCY.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].VacancyOwnerRelationshipId 
			 AND VACANCY.VacancyStatusId IN (2,6) )THEN 0 ELSE 1 END
			 ) as IsRelationshipDeletable	     
		FROM [dbo].[VacancyOwnerRelationship]  
		INNER JOIN dbo.ProviderSiteRelationship ON VacancyOwnerRelationship.[ProviderSiteID]
		= ProviderSiteRelationship.ProviderSiteID  
			Left Outer Join VacancyProvisionRelationshipStatusType on [VacancyOwnerRelationship].StatusTypeId = VacancyProvisionRelationshipStatusType.VacancyProvisionRelationshipStatusTypeId
		WHERE [VacancyOwnerRelationshipId] = @VacancyManagerId And 
			VacancyProvisionRelationshipStatusType.FullName != 'Deleted'
 
	SET NOCOUNT OFF    
END