/*
-------------------------------------------------------------------                  
Name  : ReportGetFrameworkFor                  
Description :  returns ordered unique sector for framework

                
History:                  
--------                  
Date			Version		Author			Comment
27-Aug-2008		1.0			Femma Ashraf	first version
---------------------------------------------------------------------- */                 

CREATE procedure [dbo].[ReportGetFrameworkFor]
(@Occupation int)
as

BEGIN  
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 
    BEGIN TRY 
		create table #tmp1(id int, fullname varchar(1000))
		create table #tmp2(id int, fullname varchar(1000))
		
		insert into #tmp1		
			SELECT	ApprenticeshipFrameworkId,
					CodeName + ' ' + FullName
			FROM	dbo.ApprenticeshipFramework
			WHERE	ApprenticeshipFrameworkId <> 0
				and ((@Occupation=-1) or (@Occupation<>-1 
				and ApprenticeshipOccupationId=@Occupation))
				AND	ApprenticeshipFrameworkStatusTypeId <> 2
			order by CodeName 

		insert into #tmp2
			SELECT	ApprenticeshipFrameworkId,
					'CLOSED ' + CodeName + ' ' + FullName
			FROM	dbo.ApprenticeshipFramework
			WHERE	ApprenticeshipFrameworkId <> 0
				and ((@Occupation=-1) or (@Occupation<>-1 
				and ApprenticeshipOccupationId=@Occupation))
				AND	ApprenticeshipFrameworkStatusTypeId = 2
			order by CodeName		

		select	-1 as ApprenticeshipFrameworkId,
					'All' as FrameworkName,
					0 as ord
		union all
			SELECT	id,FullName,1 as Ord
			FROM	#tmp1
		union all
			SELECT	id,FullName,2 as Ord
			FROM	#tmp2
		ORDER BY ord,FrameworkName

END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END