CREATE PROCEDURE [dbo].[ReportGetDivisions]
	@type int  -- if type<>4 then put n/a
AS
BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  
		if @type=4 
		begin

			SELECT 
					DivisionID,
					DivisionFullName AS Division
			FROM 
					vwDivisions
		
		end
		else
		begin
	SELECT -1 as DivisionID, 'n/a' as Division
		end
	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END