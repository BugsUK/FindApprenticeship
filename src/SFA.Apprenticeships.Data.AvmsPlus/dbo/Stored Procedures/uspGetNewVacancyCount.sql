create PROCEDURE [dbo].[uspGetNewVacancyCount] 
	@DateFrom DateTime,
	@NewVacancyCount Int = 0 Output 
AS
BEGIN
	SET NOCOUNT ON
	BEGIN
		SELECT @NewVacancyCount = count([vacancy].[vacancyid]) 
		FROM [dbo].[vacancy] with (nolock) 
		INNER JOIN VacancyHistory ON [vacancy].[VacancyId] = [vacancyhistory].[VacancyId]
		WHERE [vacancyhistory].[HistoryDate] >= @DateFrom
	END
	SET NOCOUNT OFF
END