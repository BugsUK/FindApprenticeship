create  PROCEDURE [dbo].[uspGetNotProgressedApplicationsCount]

@ManagingAreaId int,
@DaysNotProgressed int,

@Count int output

AS
BEGIN

SET NOCOUNT ON
	
SELECT

@Count = count(app.ApplicationId) 

FROM

	[VacancyOwnerRelationship] vpr
	 

	INNER JOIN
	[ProviderSite] tp
	ON vpr.ProviderSiteID = tp.ProviderSiteID 

	INNER JOIN
	Vacancy vac
	ON vpr.VacancyOwnerRelationshipId = vac.VacancyOwnerRelationshipId 

	INNER JOIN
	[Application] app
	ON vac.VacancyId = app.VacancyId 

	INNER JOIN
	ApplicationStatusType ast
	ON app.ApplicationStatusTypeId = ast.ApplicationStatusTypeId 

	INNER JOIN
	ApplicationHistory ah
	ON app.ApplicationId = ah.ApplicationId 

WHERE	
	UPPER(ast.CodeName) in ('NEW', 'APP') 
	AND tp.ManagingAreaID = @ManagingAreaId
      
	AND ah.ApplicationHistoryEventSubTypeId = app.ApplicationStatusTypeId      
   AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE())   


SET NOCOUNT OFF
END