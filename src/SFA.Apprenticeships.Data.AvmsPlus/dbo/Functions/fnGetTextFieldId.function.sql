/*----------------------------------------------------------------------                               
Name		:	dbo.GetTextFieldId
Description :	returns the text field if from the given code
                
History:                  
--------                  
Date			Version		Author			Comment
29-09-2016		1.0			Aaron Rhodes	first version

---------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[GetTextFieldId] (@codeName VARCHAR(5))
RETURNS INT
AS BEGIN
	DECLARE @id INT
	SELECT @id = VacancyTextFieldValueId FROM VacancyTextFieldValue WHERE CodeName = @codeName
	
	RETURN @id
END