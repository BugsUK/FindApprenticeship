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
	vacancyId int not null,					
	historyDate datetime,
	fullName varchar(25));

	-- date of outcome for Live
	INSERT into @vacancyHistory(vacancyId, historyDate, fullName)	
	SELECT vh.VacancyId, Min(vh.HistoryDate), vst.FullName
	FROM  dbo.VacancyHistory vh
	JOIN dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId
	WHERE (vh.VacancyHistoryEventTypeID = 1) AND (vst.CodeName = 'Lve')-- OR vst.CodeName = 'Ref')
	GROUP BY vh.VacancyId,vst.FullName

	-- date of outcome for Reffered
	INSERT into @vacancyHistory(vacancyId, historyDate, fullName)	
	SELECT vh.VacancyId, vh.HistoryDate, vst.FullName
	FROM  dbo.VacancyHistory vh
	JOIN dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId
	WHERE (vh.VacancyHistoryEventTypeID = 1) AND vst.CodeName = 'Ref'
	
	-- remove vacancies out of range
	SELECT @dateTo = dbo.fngetendOfDay(@dateTo);
	DELETE FROM @vacancyHistory WHERE historyDate not between @dateFrom and @dateTo;

	SELECT V.QAUserName,
	V.VacancyReferenceNumber,
	P.FullName,
	dbo.GetSubmittedDate(V.VacancyID) AS DateSubmitted,
	vh.FullName AS Outcome,
	vh.HistoryDate AS OutComeDate
	from @vacancyHistory vh 
	JOIN Vacancy V ON V.VacancyId = vh.VacancyId	
	JOIN Provider P ON P.ProviderID = V.ContractOwnerId	
	ORDER BY V.VacancyReferenceNumber


