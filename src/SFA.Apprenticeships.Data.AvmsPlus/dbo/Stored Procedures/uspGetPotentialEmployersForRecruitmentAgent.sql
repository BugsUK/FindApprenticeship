CREATE PROCEDURE [dbo].[uspGetPotentialEmployersForRecruitmentAgent]
	@ProviderSiteId int,
	@RecruitmentAgentId int,
	@EmployerName VARCHAR(255),
	@PageNo INT=1, 
	@PageSize INT=10, 
	@SortOrder BIT=1,
	@SortByField NVARCHAR (100)='EmployerName'
AS
BEGIN

	IF @EmployerName = '' SET @EmployerName = null

	DECLARE @ProviderId int
	SELECT @ProviderId=ProviderID
	FROM ProviderSiteRelationship 
	WHERE ProviderSiteID = @ProviderSiteId AND ProviderSiteRelationShipTypeID = 1

	/*********Set Page Number**********************************************/                  
	declare @StartRowNo int                  
	declare @EndRowNo int                  
	set @StartRowNo =((@PageNo-1)* @PageSize)+1                   
	set @EndRowNo =(@PageNo * @PageSize)                      
	/***********************************************************************/   

	/**************Total Number of Rows*************************************/            
	DECLARE @TotalRows int            
	SELECT @TotalRows= COUNT(distinct vor.EmployerId)
	FROM VacancyOwnerRelationship vor
		INNER JOIN ProviderSiteRelationship psr on psr.ProviderSiteID = vor.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID=1
		INNER JOIN Employer e on e.Employerid = vor.EmployerId            
		LEFT JOIN VacancyProvisionRelationshipStatusType vprst on vor.StatusTypeId = vprst.VacancyProvisionRelationshipStatusTypeId
		LEFT JOIN EmployerTrainingProviderStatus etps on e.EmployerStatusTypeId = etps.EmployerTrainingProviderStatusId    
	WHERE
		psr.ProviderID = @ProviderId
		AND vprst.FullName != 'Deleted'  
		AND etps.FullName = 'Activated'  
		AND (e.FullName like '%' + @EmployerName + '%' OR @EmployerName IS NULL)  
   
	/***********************************************************************/           

	/*********set the order by**********************************************/                  
	 if @SortOrder = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END                  
	 if @SortOrder = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END             
	/***********************************************************************/   

	DECLARE @results TABLE
	(
		RowNumber INT,
		EmployerId	INT,
		EmployerName NVARCHAR(255)
	);
	

	INSERT INTO @results
	SELECT ROW_NUMBER() OVER( ORDER BY                   
					CASE WHEN @SortByField='EmployerName Asc'  then EmployerName  End ASC,                  
					CASE WHEN @SortByField='EmployerName desc'  then EmployerName  End DESC               
					) as RowNum,  *
		FROM
		(
			SELECT distinct vor.EmployerId, e.FullName as 'EmployerName'
			FROM VacancyOwnerRelationship vor
				INNER JOIN ProviderSiteRelationship psr on psr.ProviderSiteID = vor.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID=1
				INNER JOIN Employer e on e.Employerid = vor.EmployerId            
				LEFT JOIN VacancyProvisionRelationshipStatusType vprst on vor.StatusTypeId = vprst.VacancyProvisionRelationshipStatusTypeId
				LEFT JOIN EmployerTrainingProviderStatus etps on e.EmployerStatusTypeId = etps.EmployerTrainingProviderStatusId    
			WHERE
				psr.ProviderID = @ProviderId
				AND vprst.FullName != 'Deleted'  
				AND etps.FullName = 'Activated'  
				AND (e.FullName like '%' + @EmployerName + '%' OR @EmployerName IS NULL)  
		) e
		
	SELECT 
		@TotalRows as 'TotalRow',
		r.EmployerId,
		r.EmployerName,
		e.Town as 'EmployerTown',
		e.PostCode as 'EmployerPostcode',
		vor.ProviderSiteID,
		ps.TradingName as 'ProviderName',
		ps.Town as 'ProviderTown',
		ps.PostCode as 'ProviderPostcode',
		CASE WHEN ralr.ProviderSiteRelationshipID IS NULL THEN 0 ELSE 1 END AS 'IsLinked',
		COUNT(distinct TempVacancy.VacancyID) AS ActiveVacancies
	FROM @results r
		INNER JOIN Employer e on e.Employerid = r.EmployerId 
		INNER JOIN VacancyOwnerRelationship vor on e.Employerid = vor.EmployerId 
		INNER JOIN ProviderSiteRelationship psr on psr.ProviderSiteID = vor.ProviderSiteID AND psr.ProviderSiteRelationShipTypeID=1 AND psr.ProviderID = @ProviderId
		INNER JOIN ProviderSite ps on ps.ProviderSiteID = psr.ProviderSiteID
		LEFT JOIN ProviderSiteRelationship psrRA on psrRA.ProviderSiteID = @RecruitmentAgentId AND psrRA.ProviderSiteRelationShipTypeID=3 AND psrRA.ProviderID = @ProviderId		
		LEFT JOIN RecruitmentAgentLinkedRelationships ralr on ralr.VacancyOwnerRelationshipID = vor.VacancyOwnerRelationshipId AND ralr.ProviderSiteRelationshipID = psrRA.ProviderSiteRelationshipID
		LEFT JOIN 
				(
					SELECT Vacancy.VacancyID, Vacancy.VacancyManagerID, VacancyOwnerRelationship.VacancyOwnerRelationshipId
					FROM Vacancy 
						INNER JOIN VacancyOwnerRelationship ON VacancyOwnerRelationship.VacancyOwnerRelationshipId = Vacancy.VacancyOwnerRelationshipId 
					WHERE Vacancy.VacancyStatusId in (1,2,3,5)	
				) TempVacancy ON TempVacancy.VacancyManagerID = @RecruitmentAgentId AND TempVacancy.VacancyOwnerRelationshipId = vor.VacancyOwnerRelationshipId
	WHERE
		r.RowNumber BETWEEN @StartRowNo AND @EndRowNo     
	GROUP BY r.RowNumber, r.EmployerId,r.EmployerName, e.Town, e.PostCode, vor.ProviderSiteID, ps.TradingName, ps.Town, ps.PostCode, ralr.ProviderSiteRelationshipID
	ORDER BY r.RowNumber
		
END