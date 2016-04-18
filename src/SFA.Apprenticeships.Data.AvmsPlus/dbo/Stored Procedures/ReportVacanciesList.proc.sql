/*----------------------------------------------------------------------                               
Name  : ReportVacanciesList                  
Description :  returns list for report

                
History:                  
--------                  
Date			Version		Author			Comment
09-SEP-2008		1.0			Femma Ashraf	first version
13-Nov-2008		1.1			Daniel Winter	modified use of postcode and local authority parameters
04-Dec-2008		1.2			Femma Ashraf	Changed the Filters for LSC Region, Local Authority and Postcode
											so that they come off the Vacancy table. Removed the ManagerIsEmployer logic check
01-May-2009		1.3			Manish Excluded National Vacancies											
14-Jan-2010     1.4         Hitesh          Vacancy posted date - ITSM668658 - two date for live status issue in Vacancyhistory table
22-Apr-2010		1.4			Mike Davies		ITSM550094 & ITSM1079564
30-Jun-2010             1.5                     Gareth Gange            ITSM1023128 - For Vacancies with Closing Date of today show as Live
---------------------------------------------------------------------- */                 

create procedure [dbo].[ReportVacanciesList](
	@ManagedBy			nvarchar(5),
	@type				int,
	@lscRegion			int,
	@localAuthority		int,
	@Postcode			varchar(8000),
	@sector				int,
	@framework			int,
	
	@vacancyType		int,
	@dateFrom			datetime,
	@dateTo				datetime,
	@VacancyStatus		INT = -1,
	
	@ProviderSiteID		INT = -1,
	@RecAgentID		    INT = -1,
	@EmployerID			INT = -1,
	@rowcount			int = 0
)

as

/************************* start of debug ******************************	
DECLARE	
	@type				INT,
	@lscRegion			INT,
	@localAuthority		INT,
	@postcode			VARCHAR(8000),
	@sector				INT,
	@framework			INT,
	@trainingProvider	INT,
	@vacancyType		INT,
	@dateFrom			DATETIME,
	@dateTo				DATETIME,
	@displayResults		INT,
	@TypeID				INT,
	@ID					INT

SET @lscRegion	= -1
SET @type		= -1
SET @localAuthority = -1
SET @postcode	= 'n/a'--nn8,le19'
SET @dateFrom	= '01-Jan-08'
SET @dateTo		= '22-Dec-08'
SET @sector  	= -1
SET @framework	= -1
SET @vacancyType = -1 
SET @trainingProvider = -1
SET @displayResults = 1
SET @typeid = 99

DROP TABLE #tmpPostcode
************************* end of debug ******************************/
	
set nocount on  
set transaction isolation level read uncommitted

-- set the max number of postcodes
declare @MaxNumberOfPostcodes int;
select @MaxNumberOfPostcodes = ParameterValue from SystemParameters where ParameterName='ReportMaxNumberOfPostcodes';

--Populate postcodes
declare @tmpPostcode table(Postcode varchar(20));
insert into @tmpPostcode(Postcode) select ID from dbo.fnReportSplitListToTable (@Postcode);

--check the max number of postcodes has not been exceeded
declare @cntPostcode as int
select @cntPostcode= count (*) from @tmpPostcode

if @Type = 3 and @cntPostcode > @MaxNumberOfPostcodes
	return;

declare @liveStatusNumber int,
		@SuccessfulApp int;
select @liveStatusNumber = VacancyStatusTypeID from VacancyStatusType where codename='Lve';
select @SuccessfulApp = ApplicationStatusTypeId from ApplicationStatusType where codename='SUC';

declare @vacancyHistory table( 
	vacancyId int not null primary key,					
	historyDate datetime);
	
-- date when vacany became Live	
insert into @vacancyHistory(vacancyId, historyDate)	
select vh.VacancyId, max(vh.HistoryDate) 
from  dbo.VacancyHistory vh
join dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = vh.VacancyHistoryEventSubTypeId
where vh.VacancyHistoryEventTypeID = 1
	and vst.CodeName = 'Lve'
group by vh.VacancyId

-- remove vacancies out of range
select @dateTo = dbo.fngetendOfDay(@dateTo);
delete from @vacancyHistory where historyDate not between @dateFrom and @dateTo;


declare @managingAreas table(LocalAuthorityGroupID int);
-- 
if (@ManagedBy <> '-1')
	insert into @managingAreas(LocalAuthorityGroupID)
	select LocalAuthorityGroupID 
	from dbo.ReportGetChildManagingAreas(@ManagedBy);


declare @application table(
	VacancyId int not null primary key,
	NumberOfApplications int not null,
	NumberOfSuccessfulApplications int not null 
	);

-- total number of applications and number of successful applications	
insert into @application(VacancyId, NumberOfApplications, NumberOfSuccessfulApplications)
select 
	app1.VacancyId
	,Count(app1.ApplicationId)
	, (select Count(app2.ApplicationId) from Application app2 where app2.VacancyId = app1.VacancyId and app2.ApplicationStatusTypeId = @SuccessfulApp)
