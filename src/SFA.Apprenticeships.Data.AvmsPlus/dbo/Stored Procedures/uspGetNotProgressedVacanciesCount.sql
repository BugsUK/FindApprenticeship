CREATE PROCEDURE [dbo].[uspGetNotProgressedVacanciesCount]

@RegionId int,
@DaysNotProgressed int,

@Count int output

AS
BEGIN

SET NOCOUNT ON
	
SELECT @Count = count(*) 
FROM 
	(SELECT DISTINCT vac.VacancyID 
		FROM

		[VacancyOwnerRelationship] vpr
		

		INNER JOIN
		[ProviderSite] tp
		ON vpr.[ProviderSiteID] = tp.ProviderSiteID 

		INNER JOIN
		Vacancy vac
		ON vpr.[VacancyOwnerRelationshipId] = vac.[VacancyOwnerRelationshipId] 

		INNER JOIN
		[Application] app1
		ON vac.VacancyId = app1.VacancyId 

		INNER JOIN
		ApplicationStatusType ast
		ON app1.ApplicationStatusTypeId = ast.ApplicationStatusTypeId 

		INNER JOIN
		ApplicationHistory ah
		ON app1.ApplicationId = ah.ApplicationId 

	WHERE	
		UPPER(ast.CodeName) in ('NEW', 'APP') 
		AND tp.managingareaID = @regionID
		AND ah.ApplicationHistoryEventSubTypeId = app1.ApplicationStatusTypeId
		AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE()) 
	)x

SET NOCOUNT OFF
END