CREATE PROCEDURE [dbo].[uspCandidateUpdateVoucherReferenceNumber] 
    @CandidateId int ,  
    @VoucherNo int OUT

   
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
 
 --DECLARE @newVoucherNo int
 --SELECT @newVoucherNo = MAX(VacancyReferenceNumber) + 1 from Vacancy

-- UPDATE 
--    CANDIDATE
-- SET 
--   VoucherReferenceNumber =  @newVoucherNo
-- WHERE
--    CandidateId =  @CandidateId

SELECT @VoucherNo = RIGHT('0000000000'+ CAST(CandidateId AS VARCHAR(9)),9)
from Candidate 
 WHERE
    CandidateId =  @CandidateId

UPDATE 
    Candidate
 SET 
   VoucherReferenceNumber =  @VoucherNo
 WHERE
    CandidateId =  @CandidateId
  


 END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END