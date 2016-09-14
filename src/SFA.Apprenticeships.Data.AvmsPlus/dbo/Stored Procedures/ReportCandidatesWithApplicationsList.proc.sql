/*----------------------------------------------------------------------                                 
Name  : ReportCandidatesWithApplicationsList                    
Description :  returns summary data for report  
  
                  
History:                    
--------                    
Date		 Version  Author			Comment  
20-Aug-2008  1.0   Ian Emery		first version  
08-Aug-2008  1.01  Ian Emery		corrected joins and where clauses  
21-Oct-2008  1.02  Ian Emery		changed the filtering on ethnicity  
11-Nov-2008  1.03  Ian Emery		fixed disabililty and corrected join to outline postcode removing view.  
12-Nov-2008  1.04  Ian Emery		added town and postcode to school, training provider and employer  
17-Nov-2008  1.05  Femma Ashraf		Added @toDate parameter to fnReportGetAgeRangeDates  
24-Nov-2008  1.06  Ian Emery		Corrected LP count this is now a count distinct not a count  
25-Nov-2008  1.07  Femma Ashraf		Added the fnDisplayEthnicity around the ethnicity column so that   
							'		Please Select' etc are displayed as unspecified  
25-Nov-2008  1.08  Ian Emery		Added the fnDisplayDisability around the disability column so that   
									'Please Select...'  are displayed as Not Selected  
03-Dec-2008  1.09  Femma Ashraf		Changed the logic for In/Out region so it comes off the Vacancy table.  
18 Jan 2010  1.10  John Hope		To stop nulled 1st name nullifying whole candidate name  
14 Sept 2016 1.11  Shoma Gujjar		Add CandidateId to the report.
---------------------------------------------------------------------- */                   
 
  
Create procedure [dbo].[ReportCandidatesWithApplicationsList](  
   
   
 @ManagedBy  nvarchar(3),  
 @GeoRegion  int,   
 @type   int,  
 @LocalAuthority int,  
 @Postcode  varchar(8000),  
 @GenderID  int,  
 @EthnicityID int,  
 @DisabilityID int,  
 @AgeRange  int,  
 @ProviderSiteID int,  
 @ApplicationStatus int,  
 @EmployerID  int,  
 @FromDate datetime,  
 @ToDate  datetime,  
 @LSCInOut int,  
 @VacancyReferenceNumber int,  
 @VacancyTitle VARCHAR(100) = '',  
 @RecAgentID int = -1,
 @rowcount int = 0  
   
  
)  
as  
  
---- ******************************************debug start **************************************  
--declare   
-- @LSCRegion  int,   
-- @type   int,  
-- @LocalAuthority int,  
-- @Postcode  varchar(8000),  
-- @GenderID  int,  
-- @Ethnicity VARCHAR(8000),  
-- @DisabilityID int,  
-- @AgeRange  int,  
-- @LearningProviderID int,  
-- @School  int,  
-- @FromDate datetime,  
-- @ToDate  datetime,  
-- @LSCInOut int,   
-- @DisplayResults int  
--  
--  
--set @LSCRegion=N'5'  
--set @type=1  
--set @LocalAuthority=N'-1'  
--set @Postcode=N'n/a'  
--set @GenderID=N'-1'  
--set @Ethnicity=N''  
--set @DisabilityID=N'0'  
--set @AgeRange=N'-1'  
--set @LearningProviderID=N'-1'  
--set @School=N'-1'  
--set @FromDate='jun  2 2007 12:00:00:000AM'  
--set @ToDate='jul  1 2009 12:00:00:000AM'  
--set @DisplayResults=N'1'  
--set @LSCInOut=-1  
--  
--drop table #tmpPostcode  
--drop table #ethnicity  

