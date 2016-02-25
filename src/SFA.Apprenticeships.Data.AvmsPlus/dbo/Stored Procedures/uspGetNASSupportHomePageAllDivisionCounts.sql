CREATE PROCEDURE [dbo].[uspGetNASSupportHomePageAllDivisionCounts]
 @DaysNotProgressed int = 60,   
 @daysFromClosingDateForVacancyNotFilled INT = 10,                  
 @daysFromClosingDateFor0ApplicationVacancies INT = 10,                  
 @numberOfDaysForFilledVacanciesWithOpenApplications INT = 60,                
 @DaysEmployerWithoutTrainingProvider int = -1    
AS
	
 DECLARE @CodeName as char(3)  
	 Set @CodeName = 'CRE'  
	 Declare @DelCodeName as char(3)  
	 SET @DelCodeName = 'DEL'  
	 DECLARE @Status as char(3)  
	 Set @Status = 'SUB'  



	--SELECT DivisionID,DivisionCode, DivisionFullName,DivisionShortName
	--FROM vwdivisions	
	
	

---------------------------------------------------
	
---[uspGetEmployersWithoutTrainingPartnersCount]
 
		SELECT DIV.DivisionId, 
		ISNULL(A.EmployersWithoutTrainingPartnersCount,0) 'EmployersWithoutTrainingPartnersCount', 
		ISNULL(B.PendingVacanciesCount,0) 'PendingVacanciesCount', 
		ISNULL(C.NotProgressedApplicationsCount,0) 'NotProgressedApplicationsCount',
		ISNULL(D.FilledWithOpenApplicationsCount,0) 'FilledWithOpenApplicationsCount', 
		ISNULL(E.ClosingCount,0) 'ClosingCount' , 
		ISNULL(F.ClosedUnfilledCount,0) 'ClosedUnfilledCount',
		ISNULL(G.TotalMessagesCount,0) 'TotalMessagesCount',
		ISNULL(H.ReadMessagesCount,0) 'ReadMessagesCount',
		DIV.DivisionCode,
		DIV.DivisionFullName,
		DIV.DivisionShortName,
		CASE
		 WHEN DIV.DivisionCode = 'NAT' THEN 'ZNAT'
		 ELSE DIV.DivisionFullName
		 
		END AS 'ShortByFullName'
		
		
		FROM 
			(
			SELECT vwdivisions.DivisionID,
				   vwdivisions.DivisionCode, 
				   vwdivisions.DivisionFullName,
				   vwdivisions.DivisionShortName
			FROM   vwdivisions
			) DIV
			LEFT JOIN  
			(
				
    SELECT  ema.DivisionID ,
            COUNT(emp.EmployerId) 'EmployersWithoutTrainingPartnersCount'
    FROM    ( SELECT    EmployerID ,
                        ISNULL(DivisionID, ( SELECT DIvisionID
                                             FROM   dbo.vwManagingAreas
                                             WHERE  DivisionName = 'National'
                                           )) AS DivisionID
              FROM      employer
                        LEFT JOIN vwManagingAreaAndLocalAuthority MALA ON dbo.Employer.LocalAuthorityId = MALA.LocalAuthorityID
                        LEFT JOIN dbo.vwManagingAreas ON MALA.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
            ) ema
            JOIN Employer emp ON ema.EmployerID = emp.EmployerId
            INNER JOIN EmployerHistory eh ON emp.employerid = eh.employerid
            INNER JOIN EmployerHistoryEventType ehet ON eh.[event] = ehet.employerhistoryeventtypeid
    WHERE   emp.employerid NOT IN (
            SELECT  vpr.employerid
            FROM    [VacancyOwnerRelationship] vpr
                    INNER JOIN vacancyprovisionrelationshipStatustype vprst ON vprst.vacancyprovisionrelationshipStatustypeID = vpr.StatusTypeID
            WHERE   UPPER(vprst.CodeName) <> @DelCodeName )
            AND DATEADD(d, @DaysEmployerWithoutTrainingProvider, eh.[date]) < GETDATE()
            AND UPPER(ehet.codename) = @CodeName
            AND emp.EmployerStatusTypeId = 1
			AND eh.Date = (SELECT MAX(eh1.Date) FROM EmployerHistory eh1 WHERE eh1.EmployerId = emp.EmployerId   AND eh1.Event=1 AND DATEADD(d, @DaysEmployerWithoutTrainingProvider, eh1.[date]) <  getdate())
    GROUP BY ema.DivisionID 
			) A
			ON DIV.DivisionID = A.DivisionID
			
			
