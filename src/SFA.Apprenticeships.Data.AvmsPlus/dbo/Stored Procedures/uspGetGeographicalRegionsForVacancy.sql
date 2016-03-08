CREATE PROCEDURE  dbo.uspGetGeographicalRegionsForVacancy
	    @providerSiteIds NVARCHAR(400) = ''
AS
BEGIN
	
	SET NOCOUNT ON;	

	BEGIN TRY  

	DECLARE @tpIds TABLE  
		(  
			TPId   NVARCHAR(20)  
		)  
		DECLARE @position   INT  
		DECLARE @piece      NVARCHAR(20)  
		DECLARE @workingIds NVARCHAR(400)  
	  
		-- Need to tack a delimiter onto the end of the input string if one doesn?t exist  
		SET @workingIds = @providerSiteIds  
		IF RIGHT(RTRIM(@workingIds),1) <> ','  
			SET @workingIds = @workingIds  + ','  
	   
		SET @position =  PATINDEX('%,%' , @workingIds)  
		WHILE @position <> 0  
		BEGIN  
			SET @piece = LEFT(@workingIds, @position - 1)  
	   
		-- You have a piece of data, so insert it, print it, do whatever you want to with it.  
			INSERT INTO @tpIds VALUES ( @piece )  
	  
			SET @workingIds = STUFF(@workingIds, 1, @position, '')  
			SET @position =  PATINDEX('%,%' , @workingIds)  
		END  

		SELECT DISTINCT LAG.[LocalAuthorityGroupID]
		  ,LAG.[CodeName]
		  ,LAG.[ShortName]
		  ,LAG.[FullName]
		  ,LAG.[LocalAuthorityGroupTypeID]
		  ,LAG.[LocalAuthorityGroupPurposeID]
		  ,LAG.[ParentLocalAuthorityGroupID]
		FROM 
			ProviderSite ps
			INNER JOIN Vacancy v ON v.VacancyManagerID = ps.ProviderSiteId
			LEFT JOIN dbo.LocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId
			LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = LA.[CountyId]
			INNER JOIN dbo.LocalAuthorityGroupMembership LAGM
				ON  LAGM.LocalAuthorityID =  v.LocalAuthorityId
			INNER JOIN dbo.LocalAuthorityGroup LAG 
				ON  LAG.LocalAuthorityGroupID = LAGM.LocalAuthorityGroupID 
			INNER JOIN dbo.LocalAuthorityGroupType LAGT 
				ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					AND LocalAuthorityGroupTypeName = N'Region'
		WHERE
			( ps.ProviderSiteID IN ( SELECT TPId FROM @tpIds ) OR @providerSiteIds = '' ) 	
		ORDER BY LAG.FullName, 
		   LAG.[CodeName]
		  ,LAG.[ShortName]
		  ,LAG.[LocalAuthorityGroupTypeID]
		  ,LAG.[LocalAuthorityGroupPurposeID]
		  ,LAG.[ParentLocalAuthorityGroupID]
		  
		  			
    END TRY  
	BEGIN CATCH  
		EXEC RethrowError;  
	END CATCH  
  
    SET NOCOUNT OFF  
		
END