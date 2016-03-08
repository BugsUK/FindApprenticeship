CREATE PROCEDURE [dbo].[uspSubVacancyInsert]                        
(                    
 @VacancyId int,                    
 @CandidateId int,  
 --@ILRNumber nchar(24),  
 @ILRStartDate datetime,
 @ApplicationId Int,
-- @ULN bigint,     
 @SubVacancyId int out   
)                    
AS                    
BEGIN                    
SET NOCOUNT ON                    
	if exists (select * from Subvacancy where VacancyId=@VacancyId 
				and AllocatedApplicationId=@ApplicationId)
		begin
			Update [dbo].[Subvacancy]  
					set          
						--ILRNumber = @ILRNumber,
						StartDate = @ILRStartDate        
					where 
						VacancyId=@VacancyId and 
						AllocatedApplicationId=@ApplicationId
			set @SubVacancyId  = 0
		end
	else
		begin
			insert into SubVacancy  
			(VacancyId,  
			AllocatedApplicationId,  
			--ILRNumber,  
			StartDate)  
			Values  
			(@VacancyId,  
			 @ApplicationId,--Pass in applicationid
			-- @ILRNumber,  
			 @ILRStartDate)  
	set @SubVacancyId  = SCOPE_IDENTITY()
		end             

--	update candidate 
--	set 
--		UniqueLearnerNumber=@ULN
--	where 
--		CandidateId=@CandidateId

                     
SET NOCOUNT OFF                    
END