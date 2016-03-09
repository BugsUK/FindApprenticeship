CREATE PROCEDURE [dbo].[uspEmployerSICCodesDeleteByEmployerId]
	@employerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM dbo.EmployerSICCodes WHERE EmployerId=@employerId

	SET NOCOUNT OFF
END