CREATE PROCEDURE [dbo].[uspApplicationSelectNewSummaryBySearchCriteria]
@EmployerId INT, @Trainingproviderid INT, @VacancyManagerId int = null, @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='EmployerName'
AS
BEGIN            
            
	SET NOCOUNT ON            
	    
	Declare @StatusNew varchar(20)    
	set @StatusNew  = 'New'  
	Declare @StatusDraft varchar(20)      
	set @StatusDraft  = 'DRF'   
	    
	/*********Set Page Number**********************************************/      
	declare @StartRowNo int      
	declare @EndRowNo int      
	set @StartRowNo =((@PageIndex-1)* @PageSize)+1       
	set @EndRowNo =(@PageIndex * @PageSize)          
	/***********************************************************************/      	  
	  
	/*********set the order by**********************************************/     
	declare @OrderBywithSort varchar(500)              
	                 
	if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END              
	if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END         
	/***********************************************************************/              
  
	SELECT MyTable.*  FROM            
		( 
			select 
				TotalRows = count(1) over(),
				ROW_NUMBER() 
				OVER( 
					ORDER BY  
						Case when @SortByField='EmployerName Asc'  then t2.EmployerName  End ASC,      
						Case when @SortByField='EmployerName desc'  then t2.EmployerName End DESC,      
						Case when @SortByField='VacancyTitle Asc'  then t2.VacancyTitle  End ASC,      
						Case when @SortByField='VacancyTitle desc'  then t2.VacancyTitle End DESC ,   
						Case when @SortByField='FrameworkName Asc'  then t2.FrameworkName  End ASC,      
						Case when @SortByField='FrameworkName desc'  then t2.FrameworkName End DESC ,       
						Case when @SortByField='NumberOfPositions Asc'  then t2.NumberOfPositions  End ASC,      
						Case when @SortByField='NumberOfPositions desc'  then t2.NumberOfPositions End DESC ,    
						Case when @SortByField='ApplicationsCount Asc'  then t2.ApplicationsCount End ASC,      
						Case when @SortByField='ApplicationsCount desc' then t2.ApplicationsCount End DESC   ,
						Case when @SortByField='VacancyStatus Asc'  then t2.VacancyStatus  End ASC,      
						Case when @SortByField='VacancyStatus desc'  then t2.VacancyStatus End DESC ,    
						Case when @SortByField='ClosingDate Asc'  then t2.ClosingDate  End ASC,      
						Case when @SortByField='ClosingDate desc'  then t2.ClosingDate End DESC  , 
						Case when @SortByField='NewApplicationsCount Asc'  then t1.NewApplicationsCount  End ASC,   
						Case when @SortByField='NewApplicationsCount desc'  then  t1.NewApplicationsCount End DESC 
				) as RowNum,    
		    
				t1.VacancyId,
				t1.NewApplicationsCount,
				t2.ApplicationsCount,
				t2.RelationshipId, 
				t2.ClosingDate,    
				t2.EmployerId,
				t2.[ProviderSiteID],
				t2.EmployerName,
				t2.NumberOfPositions,
				t2.VacancyTitle,
				t2.ApprenticeshipFrameworkId,
				t2.FrameworkName,
				t2.ApprenticeshipOccupationId,    
				t2.VacancyStatus,
				t2.StatusId,
				t2.VacancyType    
			FROM 
				(
					SELECT 
							v.VacancyId,
							count(a.ApplicationId) as 'NewApplicationsCount',
							vpr.[VacancyOwnerRelationshipId] as 'RelationshipId'
						FROM [VacancyOwnerRelationship] vpr
							inner join vacancy v on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
							inner join application a on a.vacancyid = v.vacancyid    
							inner join applicationstatustype ast on ast.applicationstatustypeid = a.applicationstatustypeid    
						WHERE ast.CodeName = @StatusNew
						AND (@EmployerId IS NULL AND vpr.managerisemployer = 0) OR vpr.managerisemployer = 1       
						GROUP BY v.vacancyId,vpr.[VacancyOwnerRelationshipId]
				) t1 
				INNER JOIN
				(
					SELECT    
							v.VacancyId,
							count(a.ApplicationId) as 'ApplicationsCount',
							vpr.[VacancyOwnerRelationshipId] as 'RelationshipId',
							v.ApplicationClosingDate as 'ClosingDate',    
							vpr.EmployerId as 'EmployerId', 
							ps.ProviderSiteID as 'ProviderSiteID', 
							E.FullName as 'EmployerName',
							v.NumberOfPositions as 'NumberOfPositions',
							v.Title as 'VacancyTitle',    
							v.ApprenticeshipType as 'VacancyType',
							af.ApprenticeshipFrameworkId  as 'ApprenticeshipFrameworkId',
							af.Fullname as 'FrameworkName',
							af.ApprenticeshipOccupationId as 'ApprenticeshipOccupationId',    
							vst.FullName as 'VacancyStatus',
							v.VacancyStatusId as 'StatusId',
							v.VacancyManagerId    
						FROM [VacancyOwnerRelationship] vpr    
							inner join vacancy v on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
							inner join [application] a on a.vacancyid = v.vacancyid    
							inner join applicationstatustype ast on ast.applicationstatustypeid = a.applicationstatustypeid    
							inner join employer e on e.employerid = vpr.employerid    
							inner join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId    
							inner join VacancyStatusType vst on vst.VacancyStatusTypeId = v.VacancyStatusId  
							inner join ProviderSiteRelationship psr on vpr.[ProviderSiteID]  = PSR.ProviderSiteID
							inner join ProviderSiteRelationshipType psrt on psr.ProviderSiteRelationshipTypeID = psrt.ProviderSiteRelationshipTypeID
							AND psrt.ProviderSiteRelationshipTYpeName = N'Owner'
							JOIN ProviderSite ps ON vpr.ProviderSiteID = ps.ProviderSiteID
						WHERE ast.CodeName != @StatusDraft
						GROUP BY v.vacancyId, vpr.[VacancyOwnerRelationshipId],
							v.ApplicationClosingDate,E.FullName,vpr.EmployerId,ps.[ProviderSiteID],
							v.NumberOfPositions,v.Title,v.ApprenticeshipType,af.ApprenticeshipFrameworkId,af.Fullname,af.ApprenticeshipOccupationId,    
							vst.FullName, v.VacancyStatusId, v.VacancyManagerId    
				) t2

				ON t2.vacancyid = t1.vacancyid and t2.RelationshipId= t1.RelationshipId    
			WHERE     
				(t2.EmployerId = @EmployerId or @EmployerId is null)    
				and    
				(t2.[ProviderSiteID]= @TrainingProviderId or @TrainingProviderId is null)
				and (@VacancyManagerId is null or t2.VacancyManagerId = @VacancyManagerId)
	) as MyTable    

	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo     

	SET NOCOUNT OFF            
END