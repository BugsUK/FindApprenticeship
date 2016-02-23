/*----------------------------------------------------------------------                  
Name  : ReportGetSectorForVacancy                  
Description :  returns ordered unique sector for vacancy

                
History:                  
--------                  
Date			Version		Author			Comment
27-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetSectorForVacancy]

as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 
		create table #tmp1(id int, fullname varchar(1000))
		insert into #tmp1
			SELECT	ApprenticeshipOccupationId,ShortName + ' ' + FullName
			FROM	dbo.ApprenticeshipOccupation 
			where ApprenticeshipOccupationId<>0 and ApprenticeshipOccupationStatusTypeId<>2
			order by CodeName 

		create table #tmp2(id int, fullname varchar(1000))
		insert into #tmp2
			SELECT	ApprenticeshipOccupationId,'CLOSED ' + ShortName + ' ' + FullName
			FROM	dbo.ApprenticeshipOccupation 
			where ApprenticeshipOccupationId<>0 and ApprenticeshipOccupationStatusTypeId=2
			order by CodeName 

		select 
				-1 as ApprenticeshipOccupationId,
				'All' as FullName,
				0 as ord
		union all
			SELECT	id,FullName,1 as Ord
			FROM	#tmp1
		union all
			SELECT	id,FullName,2 as Ord
			FROM	#tmp2
		ORDER BY Ord, FullName
		
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END