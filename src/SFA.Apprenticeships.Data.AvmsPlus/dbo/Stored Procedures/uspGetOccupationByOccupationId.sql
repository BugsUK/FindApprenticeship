CREATE PROCEDURE [dbo].[uspGetOccupationByOccupationId]
	@OccupationId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ApprenticeshipOccupationId, 
		Codename, ShortName, FullName, ClosedDate,
		ApprenticeshipOccupationStatusTypeId as 'StatusType' 
	FROM ApprenticeshipOccupation
	WHERE ApprenticeshipOccupation.ApprenticeshipOccupationId = @OccupationId
END