from Application app1
where app1.ApplicationStatusTypeId > 1
group by app1.VacancyId;

declare @region table(
	LocalAuthorityId int not null primary key,
	LocalAuthorityGroupId int not null
);

-- load regions
insert into @region(LocalAuthorityId, LocalAuthorityGroupId)
select LAGM.LocalAuthorityID, lagm.LocalAuthorityGroupID
from LocalAuthorityGroupType LAGT 
join  LocalAuthorityGroup LAG ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
join LocalAuthorityGroupMembership LAGM ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
where LAGT.LocalAuthorityGroupTypeName = N'Region';


set rowcount @rowcount;
select 
	 vac.vacancyid
	,vac.Title as VacancyTitle
	,apt.FullName as VacancyType
	,'VAC'+replicate('0',9 - len(convert(varchar(20),vac.VacancyReferenceNumber)))+ convert(varchar(20),vac.VacancyReferenceNumber) AS Reference
	,case 
		when vac.EmployerAnonymousName is not null then vac.EmployerAnonymousName
		else emp.TradingName + ' (' + emp.Town +')' 
	 end as EmployerName
    ,emp.TradingName + ' (' + emp.Town +')' as EmployerNameActual  
	,vac.EmployerAnonymousName as EmployerAnonymousName  
	,case 
		when isnull(vac.EmployerAnonymousName,'') = '' then 'No' 
		else 'Yes' 
	end as IsEmployerAnonymous       
	,vac.PostCode as Postcode
	,(aoc.ShortName + ' ' + aoc.FullName) as Sector
	,(apf.CodeName + ' ' + apf.FullName) as Framework
	,afst.Fullname as FrameworkStatus
	,tpr.TradingName + ' (' + tpr.Town + ', ' + tpr.PostCode +')' as LearningProvider
	,vac.NumberofPositions as NumberOfPositions
	,vh.HistoryDate DatePosted
	,vac.applicationClosingDate ClosingDate
	,vac.NumberofPositions - app.NumberOfSuccessfulApplications	NoOfPositionsAvailable
	,app.NumberOfApplications NoOfApplications
	,case 
		when vac.applicationClosingDate < GetDate() and vac.vacancystatusId = 2 then 'Closed'
		else vst.FullName
	 end as Status
	,psd.TradingName + ' (' + psd.Town + ', ' + psd.PostCode +')' as DeliverySite		
from @vacancyHistory vh 
join Vacancy vac on vac.VacancyId = vh.VacancyId 
join ApprenticeshipFramework apf on apf.ApprenticeshipFrameworkId = vac.ApprenticeshipFrameworkId 
join ApprenticeshipFrameworkStatusType afst on apf.ApprenticeshipFrameworkStatusTypeId = afst.ApprenticeshipFrameworkStatusTypeId
join dbo.VacancyOwnerRelationship vpr on vpr.VacancyOwnerRelationshipId = vac.VacancyOwnerRelationshipId
join Employer emp on vpr.EmployerId = emp.EmployerId 
join  ApprenticeshipOccupation aoc on  apf.ApprenticeshipOccupationId = aoc.ApprenticeshipOccupationId		
join dbo.ApprenticeshipType apt on apt.ApprenticeshipTypeId = vac.ApprenticeshipType
join dbo.ProviderSite tpr on tpr.ProviderSiteId = vpr.ProviderSiteId
join dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = vac.vacancystatusId
join @region reg on reg.LocalAuthorityId = vac.LocalAuthorityId
left outer join dbo.ProviderSite psd on psd.ProviderSiteID = VAC.DeliveryOrganisationID
left outer join @tmpPostcode tpc on left(vac.postcode, len(tpc.Postcode)+1) = tpc.Postcode
left outer join @application app on app.VacancyID = vac.VacancyID 
where
	(	   (@type = -1) 
		or ((@type = 1) and reg.LocalAuthorityGroupID = @lscRegion)
		or ((@type = 2) and vac.LocalAuthorityId = @localAuthority)
		or ((@Type = 3) and (@cntPostcode = 0 or @cntPostcode > 0 and tpc.Postcode is not null))
	)
	and (@sector = -1 or aoc.ApprenticeshipOccupationId = @sector)    
	and (@framework = -1 or apf.ApprenticeshipFrameworkId = @framework )
	and (@managedBy = '-1' or tpr.ManagingAreaID in (select LocalAuthorityGroupID from @managingAreas))
	and (@ProviderSiteID = -1 or tpr.ProviderSiteId=@ProviderSiteID) 
	and (@RecAgentID = -1 or vac.VacancyManagerID = @RecAgentID) 
	and (@VacancyStatus = -1 or vac.VacancyStatusId = @VacancyStatus) 
	and (@EmployerID = -1 or vpr.EmployerId = @EmployerID) 		
	and (@vacancyType = -1 or apt.ApprenticeshipTypeId = @vacancyType)
	and isnull(vac.vacancylocationtypeid, -1) <> 3
order by vac.Title
