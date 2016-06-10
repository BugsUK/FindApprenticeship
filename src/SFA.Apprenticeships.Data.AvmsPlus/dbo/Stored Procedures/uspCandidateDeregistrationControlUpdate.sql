CREATE PROCEDURE [dbo].[uspCandidateDeregistrationControlUpdate]
@CandidateId INT, @CandidateDeRegistrationControlId INT, @isDeletedFromAdam BIT, @isDeleteFromAOL BIT, @RoleId INT
AS
BEGIN  
  SET NOCOUNT ON  
   
   BEGIN TRY
   update [CandidateDeRegistrationControl]
   set [CandidateId] = @CandidateId,
   [isDeletedFromAdam] = @isDeletedFromAdam,
   [isDeleteFromAOL] = @isDeleteFromAOL,
   [roleId] = @RoleId
   
   where [CandidateDeRegistrationControlId] = @CandidateDeRegistrationControlId
   END TRY  
  BEGIN CATCH  
    EXEC RethrowError;  
  END CATCH  
  SET NOCOUNT OFF  
END