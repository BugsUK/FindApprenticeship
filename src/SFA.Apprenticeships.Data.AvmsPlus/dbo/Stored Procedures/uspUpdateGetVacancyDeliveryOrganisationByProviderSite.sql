CREATE PROCEDURE [dbo].[uspUpdateGetVacancyDeliveryOrganisationByProviderSite]
     @OldProviderSiteId INT, 
     @NewProviderSiteId INT
AS
BEGIN
	SET NOCOUNT ON

    /*SELECT VACANCIES AFFECTED BY THE UPDATE STATEMENT */
	SELECT V.VacancyId,
		   V.VacancyReferenceNumber,
		   V.Title,
           V.VacancyStatusId,
		   V.DeliveryOrganisationID,
		   V.VacancyManagerID
	FROM   Vacancy V
           INNER JOIN VacancyStatusType VST ON V.VacancyStatusId = VST.VacancyStatusTypeId
    WHERE  (
            /* SINGLE SITE VACANCIES */
            (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND V.DeliveryOrganisationID = @OldProviderSiteId
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
                                 WHERE V.VacancyLocationTypeId = 2 AND V.DeliveryOrganisationID = @OldProviderSiteId
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
	SET    V.DeliveryOrganisationID = @NewProviderSiteId
	FROM   Vacancy V
           INNER JOIN VacancyStatusType VST ON V.VacancyStatusId = VST.VacancyStatusTypeId
    WHERE  (
            /* SINGLE SITE VACANCIES */
            (V.VacancyLocationTypeId != 2 OR V.VacancyLocationTypeId IS NULL) AND V.DeliveryOrganisationID = @OldProviderSiteId
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
                                 WHERE V.VacancyLocationTypeId = 2 AND V.DeliveryOrganisationID = @OldProviderSiteId
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