-- [ReportCandidatesWithApplicationsList] '-1',1,-1,1,'B1 1LY',-1,-1,-1,-1,-1,1,-1,'12/12/2011','12/01/2012',-1,-1,''
---- ******************************************debug end **************************************   
  
  
 SET NOCOUNT ON    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
  
  
 --todo need to get the location for the global parameter defined as 5 days for now  
 Declare @CandidateInactiveDays int  
  
 -- set the number of Candidate Inactive Days  
 select @CandidateInactiveDays = ParameterValue from SystemParameters   
  where ParameterName='ReportCandidateInactiveDays'  
  
  
 Declare @MaxNumberOfPostcodes int  
  
 -- set the max number of postcodes  
 select @MaxNumberOfPostcodes = ParameterValue from SystemParameters   
  where ParameterName='ReportMaxNumberOfPostcodes'  
  
  
 select @FromDate = dbo.fngetStartOfDay(@FromDate)  
 select @ToDate = dbo.fngetendOfDay(@ToDate)  
  
  
 declare @astNewID int  
 select @astNewID = ApplicationStatusTypeID from dbo.ApplicationStatusType where codeName = 'NEW'  
  
 --create temp table for postcodes  
 create table #tmpPostcode(outlinePostcode varchar(20) PRIMARY KEY)  
                   
 --populate the temp postcode table  
 insert into #tmpPostcode(outlinePostcode) SELECT DISTINCT ID from dbo.fnReportSplitListToTable (@Postcode)  
   
   
 --ethnicity  
 create table #ethnicity(id int PRIMARY KEY)  
                   
 --populate the temp postcode table  
 insert into #ethnicity(id) select DISTINCT ID from DBO.fnPopulateEthnicTempTable(@EthnicityID)  
   
  
 declare @cntPostcode as int  
  
 select @cntPostcode= count (*) from #tmpPostcode  
   
  
 --get the age range dates  
 DECLARE @MINDT DATETIME  
 DECLARE @MAXDT DATETIME  
  
 IF @AgeRange <> -1   
 BEGIN  
  select @MINDT=mindate,  @MAXDT=maxdate from dbo.fnReportGetAgeRangeDates (@AgeRange,@ToDate)  
 END  
  
  
 if ((@Type = 3) and (@cntPostcode<= @MaxNumberOfPostcodes)) or (@Type <> 3)  
 begin   
 SET ROWCOUNT @rowcount;    
  
 SELECT   
  CAN.CandidateId as CandidateId,  
   --app.applicationid,  
  isnull (p.FirstName + ' ', '') + p.Surname as Name,  
  can.DateofBirth,  
  --gen.FullName Gender,  
  case when can.Gender = 0 then 'Unspecified' else Gen.FullName end as Gender,  
  p.Email,  
  --ceo.FullName Ethnicity,  
  dbo.fnDisplayEthnicity(CEO.FullName)as 'Ethnicity',  
  --CASE when  @DisabilityID =0 THEN 'None' ELSE dis.FullName END Disability,  
  dbo.fnDisplayDisability(dis.FullName) as Disability,  
  can.Postcode,  
  --isnull(s.schoolName,sa.OtherSchoolName) LastSchool,  
  isnull(s.SchoolName + ' (' + s.Address + ')',sa.OtherSchoolName +  ' (' + sa.OtherSchoolTown + ')') as LastSchool,  
   isnull(s.SchoolName,sa.OtherSchoolName) AS 'SchoolName',
  ISNULL(s.Address1,'') AS 'SchoolAddress1',
  ISNULL(s.Address2,'') AS 'SchoolAddress2',
  ISNULL(s.Area,'') AS 'SchoolArea',
  coalesce(s.Town,sa.OtherSchoolTown,'') AS 'SchoolTown',
  ISNULL(s.County,'') AS 'SchoolCounty' ,
  ISNULL(s.Postcode,'') AS 'SchoolPostcode',
  --tpr.TradingName LearningProvider,  
  tpr.TradingName + ' (' + tpr.Town + ', ' + tpr.PostCode +')' LearningProvider,   
  dbo.fnReportGenerateVacancyRefNo( vac.VacancyReferenceNumber ) as VacancyReferenceNumber,  
    
  --Extra fields for export version  
  p.FirstName,  
  p.MiddleNames,  
  p.Surname,  
  can.addressLine1,  
  can.addressLine2,  
  can.addressLine3,  
  Can.town,  
  CandidateRegion.GeographicFullName AS CandidateRegion,  
  County.FullName,  
  p.LandlineNumber,  
  p.MobileNumber,  
  ch.EventDate as DateRegistered,  
  can.LastAccessedDate as DateLastLoggedOn,  
  emp.tradingName AS EmployerName,  
  emp.addressline1 AS empAddressLine1,  
  emp.addressline2 AS empAddressLine2,  
  emp.addressline3 AS empAddressLine3,  
  emp.addressline4 AS empAddressLine4, 
  emp.addressline5 AS empAddressLine5, 
  emp.town AS empTown,  
  empCounty.FullName AS empCounty,  
  emp.Postcode AS empPostCode,  
  Vac.Title AS VacancyTitle,  
  at.Fullname VancacyType, --vac.VancacyType,  
  af.Fullname VacancyCategory,  
  ao.FullName VacancySector,  
  ast.Fullname ApplicationStatus,  
  DATEDIFF(dd,aph2.ApplicationHistoryEventDate, getDate()) as  NumberOfDaysApplicationAtThisStatus,  
  aph.ApplicationHistoryEventDate ApplicationHistoryEventDate,--ah.ApplicationHistoryEventDate DateApplicationMade, -- need date when it was created....  
  aph2.ApplicationHistoryEventDate ApplicationStatusSetDate,  
  vac.ApplicationClosingDate VacancyClosingDate,
 can.addressLine4,  
 can.addressLine5,
 can.Town + ', ' + can.Postcode AS ShortAddress,  
  CASE WHEN ((vac.ApplicationClosingDate < GETDATE()) AND (vst.CodeName = 'Lve')) THEN 'Closed' ELSE vst.FullName END VacancyStatus,  
  
 1 AS AppsMade,  
 CASE WHEN App.ApplicationStatusTypeID in (2,3) THEN 1 ELSE 0 END as Ongoing,  
 CASE WHEN App.ApplicationStatusTypeID = 6 THEN 1 ELSE 0 END  as Successful,  
 CASE WHEN App.ApplicationStatusTypeID = 5 THEN 1 ELSE 0 END  as Unsuccessful,  
 CASE WHEN App.ApplicationStatusTypeID = 4 THEN 1 ELSE 0 END  as Withdrawn  
  
  
  FROM dbo.Application app  
