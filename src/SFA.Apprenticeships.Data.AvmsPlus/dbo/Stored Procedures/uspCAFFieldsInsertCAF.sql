CREATE PROCEDURE [dbo].[uspCAFFieldsInsertCAF]  
	@cafFieldsId int out,  
	@candidateId int,  
	@applicationId int = null,  
	@field int,  
	@value varchar(8000)  
   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
	If @applicationId is null 
		begin
			INSERT INTO [dbo].[CAFFields] ([CandidateId], [ApplicationId], [Field], [Value])  
			VALUES (@candidateId, @applicationId, @field, @value) 
		end
	else
		Begin
			INSERT INTO [dbo].[CAFFields] ([CandidateId], [ApplicationId], [Field], [Value])  
			VALUES (@candidateId, @applicationId, @field, @value)
	
			Update [dbo].[CAFFields]  
					set          
						[Value] = @value        
					where [ApplicationId] is null
						And CandidateId = @candidateId 
						And [Field] = @field 
						And Value is null 
		End 
   
 SET @cafFieldsId = SCOPE_IDENTITY()  
  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END