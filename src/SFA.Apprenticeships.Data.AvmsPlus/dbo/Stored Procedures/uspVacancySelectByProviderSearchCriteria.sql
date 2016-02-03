CREATE PROCEDURE [dbo].[uspVacancySelectByProviderSearchCriteria]                  
(           
	@providerId int,     
	@recruitmentAgencyId int = 0,       
	@apprenticeshipOccupation varchar(200),      
	@apprenticeshipFramework varchar(200),      
	@employerName nvarchar(200),      
	@vacancyReference nvarchar(100),      
	@vacancyStatus nvarchar(200),      
	@vacancyType int,      
	@allowDraftVacancies bit,   
	@excludeRecruitmentAgencies bit = 0,   
	@ShowVacanciesFromSuspendedContractProviders int = 0,
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
Declare @ownerProviderId int    

SELECT @ownerProviderId = ProviderId
FROM ProviderSiteRelationship 
WHERE ProviderSiteID = @providerId AND ProviderSiteRelationShipTypeID=1
      
if @allowDraftVacancies  = 1      
 set @allowDraftVacancies = null      
if @allowDraftVacancies  = 1          
 set @allowDraftVacancies = null          
if @employerName = ''      
 set @employerName = null      
if @vacancyReference= ''      
 set @vacancyReference = null      
begin    
if @apprenticeshipOccupation = ''  or  @apprenticeshipOccupation = 0    
    set @apprenticeshipOccupation = null      
else    
    begin    
    SET @apprenticeshipOccupationId = CAST(@apprenticeshipOccupation as int)    
    SELECT @apprenticeshipOccupation = FullName from apprenticeshipoccupation Where ApprenticeshipOccupationId = @apprenticeshipOccupationId    
    end    
end    
    
begin    
if @apprenticeshipFramework = ''   or @apprenticeshipFramework = 0    
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
      
  
SELECT MyTable.*  FROM            
	( 
	select 
	TotalRows = count(1) over(),
	ROW_NUMBER() OVER( ORDER BY  
	    
	Case when @SortByField='EmployerName Asc'  then e.FullName End ASC,        
	Case when @SortByField='EmployerName desc'  then e.FullName End DESC,        
	Case when @SortByField='ProviderName Asc'  then tp.TradingName  End ASC,        
	Case when @SortByField='ProviderName desc'  then tp.TradingName End DESC,    
	v.Title ASC         
	--Case when @SortByField='ClosingDate Asc'  then v.ApplicationClosingDate  End ASC,        
	--    Case when @SortByField='ClosingDate desc'  then v.ApplicationClosingDate End DESC        
	) as RowNum,      
	v.VacancyId,      
	ISNULL(v.ApprenticeshipFrameworkId, 0) As 'ApprenticeshipFrameworkId',  
	ISNULL(af.Fullname, '') as 'Fullname',  
	e.employerId,  
	e.FullName as 'employerName',       
	ISNULL((select count(a1.applicationid) from application a1  
	where a1.ApplicationStatusTypeId in (select ApplicationStatusTypeId from applicationstatustype ast where ast.CodeName != 'DRF')  
	and a1.vacancyid = v.vacancyid),0) as 'numberOfApplications',      
	v.numberOfPositions,       
	VPR.[ProviderSiteID] as 'providerId',       
	tp.TradingName as 'providerName',  
	v.VacancyReferenceNumber as 'vacancyReference',       
	vst.FullName as 'vacancyStatus',      
	VST.VacancyStatusTypeId as 'vacancyStatusId',      
	NULL as 'vacancyStatusDate',      
	v.Title as 'vacancyTitle',     
	v.ApplicationClosingDate as 'ClosingDate',       
	v.ApprenticeshipType as 'vacancyType',      
	at.FullName as 'vacancyTypeName'      
from vacancy v     
	inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]       
	inner join dbo.Employer e on e.EmployerId =  VPR.EmployerId          
	inner join dbo.[ProviderSite] tp on tp.ProviderSiteID = VPR.[ProviderSiteID]
	LEFT JOIN PROVIDER CO ON v.contractownerid = CO.ProviderID
	inner join VacancyStatusType VST on VST.VacancyStatusTypeId = v.VacancyStatusId      
	left outer join [Application] a on a.vacancyId = v.vacancyId      
	left outer join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId      
	left outer join apprenticeshipOccupation ao on ao.apprenticeshipOccupationId = af.apprenticeshipOccupationId      
	left outer join ApprenticeshipType at on at.ApprenticeshipTypeId = v.ApprenticeshipType      


    
Where
	(
		(@allowDraftVacancies is null AND CO.ProviderId IS NULL)
		OR
		(@ShowVacanciesFromSuspendedContractProviders = 1 AND co.ProviderId <> @ownerProviderId AND co.ProviderStatusTypeID = 3) 
		OR 
		(@ShowVacanciesFromSuspendedContractProviders = 0 AND (co.ProviderId = @ownerProviderId OR co.ProviderStatusTypeID != 3 ))
	)  
	AND (vst.VacancyStatusTypeId <> 4)  
	and (@allowDraftVacancies is null or vst.FullName  != case when @allowDraftVacancies = 0 then 'Draft' end )      
	and VPR.[ProviderSiteID] = @providerId       
	and (@recruitmentAgencyId = 0 OR v.VacancyManagerID = @recruitmentAgencyId)
	and (@excludeRecruitmentAgencies = 0 OR v.VacancyManagerID = VPR.ProviderSiteID)
	and (ao.FullName = @apprenticeshipOccupation or @apprenticeshipOccupation is null)      
	and (af.Fullname = @apprenticeshipFramework or @apprenticeshipFramework is null)      
	and (e.FullName like + '%' + @employerName + '%' or @employerName is null)      
	and (v.VacancyReferenceNumber = @vacancyReference or @vacancyReference is null)      
	and (vst.Fullname = @vacancyStatus or @vacancyStatus is null)      
	and (v.ApprenticeshipType = @vacancyType or @vacancyType is null)    
      
group by v.vacancyId,      
  ISNULL(v.ApprenticeshipFrameworkId, 0),  
  ISNULL(af.Fullname, ''),  
  e.employerId,  
  e.FullName,  
  v.numberOfPositions,  
  VPR.[ProviderSiteID],  
  v.VacancyReferenceNumber,      
  tp.TradingName,  
  vst.FullName,  
  VST.VacancyStatusTypeId,  
  v.Title,  
  v.ApplicationClosingDate,  
  v.ApprenticeshipType,  
 -- vh.HistoryDate,  
  at.FullName      
) as MyTable            
			   
			WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo            
            		ORDER BY RowNum      
               
SET NOCOUNT OFF              
END