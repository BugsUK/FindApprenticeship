/*----------------------------------------------------------------------                
               
Name  : uspVacancySelectByEmployerSearchCriteria                
                
Description :  Retrieve all vacancy records with specified                 
    search criteria                
                
Returns:       Resultset                
                
History:                
--------                
11 Aug 2008  1.00.   H Patel.         
11 Aug 2008  2.00.   H Patel. set null value ,set NULL value for @vacancyType, as requested by UI person            
15 Aug 2008  3.00    H Patel. employerId nd TrainingProviderId is removed from VacancyTable    
19 Aug 2008  4.00    Jismon. Application Closing Date added,vacancy title sort included.    
21 Aug 2008  5.00    Jismon. Replaced FullName search with tradingName    
24 Sep 2008  6.0     A. Perkins Correct join to VacancyHistory table for vacancyStatusDate.    
02 Dec 2008  6.0     Manish Poddar put a check to not count Draft Applications in total no. of Application  
---------------------------------------------------------------------- */                
--exec uspVacancySelectByEmployerSearchCriteria  1196,0,null,null,null,null,null,0,1,50,1,'VacancyTitle'            
CREATE PROCEDURE [dbo].[uspVacancySelectByEmployerSearchCriteria]                        
(                 
 @employerId int,            
 @apprenticeshipOccupation varchar(200),            
 @apprenticeshipFramework varchar(200),            
 @providerName nvarchar(200),            
 @vacancyReference nvarchar(100),            
 @vacancyStatus nvarchar(200),            
 @vacancyType int,            
 @allowDraftVacancies bit,            
 @PageIndex int =  1,              
 @PageSize int = 20,              
 @IsSortAsc bit= 1,              
 @SortByField nvarchar(100) = 'EmployerName'                 
)                  
AS                    
BEGIN                    
            
                
SET NOCOUNT ON                    
      
Declare @statusDraft varchar(20)    
set   @statusDraft='DRF'    
    
Declare @apprenticeshipOccupationId int      
Declare @apprenticeshipFrameworkId int      
            
if @allowDraftVacancies  = 1            
 set @allowDraftVacancies = null            
if @providerName = ''        
 set @providerName = null        
if @vacancyReference= ''        
 set @vacancyReference = null        
begin      
if @apprenticeshipOccupation = '' or  @apprenticeshipOccupation = 0      
    set @apprenticeshipOccupation = null        
else      
    begin      
    SET @apprenticeshipOccupationId = CAST(@apprenticeshipOccupation as int)      
    SELECT @apprenticeshipOccupation = FullName from apprenticeshipoccupation Where ApprenticeshipOccupationId = @apprenticeshipOccupationId      
    end      
end      
      
begin      
if @apprenticeshipFramework = ''  or @apprenticeshipFramework = 0      
    set @apprenticeshipFramework = null        
else      
    begin      
    SET @apprenticeshipFrameworkId = CAST(@apprenticeshipFramework as int)      
    SELECT @apprenticeshipFramework = FullName from ApprenticeshipFramework Where ApprenticeshipFrameworkId = @apprenticeshipFrameworkId      
    end      
end      
-- Value in database is "Plese Select"         
if @vacancyType = 0        
 set @vacancyType = null        
        
/*********Set Page Number**********************************************/              
declare @StartRowNo int              
declare @EndRowNo int              
set @StartRowNo =((@PageIndex-1)* @PageSize)+1               
set @EndRowNo =(@PageIndex * @PageSize)                  
/***********************************************************************/              
/*********set the order by**********************************************/              
declare @OrderBywithSort varchar(500)          
if @SortByField = 'EmployerName'          
Begin           
 if @IsSortAsc = 1 BEGIN set  @SortByField = 'EmployerName Asc' END          
 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'EmployerName desc' END          
End         
if @SortByField = 'ProviderName'          
Begin      
 if @IsSortAsc = 1 BEGIN set  @SortByField = 'ProviderName Asc' END          
 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'ProviderName desc' END          
End         
--if @SortByField = 'ClosingDate'          
--Begin           
-- if @IsSortAsc = 1 BEGIN set  @SortByField = 'ClosingDate Asc' END          
-- if @IsSortAsc = 0 BEGIN  set  @SortByField = 'ClosingDate desc' END          
--End                 
/***********************************************************************/              
            
/**************Total Number of Rows*************************************/              
declare @TotalRows int              
select @TotalRows = count(1)            
from             
(Select   v.VacancyId            
from vacancy v           
inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]         
inner join dbo.Employer e on e.EmployerId =  VPR.EmployerId            
inner join dbo.[ProviderSite] tp on tp.ProviderSiteID = VPR.[ProviderSiteID] 
AND tp.TrainingProviderStatusTypeId != 3
INNER JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID
AND co.ProviderStatusTypeID != 3          
inner join VacancyStatusType VST on VST.VacancyStatusTypeId = v.VacancyStatusId            
left outer join [Application] a on a.vacancyId = v.vacancyId            
left outer join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId            
left outer join apprenticeshipOccupation ao on ao.apprenticeshipOccupationId = af.apprenticeshipOccupationId            
left outer join ApprenticeshipType at on at.ApprenticeshipTypeId = v.ApprenticeshipType            
left outer join       
 vacancyHistory vh on       
   v.VacancyId = vh.VacancyId       
  and       
   v.VacancyStatusId = vh.VacancyHistoryEventSubTypeId      
  and      
   vh.VacancyHistoryEventTypeId = 1--TODO: Check this value      
  and       
   vh.HistoryDate =        
   (select max(HistoryDate)       
    from vacancyHistory vh1      
    where       
    v.VacancyId = vh1.VacancyId       
    and       
    v.VacancyStatusId = vh1.VacancyHistoryEventSubTypeId      
    and      
    vh1.VacancyHistoryEventTypeId = 1      
   )      
