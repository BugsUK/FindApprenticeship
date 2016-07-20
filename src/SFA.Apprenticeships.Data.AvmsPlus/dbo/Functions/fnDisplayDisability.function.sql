/*----------------------------------------------------------------------                               
Name		:	dbo.fnDisplayDisability
Description :	returns Disability removing Please Select... 
                
History:                  
--------                  
Date			Version		Author			Comment
25-Nov-2008		1.0			Ian Emery	first version

---------------------------------------------------------------------- */                 

Create function  [dbo].[fnDisplayDisability](@Disability	varchar(100)) returns varchar(100)
	
as
BEGIN 
	declare @DisabilityReturn varchar(100) 
	 	
	IF  (@Disability = 'Please Select...')
	set @DisabilityReturn = 'Not Selected' 
	else set @DisabilityReturn = @Disability 
	
	return @DisabilityReturn

END