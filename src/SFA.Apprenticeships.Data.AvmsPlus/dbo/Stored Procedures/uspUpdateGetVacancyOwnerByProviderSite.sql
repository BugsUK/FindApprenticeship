CREATE PROCEDURE [dbo].[uspUpdateGetVacancyOwnerByProviderSite]
     @OldProviderSiteId INT, 
     @NewProviderSiteId INT
AS
BEGIN
	SET NOCOUNT ON
	    
             --CREATE RELATIONSHIPS FOR NEW PROVIDER SITE BASED ON OLD PROVIDER SITE MAKING SURE THEY DON'T EXIST ALREADY
             INSERT INTO VacancyOwnerRelationship
                     (EmployerId
                     ,ProviderSiteID  
                     ,ContractHolderIsEmployer
                     ,ManagerIsEmployer
                     ,StatusTypeId
                     ,Notes
                     ,EmployerDescription
                     ,EmployerWebsite
                     ,EmployerLogoAttachmentId
                     ,NationWideAllowed)
              SELECT  VOR.EmployerId
                     ,@NewProviderSiteID
                     ,VOR.ContractHolderIsEmployer
                     ,VOR.ManagerIsEmployer
                     ,VOR.StatusTypeId
                     ,VOR.Notes
                     ,VOR.EmployerDescription
                     ,VOR.EmployerWebsite
                     ,VOR.EmployerLogoAttachmentId
                     ,VOR.NationWideAllowed
              FROM VacancyOwnerRelationship  VOR
              WHERE VOR.ProviderSiteID = @OldProviderSiteID
              AND NOT EXISTS (SELECT * 
                              FROM VacancyOwnerRelationship VOR2 
                                         WHERE VOR2.EmployerId = VOR.EmployerId 
                                         AND VOR2.ProviderSiteID = @NewProviderSiteID)
              

			  --SELECT VACANCIES AFFECTED BY UPADTE
			  SELECT V.VacancyId,
			         V.VacancyReferenceNumber,
					 V.Title,
                     V.VacancyStatusId,
					 V.DeliveryOrganisationID,
					 V.VacancyManagerID
              FROM   Vacancy V 
                     JOIN VacancyOwnerRelationship VOR ON V.VacancyOwnerRelationshipId = VOR.VacancyOwnerRelationshipId
                         AND  VOR.ProviderSiteID = @OldProviderSiteID
                     JOIN VacancyOwnerRelationship AS VOR2 ON VOR.EmployerId = VOR2.employerId 
                         AND VOR2.ProviderSiteId = @newProviderSiteId
              WHERE  (
                      /* SINGLE SITE VACANCIES */
                      (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND VOR.ProviderSiteID = @OldProviderSiteID
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
                                                   JOIN VacancyOwnerRelationship VOR ON V.VacancyOwnerRelationshipId = VOR.VacancyOwnerRelationshipId            
                                            WHERE V.VacancyLocationTypeId = 2 AND VOR.ProviderSiteID = @OldProviderSiteID
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

              --NOW UPDATE VACANCY TABLE WITH NEW RELATIONSHIPS
              UPDATE V
              SET    V.VacancyOwnerRelationshipId = VOR2.VacancyOwnerRelationshipId
              FROM   Vacancy V 
                     JOIN VacancyOwnerRelationship VOR ON V.VacancyOwnerRelationshipId = VOR.VacancyOwnerRelationshipId
                         AND  VOR.ProviderSiteID = @OldProviderSiteID
                     JOIN VacancyOwnerRelationship AS VOR2 ON VOR.EmployerId = VOR2.employerId 
                         AND VOR2.ProviderSiteId = @newProviderSiteId
              WHERE  (
                      /* SINGLE SITE VACANCIES */
                      (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND VOR.ProviderSiteID = @OldProviderSiteID
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
                                                   JOIN VacancyOwnerRelationship VOR ON V.VacancyOwnerRelationshipId = VOR.VacancyOwnerRelationshipId
                                            WHERE V.VacancyLocationTypeId = 2 AND VOR.ProviderSiteID = @OldProviderSiteID
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