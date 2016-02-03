create PROCEDURE [dbo].[uspApplicationSelectByVacIdandCandidateID]   
	@VacancyId int,
	@CandidateId int     
AS  
BEGIN  
  
 SET NOCOUNT ON  
	select ApplicationId
	from [Application]
	Where CandidateId = @CandidateId and VacancyId = @VacancyId
   
 SET NOCOUNT OFF  
END