CREATE PROCEDURE [dbo].[uspConvertOwnedSiteToSubContractor] (@EDSURN INT, @UKPRN INT, @UPIN INT)
AS
BEGIN
DECLARE @ProviderSiteID INT = (SELECT ProviderSiteID FROM dbo.ProviderSite WHERE EDSURN = @EDSURN);

IF @ProviderSiteID IS NULL 
	BEGIN
	RAISERROR('Couldn''t find EDSURN %d in the ProviderSite table, exiting', 16, 1 , @EDSURN);
	RETURN -1
	END
	
--Change the existing relationship from type 1 to type 2
BEGIN TRAN
DECLARE @ExistingPSRID TABLE(ExistingPSRID INT);

UPDATE dbo.ProviderSiteRelationship
SET ProviderSiteRelationShipTypeID = 2
OUTPUT INSERTED.ProviderSiteRelationshipID INTO @ExistingPSRID
WHERE ProviderSiteID = @ProviderSiteID
AND ProviderSiteRelationShipTypeID = 1;


--See if the new Provider exists

DECLARE @ProviderID INT = (SELECT ProviderID FROM dbo.Provider WHERE UKPRN = @UKPRN);

IF @ProviderID IS NULL
	BEGIN
	RAISERROR ('Couldn''t find UKPRN %d in the Provider table, it will be created', 10, 1, @UKPRN);
	DECLARE @InsertedProviderID TABLE (insertedID INT);
	INSERT INTO [Provider]	
           ([UPIN]
           ,[UKPRN]
           ,[FullName]
           ,[TradingName]
           ,[IsContracted]
           ,[ContractedFrom]
           ,[ContractedTo]
           ,[OriginalUPIN]
           )
           OUTPUT INSERTED.ProviderID INTO @InsertedProviderID
     VALUES
     (		@UPIN
           ,@UKPRN    
           ,'To be updated'
           ,'To be updated'
           ,0
           ,null
           ,null
           ,@UPIN
           ) ;
	
	SET @ProviderID = (SELECT insertedID FROM @InsertedProviderID);
	END
	
-- now create the new Type 1 relationship
	DECLARE @InsertedPSRID TABLE (insertedPSRID INT);
	INSERT INTO dbo.ProviderSiteRelationship
	        ( ProviderID ,
	          ProviderSiteID ,
	          ProviderSiteRelationShipTypeID
	        )
			OUTPUT INSERTED.ProviderSiteRelationshipID INTO @InsertedPSRID
	VALUES  ( @ProviderID , 
	          @ProviderSiteID , 
	          1 
	        )

-- now the profile stuff


	INSERT INTO dbo.ProviderSiteLocalAuthority
	        ( ProviderSiteRelationshipID ,
	          LocalAuthorityId
	        )
	SELECT I.insertedPSRID , LocalAuthorityId
	FROM dbo.ProviderSiteLocalAuthority PSLA
	INNER JOIN @ExistingPSRID E ON PSLA.ProviderSiteRelationshipID = E.ExistingPSRID
	CROSS JOIN @InsertedPSRID I

	INSERT INTO dbo.ProviderSiteFramework
	        ( ProviderSiteRelationshipID ,
	          FrameworkId
	        )
	SELECT I.insertedPSRID, FrameworkId
	FROM dbo.ProviderSiteFramework PSF
	INNER JOIN @ExistingPSRID E ON PSF.ProviderSiteRelationshipID = E.ExistingPSRID
	CROSS JOIN @InsertedPSRID I
	
	INSERT INTO dbo.ProviderSiteOffer
	        ( ProviderSiteLocalAuthorityID ,
	          ProviderSiteFrameworkID ,
	          Apprenticeship ,
	          AdvancedApprenticeship ,
	          HigherApprenticeship
	        )
	SELECT NEWPSLA.ProviderSiteLocalAuthorityID, NEWPSF.ProviderSiteFrameworkID,
	OLDPSO.Apprenticeship, OLDPSO.AdvancedApprenticeship, OLDPSO.HigherApprenticeship
	
	FROM
	
	ProviderSiteOffer OLDPSO
	JOIN dbo.ProviderSiteLocalAuthority OLDPSLA
	ON OLDPSO.ProviderSiteLocalAuthorityID = OLDPSLA.ProviderSiteLocalAuthorityID
	JOIN @ExistingPSRID E ON e.ExistingPSRID = OLDPSLA.ProviderSiteRelationshipID
	JOIN dbo.ProviderSiteLocalAuthority NEWPSLA ON OLDPSLA.LocalAuthorityId = NEWPSLA.LocalAuthorityId
	JOIN @InsertedPSRID I ON NEWPSLA.ProviderSiteRelationshipID = I.insertedPSRID
	
	JOIN dbo.ProviderSiteFramework OLDPSF
	ON OLDPSO.ProviderSiteFrameworkID = OLDPSF.ProviderSiteFrameworkID
	JOIN @ExistingPSRID E2 ON e2.ExistingPSRID = OLDPSF.ProviderSiteRelationshipID
	JOIN dbo.ProviderSiteFramework NEWPSF ON OLDPSF.FrameworkId = NEWPSF.FrameworkId
	JOIN @InsertedPSRID I2 ON NEWPSF.ProviderSiteRelationshipID = I2.insertedPSRID
	
	commit
	
END