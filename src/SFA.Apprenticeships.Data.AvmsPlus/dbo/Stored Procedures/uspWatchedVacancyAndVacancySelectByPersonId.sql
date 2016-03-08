Create PROCEDURE [dbo].[uspWatchedVacancyAndVacancySelectByPersonId]
(
@personId int,
@PageIndex int,
@PageSize int,
@IsSortAsc bit,
@SortByField nvarchar(100)    
)
AS
BEGIN
SET NOCOUNT ON;

/*************Set the today's date to use for date comparison***********/
-- need to take the time off the date, so we don't get unexpected results when
-- comparing dates
DECLARE @today datetime
SET @today=dbo.fnx_RemoveTime(GetDate())
/************************************************************************/

/*********Set Page Number**********************************************/
declare @StartRowNo int
declare @EndRowNo int
set @StartRowNo =((@PageIndex-1)* @PageSize)+1 
set @EndRowNo =(@PageIndex * @PageSize)    
/***********************************************************************/

/**************Total Number of Rows*************************************/
declare @TotalRows int
select @TotalRows= count(1) 	
FROM dbo.WatchedVacancy wv
		INNER JOIN dbo.Vacancy v ON wv.VacancyId=v.VacancyId
		inner join [VacancyOwnerRelationship] VP on VP.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]  
		LEFT JOIN dbo.ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId=v.ApprenticeshipFrameworkId
		LEFT JOIN dbo.[ProviderSite] tp ON tp.ProviderSiteID = VP.[ProviderSiteID]
		inner join Employer e on e.EmployerId = VP.EmployerId
		left outer JOIN vacancytextfield vacancytextfield1 	on v.[VacancyId] = vacancytextfield1.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Future prospects') 
			left outer JOIN vacancytextfield vacancytextfield2 	on v.[VacancyId] = vacancytextfield2.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Training to be provided') 
			left outer JOIN vacancytextfield vacancytextfield3 	on v.[VacancyId] = vacancytextfield3.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Skills required') 
			left outer JOIN vacancytextfield vacancytextfield4 	on v.[VacancyId] = vacancytextfield4.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Qualification Required') 
			left outer JOIN vacancytextfield vacancytextfield5 	on v.[VacancyId] = vacancytextfield5.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Personal qualities') 
			left outer JOIN vacancytextfield vacancytextfield6 	on v.[VacancyId] = vacancytextfield6.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Reality check') 
			left outer JOIN vacancytextfield vacancytextfield7 	on v.[VacancyId] = vacancytextfield7.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Other important information') 
		INNER JOIN [VacancyStatusType] ON [VacancyStatusType].[VacancyStatusTypeId] = v.[VacancyStatusId]
		LEFT OUTER JOIN (
			SELECT VacancyId, Max([HistoryDate]) as 'HistoryDate'
			FROM VacancyHistory  
			WHERE VacancyId = @PersonId 
				AND VacancyHistoryEventTypeId = (select VacancyHistoryEventTypeId from VacancyHistoryEventType where CodeName = 'STC')
				AND VacancyHistoryEventSubTypeId = (    
					select  VacancyStatusTypeId     
					from    VacancyStatusType     
					where VacancyStatusType.[FullName] = 'Live') 
			GROUP BY VacancyId ) vh on vh.VacancyId = v.VacancyId 
	WHERE
	wv.candidateId=@PersonId AND v.VacancyId NOT IN
		(SELECT vacancyid FROM [Application] INNER JOIN ApplicationStatusType ON 
			[Application].ApplicationStatusTypeId = ApplicationStatusType.ApplicationStatusTypeId
		WHERE CandidateId=@personid AND ApplicationStatusType.FullName IN ('New','Applied','Successful'))
	AND DateDiff(dd,@today,v.ApplicationClosingDate) >= -30
/***********************************************************************/


/*********set the order by**********************************************/            
			            
declare @OrderBywithSort varchar(500)            
			            
if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END       
/***********************************************************************/     

