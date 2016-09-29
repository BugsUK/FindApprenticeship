/*----------------------------------------------------------------------                               
Name		:	dbo.GetCreatedDate
Description :	returns the created date from history for a given vacancy
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetCreatedDate] (@vacancyId INT)
RETURNS DATE
AS BEGIN
	DECLARE @date DATE
	SELECT TOP 1 @date = HistoryDate FROM dbo.VacancyHistory WHERE VacancyId = @vacancyId AND VacancyHistoryEventSubTypeId = 1

	RETURN @date
END