/*----------------------------------------------------------------------                               
Name		:	dbo.GetCommentFieldId
Description :	returns the comment field id for the given codename
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetCommentFieldId] (@codeName VARCHAR(5))
RETURNS INT
AS BEGIN
	DECLARE @id INT
	SELECT @id = VacancyReferralCommentsFieldTypeId FROM VacancyReferralCommentsFieldType WHERE CodeName = @codeName
	
	RETURN @id
END