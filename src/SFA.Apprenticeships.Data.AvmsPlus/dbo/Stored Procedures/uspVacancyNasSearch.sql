CREATE PROCEDURE [dbo].[uspVacancyNasSearch]
@apprenticeshipOccupation VARCHAR (200), @apprenticeshipFramework VARCHAR (200), @EmployerName NVARCHAR (200), @providerName NVARCHAR (200), @vacancyReference NVARCHAR (100), @vacancyStatus NVARCHAR (200), @vacancyTitle NVARCHAR (200), @lscRegion INT, @vacancyType INT, @PageIndex INT=1, @PageSize INT=20, @IsSortAsc BIT=1, @SortByField NVARCHAR (100)='ProviderName'
AS
BEGIN              
          
SET NOCOUNT ON              

DECLARE @allowDraftVacancies bit
Declare @statusDraft varchar(20)
set @statusDraft='DRF'

Declare @apprenticeshipOccupationId int
Declare @apprenticeshipFrameworkId int

if @allowDraftVacancies  = 1      
 set @allowDraftVacancies = null
if @employerName = ''  
 set @employerName = null 
if @providerName = ''  
 set @providerName = null  
if @vacancyReference= ''  
 set @vacancyReference = null  
if @vacancyTitle= ''  
 set @vacancyTitle = null

if @lscRegion = 0
 set @lscRegion = null

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

/*********CCR11193*****************************************************/

declare @providerName2 nvarchar(200)

if @providerName = 'and'
	set @providerName2 = '&'
else if @providerName = '&'
	set @providerName2 = ' and '
else if charindex('&', @providerName) > 0
	set @providerName2 = replace(@providerName, '&', 'and')
else if charindex('and', @providerName) > 0
	set @providerName2 = replace(@providerName, 'and', '&')

/**********************************************************************/  
  
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
if @SortByField = 'VacancyTitle'    
Begin     
 if @IsSortAsc = 1 BEGIN set  @SortByField = 'VacancyTitle Asc' END    
 if @IsSortAsc = 0 BEGIN  set  @SortByField = 'VacancyTitle desc' END    
End           
/***********************************************************************/        
      
declare @TotalRows int        
      
SELECT * FROM        
(         
Select ROW_NUMBER() OVER( ORDER BY     
 Case when @SortByField='EmployerName Asc'  then e.FullName End ASC,    
 Case when @SortByField='EmployerName desc'  then e.FullName End DESC,    
 Case when @SortByField='ProviderName Asc'  then tp.TradingName  End ASC,    
 Case when @SortByField='ProviderName desc'  then tp.TradingName End DESC,
 Case when @SortByField='VacancyTitle Asc'  then v.Title  End ASC,    
 Case when @SortByField='VacancyTitle desc'  then v.Title End DESC,
 v.VacancyId
 ) as RowNum,       
v.VacancyId,      
v.ApprenticeshipFrameworkId,af.Fullname as 'frameworkName',VPR.employerId, e.FullName as 'employerName',       
ISNULL((select count(a1.applicationid) from application a1  
where a1.ApplicationStatusTypeId in (select ApplicationStatusTypeId from applicationstatustype ast where ast.CodeName != 'DRF')  
and a1.vacancyid = v.vacancyid),0) as 'numberOfApplications',      
v.numberOfPositions,       
VPR.[ProviderSiteID] as 'providerId',       
tp.TradingName as 'providerName', v.VacancyReferenceNumber as 'vacancyReference',       
vst.FullName as 'vacancyStatus',      
VST.VacancyStatusTypeId as 'vacancyStatusId',         
v.Title as 'vacancyTitle',
v.ApplicationClosingDate as 'ClosingDate',       
v.ApprenticeshipType as 'vacancyType',      
at.FullName as 'vacancyTypeName',
TotalRows = count(1) over()           
from vacancy v     
inner join [VacancyOwnerRelationship] VPR on VPR.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]   
inner join dbo.Employer e on e.EmployerId =  VPR.EmployerId      
inner join dbo.[ProviderSite] tp on tp.ProviderSiteID = VPR.[ProviderSiteID]      
inner join VacancyStatusType VST on VST.VacancyStatusTypeId = v.VacancyStatusId 
inner join LocalAuthority LA on LA.LocalAuthorityId = v.LocalAuthorityId   
INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
AND LocalAuthorityGroupTypeName = N'Region'  
left outer join [Application] a on a.vacancyId = v.vacancyId      
left outer join ApprenticeshipFramework af on af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId      
left outer join apprenticeshipOccupation ao on ao.apprenticeshipOccupationId = af.apprenticeshipOccupationId      
left outer join ApprenticeshipType at on at.ApprenticeshipTypeId = v.ApprenticeshipType      

Where
(@allowDraftVacancies is null or vst.FullName  != case when @allowDraftVacancies = 0 then 'Draft' end )     
and (ao.FullName = @apprenticeshipOccupation or @apprenticeshipOccupation is null)      
and (af.Fullname = @apprenticeshipFramework or @apprenticeshipFramework is null) 
and (e.FullName like + '%' + @employerName + '%' or e.TradingName like + '%' + @employerName + '%' or @employerName is null)           
and ((tp.TradingName like + '%' + @providerName + '%' or @providerName is null) or (tp.TradingName like + '%' + @providerName2 + '%' or @providerName is null))      
and (v.VacancyReferenceNumber = @vacancyReference or @vacancyReference is null) 
and (v.Title  like + '%' + @vacancyTitle + '%' or @vacancyTitle is null)  
--and (LA.LscRegionId = @lscRegion or @lscRegion is null) 
AND  (LAG.LocalAuthorityGroupID = @lscRegion or @lscRegion is null)         
and (vst.Fullname = @vacancyStatus or @vacancyStatus is null)      
and (v.ApprenticeshipType = @vacancyType or @vacancyType is null)      
      
group by v.vacancyId,      
v.ApprenticeshipFrameworkId,af.Fullname, VPR.employerId, e.FullName,v.numberOfPositions, VPR.[ProviderSiteID],v.VacancyReferenceNumber,      
tp.TradingName, vst.FullName, VST.VacancyStatusTypeId,v.Title,v.ApplicationClosingDate, v.ApprenticeshipType,at.FullName      
) as DerivedTable        
WHERE RowNum BETWEEN @StartRowNo AND @EndRowNo        
ORDER BY RowNum
               
SET NOCOUNT OFF              
END