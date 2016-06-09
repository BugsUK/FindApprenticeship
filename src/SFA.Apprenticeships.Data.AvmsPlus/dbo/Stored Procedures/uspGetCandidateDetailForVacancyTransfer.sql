CREATE PROCEDURE uspGetCandidateDetailForVacancyTransfer
 (
       @vacancyXml XML
 )
 AS
 BEGIN        
 SET NOCOUNT ON        
 BEGIN TRY      
 
	DECLARE @vacancyIds TABLE (
	VacancyId int
	)
 
	INSERT INTO @vacancyIds
	SELECT Tbl.Col.value('@VacancyId', 'INT')
	FROM   @vacancyXml.nodes('//Vacancies/Vacancy') Tbl(Col)
	
	SELECT 
		v.VacancyId, 
		a.CandidateId, 
		p.Email,
		Title,
		OtherTitle,
		FirstName,
		MiddleNames,
		Surname 
	FROM 
		@vacancyIds v
		INNER JOIN [Application] a ON v.VacancyId = a.VacancyId
		INNER JOIN Candidate c ON c.CandidateId = a.CandidateId
		INNER JOIN Person p ON p.PersonId = c.PersonId
		INNER JOIN ApplicationStatusType ast ON ast.ApplicationStatusTypeId = a.ApplicationStatusTypeId
		WHERE ast.CodeName  NOT IN ('WTD', 'REJ')
		
    END TRY        
        
    BEGIN CATCH        
  EXEC RethrowError;        
 END CATCH         
        
    SET NOCOUNT OFF        
END