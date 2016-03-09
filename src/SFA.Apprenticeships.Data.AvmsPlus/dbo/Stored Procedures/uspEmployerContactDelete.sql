CREATE PROCEDURE [dbo].[uspEmployerContactDelete]
--	 @employerId int,
--	@personId int
	@employerContactId int
AS
BEGIN
	SET NOCOUNT ON
	
    DELETE FROM [dbo].[EmployerContact]
	--WHERE [PersonId]=@personId
	WHERE [EmployerContactId]=@employerContactId
    
    SET NOCOUNT OFF
END