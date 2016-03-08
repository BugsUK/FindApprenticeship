CREATE PROCEDURE [dbo].[uspSchoolUpdate]
    
    @SchoolId int,
    @URN nvarchar(100),
    @SchoolName nvarchar(120),
    @Address nvarchar(2000) = NULL,
    @Address1 nvarchar(100) = NULL,
    @Address2 nvarchar(100) = NULL,
    @Area nvarchar(100) = NULL,
    @Town nvarchar(100) = NULL,
    @County nvarchar(100) = NULL,
    @Postcode nvarchar(10) = NULL,
    @SchoolNameForSearch nvarchar(120)
    
AS    

BEGIN    
  
  SET NOCOUNT ON   
    
BEGIN TRY    
   
	IF @Address IS NULL SET @Address = 'Address Field NOT used after address was normalised'

	UPDATE [dbo].[School]
	SET [URN] = @URN
		,[SchoolName] = @SchoolName
		,[Address] = @Address
		,[Address1] = @Address1
		,[Address2] = @Address2
		,[Area] = @Area
		,[Town] = @Town
		,[County] = @County
		,[Postcode] = @Postcode
		,[SchoolNameForSearch] = @SchoolNameForSearch

    WHERE [SchoolId] = @SchoolId      

    IF @@ROWCOUNT = 0    
    BEGIN 
       
        RAISERROR('Concurrent update error. Updated aborted.', 16, 2)    
        
    END 
       
END TRY    
    
BEGIN CATCH    
    EXEC RethrowError;    
END CATCH     
    
    SET NOCOUNT OFF    
    
END