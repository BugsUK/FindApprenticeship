/*----------------------------------------------------------------------                  
Name  : ReportGetCandidateEthnicOrigin                  
Description :  returns Grouping and main ethnicity list.

                
History:                  
--------                  
Date			Version		Author			Comment
27-Aug-2008		1.0			Femma Ashraf	first version
31-Oct-2008		1.01		Ian Emery		removed anonymolus items such as please select
---------------------------------------------------------------------- */                 
          


CREATE procedure [dbo].[ReportGetCandidateEthnicOrigin]
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY  
		SELECT
		NAME, ID 
		FROM
		(
			SELECT 
				DISTINCT ShortName AS NAME,
				MIN (CandidateEthnicOriginId) ID,
				0 ord	
			FROM 
				dbo.CandidateEthnicOrigin
			WHERE 
				CandidateEthnicOriginId > 0
				AND ShortName <> 'Don''t know or don''t want to say' 
				AND shortName <> 'Other (please specify)'
			 GROUP BY 
				ShortName
			 
			UNION 
			SELECT DISTINCT 
				'  '+FullName AS NAME, -- spaces inserted using alt + 255
				MIN (CandidateEthnicOriginId) ID,
				1 ord	
			 FROM 
				dbo.CandidateEthnicOrigin
			 WHERE
				CandidateEthnicOriginId>0
				AND FullName <> 'Not applicable' 
				AND FullName <> 'Please Select'
				AND FullName <> 'Chinese'
			 GROUP BY
				FullName
			
			 
			UNION 
				Select 
				'All' AS NAME,
				-1 ID,
				0 ord		
		)a
		ORDER BY 
			ID,ord
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END