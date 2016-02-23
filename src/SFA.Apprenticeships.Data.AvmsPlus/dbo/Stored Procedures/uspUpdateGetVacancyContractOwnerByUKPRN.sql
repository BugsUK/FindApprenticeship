CREATE PROCEDURE [dbo].[uspUpdateGetVacancyContractOwnerByUKPRN]
     @OldUKPRN INT, 
     @NewUKPRN INT
AS
BEGIN
	
	SET NOCOUNT ON
	
	DECLARE @OldProviderId int;
	DECLARE @NewProviderId int;

	SELECT @OldProviderId = (SELECT ProviderID FROM Provider WHERE UKPRN = @OldUKPRN AND ProviderStatusTypeID <> 2 )
	SELECT @NewProviderId = (SELECT ProviderID FROM Provider WHERE UKPRN = @NewUKPRN AND ProviderStatusTypeID <> 2 )
	
	
	/*CREATE RECRUITMENT AGENT RELATIONSHIPS */
	INSERT INTO ProviderSiteRelationship
            (ProviderID
            ,ProviderSiteID
            ,ProviderSiteRelationShipTypeID)
     SELECT @NewProviderId
            ,ProviderSiteID
            ,ProviderSiteRelationShipTypeID      
     FROM   ProviderSiteRelationship PSR
     WHERE  PSR.ProviderId = @OldProviderId
     AND    ProviderSiteRelationShipTypeID = 3 -- Recruitment Agent
     AND NOT EXISTS (SELECT * 
                     FROM ProviderSiteRelationship PSR2 
                     WHERE PSR2.ProviderSiteID = PSR.ProviderSiteID
                     AND   PSR2.ProviderSiteRelationShipTypeID = 3 -- Recruitment Agent
                     AND PSR2.ProviderID = @NewProviderId)


	/*CREATE CONTRACTOR SUB CONTRACTOR RELATIONSHIPS */
	INSERT INTO ProviderSiteRelationship
            (ProviderID
            ,ProviderSiteID
            ,ProviderSiteRelationShipTypeID)
     SELECT @NewProviderId
            ,ProviderSiteID
            ,ProviderSiteRelationShipTypeID      
     FROM   ProviderSiteRelationship PSR
     WHERE  PSR.ProviderId = @OldProviderId
     AND    ProviderSiteRelationShipTypeID = 2 -- Subcontractor 
     AND NOT EXISTS (SELECT * 
                     FROM ProviderSiteRelationship PSR2 
                     WHERE PSR2.ProviderSiteID = PSR.ProviderSiteID 
                     AND  (PSR2.ProviderSiteRelationShipTypeID = 1 
                           OR PSR2.ProviderSiteRelationShipTypeID = 2) --need to check that contractor or subcontractor relationship doesn't already exist.
                     AND PSR2.ProviderID = @NewProviderId)

	/*SELECT VACANCIES AFFECTED BY THE UPDATE STATEMENT */
	SELECT VacancyId,
	       VacancyReferenceNumber,
           Title,
           VacancyStatusId
	FROM   Vacancy V
           INNER JOIN VacancyStatusType VST ON V.VacancyStatusId = VST.VacancyStatusTypeId
    WHERE  (
            /* SINGLE SITE VACANCIES */
            (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND V.ContractOwnerID = @OldProviderId
            AND (V.VacancyStatusId = 1
                 OR   V.VacancyStatusId = 2
                 OR   V.VacancyStatusId = 3
                 OR   V.VacancyStatusId = 5
				 OR   V.VacancyStatusId = 4
                 OR   V.VacancyStatusId = 7
                 OR   V.VacancyStatusId = 8
                 OR   V.VacancyStatusId = 9
                 OR   V.VacancyStatusId = 6)
            )
	OR      (
            /* MULTI SITE VACANCIES */
            MasterVacancyId IN  (SELECT VacancyId 
                                 FROM   Vacancy V
                                 WHERE V.VacancyLocationTypeId = 2 AND V.ContractOwnerID = @OldProviderId
                                 AND (V.VacancyStatusId = 1
                                      OR   V.VacancyStatusId = 2
                                      OR   V.VacancyStatusId = 3
                                      OR   V.VacancyStatusId = 5
									  OR   V.VacancyStatusId = 4
                                      OR   V.VacancyStatusId = 7
                                      OR   V.VacancyStatusId = 8
                                      OR   V.VacancyStatusId = 9
                                      OR   V.VacancyStatusId = 6))             
           )
     
        
	/*UPDATE STATEMENT */
	UPDATE V
	SET    V.ContractOwnerID = @NewProviderId
	FROM   Vacancy V
           INNER JOIN VacancyStatusType VST ON V.VacancyStatusId = VST.VacancyStatusTypeId
    WHERE  (
            /* SINGLE SITE VACANCIES */
            (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND V.ContractOwnerID = @OldProviderId
            AND (V.VacancyStatusId = 1
                 OR   V.VacancyStatusId = 2
                 OR   V.VacancyStatusId = 3
                 OR   V.VacancyStatusId = 5
				 OR   V.VacancyStatusId = 4
                 OR   V.VacancyStatusId = 7
                 OR   V.VacancyStatusId = 8
                 OR   V.VacancyStatusId = 9
                 OR   V.VacancyStatusId = 6)
            )
	OR      (
            /* MULTI SITE VACANCIES */
            MasterVacancyId IN  (SELECT VacancyId 
                                 FROM   Vacancy V
                                 WHERE V.VacancyLocationTypeId = 2 AND V.ContractOwnerID = @OldProviderId
                                 AND (V.VacancyStatusId = 1
                                      OR   V.VacancyStatusId = 2
                                      OR   V.VacancyStatusId = 3
                                      OR   V.VacancyStatusId = 5
									  OR   V.VacancyStatusId = 4
                                      OR   V.VacancyStatusId = 7
                                      OR   V.VacancyStatusId = 8
                                      OR   V.VacancyStatusId = 9
                                      OR   V.VacancyStatusId = 6))             
             )

    SET NOCOUNT OFF
    
END