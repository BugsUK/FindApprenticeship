CREATE PROCEDURE [dbo].[uspSchoolInsert]

    @SchoolId int OUT,
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

    INSERT INTO 
        [dbo].[School]
        ([URN] ,[SchoolName], [Address], [Address1], [Address2], [Area], [Town], [County], [Postcode], [SchoolNameForSearch])
	
	VALUES 
	    (@URN, @SchoolName, @Address, @Address1, @Address2, @Area, @Town, @County, @Postcode, @SchoolNameForSearch)
    
    SET @SchoolId = SCOPE_IDENTITY()
    
    END TRY

    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
    
END