CREATE PROCEDURE [dbo].[uspEmployerStatusChange]
@employerId INT, @statusId INT
AS
UPDATE Employer
	SET EmployerStatusTypeId = @statusId
	WHERE EmployerId =  @employerId

RETURN 0