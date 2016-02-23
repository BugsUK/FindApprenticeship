CREATE PROCEDURE [dbo].[uspEmployerAndEmployerContactSelectByEmployerId] 
	@EmployerId int
AS
BEGIN

	SET NOCOUNT ON

    SELECT E.FullName
	,E.TradingName
	,E.EmployerId	
	,E.NumberOfEmployeesAtSite	
	,Person.Title  
	,Person.FirstName
	,Person.Surname	
    ,Person.OtherTitle -- Changed as per new design - 19-Jun-08
	,Person.LandLineNumber
	,Person.MobileNumber
	,Person.Email
	FROM dbo.Employer E 
	INNER JOIN dbo.Person Person
	ON Person.PersonId=E.PrimaryContact
	WHERE
	E.EmployerId=@EmployerId	
	
END