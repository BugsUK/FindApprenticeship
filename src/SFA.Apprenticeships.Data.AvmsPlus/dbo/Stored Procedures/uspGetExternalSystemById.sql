CREATE PROCEDURE [dbo].[uspGetExternalSystemById]
	@SystemId int
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
 WHERE [ID] = @SystemId