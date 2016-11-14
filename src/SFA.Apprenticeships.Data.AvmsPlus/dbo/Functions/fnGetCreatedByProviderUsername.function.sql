/*----------------------------------------------------------------------                               
Name		:	dbo.GetCreatedByProviderUsername
Description :	returns the username that created the vacancy
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetCreatedByProviderUsername] (@vacancyId INT)
RETURNS VARCHAR(255)
AS BEGIN
	DECLARE @user VARCHAR(255)
	SELECT TOP 1 @user = UserName FROM VacancyHistory WHERE VacancyId = @vacancyId AND VacancyHistoryEventSubTypeId = 1 ORDER BY HistoryDate

	RETURN @user
END