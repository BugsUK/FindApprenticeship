CREATE PROCEDURE [dbo].[uspRegisteredRecruitmentAgencyProviderEmployerUpdate]
	@ProviderSiteId int,
	@RecruitmentAgentId int,
	@EmployerId int,
	@Linked bit
AS
BEGIN
	-- determine the provider site relationship row for the RA - PS 
	DECLARE @ProviderSiteRelationshipId int
	SELECT @ProviderSiteRelationshipId=psrRA.ProviderSiteRelationshipId 
	FROM ProviderSiteRelationship psrOwner
		INNER JOIN ProviderSiteRelationship psrRA on psrOwner.ProviderID = psrRA.ProviderID 
													AND psrOwner.ProviderSiteRelationShipTypeID = 1
													AND psrRA.ProviderSiteRelationShipTypeID = 3
	WHERE psrOwner.ProviderSiteID = @ProviderSiteId 
		AND psrRA.ProviderSiteID = @RecruitmentAgentId

	-- determine the vacancy owner relationship row for the PS - employer 
	DECLARE @VacancyOwnerRelationshipID int
	SELECT @VacancyOwnerRelationshipID=VacancyOwnerRelationshipID 
	FROM VacancyOwnerRelationship 
	WHERE EmployerId = @EmployerId AND ProviderSiteID = @ProviderSiteId


	IF(@Linked = 1)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM RecruitmentAgentLinkedRelationships WHERE VacancyOwnerRelationshipID=@VacancyOwnerRelationshipID AND ProviderSiteRelationshipId=@ProviderSiteRelationshipId)
			INSERT INTO RecruitmentAgentLinkedRelationships (VacancyOwnerRelationshipID, ProviderSiteRelationshipId) VALUES (@VacancyOwnerRelationshipID, @ProviderSiteRelationshipId)
	END
	ELSE
	BEGIN
		DELETE FROM RecruitmentAgentLinkedRelationships WHERE VacancyOwnerRelationshipID=@VacancyOwnerRelationshipID AND ProviderSiteRelationshipId=@ProviderSiteRelationshipId
	END
END