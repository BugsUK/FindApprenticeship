CREATE PROCEDURE [dbo].[uspVacancyManagerSelectByEmployerProvider]
@EmployerId INT=null, 
@TrainingProviderId INT=null, 
@EmployerName NVARCHAR (510)=null, 
@EmployerPostcode NVARCHAR (100)=null, 
@RetrieveAllProviders BIT= 0,
@RecruitmentAgentId INT= NULL,
@PageIndex INT=1, 
@PageSize INT=20, 
@IsSortAsc BIT=1, 
@SortByField1 NVARCHAR (100)='TrainingProviderName', 
@SortByField2 NVARCHAR (100)='EmployerName'
AS
BEGIN                        
SET NOCOUNT ON         

IF @RecruitmentAgentId = 0
	SET @RecruitmentAgentId = NULL               
                 
/*********Set Page Number**********************************************/            
declare @StartRowNo int            
declare @EndRowNo int            
set @StartRowNo =((@PageIndex-1)* @PageSize)+1             
set @EndRowNo =(@PageIndex * @PageSize)                
/***********************************************************************/            
            
/**************Total Number of Rows*************************************/            
declare @TotalRows int            
select @TotalRows= count(1)  from [VacancyOwnerRelationship]                        
  inner join [ProviderSite] on [VacancyOwnerRelationship].[ProviderSiteID] = [ProviderSite].ProviderSiteID                         
  inner join Employer on Employer.EmployerId = [VacancyOwnerRelationship].EmployerId            
  Left outer join VacancyProvisionRelationshipStatusType on [VacancyOwnerRelationship].StatusTypeId = VacancyProvisionRelationshipStatusType.VacancyProvisionRelationshipStatusTypeId
  Left outer join EmployerTrainingProviderStatus ETPS1 on Employer.EmployerStatusTypeId = ETPS1.EmployerTrainingProviderStatusId    
  INNER JOIN ProviderSiteRelationship psrOwner on psrOwner.ProviderSiteID = [ProviderSite].ProviderSiteID AND psrOwner.ProviderSiteRelationShipTypeID =1
  LEFT JOIN ProviderSiteRelationship psrRA on psrRA.ProviderID = psrOwner.ProviderID AND psrRA.ProviderSiteRelationShipTypeID =3 AND psrRA.ProviderSiteID = @RecruitmentAgentId
  LEFT JOIN RecruitmentAgentLinkedRelationships ralr on ralr.ProviderSiteRelationshipID = psrRA.ProviderSiteRelationshipID AND ralr.VacancyOwnerRelationshipID = [VacancyOwnerRelationship].VacancyOwnerRelationshipId
  where       
  ([VacancyOwnerRelationship].[ProviderSiteID] = @TrainingProviderId  or @TrainingProviderId is null)      
  and  ([VacancyOwnerRelationship].EmployerId = @EmployerId  or @EmployerId  is null)      
  and (Employer.FullName like '%' + @EmployerName + '%' or @EmployerName is null)      
  and (Employer.PostCode like @EmployerPostcode + '%' or @EmployerPostcode is null)      
  and VacancyProvisionRelationshipStatusType.FullName != 'Deleted'  
  and (@RetrieveAllProviders = 1 OR [ProviderSite].TrainingProviderStatusTypeId = 1)
  and ETPS1.FullName !=  'Deleted'  
  AND (@RecruitmentAgentId is NULL OR ralr.ProviderSiteRelationshipID IS NOT NULL)
                            
/***********************************************************************/            
           
if (@SortByField1 = 'Town')  
Begin  
 if @EmployerId is null  
  set @SortByField1 = 'EmployerTown'   
 else  
  set @SortByField1 = 'TrainingProviderTown'   
End  
if (@SortByField2 = 'Town')  
Begin  
 if @EmployerId is null  
  set @SortByField2 = 'EmployerTown'   
 else  
  set @SortByField2 = 'TrainingProviderTown'   
End  
  
  
  
/*********set the order by**********************************************/            
if @IsSortAsc = 1 BEGIN set  @SortByField1 = @SortByField1 + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField1 = @SortByField1 + ' desc' END   
  
   
if @IsSortAsc = 1 BEGIN set  @SortByField2 = @SortByField2 + ' Asc' END            
if @IsSortAsc = 0 BEGIN  set  @SortByField2 = @SortByField2 + ' desc' END  
  
  
  
/***********************************************************************/            
                 
