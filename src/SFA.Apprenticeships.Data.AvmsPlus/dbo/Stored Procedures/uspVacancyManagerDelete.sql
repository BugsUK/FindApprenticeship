CREATE PROCEDURE [dbo].[uspVacancyManagerDelete]              
(          
  @VacancyProvisionRelationshipId int   
)          
AS          
BEGIN          
 SET NOCOUNT ON          
      

update [VacancyOwnerRelationship]          
set StatusTypeId = (
	select 
		MAX([VacancyProvisionRelationshipStatusTypeId] )
	from 
		[VacancyProvisionRelationshipStatusType] 
	where 
		FullName = 'Deleted')      
where [VacancyOwnerRelationshipId] = @VacancyProvisionRelationshipId  

/* Hard Delete All the vacancies related to the Relationship ID*/
--UPDATE Vacancy
--SET VacancyStatusId = 4 
--WHERE ( VacancyStatusId = 1 OR VacancyStatusId = 3 OR VacancyStatusId = 5)
--AND VacancyProvisionRelationshipId = @VacancyProvisionRelationshipId

DECLARE @RowToProcess INT
DECLARE @CurrentRow INT
DECLARE @SelectedVacancy INT


DECLARE @VacancyToDelete Table (RowID INT not null primary key identity(1,1), VacancyId INT)
INSERT INTO @VacancyToDelete 
	SELECT vacancyid FROM vacancy WHERE ( VacancyStatusId = 1 OR VacancyStatusId = 3 OR VacancyStatusId = 5)
	AND [VacancyOwnerRelationshipId] = @VacancyProvisionRelationshipId

SET @RowToProcess  = @@ROWCOUNT
Set @CurrentRow = 0
WHILE @CurrentRow < @RowToProcess 
BEGIN
	Set  @CurrentRow = @CurrentRow + 1 
	SELECT @SelectedVacancy = VacancyID from @VacancyToDelete where RowID = @CurrentRow
	--delete
	EXEC uspVacancyDelete @SelectedVacancy

END

SET NOCOUNT OFF          
END