SELECT *,@TotalRows AS TotalRows FROM
( 
SELECT  ROW_NUMBER() OVER( ORDER BY 		
		CASE
			WHEN v.VacancyStatusId=7 THEN 'Withdrawn'
			WHEN (datediff(dd,@today,v.ApplicationClosingDate)) < 0 THEN 'Expired'
		END,
	Case when @SortByField='Vacancy Asc'  then v.Title   End ASC,            
    Case when @SortByField='Vacancy desc'  then v.Title End DESC,
	Case when @SortByField='EmployerName Asc'  then e.FullName   End ASC,            
    Case when @SortByField='EmployerName desc'  then e.FullName End DESC,
	Case when @SortByField='ClosingDate Asc'  then v.ApplicationClosingDate  End ASC,
    Case when @SortByField='ClosingDate Desc'  then v.ApplicationClosingDate  End DESC
	
	) as RowNum,     
		v.ApplicationClosingDate,
		v.ApprenticeshipType,
		v.ApplicationClosingDate as 'ClosingDate',
		v.ApprenticeshipType as 'VacancyType',
		v.Description,
		e.FullName as 'Employer',
		--v.EmployerisAnonymous as 'EmployerAnonymous',
		v.ShortDescription as 'VacancyTitle',
		v.EmployerAnonymousName,
		v.ExpectedDuration,
		v.ExpectedStartDate,
		af.Fullname,
		isnull(vacancytextfield1.[Value],'') as 'FutureProspects',
		v.WorkingWeek,
		--v.ModifiedDate as 'InfoUpToDate',
		v.InterviewsFromDate,
		--v.JobRole,
		isnull(vacancytextfield7.[Value],'') as 'OtherImportantInfo',
		isnull(vacancytextfield5.[Value],'') as 'PersonalQualities',
		isnull(vacancytextfield4.[Value],'') as 'QualificationsRequired',
		isnull(vacancytextfield6.[Value],'') as 'RealityCheck',
		v.ShortDescription,
		isnull(vacancytextfield3.[Value],'') as 'SkillsRequired',
--		v.SuccessRate,
		v.Title,
		v.Town,
		isnull(vacancytextfield2.[Value],'')  as 'Training Available',
		tp.FullName as 'TrainingProvider',
		v.VacancyId,
		vh.[HistoryDate] as 'vacancyposteddate',
		v.VacancyReferenceNumber,
		v.WeeklyWage,
		af.Fullname as 'ApprenticeshipFrameworkName',
		v.ApprenticeshipFrameworkId as 'ApprenticeshipFrameworkId',
		
		CASE
			WHEN v.VacancyStatusId=7 THEN 'Withdrawn'
			WHEN v.VacancyStatusId=9 THEN 'Withdrawn'
			WHEN v.VacancyStatusId=8 THEN 'Completed'
			WHEN (datediff(dd,@today,v.ApplicationClosingDate)) < 0 THEN 'Expired'
		END as TimeToClosingDate

		
	FROM dbo.WatchedVacancy wv
		INNER JOIN dbo.Vacancy v ON wv.VacancyId=v.VacancyId
		inner join [VacancyOwnerRelationship] VP on VP.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]  
		LEFT JOIN dbo.ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId=v.ApprenticeshipFrameworkId
		LEFT JOIN dbo.[ProviderSite] tp ON tp.ProviderSiteID = VP.[ProviderSiteID]
		inner join Employer e on e.EmployerId = VP.EmployerId
		left outer JOIN vacancytextfield vacancytextfield1 	on v.[VacancyId] = vacancytextfield1.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Future prospects') 
			left outer JOIN vacancytextfield vacancytextfield2 	on v.[VacancyId] = vacancytextfield2.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Training to be provided') 
			left outer JOIN vacancytextfield vacancytextfield3 	on v.[VacancyId] = vacancytextfield3.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Skills required') 
			left outer JOIN vacancytextfield vacancytextfield4 	on v.[VacancyId] = vacancytextfield4.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Qualification Required') 
			left outer JOIN vacancytextfield vacancytextfield5 	on v.[VacancyId] = vacancytextfield5.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Personal qualities') 
			left outer JOIN vacancytextfield vacancytextfield6 	on v.[VacancyId] = vacancytextfield6.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Reality check') 
			left outer JOIN vacancytextfield vacancytextfield7 	on v.[VacancyId] = vacancytextfield7.[VacancyId] and vacancytextfield1.Field = 
		(Select vacancytextfieldValueId from vacancytextfieldValue where FullName = 'Other important information') 
		INNER JOIN 
			[VacancyStatusType] ON 
				[VacancyStatusType].[VacancyStatusTypeId] = v.[VacancyStatusId]
		LEFT OUTER JOIN (
			SELECT VacancyId, Max([HistoryDate]) as 'HistoryDate'
			FROM VacancyHistory  
			WHERE VacancyId = @PersonId 
				AND VacancyHistoryEventTypeId = (select VacancyHistoryEventTypeId from VacancyHistoryEventType where CodeName = 'STC')
				AND VacancyHistoryEventSubTypeId = (    
					select  VacancyStatusTypeId     
					from    VacancyStatusType     
					where VacancyStatusType.[FullName] = 'Live') 
			GROUP BY VacancyId ) vh on vh.VacancyId = v.VacancyId 
	  

	WHERE
	wv.candidateId=@PersonId AND v.VacancyId NOT IN
		(SELECT vacancyid FROM [Application] INNER JOIN ApplicationStatusType ON 
			[Application].ApplicationStatusTypeId = ApplicationStatusType.ApplicationStatusTypeId
		WHERE CandidateId=@personid AND ApplicationStatusType.FullName IN ('New','Applied','Successful'))  
	AND DateDiff(dd,@today,v.ApplicationClosingDate) >= -30) as DerivedTable
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo

END