CREATE PROCEDURE [dbo].[uspVacancySelectReEditByRelationshipId] 
  @providerId int    

AS    
BEGIN    

  SET NOCOUNT ON    

  declare @FilledVacToComplete int
  declare @MissingILRDetails int
  declare @UnfilledVacsClosingDatePassed int
  declare @VacDescRework int
  declare @VacWithNewApp int
  declare @VacWithWithrawnApp int

  -- SELECT count for each task

  -- Filled Vacancies To Complete
  SELECT @FilledVacToComplete = 0

  -- Vacancies with Missing ILR Details
  SELECT @MissingILRDetails = 0

  -- Unfilled Vacancies past their closing date
  SELECT @UnfilledVacsClosingDatePassed = 0

  -- Vacancies requiring rework of their description
  SELECT @VacDescRework = COUNT(1)--select *
  FROM vacancy     
  INNER JOIN [VacancyOwnerRelationship] 
	ON [vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
	AND 
    [VacancyOwnerRelationship].[ManagerIsEmployer] = 0    
  INNER JOIN vacancyStatustype 
	ON Vacancy.vacancyStatusId = vacancyStatustype.vacancyStatusTypeId   
  WHERE [VacancyOwnerRelationship].[ProviderSiteID] = @providerId  
  AND  VacancyStatusType.CodeName != 'Ref'   

  -- Vacancies with new Applications
  SELECT @VacWithNewApp = COUNT(T.VacancyId) 
  FROM (SELECT DISTINCT vacancy.VACANCYID AS 'VacancyId'  
        FROM vacancy     
        INNER JOIN [VacancyOwnerRelationship] ON [vacancy].[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] AND 
		  [VacancyOwnerRelationship].[ManagerIsEmployer] = 0    
        INNER JOIN [Application] ON [Vacancy].[VacancyId] = [Application].[VacancyId]     
        INNER JOIN [ApplicationStatusType] ON [Application].[ApplicationStatusTypeId] = [ApplicationStatusType].[ApplicationStatusTypeId] AND 
		  [ApplicationStatusType].[CodeName] = 'NEW'    
        INNER JOIN vacancyStatustype ON 
			Vacancy.vacancyStatusId = vacancyStatustype.vacancyStatusTypeId   
        WHERE 
			[VacancyOwnerRelationship].[ProviderSiteID] = @providerId  
			AND  VacancyStatusType.CodeName != 'DEL'   
        GROUP BY VACANCY.VACANCYID) AS T

  SELECT @VacWithWithrawnApp = 0

  -- Return all the Task count results
  SELECT @VacWithWithrawnApp            'VacWithWithrawnApp',
         @MissingILRDetails             'MissingILRDetails',
         @UnfilledVacsClosingDatePassed 'UnfilledVacsClosingDatePassed',
         @VacDescRework                 'VacDescRework',
         @VacWithNewApp                 'VacWithNewApp',
         @VacWithWithrawnApp            'VacWithWithrawnApp'
    
  SET NOCOUNT OFF    
END