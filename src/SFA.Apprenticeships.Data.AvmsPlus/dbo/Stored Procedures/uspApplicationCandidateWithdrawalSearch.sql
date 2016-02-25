CREATE PROCEDURE [dbo].[uspApplicationCandidateWithdrawalSearch]
		@providerId int,
		@VacancyManagerId int = null,
		@PageIndex INT=1, 
		@PageSize INT=20, 
		@IsSortAsc BIT=1, 
		@SortByField NVARCHAR (100)='DateWithdrawn'  
AS
BEGIN
	SET NOCOUNT ON;

	declare @StartRowNo int        
	declare @EndRowNo int        
	set @StartRowNo =((@PageIndex-1)* @PageSize)+1         
	set @EndRowNo =(@PageIndex * @PageSize)            
	
	--	Sort order
	declare @OrderBywithSort varchar(500)        
			 
	  if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END;        
	  if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END   ;

	--	CORE QUERY
	WITH CORE as 
	(
		select 
			app.ApplicationId,
			case when p.FirstName is null then '' else (p.FirstName + ' ')  end +
			case when p.middlenames is null then '' else (p.MiddleNames + ' ') end + 
			p.Surname as CandidateName,
			(
			select top 1 ah.ApplicationHistoryEventDate
			from 
			[application] a inner join ApplicationHistory ah on a.ApplicationId=ah.ApplicationId
			inner join 
			dbo.ApplicationHistoryEvent ahe on ahe.ApplicationHistoryEventId=ah.ApplicationHistoryEventTypeId
			where 
			ahe.ApplicationHistoryEventId=1 and ApplicationHistoryEventSubTypeId=4
			and a.applicationid=app.applicationid
			order by ah.ApplicationHistoryEventDate desc
			) as DateWithdrawn,
			e.FullName as EmployerName,
			af.FullName as FrameworkName,
			v.Title as VacancyTitle 
		from [application] app  
		inner join vacancy v on app.VacancyId=v.VacancyId
		inner join [VacancyOwnerRelationship] vpr on vpr.[VacancyOwnerRelationshipId]=v.[VacancyOwnerRelationshipId]
		inner join [ProviderSite] prov on prov.ProviderSiteID=vpr.[ProviderSiteID]
		inner join Candidate c on c.CandidateId=app.CandidateId
		inner join Person p on c.PersonId=p.PersonId
		inner join Employer e on e.EmployerId=vpr.EmployerId
		inner join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId=v.ApprenticeshipFrameworkId
		where 
		app.ApplicationStatusTypeId=4 /*@app_status_id */
		AND
		(
			@VacancyManagerId IS NULL OR v.VacancyManagerId = @VacancyManagerId
		)
		and prov.ProviderSiteID=@providerId
		-- NOTE: CHECK IF ACK = 0 OR 1
		and app.WithdrawalAcknowledged=0
	) 

	select * from 
		(select CORE.*,
				case 	        
					when @SortByField='ApplicationId ASC' then ROW_NUMBER() OVER( ORDER BY ApplicationId ASC)
					when @SortByField='ApplicationId DESC' then ROW_NUMBER() OVER( ORDER BY ApplicationId DESC)
					when @SortByField='CandidateName ASC' then ROW_NUMBER() OVER( ORDER BY CandidateName ASC)
					when @SortByField='CandidateName DESC' then ROW_NUMBER() OVER( ORDER BY CandidateName  DESC)
					when @SortByField='DateWithdrawn ASC' then ROW_NUMBER() over( order by DateWithdrawn ASC)
					when @SortByField='DateWithdrawn DESC' then ROW_NUMBER() over( order by DateWithdrawn DESC)
					when @SortByField='EmployerName ASC' then ROW_NUMBER() OVER( ORDER BY EmployerName ASC)
					when @SortByField='EmployerName DESC' then ROW_NUMBER() OVER( ORDER BY EmployerName DESC)
					when @SortByField='FrameworkName ASC' then ROW_NUMBER() OVER( ORDER BY FrameworkName ASC)
					when @SortByField='FrameworkName DESC' then ROW_NUMBER() OVER( ORDER BY FrameworkName DESC)
					when @SortByField='VacancyTitle ASC' then ROW_NUMBER() OVER( ORDER BY VacancyTitle ASC)
					when @SortByField='VacancyTitle DESC' then ROW_NUMBER() OVER( ORDER BY VacancyTitle DESC)
					else ROW_NUMBER() OVER (ORDER BY DateWithdrawn ASC)
				end as RowNum
		from CORE) OUTER_CORE 
	where RowNum BETWEEN @StartRowNo AND @EndRowNo  
	order by RowNum 
	SET NOCOUNT OFF  
END