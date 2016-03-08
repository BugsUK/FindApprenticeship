/*----------------------------------------------------------------------                  
Name  : ReportGetAllPostcodes
Description :  returns ordered unique Employeed trading names   

                
History:                  
--------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
08-Oct-2008		1.01		Ian Emery	now using the PostcodeOutcode table
---------------------------------------------------------------------- */                 

create procedure [dbo].[reportGetAllPostcodes]
(@type int)  -- if type<>1 then put n/a
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
  
  BEGIN TRY  
		if @type=3
		begin

SELECT Outcode  POSTCODE FROM PostcodeOutcode
		end
		else
		begin
	SELECT 'n/a' as 'POSTCODE'
		end
	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END