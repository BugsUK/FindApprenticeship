/*----------------------------------------------------------------------                               
Name		:	dbo.GetSubmittedDate
Description :	returns the last date a given vacancy was submitted from the history
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetSubmittedDate] (@vacancyId INT)
RETURNS DATE
AS BEGIN
	DECLARE @date DATE
	SELECT @date = MAX(HistoryDate) FROM dbo.VacancyHistory WHERE VacancyId = @vacancyId AND VacancyHistoryEventSubTypeId = 5

	RETURN @date
END