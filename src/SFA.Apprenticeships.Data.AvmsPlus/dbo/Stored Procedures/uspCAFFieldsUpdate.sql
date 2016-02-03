CREATE PROCEDURE [dbo].[uspCAFFieldsUpdate]        
	@CAFFieldId int Out,        
	@candidateId int,          
	@applicationId int,        
	@field int,        
	@value varchar(8000)        
AS        
BEGIN        
SET NOCOUNT ON        
  BEGIN TRY        
	-- This is a CAF update	
	If @applicationId is null  
		Begin
			-- Other qualification update  
			if @field = 5
				Begin 
					update [dbo].[CAFFields]         
					set        
					 [Value] = @value
					where (([ApplicationId] Is Null) 
						And (CandidateId = @candidateId) 
						And [Field] = @field)
				End
			-- CAF fields update
			Else
				Begin
					update [dbo].[CAFFields]         
					set        
					 [Value] = @value
					where (([ApplicationId] Is Null) 
						And (CandidateId = @candidateId) 
						And [Field] = @field)
				End
		End 
	-- This is a application related field update	
	Else  
		Begin  
			-- Other Qualifications update for application form
			If @field = 5 
					-- And it does not exist in CAF
				And	Not Exists (Select value 
									from CAFFields 
									Where CandidateId = @candidateId 
										and ApplicationId is null
										and Field = 5)  
				Begin
					-- Insert a new CAF row  
					Execute uspCAFFieldsInsertCAF 0, @candidateId, null, 5, @value
					--Execute uspCAFFieldsInsertCAF 0, @candidateId, @applicationId, 5, @value
					
				End
			Else If @field = 5 
					-- And exist in CAF
				And	Exists (Select value 
						from CAFFields 
						Where CandidateId = @candidateId 
							and ApplicationId is null
							and Field = 5)  
				Begin
					update [dbo].[CAFFields]         
					set        
					 [Value] = @value
					where (([ApplicationId] Is Null) 
						And (CandidateId = @candidateId) 
						And [Field] = @field)
--					If Exists (Select value 
--						from CAFFields 
--						Where CandidateId = @candidateId 
--							and ApplicationId = @applicationId
--							and Field = 5)  
--						begin		
--							Update [dbo].[CAFFields]  
--							set          
--								[Value] = @value        
--							where 
--								[ApplicationId] = @applicationId 
--								And CandidateId = @candidateId 
--								And [Field] = @field 
--						end
--					else
--						begin
--							Execute uspCAFFieldsInsertCAF 0, @candidateId, @applicationId, 5, @value
--						end

				End
			-- otherwise update application related fields
			Else  
				Begin  
					Update [dbo].[CAFFields]  
					set          
						[Value] = @value        
					where ([CAFFieldsId] = @CAFFieldId)   
						Or ((([ApplicationId] = @applicationId) 
							-- To capture an update to existing CAF field of field type 5
							--Or [ApplicationId] Is Null
														)
						And (CandidateId = @candidateId) 
							And [Field] = @field) 

				End  
		End 
 END TRY        
 BEGIN CATCH        
	EXEC RethrowError;        
 END CATCH        
 
SET NOCOUNT OFF        

END