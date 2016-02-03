CREATE PROCEDURE [dbo].[uspVacancyGetNewReference]         
AS        
BEGIN        
	SET NOCOUNT ON

	Update VacancyReferenceNumber
		Set LastVacancyReferenceNumber = LastVacancyReferenceNumber+1
	
	Select CAST(LastVacancyReferenceNumber AS NVarchar(15)) As VacancyReferenceNumber
		From VacancyReferenceNumber

	SET NOCOUNT OFF
END