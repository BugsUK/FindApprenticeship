/*----------------------------------------------------------------------                               
Name		:	dbo.GetVacancyLocationCount
Description :	returns the number of addresses added for the given vacancy
                
History:                  
--------                  
Date			Version		Author			Comment
21-12-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetVacancyLocationCount] (@vacancyId INT)
RETURNS INT
AS BEGIN
	DECLARE @count INT
	SELECT @count = COUNT(*) FROM VacancyLocation WHERE VacancyId = @vacancyId
	
	RETURN @count
END
GO