--   join  dbo.Application app1  
--    on app1.CandidateID = app.CandidateID  
   JOIN dbo.ApplicationHistory aph  
    on aph.ApplicationId = app.ApplicationId  
    and aph.ApplicationHistoryEventSubTypeId = @astNewID  
    and aph.ApplicationHistoryEventTypeId = 1  
   JOIN ApplicationHistory aph2 on aph2.ApplicationId = app.ApplicationId  
    AND aph2.ApplicationHistoryId = (SELECT MIN(ApplicationHistoryId)  
            FROM ApplicationHistory ah1  
            WHERE ah1.applicationid = app.applicationid  
            AND ah1.ApplicationHistoryEventTypeId = 1  
            AND ah1.ApplicationHistoryEventSubTypeId = app.ApplicationStatusTypeId)  
   JOIN dbo.ApplicationStatusType ast  
    on ast.ApplicationStatusTypeId = aph.ApplicationHistoryEventSubTypeId  
  
   JOIN  dbo.Candidate can   
    on can.CandidateId = app.CandidateId  
     
   JOIN dbo.CandidateGender gen   
    on gen.CandidateGenderID = can.Gender   
   JOIN dbo.CandidateDisability dis  
    on dis.CandidateDisabilityId = can.Disability   
   JOIN dbo.CandidateEthnicOrigin ceo  
    on ceo.CandidateEthnicOriginId = can.EthnicOrigin  
   JOIN dbo.CandidateHistory ch ON can.CandidateId = ch.CandidateId  
   AND ch.CandidateHistoryEventTypeId = 1  
    AND ch.CandidateHistorySubEventTypeId = (SELECT candidatestatusID FROM dbo.CandidateStatus WHERE CodeName = 'ATV')  
   --exclude deregistered candidates here if required  
   JOIN dbo.Vacancy vac  
    on vac.VacancyId = app.VacancyId AND (@RecAgentID = -1 OR vac.VacancyManagerId = @RecAgentID)
   JOIN dbo.VacancyStatusType VST  
   ON vac.VacancyStatusId = vst.VacancyStatusTypeId  
   JOIN ApprenticeshipType at  
    on at.ApprenticeshipTypeId = vac.ApprenticeshipType  
   JOIN ApprenticeshipFramework af  
    on af.ApprenticeshipFrameworkId = vac.ApprenticeshipFrameworkId  
   JOIN dbo.ApprenticeshipOccupation AO ON af.ApprenticeshipOccupationId  
   = AO.ApprenticeshipOccupationId  
  
   JOIN dbo.VacancyOwnerRelationship vpr  
    on vpr.VacancyOwnerRelationshipId = vac.VacancyOwnerRelationshipId  
    JOIN dbo.ProviderSite tpr  
    on tpr.ProviderSiteID = vpr.ProviderSiteID  
    JOIN dbo.Employer emp  
    on emp.EmployerId = vpr.EmployerId   
   JOIN county AS empCounty  
   ON emp.countyID = empCounty.COuntyID  
   JOIN dbo.LocalAuthority la  
    on la.LocalAuthorityId = vac.LocalAuthorityId   
   JOIN dbo.vwRegionsAndLocalAuthority RLA  
   ON vac.LocalAuthorityId = RLA.LocalAuthorityId  
   INNER JOIN dbo.vwRegionsAndLocalAuthority CandidateRegion  
   ON Can.LocalAuthorityId = CandidateRegion.LocalAuthorityId  
   LEFT JOIN dbo.County AS County ON can.CountyId = County.CountyId  
   LEFT JOIN dbo.SchoolAttended sa   
     on sa.CandidateId = can.CandidateId  
     and sa.applicationid is null   
     and sa.enddate =(select max(enddate) from SchoolAttended sa1 where sa1.CandidateId=sa.CandidateId )  
   LEFT JOIN dbo.School s  
    on s.SchoolId = sa.SchoolId  
   JOIN dbo.Person p   
    on p.PersonId = can.PersonID  
