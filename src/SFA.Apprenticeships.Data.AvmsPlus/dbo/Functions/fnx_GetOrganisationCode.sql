CREATE FUNCTION [dbo].[fnx_GetOrganisationCode]
(
	@OrganisationID INT, @OrgType INT
)
RETURNS INT
AS
BEGIN
	
DECLARE @Code INT 

IF (@OrgType = 1 )
	SELECT @Code = EdsUrn from Employer where EmployerId = @OrganisationID

IF (@OrgType = 2 )
	SELECT @Code = UKPRN from Provider JOIN ProviderSiteRelationship
	ON Provider.ProviderID = ProviderSiteRelationship.ProviderID
	JOIN ProviderSiteRelationshipType on ProviderSiteRelationship.ProviderSiteRelationShipTypeID
	= ProviderSiteRelationshipType.ProviderSiteRelationshipTypeID
	AND ProviderSiteRelationshipTYpe.ProviderSiteRelationshipTYpeName = N'Owner'
	JOIN ProviderSite ON ProviderSiteRelationShip.ProviderSiteID = ProviderSite.ProviderSiteID where ProviderSite.ProviderSiteID = @OrganisationID

IF (@OrgType = 3 )
	SELECT @Code = EDSURN from ThirdParty where ID = @OrganisationID

Return @Code
END