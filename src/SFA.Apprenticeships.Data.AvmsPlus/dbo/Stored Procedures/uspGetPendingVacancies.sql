CREATE PROCEDURE [dbo].[uspGetPendingVacancies]     
(    
 @ManagingArea int = 0,    
 @PageIndex int =  1,          
 @PageSize int = 20,          
 @IsSortAsc bit= 1,          
 @SortByField nvarchar(100) = 'SubmissionDate'      
)    
AS    
BEGIN    
    
 SET NOCOUNT ON    
    
 /*********Set Page Number**********************************************/          
 declare @StartRowNo int          
 declare @EndRowNo int          
 set @StartRowNo =((@PageIndex-1)* @PageSize)+1           
 set @EndRowNo =(@PageIndex * @PageSize)              
 /***********************************************************************/          
           
 /**************Total Number of Rows*************************************/          
 declare @TotalRows int      
 /* call esisting sp to retrieve the total number of rows */    
 exec uspGetPendingVacanciesCount @ManagingArea,  @TotalRows output    
 /***********************************************************************/          
           
 /*********set the order by**********************************************/          
 declare @OrderBywithSort varchar(500)          
           
  if @IsSortAsc = 1 BEGIN set  @SortByField = @SortByField + ' Asc' END          
  if @IsSortAsc = 0 BEGIN  set  @SortByField = @SortByField + ' desc' END     
 /***********************************************************************/       
     
 DECLARE @Status as char(3)    
 Set @Status = 'SUB'    
    
 -- create a temporary table and populate with the vacancy ids that     
 -- have status submitted and in the relevent region    
 declare @vacancytemp table(VacancyID int, LSCRegion int)    
    
   
  insert into @vacancytemp( VacancyID, LSCRegion)    
  select VacancyId as VacancyID,    
    tp.ManagingAreaID as RegionId    
  from Vacancy vac    
    inner join [VacancyOwnerRelationship] vpr    
     on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
       inner join [ProviderSite] tp    
     on vpr.[ProviderSiteID] = tp.ProviderSiteID     
    inner join VacancyStatusType vst    
     on vac.VacancyStatusId = vst.VacancyStatusTypeId    
  where UPPER(vst.CodeName) = @Status     
    and  tp.ManagingAreaId = @ManagingArea;    
    
    
  -- return data    
  select *     
  from (    
     select *,    
       ROW_NUMBER() OVER( ORDER BY           
        Case when @SortByField='SubmissionDate Asc'  then SubmissionDate  End ASC,          
        Case when @SortByField='SubmissionDate desc'  then SubmissionDate  End DESC,          
        Case when @SortByField='SubmissionCount Asc'  then SubmissionCount  End ASC,          
        Case when @SortByField='SubmissionCount desc'  then SubmissionCount  End DESC,          
        Case when @SortByField='VacancyManager Asc'  then VacancyManager  End ASC,          
        Case when @SortByField='VacancyManager desc'  then VacancyManager  End DESC,          
        Case when @SortByField='VacancyTitle Asc'  then VacancyTitle  End ASC,          
        Case when @SortByField='VacancyTitle desc'  then VacancyTitle  End DESC          
       ) as RowNum,    
       @TotalRows  as 'TotalRows'     
    
     from      
       (    
       select (    
    
          Select Max(vh.HistoryDate)    
            From Vacancy vac2    
              inner join VacancyHistory vh    
               on vh.VacancyId = vac2.VacancyId    
              inner join VacancyHistoryEventType vhe    
               on vhe.VacancyHistoryEventTypeId = vh.VacancyHistoryEventTypeId    
              inner join VacancyStatusType vst    
               on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId    
            where vac2.VacancyID = vac.VacancyId    
              and UPPER(vst.CodeName) = @Status    
         ) as SubmissionDate,    
         (     
    
         Select Count(vh.VacancyHistoryID)    
         From Vacancy vac2    
           inner join VacancyHistory vh    
            on vh.VacancyId = vac2.VacancyId    
           inner join VacancyHistoryEventType vhe    
            on vhe.VacancyHistoryEventTypeId = vh.VacancyHistoryEventTypeId    
           inner join VacancyStatusType vst    
            on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId    
         where vac2.VacancyID = vac.VacancyId    
           and UPPER(vst.CodeName) = @Status    
    
         ) as SubmissionCount,    
         (    
    
          select Case WHEN vpr.ManagerIsEmployer = 1 THEN emp.FullName ELSE tp.FullName End as VacancyManager    
          from Vacancy vac3    
            inner join [VacancyOwnerRelationship] vpr    
             on vac3.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]    
            inner join Employer emp    
             on vpr.EmployerId = emp.EmployerId    
            inner join [ProviderSite] tp    
             on vpr.[ProviderSiteID] = tp.ProviderSiteID     
          where vac3.VacancyId = vac.VacancyId     
    
         ) as VacancyManager,    
         vac.Title as VacancyTitle,    
         vac.VacancyID as VacancyID,    
         (Case WHEN vac.LockedForSupportUntil > getdate() THEN vac.BeingSupportedBy ELSE '' END) as OpenedBy    
    
       from Vacancy vac    
         inner join @vacancytemp vat    
          on vac.VacancyId = vat.VacancyId    
        ) x     
       ) z    
  WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo;     
         
 SET NOCOUNT OFF    
    
END