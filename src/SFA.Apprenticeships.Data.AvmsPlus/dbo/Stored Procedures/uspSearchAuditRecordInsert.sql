CREATE PROCEDURE [dbo].[uspSearchAuditRecordInsert]  
    @CandidateId int,  
 @recordCount int = NULL,  
 @runDate datetime,  
 @runTime datetime = NULL,  
 @searchAuditRecordId int OUT,  
 @searchCriteria nvarchar(50) = NULL  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
    INSERT INTO [dbo].[SearchAuditRecord] ([CandidateId], [RecordCount], [RunDate], [RunTime], [SearchCriteria])  
 VALUES (@CandidateId, @recordCount, @runDate, @runTime, @searchCriteria)  
    SET @searchAuditRecordId = SCOPE_IDENTITY()  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END