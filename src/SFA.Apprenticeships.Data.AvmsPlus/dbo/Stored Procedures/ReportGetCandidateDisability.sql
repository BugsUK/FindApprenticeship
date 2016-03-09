/*----------------------------------------------------------------------                  
Name  : ReportGetCandidateDisability                  
Description :  returns ordered unique Candidate Disability 

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
11-Nov-2008		1.01		Ian Emery		changed the None value from -1 to 0
24-Nov-2008		1.02		Ian Emery		Added all value- this is all candidates (with and without disabilities)
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportGetCandidateDisability]
as

BEGIN  

	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 
 		SELECT -1 as CandidateDisabilityId,
					'All' as FullName,
					-1 as ord
			union all
			SELECT 0 as CandidateDisabilityId,
					'None' as FullName,
					0 as ord
			union all
			SELECT	CandidateDisabilityId,
					FullName,
					1 as ord
			FROM
					dbo.CandidateDisability
			where
					CandidateDisabilityId <>0
			ORDER BY ord,FullName
	END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END