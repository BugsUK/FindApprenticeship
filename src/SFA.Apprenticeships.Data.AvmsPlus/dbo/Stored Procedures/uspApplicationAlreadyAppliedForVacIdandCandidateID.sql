CREATE PROCEDURE [dbo].[uspApplicationAlreadyAppliedForVacIdandCandidateID]     
	@VacancyId int,  
	@CandidateId int,       
	@IsApplicationExists bit OUTPUT
AS    
BEGIN    
    
	Set @IsApplicationExists = 0
	
	SET NOCOUNT ON    
	if Exists(Select	ApplicationId  
				From	[Application]  
				Where	CandidateId = @CandidateId And VacancyId = @VacancyId )
				--And		ApplicationStatusTypeId 
				--		in (Select ApplicationStatusTypeId From ApplicationStatusType 
				--			Where CodeName In ('DRF', 'NEW', 'APP', 'SUC')))
		Set @IsApplicationExists = 1	-- True / Applications Exists
	else
		Set @IsApplicationExists = 0	-- False/ Applications Doesnot Exist

	SET NOCOUNT OFF
END