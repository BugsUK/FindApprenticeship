/*----------------------------------------------------------------------                               
Name  : ReportRegisteredCandidatesList                  
Description :  returns summary data for report

                
History:                  
------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
08-Aug-2008		1.01		Ian Emery   corrected joins and where clauses
12-Nov-2008		1.02		Ian Emery	corrected where clause for postcode
17-Nov-2008		1.03		Femma Ashraf	Added @toDate parameter to fnReportGetAgeRangeDates
19 Jan 2010		1.04		John Hope	To stop nulled 1st name nullifying whole candidate name  
-------------------------------------------------------------------- */                 

Create procedure [dbo].[ReportRegisteredCandidatesList](  
   
 @LSCRegion int,   
 @type  int,  
 @LocalAuthority int,  
 @Postcode varchar(8000),  
 @FromDate datetime,  
 @ToDate  datetime,  
 @AgeRange int,  
 @IncludeDeregisteredCandidates int,
 @MarketMessagesOnly int,  
 @EthnicityID int,  
 @GenderID int,  
 @ProviderSiteID int,  
 @Rowcount INT = 0,  
 @ManagedBy  nvarchar(3)

  
   
  
)  
as  
   
---- ********************* TESTING BEGIN **************************************  
--DECLARE  
-- @LSCRegion int,   
-- @type  int,  
-- @LocalAuthority int,  
-- @Postcode varchar(8000),  
-- @FromDate datetime,  
-- @ToDate  datetime,  
-- @AgeRange int,  
-- @SchoolSearch varchar(200),  
-- @School  int,  
-- @DisplayResults int  
--  
--set @LSCRegion=N'3'  
--set @type=-1  
--set @LocalAuthority=N'-1'  
--set @Postcode=N'sl1,bs6,dy6'  
--set @FromDate='oct 1 2007 12:00:00:000AM'  
--set @ToDate='Oct  7 2009 12:00:00:000AM'  
--set @AgeRange=N'-1'  
--set @SchoolSearch=''  
--set @School = -1  
--set @DisplayResults=N'1'  
--  
--drop table #tmpPostcode  
---- ********************* TESTING END ****************************************  
  
  
 SET NOCOUNT ON    
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED   
  
  
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
  
 create table #ethnicity(id int PRIMARY KEY)  
                   
   
 insert into #ethnicity(id) select DISTINCT ID from DBO.fnPopulateEthnicTempTable(@EthnicityID)  
     
 --create temp table for postcodes  
 create table #tmpPostcode(outlinePostcode varchar(20))  
                   
 --populate the temp postcode table  
 insert into #tmpPostcode(outlinePostcode) select ID from dbo.fnReportSplitListToTable (@Postcode)  
  
  
 declare @cntPostcode as int  
  
 select @cntPostcode= count (*) from #tmpPostcode  
  
 --get the age range dates  
 DECLARE @MINDT DATETIME  
 DECLARE @MAXDT DATETIME  
  
 IF @AgeRange <> -1   
 BEGIN  
  select @MINDT=mindate,  @MAXDT=maxdate from dbo.fnReportGetAgeRangeDates (@AgeRange, @ToDate)  
 END  
   
 declare @astDraftID int  
 select @astDraftID =ApplicationStatusTypeID from dbo.ApplicationStatusType where codeName = 'DRF' 
  
 declare @cheStatusChangeID int  
 select @cheStatusChangeID = CandidateHistoryEventID from dbo.CandidateHistoryEvent where codeName = 'STA'  
  
  
 declare @csPreRegID int  
 select @csPreRegID =  CandidateStatusID from dbo.CandidateStatus where codeName = 'ATV' 
  
    
 if ((@Type = 3) and (@cntPostcode<= @MaxNumberOfPostcodes)) or (@Type <> 3)  
    BEGIN  
 SET ROWCOUNT @Rowcount;    
 SELECT  DISTINCT 
	c.CandidateId,
	c.CandidateGuid,
   isnull (p.FirstName + ' ', '') + p.Surname as Name,  
   c.DateofBirth,  
   lscr.GeographicFullName Region,     
   c.AddressLine1,  
   c.AddressLine2,  
   c.AddressLine3,  
   c.AddressLine4,  
   c.AddressLine5,  
   c.Town,  
   County.FullName AS County,  
   c.Postcode,     
   c.Town + ', '+ c.Postcode AS 'ShortAddress',  
   isnull(s.SchoolName + ' (' + s.Address + ')',sa.OtherSchoolName  + ' (' + sa.OtherSchoolTown + ')') as LastSchool,  
   c.LastAccessedDate DateLastActive,  
   ch.EventDate as RegistrationDate,  
    case when c.AddressLine1 is null then '' else c.AddressLine1+', ' end+  
    case when c.AddressLine2 is null then '' else c.AddressLine2+', ' end+  
    case when c.AddressLine3 is null then '' else c.AddressLine3+', ' end+   
    isnull(c.town,'')   
   as [Address], 
   p.LandlineNumber,  
   p.Email,  
  
   case when c.Gender = 0 then 'Unspecified' else gen.FullName end as Gender,  
   (select count(*) from  dbo.candidate c1   
    JOIN dbo.Application app   
     on app.CandidateId = c1.CandidateId   
     and  app.ApplicationStatusTypeId <> @astDraftID  
  
    where c.CandidateId=c1.CandidateId)   
   as ApplicationCount, 
   
   --extra fields for Summary version  
   --sv.StartDate AS ILRStartDate,  
   p.FirstName,  
   p.MiddleNames,  
   p.Surname,  
     
   p.MobileNumber,  
   dbo.fnDisplayEthnicity(ceo.FullName)as Ethnicity,  
   case when DATEdiff(dd,C.LastAccessedDate,@TODATE)>=@CandidateInactiveDays then 1 else 0 end AS Inactive,  
   replace(ISNULL((AOC.ShortName + ' ' + AOC.FullName) , 'Not Defined'),'Please Select...','Not Defined') AS Sector,  
            replace(ISNULL((APF.CodeName + ' ' + APF.FullName), 'Not Defined'),'Please Select...','Not Defined') AS Framework,  
   replace(ISNULL(ssc.Keywords, 'Not Defined'),'Please Select...','Not Defined') AS Keyword,  
   cs.FullName 'CandidateStatus',
   CASE WHEN DeRegCan.CandidateId IS NULL THEN 0
   ELSE 1 END AS 'DregisteredCandidate',
   c.AllowMarketingMessages
   
    
 FROM  
   dbo.vwRegionsAndLocalAuthority lscr   
   JOIN dbo.LocalAuthority la   
    on lscr.LocalAuthorityId = la.LocalAuthorityId   
   JOIN dbo.candidate c   
    on c.LocalAuthorityId = la.LocalAuthorityId   
   JOIN CandidateStatus cs  
    on c.CandidateStatusTypeId = cs.CandidateStatusId   
  
   LEFT JOIN SavedSearchCriteria ssc  
    ON ssc.CandidateId = c.CandidateId  
            LEFT JOIN SearchFrameworks sf   
                ON ssc.SavedSearchCriteriaId = sf.SavedSearchCriteriaId  
            LEFT JOIN dbo.ApprenticeshipFramework apf  
                 ON sf.FrameworkId = apf.ApprenticeshipFrameworkId  
            LEFT JOIN  dbo.ApprenticeshipOccupation aoc  
    ON  apf.ApprenticeshipOccupationId = aoc.ApprenticeshipOccupationId                                                                                                
      
   JOIN dbo.CandidateGender  
   ON c.Gender = candidategender.CandidateGenderId  
   JOIN dbo.County AS County  
   ON c.CountyId = county.CountyID  
   JOIN dbo.CandidateEthnicOrigin ceo  
   ON c.EthnicOrigin = ceo.CandidateEthnicOriginId  
   left JOIN dbo.SchoolAttended sa   
    on sa.CandidateId = c.CandidateId  
    and sa.applicationid is null   
    and sa.enddate =(select max(enddate) from SchoolAttended sa1 where sa1.CandidateId=sa.CandidateId )  
   LEFT JOIN dbo.School s  
    on s.SchoolId = sa.SchoolId  
   JOIN dbo.CandidateHistory ch   
    on ch.CandidateId = c.CandidateId   
    and ch.CandidateHistoryEventTypeId = @cheStatusChangeID  
   and  ch.CandidateHistorySubEventTypeId = @csPreRegID --candidate has registered 
   
   --DeRegistered Candidate
   Left JOIN dbo.CandidateHistory DeRegCan
    on DeRegCan.CandidateId = c.CandidateId   
    and DeRegCan.CandidateHistoryEventTypeId = @cheStatusChangeID  
   and  DeRegCan.CandidateHistorySubEventTypeId > @csPreRegID 
  
   JOIN dbo.Person p   
    on p.PersonId = c.PersonId   
  
   JOIN dbo.CandidateGender gen   
    on gen.CandidateGenderID = c.Gender       
  
   LEFT JOIN #tmpPostcode tpc   
    on left(c.postcode,len(tpc.outlinePostcode)+1) = tpc.outlinePostcode  
   LEFT JOIN #ethnicity eth  
    ON eth.id = c.EthnicOrigin   
  
   LEFT JOIN Application a on c.CandidateId = a.CandidateId  
   LEFT join Vacancy V on V.VacancyId = A.VacancyId  
   LEFT join VacancyOwnerRelationship VPR on VPR.VacancyOwnerRelationshipId = V.VacancyOwnerRelationshipId  
   LEFT join ProviderSIte TP on TP.ProviderSIteID = VPR.ProviderSiteID    
  
   LEFT OUTER JOIN SubVacancy sv ON sv.AllocatedApplicationId = a.ApplicationId  
   AND sv.vacancyid = v.vacancyid  
     
   WHERE   
    ((@AgeRange = -1 ) or (@AgeRange <> -1 and c.DateofBirth between @MINDT and @MAXDT))   
    AND ((@GenderID = -1) or (@GenderID <> -1 and c.Gender=@GenderID))  
    AND ((@EthnicityID = '-1') OR (@EthnicityID <> '-1' AND eth.id IS NOT null))  
    AND ((@ProviderSiteID <>-1 and tp.ProviderSiteID=@ProviderSiteID ) or (@ProviderSiteID  = -1))    
     and ch.EventDate BETWEEN @FromDate AND @ToDate AND--Common Application form   
     ((@type = -1) or  
     ((@Type = 1) AND (lscr.GeographicRegionID = @LSCRegion)) OR  
      ((@Type = 2) AND (la.LocalAuthorityId = @LocalAuthority)) OR  
      ((@Type = 3) AND ((@cntPostcode=0) or (@cntPostcode>0 and tpc.outlinePostcode IS NOT NULL))))    
     AND ((@IncludeDeregisteredCandidates = 1)  
      OR ((@IncludeDeregisteredCandidates = 0) AND NOT EXISTS  
        (SELECT * FROM   dbo.CandidateHistory ch   
        WHERE ch.CandidateId = c.CandidateId   
        and ch.CandidateHistoryEventTypeId = @cheStatusChangeID  
        and  ch.CandidateHistorySubEventTypeId > @csPreRegID)) )      
    AND (@ManagedBy = '-1' OR TP.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))    
	AND (@MarketMessagesOnly = 0 OR (@MarketMessagesOnly = 1 AND c.AllowMarketingMessages = 1))
             
   
  order by isnull (p.FirstName + ' ', '') + p.Surname    
  
   
   
end