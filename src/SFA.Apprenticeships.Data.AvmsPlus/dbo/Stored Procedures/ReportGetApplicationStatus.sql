/*----------------------------------------------------------------------                  
Name  : ReportGetApplicationStatus                  
Description :  returns ordered unique application status 

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
01-NOV-2008		1.1			Femma Ashraf	use codename instead of full name
12-NOV-2008		1.2			Ian Emery		changed the join as data is correct in the database.
12-NOV-2008		1.3			Ian Emery		removed draft from list
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetApplicationStatus]

as

		BEGIN  
			SET NOCOUNT ON  
			SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
			BEGIN TRY  
			
			SELECT -1 ApplicationStatusTypeId,
						'All' FullName,
						0  Ord
		union all
			SELECT	ApplicationStatusTypeId,
					FullName,
					1 Ord
			FROM	dbo.ApplicationStatusType
			WHERE 
					CodeName <> 'DRF'
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END