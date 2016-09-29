/*----------------------------------------------------------------------                               
Name		:	dbo.GetApplicantCount
Description :	returns total count of applicant for a given vacancy
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetApplicantCount] (@vacancyId INT)
RETURNS INT
AS BEGIN
	DECLARE @count INT
	SELECT @count = COUNT(*) FROM [Application] WHERE VacancyId = @vacancyId AND ApplicationStatusTypeId >= 2
	
	RETURN @count
END