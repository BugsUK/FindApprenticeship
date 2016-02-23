CREATE PROCEDURE [dbo].[uspMoveSitesToNewProvider]
	@oldUKPRN int, 
	@oldUPIN int,
	@newUKPRN int,
	@newUPIN int	
AS
BEGIN



DECLARE @oldProviderID INT = (SELECT ProviderID FROM dbo.Provider WHERE UPIN = @oldUPIN
									AND UKPRN = @oldUKPRN);

IF @oldProviderID IS NULL 
	BEGIN
	RAISERROR('Couldn''t find Old UKPRN %d with UPIN %d in the Provider table, exiting', 
				16, 1 , @oldUKPRN,@oldUPIN );
	RETURN -1
	END

DECLARE @newProviderID INT = (SELECT ProviderID FROM dbo.Provider WHERE UPIN = @newUPIN
									AND UKPRN = @newUKPRN);

IF @newProviderID IS NULL
	BEGIN
	RAISERROR('Couldn''t find New UKPRN %d with UPIN %d in the Provider table, the existing UKPRN %d will be updated to %d', 
				10, 1 , @newUKPRN,@newUPIN, @oldUKPRN, @newUKPRN );

	UPDATE dbo.PROVIDER SET UKPRN = @newUKPRN WHERE UPIN = @oldUPIN AND UKPRN = @oldUKPRN; 
	RETURN 0
	END

-- Move the relationships to the new ProviderID

UPDATE dbo.ProviderSiteRelationship
SET ProviderID = @newProviderID 
OUTPUT DELETED.*, INSERTED.*
WHERE ProviderID = @oldProviderID;



IF NOT EXISTS (SELECT * FROM dbo.SectorSuccessRates WHERE ProviderID = @newProviderID)
	BEGIN
	
	UPDATE dbo.SectorSuccessRates SET ProviderID = @newProviderID
	OUTPUT DELETED.*, INSERTED.*
	WHERE ProviderID = @oldProviderID;
	
	END  


SELECT * FROM vacancy WHERE contractownerid = @oldProviderID;

UPDATE Vacancy
SET ContractOwnerID = @NewProviderID

WHERE ContractOwnerID = @OldProviderID;

SELECT * FROM vacancy WHERE contractownerid = @NewProviderID;

UPDATE dbo.Provider SET OriginalUPIN =
		 (SELECT OriginalUPIN FROM dbo.Provider WHERE ProviderID = @oldProviderID)
OUTPUT DELETED.*, INSERTED.*
WHERE dbo.Provider.ProviderID = @newProviderID;



UPDATE Provider SET ProviderStatusTypeID = 2 WHERE ProviderID = @oldProviderID
DELETE FROM dbo.SectorSuccessRates WHERE ProviderID = @oldProviderID;


END