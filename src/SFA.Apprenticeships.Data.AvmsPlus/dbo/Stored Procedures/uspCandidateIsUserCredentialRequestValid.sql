CREATE PROCEDURE [dbo].[uspCandidateIsUserCredentialRequestValid]    
 @emailAddress nvarchar(100),    
 @userDoB DateTime,  
 @Postcode NVARCHAR (16),    
 @candidateId int OUT    
AS      
BEGIN        
 -- SET NOCOUNT ON    
    
 BEGIN TRY        
  Begin      
	
   if @Postcode = '' 
   Begin
	SET @postcode = NULL
   end

Set @candidateId = 0  -- Default Zero if there's an error   
     
   Select @candidateId = Candidate.CandidateId    
   From Candidate, Person    
   Where Candidate.PersonId  = Person.PersonId    
    And  Convert(Varchar, Candidate.DateofBirth, 103) = Convert(Varchar, @userDoB, 103)     
    And  Person.Email   = @emailAddress    
    AND (Candidate.Postcode = @Postcode  or @Postcode IS NULL)
      
   return @candidateId  -- Re turning CandidateId    
  End    
    END TRY    
        
    BEGIN CATCH    
 EXEC RethrowError;    
  END CATCH    
 SET NOCOUNT OFF    
END