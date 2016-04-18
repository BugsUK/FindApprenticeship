/*----------------------------------------------------------------------                               
Name		:	dbo.fnStartOfDay
Description :	returns the start of the day
                
History:                  
--------                  
Date			Version		Author		Comment
21-Aug-2008		1.0			Ian Emery	first version
02-Nov-2008		1.01		Ian Emery	Corrected the end of date by adding +1
---------------------------------------------------------------------- */                 

Create function  [dbo].[fnGetEndOfDay](@Date	datetime) returns datetime
	
as
BEGIN  
		return  dateadd(ms,-3,cast(CEILING(cast(@Date as float)+1) as datetime))

END