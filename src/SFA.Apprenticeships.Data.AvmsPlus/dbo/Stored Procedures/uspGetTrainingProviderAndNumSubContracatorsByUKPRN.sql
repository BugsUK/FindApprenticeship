CREATE PROCEDURE [dbo].[uspGetTrainingProviderAndNumSubContracatorsByUKPRN]
	@providerId int,
	@SubcontractorUKPRN int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from              
	-- interfering with SELECT statements.              
	SET NOCOUNT ON;              
 
	IF EXISTS (SELECT 1 FROM Provider WHERE UKPRN = @SubcontractorUKPRN AND ProviderStatusTypeID <> 2)
	BEGIN
		DECLARE @SCProviderId INT 
		DECLARE @SCProviderStatus INT

		SELECT @SCProviderId = ProviderID, @SCProviderStatus = CASE WHEN ProviderStatusTypeID = 3 THEN 1 ELSE 0 END FROM Provider WHERE UKPRN = @SubcontractorUKPRN AND ProviderStatusTypeID <> 2

	
		SELECT @SCProviderId as ProviderId, @SCProviderStatus as IsSuspended, ISNULL(COUNT(SC_ProviderSiteID),0) as NumSites
			FROM vwSubContractors
			WHERE ProviderID = @providerId 
				AND SC_ProviderId=@SCProviderId

	END

END