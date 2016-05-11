/*----------------------------------------------------------------------                               
Name		:	dbo.fnStartOfDay
Description :	returns the start of the day
                
History:                  
--------                  
Date			Version		Author		Comment
21-Aug-2008		1.0			Ian Emery	first version
---------------------------------------------------------------------- */                 

Create function  [dbo].[fnGetStartOfDay](@Date	datetime) returns datetime
	
as
BEGIN  
		return  cast(FLOOR(cast(@Date as float)) as datetime)



END