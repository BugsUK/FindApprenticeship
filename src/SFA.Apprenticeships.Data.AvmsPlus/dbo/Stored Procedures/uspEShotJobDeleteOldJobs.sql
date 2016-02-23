CREATE PROCEDURE [dbo].[uspEShotJobDeleteOldJobs]
AS
	SET NOCOUNT ON

	DECLARE @NumDaysToKeep INT
	SELECT @NumDaysToKeep = CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeEShotJobs'
	
	DECLARE @COMStatusId INT
	SELECT @COMStatusId = EShotJobStatusTypeId FROM EShotJobStatusType WHERE CodeName = 'COM'

	BEGIN TRANSACTION
	BEGIN TRY
		DECLARE @temp TABLE ( Id INT )
		INSERT INTO @temp 
		SELECT EShotJobId 
		FROM EShotJob
		WHERE EShotJobStatusId = @COMStatusId AND DateUpdated < DATEADD(DAY, -@NumDaysToKeep, GETDATE())

		DELETE FROM EShotJobItem
		WHERE EShotJobId IN (SELECT Id FROM @temp)
		
		DELETE FROM EShotJob
		WHERE EShotJobId IN (SELECT Id FROM @temp)
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		EXEC RethrowError;      
	END CATCH