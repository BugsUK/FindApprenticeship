CREATE PROCEDURE dbo.uspValidateTransferVacancy   
@vacancyOwnerId     AS INT = -1,  
@contractOwnerId	AS INT = -1,
@vacancyManagerId	AS INT = -1,
@delivererId		AS INT = -1
AS
BEGIN


	DECLARE @Error Table
	(
	ErrorCode INT,
	ErrorMessage NVARCHAR(500) 
	)

	--Error Code 
	-- 1 Suspended provider or provider site.
	-- 2 Cont owner is contracted.
	-- 3 Vacancy owner or Deliverer is owned or sub-contracted.
	-- 4 Vancancy must be recruitment agency.
	-- 5 Vacancy Owner and Deliverer(if different) both owned by Contract owner.	
	
	IF NOT EXISTS (
		SELECT  1
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@vacancyOwnerId))
	BEGIN
		INSERT INTO @Error
		SELECT 0, 'VacancyOwner'
	END		
	
		IF NOT EXISTS (
		SELECT  1
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@vacancyManagerId))
	BEGIN
		INSERT INTO @Error
		SELECT 0, 'VacancyManager'
	END	
	
	IF NOT EXISTS (
		SELECT  1
		FROM	Provider 
		WHERE	ProviderID IN (@contractOwnerId))
	BEGIN
		INSERT INTO @Error
		SELECT 0, 'ContractOwner'
	END	
	
		IF NOT EXISTS (
		SELECT  1
		FROM	ProviderSite 
		WHERE	ProviderSiteID IN (@delivererId))
	BEGIN
		INSERT INTO @Error
		SELECT 0, 'Deliverer' 
	END	
		

	IF EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN EmployerTrainingProviderStatus st ON ps.TrainingProviderStatusTypeId = st.EmployerTrainingProviderStatusId
		WHERE	ProviderSiteID IN (@vacancyOwnerId)AND CodeName = 'SUS' 
	)
	BEGIN
		INSERT INTO @Error
		SELECT 1, 'VacancyOwner' + ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@vacancyOwnerId)
	END

	IF EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN EmployerTrainingProviderStatus st ON ps.TrainingProviderStatusTypeId = st.EmployerTrainingProviderStatusId
		WHERE	ProviderSiteID IN (@vacancyManagerId)
				AND CodeName = 'SUS'
	)
	BEGIN
		INSERT INTO @Error
		SELECT 1, 'VacancyManager' + ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@vacancyManagerId)
	END


	IF EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN EmployerTrainingProviderStatus st ON ps.TrainingProviderStatusTypeId = st.EmployerTrainingProviderStatusId
		WHERE	ProviderSiteID IN (@delivererId)AND CodeName = 'SUS'
	)
	BEGIN
		INSERT INTO @Error
		SELECT 1, 'Deliverer'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@delivererId)
	END

	IF EXISTS
	(
		SELECT  1
		FROM	Provider p
				INNER JOIN EmployerTrainingProviderStatus st ON p.ProviderStatusTypeID = st.EmployerTrainingProviderStatusId
		WHERE	ProviderID = @contractOwnerId
				AND CodeName = 'SUS'
	)
	BEGIN
		INSERT INTO @Error
		SELECT 1, 'ContractOwner'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), UKPRN)
		FROM	Provider
		WHERE	ProviderID = @contractOwnerId
	END

	--/***************************************************************/

	IF EXISTS
	(
		SELECT  1
		FROM	Provider p
		WHERE	ProviderID = @contractOwnerId
				AND IsContracted = 0
	)
	BEGIN
		INSERT INTO @Error
		SELECT 2, 'ContractOwner'+ ',' + ISNULL(TradingName,'Blank') + ',' + CONVERT(varchar(50), UKPRN)
		FROM	Provider
		WHERE	ProviderID = @contractOwnerId
	END

	--/**********************************************************************************/

	IF NOT EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteID
				INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
				INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
		WHERE	ps.ProviderSiteID = @vacancyOwnerId
				AND p.ProviderID = @contractOwnerId
				AND psrt.ProviderSiteRelationshipTypeID IN (1,2)
	)
	BEGIN
		INSERT INTO @Error
		SELECT 3, 'VacancyOwner'+ ',' + ISNULL(ps.TradingName,'Blank')  + ',' + CONVERT(varchar(50),ps.EDSURN) + ',' + ISNULL(p.TradingName,'Blank') + ',' + CONVERT(varchar(50), UKPRN)
		FROM	ProviderSite ps
				INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteID				
				INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
				INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
		WHERE	ps.ProviderSiteID IN (@vacancyOwnerId)
				AND p.ProviderID = @contractOwnerId
	END


	IF NOT EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
				INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
				INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
		WHERE	ps.ProviderSiteID IN (@delivererId)
				AND p.ProviderID = @contractOwnerId
				AND psrt.ProviderSiteRelationshipTypeID IN (1,2)
	)
	BEGIN
		INSERT INTO @Error
		SELECT 3, 'Deliverer'+ ',' + ISNULL(ps.TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN) + ',' + p.TradingName + ',' + CONVERT(varchar(50), UKPRN)
		FROM	ProviderSite ps
				INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
				INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
				INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
		WHERE	ps.ProviderSiteID IN (@delivererId)
				AND p.ProviderID = @contractOwnerId
	END

	/**********************************************************************************/

	IF NOT EXISTS
	(
		SELECT  1
		FROM	ProviderSite ps
				INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
				INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
		WHERE	ps.ProviderSiteID IN (@vacancyManagerId) 
				AND (@vacancyManagerId = @vacancyOwnerId OR (psr.ProviderID = @contractOwnerId AND psrt.ProviderSiteRelationShipTypeID IN (3)))
				
	)
	BEGIN
		INSERT INTO @Error
		SELECT 4, 'VacancyManager'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
		FROM	ProviderSite
		WHERE	ProviderSiteID IN (@vacancyManagerId)
	END

	/**************************************************************************/

