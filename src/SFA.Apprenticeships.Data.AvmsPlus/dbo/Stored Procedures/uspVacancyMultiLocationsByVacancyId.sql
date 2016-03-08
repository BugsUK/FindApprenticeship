CREATE PROCEDURE [dbo].[uspVacancyMultiLocationsByVacancyId]
	@VacancyId int
AS
	BEGIN       
		SET NOCOUNT ON 
		
Declare @MasterVacancyId as int
Select 	@MasterVacancyId = MASTERVACANCYID from Vacancy where VacancyId =  	@VacancyId
		
			SELECT 
					V1.VACANCYID,
					V1.MASTERVACANCYID,
					V1.TOWN 
			FROM 
					VACANCY V
			INNER JOIN 
					VACANCY V1 ON V1.MASTERVACANCYID = V.VACANCYID

			WHERE V.MASTERVACANCYID = @MasterVacancyId
			
			ORDER BY V1.TOWN ASC
			
		SET NOCOUNT OFF    
	
	END