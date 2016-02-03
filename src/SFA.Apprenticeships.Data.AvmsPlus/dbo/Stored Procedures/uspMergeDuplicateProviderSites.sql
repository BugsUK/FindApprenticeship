CREATE PROCEDURE [dbo].[uspMergeDuplicateProviderSites]
	@ukprn int, 
	@upin int
AS

BEGIN

DECLARE @newProviderID INT = (SELECT ProviderID FROM provider WHERE UKPRN = @ukprn
								AND UPIN = @upin AND ProviderStatusTypeID = 1);

DECLARE @oldProviderID INT = (SELECT ProviderID FROM provider WHERE UKPRN = @ukprn
								AND UPIN = @upin AND ProviderStatusTypeID != 1);


--First Move the Sites

UPDATE dbo.ProviderSiteRelationship SET ProviderID = @newProviderID WHERE ProviderID = @oldProviderID;


-- now the vacancies

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





DELETE FROM dbo.SectorSuccessRates WHERE ProviderID = @oldProviderID;

DELETE FROM dbo.Provider WHERE ProviderID = @oldProviderID; 
END