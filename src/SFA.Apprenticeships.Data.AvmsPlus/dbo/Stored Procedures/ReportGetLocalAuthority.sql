/*----------------------------------------------------------------------                  
Name  : ReportGetLocalAuthority                  
Description :  returns ordered unique Employeed trading names   

                
History:                  
--------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetLocalAuthority]
(@type int)  -- if type<>2 then put n/a
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  
		if @type=2 
		begin

			SELECT DISTINCT 
					LocalAuthorityId,
					FullName as 'LocalAuthority'
			FROM 
					LocalAuthority
			order by 
					FullName
		end
		else
		begin
	SELECT -1 as LocalAuthorityId, 'n/a' as 'LocalAuthority'
		end
	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END