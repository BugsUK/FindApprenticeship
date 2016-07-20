/*----------------------------------------------------------------------                               
Name		:	dbo.fnDisplayEthnicity
Description :	returns ethnicity removing Select All etc and defining those as
				unspecified 
                
History:                  
--------                  
Date			Version		Author			Comment
25-Nov-2008		1.0			Femma Ashraf	first version

---------------------------------------------------------------------- */                 

Create function  [dbo].[fnDisplayEthnicity](@ethnicity	varchar(100)) returns varchar(100)
	
as
BEGIN 
	declare @ethnicityReturn varchar(100) 
	 	
	IF  (@ethnicity = 'Please Select' OR @ethnicity = 'Select a group first' )
	set @ethnicityReturn = 'Unspecified' 
	else set @ethnicityReturn = @ethnicity 
	
	return @ethnicityReturn

END