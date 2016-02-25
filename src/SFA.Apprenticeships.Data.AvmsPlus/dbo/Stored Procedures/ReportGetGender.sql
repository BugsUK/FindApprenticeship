/*----------------------------------------------------------------------                  
Name  : ReportGetGender                  
Description :  returns ordered unique gender

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
12-Nov-2008		1.01		Ian Emery		Added unspecified
---------------------------------------------------------------------- */                 

Create procedure [dbo].[ReportGetGender]
as


BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 

		
			select	-1 as CandidateGenderId,
					'All' as FullName,
					0 as ord
		union all
			SELECT  0 as CandidateGenderId,
					'Unspecified' as FullName,
					1 as Ord
		union all
			SELECT	CandidateGenderId, 
					FullName,
					1 as ord
			FROM 
					dbo.CandidateGender
			WHERE	
					CandidateGenderId <> 0
			Order by 
					ord,FullName
		
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END