Where             
(@allowDraftVacancies is null or vst.FullName  != case when @allowDraftVacancies = 0 then 'Draft' end )            
and VPR.EmployerId = @employerId           
and (ao.FullName = @apprenticeshipOccupation or @apprenticeshipOccupation is null)            
and (af.Fullname = @apprenticeshipFramework or @apprenticeshipFramework is null)            
and (tp.TradingName like + '%' + @providerName + '%' or @providerName is null)            
and (v.VacancyReferenceNumber = @vacancyReference or @vacancyReference is null)            
and (vst.CodeName not in ( 'Dft', 'Ref' ) )    
and (vst.Fullname = @vacancyStatus or @vacancyStatus is null)            
and (v.ApprenticeshipType = @vacancyType or @vacancyType is null)        
--and a.ApplicationStatusTypeId in (select ApplicationStatusTypeId from applicationstatustype ast where ast.CodeName != @statusDraft)               
            
group by v.vacancyId,            
v.ApprenticeshipFrameworkId,af.Fullname, VPR.employerId, e.FullName,v.numberOfPositions, VPR.[ProviderSiteID],v.VacancyReferenceNumber,            
tp.TradingName, vst.FullName, VST.VacancyStatusTypeId,v.Title,v.ApplicationClosingDate, v.ApprenticeshipType,vh.HistoryDate,at.FullName  ) as derivedtotal            
            
               
/***********************************************************************/              
            
SELECT *,@TotalRows  as 'TotalRows' FROM              
(               
Select ROW_NUMBER() OVER( ORDER BY           
 Case when @SortByField='EmployerName Asc'  then e.FullName End ASC,          
 Case when @SortByField='EmployerName desc'  then e.FullName End DESC,          
 Case when @SortByField='ProviderName Asc'  then tp.TradingName  End ASC,          
 Case when @SortByField='ProviderName desc'  then tp.TradingName End DESC,      
 v.Title ASC          
 ) as RowNum,    
v.VacancyId,            
v.ApprenticeshipFrameworkId,  
af.Fullname as 'FrameworkName',  
VPR.employerId,   
e.FullName as 'employerName',             
count(a.ApplicationId) as 'numberOfApplications',            
v.numberOfPositions,             
VPR.[ProviderSiteID] as 'providerId',             
tp.TradingName as 'providerName', v.VacancyReferenceNumber as 'vacancyReference',             
vst.FullName as 'vacancyStatus',            
VST.VacancyStatusTypeId as 'vacancyStatusId',            
vh.HistoryDate as 'vacancyStatusDate',            
v.Title as 'vacancyTitle',      
v.ApplicationClosingDate as 'ClosingDate',             
v.ApprenticeshipType as 'vacancyType',            
at.FullName as 'vacancyTypeName'            
from vacancy v           
inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]         
inner join dbo.Employer e on e.EmployerId =  VPR.EmployerId            
inner join dbo.[ProviderSite] tp on tp.ProviderSiteID = VPR.[ProviderSiteID] 
AND tp.TrainingProviderStatusTypeId != 3
INNER JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID
AND co.ProviderStatusTypeID != 3  
inner join VacancyStatusType VST on VST.VacancyStatusTypeId = v.VacancyStatusId            
left outer join [Application] a on a.vacancyId = v.vacancyId            
left outer join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId            
left outer join apprenticeshipOccupation ao on ao.apprenticeshipOccupationId = af.apprenticeshipOccupationId            
left outer join ApprenticeshipType at on at.ApprenticeshipTypeId = v.ApprenticeshipType            
left outer join       
 vacancyHistory vh on       
   v.VacancyId = vh.VacancyId       
  and       
   v.VacancyStatusId = vh.VacancyHistoryEventSubTypeId      
  and      
   vh.VacancyHistoryEventTypeId = 1--TODO: Check this value      
  and       
   vh.HistoryDate =        
   (select max(HistoryDate)       
    from vacancyHistory vh1      
    where       
    v.VacancyId = vh1.VacancyId       
    and       
    v.VacancyStatusId = vh1.VacancyHistoryEventSubTypeId      
    and      
    vh1.VacancyHistoryEventTypeId = 1      
   )      
Where             
(@allowDraftVacancies is null or vst.FullName  != case when @allowDraftVacancies = 0 then 'Draft' end )            
and VPR.EmployerId = @employerId           
and (ao.FullName = @apprenticeshipOccupation or @apprenticeshipOccupation is null)            
and (af.Fullname = @apprenticeshipFramework or @apprenticeshipFramework is null)            
and (tp.TradingName like + '%' + @providerName + '%' or @providerName is null)            
and (v.VacancyReferenceNumber = @vacancyReference or @vacancyReference is null)            
and (vst.CodeName not in ( 'Dft', 'Ref' ) )    
and (vst.Fullname = @vacancyStatus or @vacancyStatus is null)            
and (v.ApprenticeshipType = @vacancyType or @vacancyType is null)     
--and a.ApplicationStatusTypeId in (select ApplicationStatusTypeId from applicationstatustype ast where ast.CodeName != @statusDraft)           
     
group by v.vacancyId,            
v.ApprenticeshipFrameworkId,af.Fullname, VPR.employerId, e.FullName,v.numberOfPositions, VPR.[ProviderSiteID],v.VacancyReferenceNumber,            
tp.TradingName, vst.FullName, VST.VacancyStatusTypeId,v.Title,v.ApplicationClosingDate, v.ApprenticeshipType,vh.HistoryDate,at.FullName            
) as DerivedTable              
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo              
                     
SET NOCOUNT OFF                    
END