DECLARE @vacancyOwner VARCHAR(500), @deliverer VARCHAR(500)
	
		IF (
			SELECT COUNT(*) FROM (
			SELECT TOP 1 1 as colcnt
			FROM	ProviderSite ps
					INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
					INNER JOIN ProviderSiteRelationShipType psrt 
							ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
					INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
			WHERE	ps.ProviderSiteID IN (@vacancyOwnerId)
					AND p.ProviderID = @contractOwnerId
					AND psrt.ProviderSiteRelationshipTypeID IN (1)
			UNION ALL
			SELECT TOP 1 1 
			FROM	ProviderSite ps
					INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
					INNER JOIN ProviderSiteRelationShipType psrt 
							ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
					INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
			WHERE	ps.ProviderSiteID IN (@delivererId)
					AND p.ProviderID = @contractOwnerId
					AND psrt.ProviderSiteRelationshipTypeID IN (1) )  A)  = 2

			BEGIN
					IF @delivererId <> @vacancyOwnerId
					BEGIN 
						
						SELECT @vacancyOwner = 'VacancyOwner'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
						FROM	ProviderSite
						WHERE	ProviderSiteID IN (@vacancyOwnerId)
						
						SELECT @deliverer = ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
						FROM	ProviderSite
						WHERE	ProviderSiteID IN (@delivererId)
						
						INSERT INTO @Error
						SELECT 5, @vacancyOwner + ',' + @deliverer
						
					END
			
			END	
		ELSE IF NOT EXISTS  (
			SELECT *
			FROM	ProviderSite ps
					INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
					INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
					INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
			WHERE	ps.ProviderSiteID IN (@vacancyOwnerId)
					AND p.ProviderID = @contractOwnerId
					AND psrt.ProviderSiteRelationshipTypeID IN (1,2))
			BEGIN
				
				SELECT @vacancyOwner = 'VacancyOwner'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
				FROM	ProviderSite
				WHERE	ProviderSiteID IN (@vacancyOwnerId)

				INSERT INTO @Error
				SELECT 6, @vacancyOwner 
				
			END					
					
		ELSE IF NOT EXISTS 
			(
			SELECT *
			FROM	ProviderSite ps
					INNER JOIN ProviderSiteRelationship psr ON ps.ProviderSiteId = psr.ProviderSiteId
					INNER JOIN ProviderSiteRelationShipType psrt ON psrt.ProviderSiteRelationShipTypeID = psr.ProviderSiteRelationShipTypeID
					INNER JOIN Provider p ON p.ProviderId = psr.ProviderId
			WHERE	ps.ProviderSiteID IN (@delivererId)
					AND p.ProviderID = @contractOwnerId
					AND psrt.ProviderSiteRelationshipTypeID IN (1,2) )
			BEGIN
							
				SELECT @deliverer = 'Deliverer'+ ',' + ISNULL(TradingName,'Blank')  + ',' + CONVERT(varchar(50), EDSURN)
				FROM	ProviderSite
				WHERE	ProviderSiteID IN (@delivererId)
				
				INSERT INTO @Error
				SELECT 7, @deliverer
				
			END								
	
	SELECT ErrorCode, ErrorMessage FROM @Error		

END