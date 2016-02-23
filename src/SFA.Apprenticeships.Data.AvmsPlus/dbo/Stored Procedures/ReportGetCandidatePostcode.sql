/*----------------------------------------------------------------------                  
Name  : ReportGetCandidatePostcode                  
Description :  returns ordered unique candidate postcode

                
History:                  
--------                  
Date			Version		Author			Comment
28-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetCandidatePostcode]
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 

		SELECT	CandidateId,
				Postcode
		FROM	dbo.Candidate
		ORDER BY Postcode
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END