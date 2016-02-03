CREATE PROCEDURE [dbo].[uspVacancySelectByEmployerOrProviderID]           
 @SearchType varchar(30),          
 @apprenticeOccupation int = 0,          
 @apprenticeFramework int = 0,          
 @postCode varchar(20) = null,          
 @location int = 0,          
 @vacPostedSince datetime = null,          
 @Employer varchar(100) = null,         
 @EmployerID int = null,          
 @trainingProvider varchar(100) = null,     
 @trainingProviderID int = null,          
 @keyWord varchar(200) = null,          
 @VacancyReferenceNumber varchar(100) = null,          
 @Latitude decimal(28,15) = null,          
 @Longitude decimal(28,15) = null,          
 @Distancefrom int = 0,          
 @WeeklyWagesFrom int = null,          
 @WeeklyWagesTo  int = null,          
 @PageNo int = 1,          
 @PageSize int = 10,          
 @SortByField nvarchar(100)='ClosingDate',          
 @SortOrder bit = 1  
AS      
        
BEGIN          
 SET NOCOUNT ON          
 DECLARE @DispRowFrom int          
 DECLARE @DispRowTo int          
          
 -- temporary measure until @location (argument) is changed to an int    
 DECLARE @locationID int    
 SET @locationID=CAST(@location as int)    
    
 if @apprenticeOccupation = 0 or @apprenticeOccupation = -1    
  set @apprenticeOccupation = null    
    
 if @apprenticeFramework = 0 or @apprenticeFramework = -1    
  set @apprenticeFramework = null    
    
 if @PageNo = 1          
 BEGIN          
  SET @DispRowFrom = 1          
  SET @DispRowTo = @DispRowFrom + @PageSize          
 END          
       
 if @location = -1    
 BEGIN    
  SET @location = NULL    
 END    
       
   if @PageNo > 1          
   BEGIN          
    SET @DispRowFrom = (@PageNo * @PageSize) + 1          
    SET @DispRowTo = (@DispRowFrom + @PageSize)          
   END          
          
   /*********set the order by**********************************************/          
   if @SortByField = 'EmployerName'          
   Begin           
    if @SortOrder = 1 BEGIN set  @SortByField = 'EmployerName Asc' END          
    if @SortOrder = 0 BEGIN  set  @SortByField = 'EmployerName desc' END          
   End      
        
   if @SortByField = 'ClosingDate'          
   Begin           
    if @SortOrder = 1 BEGIN set  @SortByField = 'ApplicationClosingDate Desc' END          
    if @SortOrder = 0 BEGIN  set  @SortByField = 'ApplicationClosingDate Asc' END          
   End          
   /***********************************************************************/          
          
   /**************Total Number of Rows*************************************/          
   declare @TotalRows int              
          
   /************** Employer Id Search *************************************/     
   if @SearchType = 'EmployerId'          
   BEGIN          
          
    select @TotalRows= count(1)           
    FROM vacancy v     
  inner join [VacancyOwnerRelationship] vpr on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
  inner join employer e on vpr.employerid = e.employerid    
  inner join [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID    
  inner join vacancysearch vs on v.vacancyid = vs.vacancyid     
 --with (nolock)          
    WHERE e.employerid = @EmployerID        
    
    SELECT *,@TotalRows AS TotalRows FROM          
    (          
     SELECT distinct          
       ROW_NUMBER() OVER( ORDER BY           
         Case when @SortByField='EmployerName Asc'  then e.[TradingName]  End ASC,          
         Case when @SortByField='EmployerName desc'  then e.[TradingName]  End DESC,          
         Case when @SortByField='ApplicationClosingDate Asc'  then v.[ApplicationClosingDate]  End ASC,          
         Case when @SortByField='ApplicationClosingDate desc'  then v.[ApplicationClosingDate]  End DESC          
         ) as RowNum,          
       v.[vacancyid] AS 'vacancyid',          
       v.[Title] AS 'Title',          
       e.[TradingName] As 'EmployerName',      
       e.[EmployerID] As 'EmployerId',     
       tp.[TradingName] As 'ProviderName',      
       tp.ProviderSiteID As 'ProviderId',          
       v.[Town] AS 'Town',          
       v.[ShortDescription] AS 'ShortDescription',          
       vs.[VacancyPostedDate] AS 'VacancyPostedDate',          
       v.[ApprenticeshipType] As 'ApprenticeShipType',          
       v.[ApplicationClosingDate] AS 'ClosingDate'          
          
    FROM vacancy v     
  inner join [VacancyOwnerRelationship] vpr on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
  inner join employer e on vpr.employerid = e.employerid    
  inner join [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID    
  inner join vacancysearch vs on v.vacancyid = vs.vacancyid      
    WHERE e.employerid = @EmployerID    
         
    ) AS D          
    WHERE RowNum >= @DispRowFrom     
 AND RowNum < @DispRowTo          
   END    
      
   if @SearchType = 'TrainingProviderId'          
   BEGIN          
          
    select @TotalRows= count(1)           
    FROM vacancy v     
  inner join [VacancyOwnerRelationship] vpr on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
  inner join employer e on vpr.employerid = e.employerid    
  inner join [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID    
  inner join vacancysearch vs on v.vacancyid = vs.vacancyid     
 --with (nolock)          
    WHERE tp.ProviderSiteID = @trainingProviderID        
    
    SELECT *,@TotalRows AS TotalRows FROM          
    (          
     SELECT distinct          
       ROW_NUMBER() OVER( ORDER BY           
         Case when @SortByField='ProviderName Asc'  then tp.[TradingName]  End ASC,          
         Case when @SortByField='ProviderName desc'  then tp.[TradingName]  End DESC,          
         Case when @SortByField='ApplicationClosingDate Asc'  then v.[ApplicationClosingDate]  End ASC,          
         Case when @SortByField='ApplicationClosingDate desc'  then v.[ApplicationClosingDate]  End DESC          
         ) as RowNum,          
       v.[vacancyid] AS 'vacancyid',          
       v.[Title] AS 'Title',          
       e.[TradingName] As 'EmployerName',      
       e.[EmployerID] As 'EmployerId',     
       tp.[TradingName] As 'ProviderName',      
       tp.ProviderSiteID As 'ProviderId',          
       v.[Town] AS 'Town',          
       v.[ShortDescription] AS 'ShortDescription',          
       vs.[VacancyPostedDate] AS 'VacancyPostedDate',          
       v.[ApprenticeshipType] As 'ApprenticeShipType',          
       v.[ApplicationClosingDate] AS 'ClosingDate'          
          
    FROM vacancy v     
  inner join [VacancyOwnerRelationship] vpr on v.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
  inner join employer e on vpr.employerid = e.employerid    
  inner join [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID    
  inner join vacancysearch vs on v.vacancyid = vs.vacancyid      
    WHERE tp.ProviderSiteID = @trainingProviderID    
         
    ) AS D          
    WHERE RowNum >= @DispRowFrom     
 AND RowNum < @DispRowTo          
   END      
    
 SET NOCOUNT OFF          
END