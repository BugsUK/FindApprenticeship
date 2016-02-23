CREATE PROCEDURE [dbo].[uspWatchedVacancySelectByPersonIdAndByVacancyId]     
 @personId int,    
 @vacancyId int    
AS    
BEGIN    
    
 SET NOCOUNT ON    
    
/*************Set the today's date to use for date comparison***********/    
-- need to take the time off the date, so we don't get unexpected results when    
-- comparing dates    
SET DATEFORMAT dmy   
DECLARE @today datetime    
SET @today=dbo.fnx_RemoveTime(GetDate())    
/************************************************************************/    
    
    SELECT     
		  v.ApplicationClosingDate,    
		  v.ApprenticeshipType,    
		  v.ApplicationClosingDate as 'ClosingDate',    
		  v.Description,    
		  e.FullName as 'Employer',    
		--  v.EmployerisAnonymous as 'EmployerAnonymous',    
		  v.EmployerAnonymousName,    
		  v.ExpectedDuration,    
		  v.ExpectedStartDate,    
		  af.Fullname as 'FrameworkName',    
		  isnull(vacancytextfield1.[Value],'') as 'FutureProspects',    
		  v.WorkingWeek,    
		  --v.ModifiedDate as 'InfoUpToDate',    
		  v.InterviewsFromDate,    
		  --v.JobRole,    
		  --v.Location,    
		  isnull(vacancytextfield7.[Value],'') as 'OtherImportantInfo',    
		  isnull(vacancytextfield5.[Value],'') as 'PersonalQualities',    
		  isnull(vacancytextfield4.[Value],'') as 'QualificationsRequired',    
		  isnull(vacancytextfield6.[Value],'') as 'RealityCheck',    
		  v.ShortDescription,    
		  isnull(vacancytextfield3.[Value],'') as 'SkillsRequired',    
		  --v.SuccessRate,    
		  v.Title,    
		  v.Town,    
		  isnull(vacancytextfield2.[Value],'')  as 'Training Available',    
		  tp.FullName as 'TrainingProvider',    
		  v.VacancyId,    
		  vh.[HistoryDate] as 'vacancyposteddate',--v.VacancyPostedDate,    
		  v.VacancyReferenceNumber,    
		  v.WeeklyWage,    
		      
		  CASE    
		   WHEN v.VacancyStatusId=15 THEN 'Withdrawn'    
		   WHEN v.VacancyStatusId=11 THEN 'Suspended'    
		   WHEN (datediff(dd,@today,v.ApplicationClosingDate)) < 0 THEN 'Expired'    
		   WHEN (datediff(dd,@today,v.ApplicationClosingDate)) < 15  THEN CAST(datediff(dd,@today,dbo.fnx_RemoveTime(v.ApplicationClosingDate)) AS varchar(255)) + ' Days Remaining'     
		   WHEN (datediff(dd,@today,v.ApplicationClosingDate)) > 14  THEN 'Closes on ' + CAST(v.ApplicationClosingDate AS varchar(255))    
		   --make sure that these two dates have the same time as well, otherwise anomalies will occur.    
		   WHEN dbo.fnx_RemoveTime(v.ApplicationClosingDate)= @today THEN 'Closes Today'     
		  END as status    
 FROM dbo.WatchedVacancy wv    
	 INNER JOIN dbo.Vacancy v ON v.VacancyId = wv.VacancyId    
	 inner join [VacancyOwnerRelationship] VP on VP.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]  
	 inner join Employer e on e.EmployerId = VP.EmployerId 
	 LEFT JOIN dbo.[ProviderSite] tp ON tp.ProviderSiteID = VP.[ProviderSiteID]
	 LEFT JOIN dbo.ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId    
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
	 INNER JOIN 
		Vacancyhistory vh ON 
			vh.VacancyId = wv.VacancyId  
			AND
			vh.[VacancyHistoryEventSubTypeId] = [VacancyStatusType].[VacancyStatusTypeId] 
			AND 
			vh.[VacancyHistoryEventTypeId] = 1 
			AND 
			[VacancyStatusType].[FullName] = 'Live'
 WHERE wv.[CandidateId]=@personId AND wv.VacancyId =@vacancyId    
    
 SET NOCOUNT OFF    
END