---------------------------------------------------------------------
---uspGetPendingVacanciesCount

		LEFT JOIN 
		 (
		  
		SELECT  DivisionID,  COUNT(VacancyId) 'PendingVacanciesCount'
        FROM    Vacancy vac
                INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                INNER JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
                INNER JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
                INNER JOIN VacancyStatusType vst ON vac.VacancyStatusId = vst.VacancyStatusTypeId
        WHERE   UPPER(vst.CodeName) = @Status
                
        GROUP BY divisionID     
			 ) B
		  
		  ON DIV.DivisionId = B.DivisionId
		  
		  
 --------------------------------------------------------------------------
 ---uspGetNotProgressedApplicationsCount
		LEFT JOIN 
		 (
			SELECT

DivisionID,  count(app.ApplicationId) 'NotProgressedApplicationsCount'

FROM

	[VacancyOwnerRelationship] vpr
	 

	INNER JOIN
	[ProviderSite] tp
	ON vpr.ProviderSiteID = tp.ProviderSiteID 
	
	INNER JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId

	INNER JOIN
	Vacancy vac
	ON vpr.VacancyOwnerRelationshipId = vac.VacancyOwnerRelationshipId 

	INNER JOIN
	[Application] app
	ON vac.VacancyId = app.VacancyId 

	INNER JOIN
	ApplicationStatusType ast
	ON app.ApplicationStatusTypeId = ast.ApplicationStatusTypeId 

	INNER JOIN
	ApplicationHistory ah
	ON app.ApplicationId = ah.ApplicationId 

WHERE	
	UPPER(ast.CodeName) in ('NEW', 'APP') 
	
      
	AND ah.ApplicationHistoryEventSubTypeId = app.ApplicationStatusTypeId      
   AND ah.ApplicationHistoryEventDate < DATEADD( dd, -@DaysNotProgressed,GETDATE())   

GROUP BY DivisionID

			 
			 ) C
			 
			 ON DIV.DivisionId = C.DivisionId	 	  
 ----------------------------------
 --uspVacancyGetFilledWithOpenApplicationsCount
 
			LEFT JOIN 
			(
			 SELECT vwMA.DivisionId , 
					COUNT(1) 'FilledWithOpenApplicationsCount' 
			 FROM Vacancy vac                    
			 INNER JOIN [VacancyOwnerRelationship] vpr                    
			 ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]        
			 INNER JOIN Employer emp                          
			 ON vpr.EmployerId = emp.EmployerId     
			 INNER JOIN dbo.LocalAuthority LA ON emp.LocalAuthorityId = LA.LocalAuthorityId    
			 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID    
			 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID    
			 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
			 AND LocalAuthorityGroupTypeName = N'Managing Area'
			 LEFT OUTER JOIN [ProviderSite] tp                    
			 on vpr.[ProviderSiteID] = tp.ProviderSiteID                     
			 INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId    
			 JOIN   dbo.vwManagingAreas vwMA ON  tp.ManagingAreaId = vwMA.ManagingAreaId                 
			 WHERE (VST.FullName='Live' OR VST.FullName='Closed')                  
			 AND vac.NumberofPositions =(SELECT COUNT(*)                         
			 FROM dbo.[Application]                        
			 INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId=dbo.[Application].ApplicationStatusTypeId                        
			 WHERE dbo.ApplicationStatusType.FullName ='Successful'                        
			 AND [Application].VacancyId=vac.VacancyId)                   
			 AND vac.VacancyId IN                   
						 (SELECT APPL.VacancyID FROM [Application] APPL INNER JOIN dbo.ApplicationHistory APPHIST ON APPL.ApplicationId = APPHIST.ApplicationId                  
						 INNER JOIN dbo.ApplicationHistoryEvent APPHISTEVT ON APPHIST.ApplicationHistoryEventTypeId = APPHISTEVT.ApplicationHistoryEventId                  
						 INNER JOIN dbo.ApplicationStatusType APPSTAT ON APPHIST.ApplicationHistoryEventSubTypeId = APPSTAT.ApplicationStatusTypeId                  
						 WHERE APPHISTEVT.FullName = 'Status Change'                  
						 and APPL.ApplicationStatusTypeId in(select ApplicationStatusTypeId   
								   from ApplicationStatusType  
								  where CodeName in ('New','App'))  
  
			 AND APPHIST.ApplicationHistoryEventDate <= GETDATE() - @numberOfDaysForFilledVacanciesWithOpenApplications)                  
			 --AND APPHIST.ApplicationHistoryEventDate <= GETDATE() -  60)
			 
			 GROUP BY vwMA.DivisionId
			  ) D
			  on DIV.DivisionId = D.DivisionId
  
