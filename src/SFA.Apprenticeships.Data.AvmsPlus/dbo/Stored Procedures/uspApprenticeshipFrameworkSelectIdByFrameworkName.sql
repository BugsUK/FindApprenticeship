CREATE PROCEDURE [dbo].[uspApprenticeshipFrameworkSelectIdByFrameworkName] 
	-- Add the parameters for the stored procedure here
	@frameworkName varchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ApprenticeshipFrameworkId FROM dbo.ApprenticeshipFramework
	WHERE FullName=@frameworkName
END