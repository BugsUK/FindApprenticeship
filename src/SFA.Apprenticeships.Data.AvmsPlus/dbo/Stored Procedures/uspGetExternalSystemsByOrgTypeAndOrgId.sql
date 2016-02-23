CREATE PROCEDURE [dbo].[uspGetExternalSystemsByOrgTypeAndOrgId]
	@OrganisationId int,
	@OrganisationType int
AS
SELECT [ID] 'SystemId'
	  ,[SystemCode]
	  ,[SystemName]
	  ,[OrganisationId]
	  ,[OrganisationType]
	  ,[ContactName]
	  ,[ContactEmail]
	  ,[ContactNumber]
	  ,[IsNasDisabled]
	  ,[IsUserEnabled]
	  ,dbo.fnx_GetOrganisationCode(OrganisationId,OrganisationType) 'OrganisationCode'
 FROM [ExternalSystem]
 WHERE OrganisationId = @OrganisationId 
 AND   OrganisationType = @OrganisationType