select *,@TotalRows  as 'TotalRows' from             
  ( select ROW_NUMBER() OVER( ORDER BY             
    Case when @SortByField1='EmployerName Asc'  then Employer.FullName  End asc,          
    Case when @SortByField1='EmployerName desc'  then Employer.FullName  End DESC,          
    Case when @SortByField1='TrainingProviderName Asc'  then [ProviderSite].TradingName  End ASC,            
    Case when @SortByField1='TrainingProviderName desc'  then [ProviderSite].TradingName End DESC,  
   
 Case when @SortByField1='EmployerTown Asc'  then Employer.Town  End ASC,            
    Case when @SortByField1='TrainingProviderTown desc'  then [ProviderSite].Town  End DESC,  
  
 Case when @SortByField2='EmployerName Asc'  then Employer.FullName  End asc,          
    Case when @SortByField2='EmployerName desc'  then Employer.FullName  End DESC,          
    Case when @SortByField2='TrainingProviderName Asc'  then [ProviderSite].TradingName  End ASC,            
    Case when @SortByField2='TrainingProviderName desc'  then [ProviderSite].TradingName  End DESC,  
  
 Case when @SortByField2='EmployerTown Asc'  then Employer.Town  End ASC,            
    Case when @SortByField2='TrainingProviderTown desc'  then [ProviderSite].Town  End DESC  ,

 Case when @SortByField1='EmployerTown Desc'  then Employer.Town  End DESC,            
    Case when @SortByField1='TrainingProviderTown Asc'  then [ProviderSite].Town  End ASC   

   
 ) as RowNum,                        
  [VacancyOwnerRelationship].[VacancyOwnerRelationshipId] as RelationshipId,            
  [ProviderSite].ProviderSiteID,            
  [VacancyOwnerRelationship].EmployerId,            
  isnull(Employer.FullName,'') as 'EmployerName',            
  isnull([ProviderSite].TradingName,'') as 'TraingingProviderName',          
  isnull(VacancyProvisionRelationshipStatusType.FullName,'') as 'VacancyRelationShipStatus',      
  isnull(Employer.Town,'') as EmployerTown,      
  isnull(Employer.PostCode,'') as EmployerPostCode,      
  isnull([ProviderSite].Town,'') as TrainingProviderTown,      
  isnull([ProviderSite].PostCode,'') as TrainingProviderPostCode           
  from [VacancyOwnerRelationship]                        
  inner join [ProviderSite] on [VacancyOwnerRelationship].[ProviderSiteID] = [ProviderSite].ProviderSiteID                         
  inner join Employer on Employer.Employerid = [VacancyOwnerRelationship].EmployerId            
  Left outer join VacancyProvisionRelationshipStatusType on [VacancyOwnerRelationship].StatusTypeId = VacancyProvisionRelationshipStatusType.VacancyProvisionRelationshipStatusTypeId
  Left outer join EmployerTrainingProviderStatus ETPS1 on Employer.EmployerStatusTypeId = ETPS1.EmployerTrainingProviderStatusId    
  INNER JOIN ProviderSiteRelationship psrOwner on psrOwner.ProviderSiteID = [ProviderSite].ProviderSiteID AND psrOwner.ProviderSiteRelationShipTypeID =1
  LEFT JOIN ProviderSiteRelationship psrRA on psrRA.ProviderID = psrOwner.ProviderID AND psrRA.ProviderSiteRelationShipTypeID =3 AND psrRA.ProviderSiteID = @RecruitmentAgentId
  LEFT JOIN RecruitmentAgentLinkedRelationships ralr on ralr.ProviderSiteRelationshipID = psrRA.ProviderSiteRelationshipID AND ralr.VacancyOwnerRelationshipID = [VacancyOwnerRelationship].VacancyOwnerRelationshipId
  
  where       
  ([VacancyOwnerRelationship].[ProviderSiteID] = @TrainingProviderId  or @TrainingProviderId is null)      
  and  ([VacancyOwnerRelationship].EmployerId = @EmployerId  or @EmployerId  is null)      
  and (Employer.FullName like '%' + @EmployerName + '%' or @EmployerName is null)      
  and (Employer.PostCode like @EmployerPostcode + '%' or @EmployerPostcode is null)      
  and VacancyProvisionRelationshipStatusType.FullName != 'Deleted'  
  and (@RetrieveAllProviders = 1 OR [ProviderSite].TrainingProviderStatusTypeId = 1)
  and ETPS1.FullName !=  'Deleted'  
  AND (@RecruitmentAgentId is NULL OR ralr.ProviderSiteRelationshipID IS NOT NULL)                        
       
  ) as DerivedTable                        
  WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo              
                          
 SET NOCOUNT OFF                        
END