--   JOIN dbo.vwReportPostcodeOutline opc   
--    on p.personid=opc.personid   
--   LEFT JOIN #tmpPostcode tpc   
--    on tpc.outlinePostcode=opc.PostCodeOutline   
   left JOIN #tmpPostcode tpc   
    on left(can.postcode,len(tpc.outlinePostcode)+1) = tpc.outlinePostcode  
   LEFT JOIN #ethnicity eth  
    ON eth.id = can.EthnicOrigin  
     
     
   WHERE  
     aph.ApplicationHistoryEventDate BETWEEN @FromDate AND @ToDate --Common Application form  
     AND ((@type = -1) or  
      ((@Type = 1 ) AND( RLA.GeographicRegionID = @GeoRegion)) OR  
      ((@Type = 2 ) AND (can.LocalAuthorityId = @LocalAuthority))  OR  
      ((@Type = 3) AND ((@cntPostcode=0) or (@cntPostcode>0 and  tpc.outlinePostcode is NOT Null ))))  
     AND (@ManagedBy = '-1' OR TPr.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))  
     AND ((@GenderID = -1) or (@GenderID <> -1 and can.Gender=@GenderID))  
     AND ((@DisabilityID=-1) OR (@DisabilityID <> -1 AND can.Disability = @DisabilityID))  
     AND ((@AgeRange = -1 ) or (@AgeRange <> -1 and can.DateofBirth between @MINDT and @MAXDT))  
     AND ((@EthnicityID = '-1') OR (@EthnicityID <> '-1' AND eth.id IS NOT null))  
     AND ((@DisabilityID = -1) OR (@DisabilityID <> -1 AND can.Disability = @DisabilityID))  
     AND ((@ProviderSiteID = -1) or (@ProviderSiteID <> -1 AND tpr.ProviderSiteID = @ProviderSiteID))  
     AND ((@VacancyReferenceNumber = -1) or (@VacancyReferenceNumber <> -1 AND vac.VacancyReferenceNumber = @VacancyReferenceNumber))  
     AND ((LTRIM(@VacancyTitle) = '') or (LTRIM(@VacancyTitle) <> '' AND vac.Title = @VacancyTitle))  
     AND ((@EmployerID = -1) or (@EmployerID <> -1 AND vpr.EmployerID = @EmployerID))  
     AND (@LSCInOut = -1   
      or (@LSCInOut = 1 --In Region  
       and (rla.GeographicRegionID = CandidateRegion.GeographicRegionID))  
      or (@LSCInOut = 2 --Out of Region  
       and (rla.GeographicRegionID <> CandidateRegion.GeographicRegionID)))  
         AND ((@ApplicationStatus  = -1) OR (aph.ApplicationHistoryEventSubTypeId  = @ApplicationStatus))
  --group by can.CandidateId,  
  -- isnull (p.FirstName + ' ', '') + p.Surname,  
  -- can.DateofBirth,  
  -- case when can.Gender = 0 then 'Unspecified' else Gen.FullName end,  
  -- p.Email,  
  -- ceo.FullName ,  
  -- dis.FullName ,  
  -- can.Postcode,  
  -- --isnull(s.schoolName,sa.OtherSchoolName) ,  
  -- isnull(s.SchoolName + ' (' + s.Address + ')',sa.OtherSchoolName +  ' (' + sa.OtherSchoolTown + ')'),  
  -- --tpr.TradingName,  
  -- tpr.TradingName + ' (' + tpr.Town + ', ' + tpr.PostCode +')',  
  -- App.CandidateID,  
  -- app.applicationid,  
  -- vac.VacancyReferenceNumber  
  
     
  
--order by p.FirstName + ' ' + p.Surname  
   
  
  
END