CREATE PROCEDURE [dbo].[uspGetFrameworkByFrameworkId]
	@FrameworkId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
	ApprenticeshipFrameworkId,
ApprenticeshipOccupationId,
CodeName,
ShortName,
FullName,
ClosedDate,
ApprenticeshipFrameworkStatusTypeId,
ISNULL(PreviousApprenticeshipOccupationId,0) as PreviousApprenticeshipOccupationId
	
	FROM ApprenticeshipFramework
		WHERE ApprenticeshipFramework.ApprenticeshipFrameworkId = @FrameworkId
END