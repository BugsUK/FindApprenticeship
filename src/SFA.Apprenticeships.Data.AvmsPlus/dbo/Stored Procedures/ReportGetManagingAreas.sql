CREATE PROCEDURE [dbo].[ReportGetManagingAreas]
	@type int  -- if type<>1 then put n/a
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  
		if @type=1 
		begin

			SELECT DISTINCT 
					ManagingAreaId,
					ManagingAreaFullName AS ManagingArea
			FROM 
					vwManagingAreas
			order by 
					ManagingAreaFullName
		end
		else
		begin
	SELECT -1 as ManagingAreaID, 'n/a' as ManagingArea
		end
	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END