CREATE PROCEDURE [dbo].[uspVacancySelectCountByEmployerId]  
 -- Add the parameters for the stored procedure here  
 @EmployerId INT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
-- SELECT COUNT(Employer.EmployerId)  
-- FROM   
-- dbo.Employer Employer INNER JOIN dbo.Vacancy Vacancy  
-- ON Employer.EmployerId=Vacancy.EmployerId  
-- WHERE Employer.EmployerId=@EmployerId  

--Added by Rajesh Kushwah.
select count([VacancyOwnerRelationship].employerid) as column1 from vacancy vc
	inner join [VacancyOwnerRelationship] on [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] = vc.[VacancyOwnerRelationshipId]
	Where [VacancyOwnerRelationship].employerid = @EmployerId

   
   
END