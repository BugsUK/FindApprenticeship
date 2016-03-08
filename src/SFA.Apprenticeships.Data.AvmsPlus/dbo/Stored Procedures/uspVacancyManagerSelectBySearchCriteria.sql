CREATE PROCEDURE [dbo].[uspVacancyManagerSelectBySearchCriteria]
@EmployerId INT, @TrainingProviderId INT, @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='TrainingProviderName'
AS
BEGIN                  
SET NOCOUNT ON                  
           
if @TrainingProviderId = 0 set @TrainingProviderId  = null  
/*********Set Page Number**********************************************/      
declare @StartRowNo int      
declare @EndRowNo int      
set @StartRowNo =((@PageIndex-1)* @PageSize)+1       
set @EndRowNo =(@PageIndex * @PageSize)          
/***********************************************************************/      
      
--/**************Total Number of Rows*************************************/      
--declare @TotalRows int      
--select @TotalRows= count(1) from VacancyProvisionRelationship                  
--  inner join TrainingProvider on VacancyProvisionRelationship.TrainingProviderId = TrainingProvider.TrainingProviderId                   
--  inner join Employer on Employer.Employerid = VacancyProvisionRelationship.EmployerId      
--  Left outer join VacancyProvisionRelationshipStatusType on VacancyProvisionRelationship.StatusTypeId = VacancyProvisionRelationshipStatusType.VacancyProvisionRelationshipStatusTypeId
--  where VacancyProvisionRelationship.EmployerId = @EmployerId                  
--  and (VacancyProvisionRelationship.TrainingProviderId = @TrainingProviderId or @TrainingProviderId is null)          
--  and VacancyProvisionRelationshipStatusType.FullName != 'Deleted'   
--  
--/***********************************************************************/      
      
/*********set the order by**********************************************/      
declare @OrderBywithSort varchar(500)      
      
 if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END      
 if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END      
     
/***********************************************************************/      
Select MyTable.* From
	(
	Select 
	TotalRows=Count(1) Over(),
	ROW_NUMBER() OVER( ORDER BY       
    Case when @SortByField='TrainingProviderName Asc'  then [ProviderSite].TradingName  End ASC,      
    Case when @SortByField='TrainingProviderName desc'  then [ProviderSite].TradingName  End DESC      
	 ) as RowNum,                  
	  [VacancyOwnerRelationshipId] as RelationshipId,   
	  [VacancyOwnerRelationship].EmployerDescription as 'EmployerDescription',
	  [ProviderSite].ProviderSiteID,      
	  [VacancyOwnerRelationship].EmployerId,      
	  isnull(Employer.FullName,'') as 'EmployerName',      
	  isnull([ProviderSite].TradingName,'') as 'TraingingProviderName',    
	  --isnull(VacancyProvisionRelationshipStatusType.FullName,'') as 'VacancyRelationShipStatus'    
	  StatusTypeId,
	  
      (
	 CASE WHEN EXISTS (
	 Select Vacancystatusid 
	 FROM VACANCY 
	 WHERE VACANCY.[VacancyOwnerRelationshipId] = [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] 
	 AND VACANCY.VacancyStatusId IN (2,6))THEN 0 ELSE 1 END
	 ) as IsDeletable	  
	  from [VacancyOwnerRelationship]                  
	  inner join [ProviderSite] on [VacancyOwnerRelationship].[ProviderSiteID] = [ProviderSite].ProviderSiteID                   
	  inner join Employer on Employer.Employerid = [VacancyOwnerRelationship].EmployerId      
	  Left outer join VacancyProvisionRelationshipStatusType on [VacancyOwnerRelationship].StatusTypeId = VacancyProvisionRelationshipStatusType.VacancyProvisionRelationshipStatusTypeId
	  where [VacancyOwnerRelationship].EmployerId = @EmployerId  
	  and VacancyProvisionRelationshipStatusType.FullName != 'Deleted'                   
	  and ([VacancyOwnerRelationship].[ProviderSiteID] = @TrainingProviderId or @TrainingProviderId is null)           
	          
	 ) as MyTable                   
	WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo
 SET NOCOUNT OFF                  

END