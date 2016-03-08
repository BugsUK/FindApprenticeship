/*----------------------------------------------------------------------                  
Name  : ReportGetGender                  
Description :  returns LSCRegionInOut

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
14 Jan 2010		1.1			John Hope		Remove literal LSC
---------------------------------------------------------------------- */                 

Create procedure [dbo].[ReportGetLSCRegionInOut]
(@type int)
as


BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 
	if @type=1
		
		begin
		select
			LSCRegionInOutID,
			FullName
		from
			(
			select	-1 as LSCRegionInOutID,
					'All' as FullName,
					0 as ord
			union all
			SELECT	1 LSCRegionInOutID, 
					'In Region' FullName,
					1 as ord
			union all
			SELECT	2 LSCRegionInOutID, 
					'Out Region' FullName,
					1 as ord
				)a
			Order by 
					ord,FullName
			end
			else
		begin
				SELECT -1 as LSCRegionInOutID, 'All' as 'FullName'
		end
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END