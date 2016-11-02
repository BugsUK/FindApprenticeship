/*----------------------------------------------------------------------                               
Name		:	dbo.GetFirstSubmittedDate
Description :	returns the date a given vacancy was QA approved from the history
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetDateQAApproved] (@vacancyId INT)
RETURNS DATE
AS BEGIN
	DECLARE @date DATE
	SELECT TOP 1 @date = HistoryDate FROM VacancyHistory WHERE VacancyId = @vacancyId AND VacancyHistoryEventSubTypeId = 2 ORDER BY HistoryDate

	RETURN @date
END
GO