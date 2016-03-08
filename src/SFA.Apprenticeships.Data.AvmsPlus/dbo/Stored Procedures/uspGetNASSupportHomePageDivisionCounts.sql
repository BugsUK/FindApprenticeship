CREATE PROCEDURE [dbo].[uspGetNASSupportHomePageDivisionCounts]
 @Division int = 0,      
 @DaysNotProgressed int = 60,   
 @nasSupportThresholdForUnSuccessfulCandidate INT = 3,  
 @daysFromClosingDateForVacancyNotFilled INT = 10,                  
 @daysFromClosingDateFor0ApplicationVacancies INT = 10,                  
 @numberOfDaysForFilledVacanciesWithOpenApplications INT = 60,                
 @noApplicationsOnly BIT = 0,  
 @DaysEmployerWithoutTrainingProvider int = -1    
AS      
BEGIN      
	SET NOCOUNT ON    
    
    
    SELECT 
		MA.ManagingAreaId,
		MA.ManagingAreaCodeName,
		MA.ManagingAreaShortName,
		MA.ManagingAreaFullName,
		ISNULL(A.EmployersWithoutTrainingPartnersCount,0) 'EmployersWithoutTrainingPartnersCount',
		ISNULL(B.PendingVacanciesCount,0) 'PendingVacanciesCount',
		ISNULL(C.NotProgressedApplicationsCount,0) 'NotProgressedApplicationsCount',
		ISNULL(D.FilledWithOpenApplicationsCount,0) 'FilledWithOpenApplicationsCount',
		ISNULL(E.ClosingCount,0) 'ClosingCount',
		ISNULL(F.ClosedUnfilledCount,0) 'ClosedUnfilledCount',
		ISNULL(G.TotalMessagesCount,0) 'TotalMessagesCount',
		ISNULL(H.ReadMessagesCount,0) 'ReadMessagesCount'
		
	FROM dbo.vwManagingAreas MA
		LEFT JOIN (
				SELECT  ema.ManagingAreaId ,
						COUNT(emp.EmployerId) 'EmployersWithoutTrainingPartnersCount'
				FROM    ( SELECT    EmployerID ,
									ISNULL(ManagingAreaId, ( SELECT ManagingAreaId
														 FROM   dbo.vwManagingAreas
														 WHERE  DivisionName = 'National'
													   )) AS ManagingAreaId
						  FROM      employer
									LEFT JOIN vwManagingAreaAndLocalAuthority MALA ON dbo.Employer.LocalAuthorityId = MALA.LocalAuthorityID
						) ema
						JOIN Employer emp ON ema.EmployerID = emp.EmployerId
						INNER JOIN EmployerHistory eh ON emp.employerid = eh.employerid
						INNER JOIN EmployerHistoryEventType ehet ON eh.[event] = ehet.employerhistoryeventtypeid
				WHERE   emp.employerid NOT IN (
						SELECT  vpr.employerid
						FROM    [VacancyOwnerRelationship] vpr
								INNER JOIN vacancyprovisionrelationshipStatustype vprst ON vprst.vacancyprovisionrelationshipStatustypeID = vpr.StatusTypeID
						WHERE   UPPER(vprst.CodeName) <> 'DEL' )
						AND DATEADD(d, @DaysEmployerWithoutTrainingProvider, eh.[date]) < GETDATE()
						AND UPPER(ehet.codename) = 'CRE'
						AND emp.EmployerStatusTypeId = 1
						AND eh.Date = (SELECT MAX(eh1.Date) FROM EmployerHistory eh1 WHERE eh1.EmployerId = emp.EmployerId   AND eh1.Event=1 AND DATEADD(d, @DaysEmployerWithoutTrainingProvider, eh1.[date]) <  getdate())
				GROUP BY ema.ManagingAreaId 
						) A ON MA.ManagingAreaId = A.ManagingAreaId
		LEFT JOIN (
				SELECT  tp.ManagingAreaId,  COUNT(VacancyId) 'PendingVacanciesCount'
				FROM    Vacancy vac
						INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
						INNER JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
						INNER JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
						INNER JOIN VacancyStatusType vst ON vac.VacancyStatusId = vst.VacancyStatusTypeId
				WHERE   UPPER(vst.CodeName) = 'SUB'
		                
				GROUP BY tp.ManagingAreaId     
					 ) B ON MA.ManagingAreaId = B.ManagingAreaId
						 
	-- Applications not progressed
		LEFT JOIN (
				SELECT tp.ManagingAreaId,  count(app.ApplicationId) 'NotProgressedApplicationsCount'
				FROM [VacancyOwnerRelationship] vpr
					INNER JOIN [ProviderSite] tp ON vpr.ProviderSiteID = tp.ProviderSiteID 
					INNER JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
					INNER JOIN Vacancy vac ON vpr.VacancyOwnerRelationshipId = vac.VacancyOwnerRelationshipId 
					INNER JOIN [Application] app ON vac.VacancyId = app.VacancyId 
					INNER JOIN ApplicationStatusType ast ON app.ApplicationStatusTypeId = ast.ApplicationStatusTypeId 
					INNER JOIN ApplicationHistory ah ON app.ApplicationId = ah.ApplicationId 
				
				WHERE UPPER(ast.CodeName) in ('NEW', 'APP') 
					AND ah.ApplicationHistoryEventSubTypeId = app.ApplicationStatusTypeId      
					AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE())   
				GROUP BY tp.ManagingAreaId
			 ) C ON MA.ManagingAreaId = C.ManagingAreaId
	
	-- Filled vacanacies with open applications
		LEFT JOIN  (
				SELECT vwMA.ManagingAreaId, COUNT(1) 'FilledWithOpenApplicationsCount' 
				FROM Vacancy vac                    
					INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]        
					INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId     
					INNER JOIN dbo.LocalAuthority LA ON emp.LocalAuthorityId = LA.LocalAuthorityId    
					INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID    
					INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID    
					INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					AND LocalAuthorityGroupTypeName = N'Managing Area'
					LEFT JOIN [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID                     
					INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId    
					INNER JOIN dbo.vwManagingAreas vwMA ON  tp.ManagingAreaId = vwMA.ManagingAreaId                 
				WHERE (VST.FullName='Live' OR VST.FullName='Closed')                  
					AND vac.NumberofPositions =(
							SELECT COUNT(*)                          
							FROM dbo.[Application]             
								INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId=dbo.[Application].ApplicationStatusTypeId                        
							WHERE dbo.ApplicationStatusType.FullName ='Successful'                        
								AND [Application].VacancyId=vac.VacancyId)                   
								AND vac.VacancyId IN (
									SELECT APPL.VacancyID 
									FROM [Application] APPL 
										INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                  
										INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                  
										INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                  
									WHERE APPHISTEVT.FullName = 'Status Change'                  
										AND APPL.ApplicationStatusTypeId in (
											SELECT ApplicationStatusTypeId   
											FROM ApplicationStatusType  
											WHERE CodeName in ('New','App'))  
  										AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications)                  
				GROUP BY vwMA.ManagingAreaId
			  ) D on MA.ManagingAreaId = D.ManagingAreaId			 
	-- Vacancies approaching closing date with 0 applications	 
		LEFT JOIN (                      
				SELECT  vwMA.ManagingAreaId, COUNT(1) AS 'ClosingCount'                   
				FROM Vacancy vac                          
					INNER JOIN VacancyOwnerRelationship vpr ON vac.VacancyOwnerRelationshipId = vpr.VacancyOwnerRelationshipId
					INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId  
					LEFT JOIN ProviderSite tp ON vpr.ProviderSiteID = tp.ProviderSiteID
					INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId      
					INNER JOIN dbo.vwManagingAreas vwMA ON  tp.ManagingAreaId = vwMA.ManagingAreaId                           
				WHERE VST.FullName='Live'                          
					AND vac.ApplyOutsideNAVMS = 0 -- Exclude applications outside NAVMS.                      
					AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) <= @daysFromClosingDateFor0ApplicationVacancies                          
					AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) >=0                          
					AND (    
						(SELECT COUNT(1) 
						 FROM dbo.[Application] appl 
						 WHERE appl.Vacancyid = vac.vacancyid) = 
						 (SELECT COUNT(1) 
						 FROM dbo.[Application] appl 
							INNER JOIN dbo.ApplicationStatusType applStatus ON applStatus.ApplicationStatusTypeId=appl.ApplicationStatusTypeId 
							WHERE vac.VacancyId=appl.vacancyid AND ApplStatus.FullName='Withdrawn'))     

				GROUP BY vwMA.ManagingAreaId
			  ) E ON MA.ManagingAreaId = E.ManagingAreaId	
	-- Vacancies not filled after 60 days from closing date
	LEFT JOIN (
			SELECT  tp.ManagingAreaID, COUNT(1) AS 'ClosedUnfilledCount'    
			FROM    Vacancy vac
					INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
					INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId
					INNER JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
					INNER JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
					INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId = VST.VacancyStatusTypeId
			WHERE   VST.CodeName IN ( 'Lve', 'Cld' )
					AND DATEDIFF(dd, ApplicationClosingDate, GETDATE()) > @daysFromClosingDateForVacancyNotFilled
					AND vac.NumberofPositions != ( SELECT   COUNT(*)
												   FROM     dbo.[Application]
															INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId = dbo.[Application].ApplicationStatusTypeId
												   WHERE    dbo.ApplicationStatusType.FullName = 'Successful'
															AND [Application].VacancyId = vac.VacancyId
												 )
			GROUP BY tp.ManagingAreaID
		   ) F ON MA.ManagingAreaId = F.ManagingAreaId
	-- Total Messages
	LEFt JOIN (
			SELECT Recipient, Count(1) as 'TotalMessagesCount'
			FROM Message
				INNER JOIN UserType on Message.RecipientType = UserType.UserTypeId
			WHERE 
				UserType.CodeName = 'SUP' 
				AND IsDeleted = 0
			GROUP By Recipient 
			) G on MA.ManagingAreaId = G.Recipient
	-- Read Messages
	LEFT JOIN (
			SELECT Recipient, Count(1) as 'ReadMessagesCount'
			FROM Message
				INNER JOIN UserType on Message.RecipientType = UserType.UserTypeId
			WHERE 
				UserType.CodeName = 'SUP' 
				AND IsDeleted = 0
				AND IsRead = 1
			GROUP By Recipient 
			) H on MA.ManagingAreaId = H.Recipient	

	WHERE DivisionId = @Division
	ORDER BY ManagingAreaShortName ASC
      
	SET NOCOUNT OFF      
END