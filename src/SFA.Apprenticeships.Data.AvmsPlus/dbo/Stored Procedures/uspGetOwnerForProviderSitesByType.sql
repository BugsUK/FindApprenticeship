CREATE PROCEDURE dbo.uspGetOwnerForProviderSitesByType
    @providerSiteIds NVARCHAR(400) = '',
    @type VARCHAR(20) = NULL
AS  
BEGIN 
 
   SET NOCOUNT ON  
  
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

		IF(@type = 'MANAGER')
		BEGIN
			SELECT  
					DISTINCT  ps.ProviderSiteId, ps.TradingName,
					 isnull(ps.[AddressLine1],'') as 'AddressLine1',  
					 isnull(ps.[AddressLine2],'') as 'AddressLine2',  
					 isnull(ps.[AddressLine3],'') as 'AddressLine3',  
					 isnull(ps.[AddressLine4],'') as 'AddressLine4',  
					 isnull(ps.[Town],'') as 'Town',  
					 [County].[FullName] as 'County'
			FROM	
					ProviderSite ps
					INNER JOIN Vacancy v ON v.VacancyManagerID = ps.ProviderSiteId
					LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = ps.[CountyId]
					INNER JOIN dbo.LocalAuthorityGroup LAG 
						ON  LAG.LocalAuthorityGroupID = ps.ManagingAreaID 
					INNER JOIN dbo.LocalAuthorityGroupType LAGT 
						ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					AND LocalAuthorityGroupTypeName = N'Managing Area'
			WHERE
					( ps.ProviderSiteID IN ( SELECT TPId FROM @tpIds ) OR @providerSiteIds = '' ) 	
		END					
		ELSE IF (@type = 'DELIVERER')
		BEGIN
			SELECT  
					DISTINCT  ps.ProviderSiteId, ps.TradingName ,
					 isnull(ps.[AddressLine1],'') as 'AddressLine1',  
					 isnull(ps.[AddressLine2],'') as 'AddressLine2',  
					 isnull(ps.[AddressLine3],'') as 'AddressLine3',  
					 isnull(ps.[AddressLine4],'') as 'AddressLine4',  
					 isnull(ps.[Town],'') as 'Town',  
					 [County].[FullName] as 'County'

			FROM	
					ProviderSite ps
					INNER JOIN Vacancy v ON v.DeliveryOrganisationID = ps.ProviderSiteId
					LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = ps.[CountyId]
					INNER JOIN dbo.LocalAuthorityGroup LAG ON  LAG.LocalAuthorityGroupID = ps.ManagingAreaID 
					INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					AND LocalAuthorityGroupTypeName = N'Managing Area'
			WHERE
					( ps.ProviderSiteID IN ( SELECT TPId FROM @tpIds ) OR @providerSiteIds = '' )
		END					
				
    END TRY  
	BEGIN CATCH  
		EXEC RethrowError;  
	END CATCH  
  
    SET NOCOUNT OFF  
END