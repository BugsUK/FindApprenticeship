CREATE PROCEDURE [dbo].[uspTrainingProviderOfferInsert]
@LocationId INT=NULL, @FrameworkId INT=NULL, @Apprenticeship BIT=NULL, @AdvancedApprenticeship BIT=NULL, @HigherApprenticeship BIT=NULL, @OfferId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	BEGIN TRY

        INSERT INTO [dbo].[ProviderSiteOffer] (
          ProviderSiteLocalAuthorityID,
          [ProviderSiteFrameworkID],
          Apprenticeship,
          AdvancedApprenticeship,
          HigherApprenticeship
        )
	    VALUES (
          @LocationId,
          @FrameworkId,
          @Apprenticeship,
          @AdvancedApprenticeship,
          @HigherApprenticeship
        )
        SET @OfferId = SCOPE_IDENTITY()
    
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END