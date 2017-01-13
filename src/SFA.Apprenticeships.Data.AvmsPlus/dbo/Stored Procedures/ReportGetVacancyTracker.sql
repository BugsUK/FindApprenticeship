/*----------------------------------------------------------------------                  
Name  : ReportGetVacancyTracker                 
Description :  Returns Actioned Vacancy by Serco between the given dates.
                
History:                  
--------                  
Date			Version		Author			Comment
16-Dec-2016		1.0			Shoma Gujjar	First version
---------------------------------------------------------------------- */ 

CREATE PROCEDURE [dbo].[ReportGetVacancyTracker]
	@dateFrom datetime,
	@dateTo datetime
AS
	set nocount on  
	set transaction isolation level read uncommitted

	DECLARE @vacancyHistory TABLE( 
	vacancyId int not null primary key,					
	historyDate datetime);

	-- date of outcome, whether Live or Referred	
	INSERT into @vacancyHistory(vacancyId, historyDate)	
	SELECT vh.VacancyId, MIN(vh.HistoryDate) 
	FROM  dbo.VacancyHistory vh
	JOIN dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId
	WHERE (vh.VacancyHistoryEventTypeID = 1) AND (vst.CodeName = 'Lve' OR vst.CodeName = 'Ref')
	GROUP BY vh.VacancyId

	-- remove vacancies out of range
	SELECT @dateTo = dbo.fngetendOfDay(@dateTo);
	DELETE FROM @vacancyHistory WHERE historyDate not between @dateFrom and @dateTo;

	SELECT V.QAUserName,
	V.VacancyReferenceNumber,
	P.FullName,
	dbo.GetSubmittedDate(V.VacancyID) AS DateSubmitted,
	VST.FullName AS Outcome,
	vh.HistoryDate AS OutComeDate
	from @vacancyHistory vh 
	JOIN Vacancy V ON V.VacancyId = vh.VacancyId
	JOIN VacancyStatusType VST ON V.VacancyStatusId = VST.VacancyStatusTypeId
	JOIN Provider P ON P.ProviderID = V.ContractOwnerId
	WHERE V.VacancyStatusId IN (2,3)
	ORDER BY V.QAUserName


