CREATE PROCEDURE [dbo].[uspGetOccupationsAndAssociatedFrameworks]
	@TotalNumberOfOccupations INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
				
SELECT TOP (100) PERCENT 
			ApprenticeshipOccupation.ApprenticeshipOccupationId, 
			ApprenticeshipOccupation.Codename AS ApprenticeshipOccupationCodeName, 
			ApprenticeshipOccupation.ShortName AS ApprenticeshipOccupationShortName, 
			ApprenticeshipOccupation.FullName AS ApprenticeshipOccupationFullName,
			ApprenticeshipOccupation.ApprenticeshipOccupationStatusTypeId AS ApprenticeshipOccupationStatus,
			ApprenticeshipFramework.ApprenticeshipFrameworkId, 
			ApprenticeshipFramework.CodeName AS ApprenticeshipFrameworkCodeName, 
			ApprenticeshipFramework.ShortName AS ApprenticeshipFrameworkShortName, 
			ApprenticeshipFramework.FullName AS ApprenticeshipFrameworkFullName,
			ApprenticeshipFramework.ApprenticeshipFrameworkStatusTypeId AS ApprenticeshipFrameworkStatus
		FROM ApprenticeshipOccupation INNER JOIN
			ApprenticeshipFramework ON ApprenticeshipOccupation.ApprenticeshipOccupationId = ApprenticeshipFramework.ApprenticeshipOccupationId
		ORDER BY dbo.ApprenticeshipOccupation.FullName
		
	SELECT @TotalNumberOfOccupations = COUNT(*)
		FROM (SELECT DISTINCT TOP (100) PERCENT ApprenticeshipOccupation.ApprenticeshipOccupationId
				FROM ApprenticeshipOccupation INNER JOIN
			ApprenticeshipFramework ON ApprenticeshipOccupation.ApprenticeshipOccupationId = ApprenticeshipFramework.ApprenticeshipOccupationId) X
END