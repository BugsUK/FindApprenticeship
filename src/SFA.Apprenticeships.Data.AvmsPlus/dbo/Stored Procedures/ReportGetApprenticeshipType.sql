/*----------------------------------------------------------------------                  
Name  : ReportGetApprenticeshipType                  
Description :  returns ordered unique Apprenticeship Type

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetApprenticeshipType]
as

		BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
			BEGIN TRY  
			SELECT
				ApprenticeshipTypeId,
				FullName
				from
			(
			SELECT -1 ApprenticeshipTypeId,
					'All' FullName,
					0 Ord
			UNION ALL 
			SELECT	ApprenticeshipTypeId,
					FullName,
					1 Ord
			FROM	
					dbo.ApprenticeshipType
			)a
			ORDER BY 
					Ord,ApprenticeshipTypeId		
		
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END