--------------------------------------------------------------------------------------------------------------
  --uspVacancyGetClosingCount
			 Left Join 
			 (                      
				SELECT  vwMA.DivisionId, COUNT(1)   'ClosingCount'                   
				FROM Vacancy vac                          
					inner join [VacancyOwnerRelationship] vpr                          
					 on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]                          
					inner join Employer emp                          
					 on vpr.EmployerId = emp.EmployerId  
					left outer join [ProviderSite] tp                          
					 on vpr.[ProviderSiteID] = tp.ProviderSiteID                           
					INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId=VST.VacancyStatusTypeId      
					JOIN   dbo.vwManagingAreas vwMA ON  tp.ManagingAreaId = vwMA.ManagingAreaId                           
				WHERE VST.FullName='Live'                          
				AND vac.ApplyOutsideNAVMS = 0 -- Exclude applications outside NAVMS.                      
				AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) <= @daysFromClosingDateFor0ApplicationVacancies                          
				--AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) <= 20                          
				AND DATEDIFF(dd,GETDATE(),ApplicationClosingDate) >=0                          
				--AND tp.ManagingAreaID  = @ManagingAreaId   
				AND (    
				(SELECT COUNT(1) FROM dbo.[Application] appl WHERE appl.Vacancyid = vac.vacancyid) = (SELECT COUNT(1) FROM dbo.[Application] appl INNER JOIN dbo.ApplicationStatusType applStatus ON applStatus.ApplicationStatusTypeId=appl.ApplicationStatusTypeId WHERE vac.
  
				VacancyId=appl.vacancyid AND ApplStatus.FullName='Withdrawn'))     

				GROUP BY vwMA.DivisionId
			  ) E
			 ON DIV.DivisionId = E.DivisionId

-------------------------------------------------------------------------------------------------------------
---uspVacancyGetClosedUnfilledCount                  
			 Left Outer Join          
			  (
        SELECT  DIvisionID,  COUNT(1) AS 'ClosedUnfilledCount'    
        FROM    Vacancy vac
                INNER JOIN [VacancyOwnerRelationship] vpr ON vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                INNER JOIN Employer emp ON vpr.EmployerId = emp.EmployerId
                JOIN [ProviderSite] tp ON vpr.[ProviderSiteID] = tp.ProviderSiteID
                JOIN dbo.vwManagingAreas ON tp.ManagingAreaID = dbo.vwManagingAreas.ManagingAreaId
                INNER JOIN dbo.VacancyStatusType VST ON vac.VacancyStatusId = VST.VacancyStatusTypeId
        WHERE   VST.CodeName IN ( 'Lve', 'Cld' )
                AND DATEDIFF(dd, ApplicationClosingDate, GETDATE()) > @daysFromClosingDateForVacancyNotFilled
                AND vac.NumberofPositions != ( SELECT   COUNT(*)
                                               FROM     dbo.[Application]
                                                        INNER JOIN dbo.ApplicationStatusType ON dbo.ApplicationStatusType.ApplicationStatusTypeId = dbo.[Application].ApplicationStatusTypeId
                                               WHERE    dbo.ApplicationStatusType.FullName = 'Successful'
                                                        AND [Application].VacancyId = vac.VacancyId
                                             )
               GROUP BY divisionid        
  
			   ) F
   
			   ON DIV.DivisionId = F.DivisionId         
			   
-- Total Messages
			LEFT JOIN (
						SELECT ma.DivisionId, Count(1) as 'TotalMessagesCount'
						FROM Message m
							INNER JOIN UserType on m.RecipientType = UserType.UserTypeId
							INNER JOIN vwManagingAreas ma on ma.ManagingAreaId = m.Recipient
						WHERE 
							UserType.CodeName = 'SUP' 
							AND IsDeleted = 0
						GROUP By ma.DivisionId 
	
					) G on DIV.DivisionId = G.DivisionId
			-- Read Messages
			LEFT JOIN (
						SELECT ma.DivisionId, Count(1) as 'ReadMessagesCount'
						FROM Message m
							INNER JOIN UserType on m.RecipientType = UserType.UserTypeId
							INNER JOIN vwManagingAreas ma on ma.ManagingAreaId = m.Recipient
						WHERE 
							UserType.CodeName = 'SUP' 
							AND IsDeleted = 0
							AND IsRead = 1
						GROUP By ma.DivisionId 
	
					) H on DIV.DivisionId = H.DivisionId			   
			   
ORDER BY 'ShortByFullName'
ASC
  
RETURN 0