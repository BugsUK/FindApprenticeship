Create PROCEDURE [dbo].[ReportUnsuccessfulCandidateApplications]
(
@ManagedBy          NVARCHAR(3),
@RegionID			INT, 
@type				INT, 
@LocalAuthority		INT, 
@Postcode			VARCHAR (8000), 
@FromDate			DATETIME, 
@ToDate				DATETIME, 
@candidateAgeRange	INT, 
@points				INT	= 1,
@MarketMessagesOnly int,  
@rowcount			INT= 0 			
)
AS
BEGIN
	--Calculate start and end date range values for later compare with dob
	declare @startdatetime0 as datetime
	declare @startdatetime1 as datetime
	declare @startdatetime2 as datetime
	declare @startdatetime3 as datetime
	declare @startdatetime4 as datetime
	declare @startdatetime5 as datetime

	declare @enddatetime0 as datetime
	declare @enddatetime1 as datetime
	declare @enddatetime2 as datetime
	declare @enddatetime3 as datetime
	declare @enddatetime4 as datetime
	declare @enddatetime5 as datetime

-- calculate the 'all' range
set @startdatetime0 = dateadd(year,99 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime0 = dateadd(dd, -1, dateadd(year,0 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

-- calculate the 'upto 15 years 364 days' range
set @startdatetime1 = dateadd(year,16 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime1 = dateadd(dd, -1, dateadd(year,1 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

-- calculate the '16 to 19 years 364 days' range
set @startdatetime2 = dateadd(year,19 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime2 = dateadd(dd, -1, dateadd(year,16 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

-- calculate the 'upto 20 years' range - note this includes their 20th birthday, but not 20 years 1 day
set @startdatetime3 = dateadd(year,20 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime3 = dateadd(dd, -1, dateadd(year,1 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

-- calculate the '19 to 24 years 364 days' range
set @startdatetime4 = dateadd(year,25 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime4 = dateadd(dd, -1, dateadd(year,19 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

-- calculate the '25 plus' range
set @startdatetime5 = dateadd(year,99 * -1, cast(convert(varchar(11),getdate(),106) as datetime))
set @enddatetime5 = dateadd(dd, -1, dateadd(year,25 * -1, cast(convert(varchar(11),getdate(),106) as datetime)))

	declare @MaxNumberOfPostcodes int

	-- set the max number of postcodes
	select @MaxNumberOfPostcodes = ParameterValue from SystemParameters 
		where ParameterName='ReportMaxNumberOfPostcodes'

	select @FromDate = dbo.fngetStartOfDay(@FromDate)
	select @ToDate = dbo.fngetendOfDay(@ToDate)
	--create temp table for postcodes

	declare @tmpPostcode table (outlinePostcode varchar(20))
	--populate the temp postcode table
	insert into @tmpPostcode(outlinePostcode) select ID from dbo.fnReportSplitListToTable (@Postcode)

	--check the max number of postcodes has not been exceeded
	declare @cntPostcode as int
	select @cntPostcode= count (*) from @tmpPostcode

	if ((@Type = 3) and (@cntPostcode<= @MaxNumberOfPostcodes)) or (@Type <> 3)
	BEGIN
	SET ROWCOUNT @rowcount; 	
	select 
		can.candidateid, 
		-- Candidate Identity
		p.FirstName as FirstName, p.MiddleNames as MiddleName, p.SurName as SurName,	
		case when gen.candidateGenderId = 0 THEN 'Unspecified' ELSE gen.Fullname END as Gender, 
		can.DateofBirth, 
		case	when Disability=0 or Disability=14 then 'No'
				else 'Yes'
		end as Disability,
		can.AllowMarketingMessages,
		can.AddressLine1, 
		can.AddressLine2,
		can.AddressLine3,
		can.AddressLine4,
		can.AddressLine5,
		can.Postcode, 
		can.Town, 
		county.FullName as AuthorityArea,
		can.Town + ', ' +can.Postcode 'ShortAddress',
		p.LandlineNumber as TelephoneNumber, 
		p.MobileNumber, 
		p.Email,
		
		-- Number of applications by candidate
		isnull((select count(*) from [application] app 
				where ApplicationStatusTypeId=5
				and app.candidateid=outerApp.candidateid),0) 
			as Unsuccessful,
		isnull((select count(*) from [application] app 
				where ApplicationStatusTypeId in (2,3)
				and app.candidateid=outerApp.candidateid),0) 
			as Ongoing,
		isnull((select count(*) from [application] app 
				where ApplicationStatusTypeId=4
				and app.candidateid=outerApp.candidateid),0) 
			as Withdrawn,
			
		-- Application detail
		ahApplied.ApplicationHistoryEventDate as DateApplied,
		vac.ApplicationClosingDate as VacancyClosingDate,
		ah.ApplicationHistoryEventDate as DateOfUnsuccessfulNotification,
		tp.FullName + ' (' + tp.Town + ', ' + tp.PostCode + ')' as LearningProvider,
		pr.UKPRN as LearningProviderUKPRN,
		'VAC' + replace(right('000000000' + str(vac.VacancyReferenceNumber), 9),' ','0') as VacancyReferenceNumber,
		vac.Title as VacancyTitle,
		aprtype.FullName as VacancyLevel,
		sec.Fullname as Sector,
		fr.Fullname as Framework,
		aurt.FullName as UnsuccessfulReason,
		outerApp.OutcomeReasonOther as Notes,
		aurt.ReferralPoints as Points		
	
	 from
		[application] outerApp inner join candidate can on can.candidateid = outerApp.candidateid
		
		
		inner join person p on p.PersonId = can.PersonId
		inner join candidategender gen on can.Gender = gen.CandidateGenderId
		inner join county on can.CountyId = county.CountyId
		inner join vacancy vac on outerApp.VacancyId = vac.VacancyId
		inner join VacancyOwnerRelationship vpr on vac.VacancyOwnerRelationshipId = vpr.VacancyOwnerRelationshipId
		inner join ProviderSite tp on vpr.ProviderSiteId = tp.ProviderSiteId
		inner join ProviderSiteRelationship psr on psr.ProviderSiteID = tp.ProviderSiteID AND vac.ContractOwnerId = psr.ProviderID
		inner join Provider pr on pr.ProviderID = psr.ProviderID
		inner join apprenticeshiptype aprtype on vac.ApprenticeshipType = aprtype.ApprenticeshipTypeId
		inner join apprenticeshipframework fr on vac.ApprenticeshipFrameworkId = fr.ApprenticeshipFrameworkId
		inner join apprenticeshipoccupation sec on fr.ApprenticeshipOccupationId = sec.ApprenticeshipOccupationId
		inner join ApplicationUnsuccessfulReasonType aurt on outerApp.UnsuccessfulReasonId = aurt.ApplicationUnsuccessfulReasonTypeId
		inner join applicationhistory ah on outerApp.ApplicationId=ah.ApplicationId and ah.ApplicationHistoryEventTypeId = 1 and ah.ApplicationHistoryEventSubTypeId = 5
		inner join applicationhistory ahApplied on outerApp.ApplicationId=ahApplied.ApplicationId and ahApplied.ApplicationHistoryEventTypeId = 1 and ahApplied.ApplicationHistoryEventSubTypeId = 2
		
		inner join LocalAuthority LA on LA.LocalAuthorityId = can.LocalAuthorityId
			INNER JOIN vwRegionsAndLocalAuthority RLA
						ON LA.LocalAuthorityID = RLA.LocalAuthorityID
			INNER JOIN dbo.vwRegionsAndLocalAuthority CandidateRegion
						ON can.LocalAuthorityId = CandidateRegion.LocalAuthorityId
		--inner join CandidateStatus cs on can.CandidateStatusId=2		-- FILTER: Active candidates
		LEFT JOIN @tmpPostcode tpc 
						on left(can.postcode,len(tpc.outlinePostcode)+1) = tpc.outlinePostcode
		where 
		(@RegionID= -1 or rla.GeographicRegionID = @RegionID) 
		and (@LocalAuthority = -1 or can.LocalAuthorityId = @LocalAuthority)
		and (@ManagedBy = '-1' OR TP.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))
		and can.CandidateStatusTYpeId = 2 
		and not exists(	-- FILTER: only return candidates that don't have a single successful application
			select candidateid from [application] where ApplicationStatusTypeId=6 and CandidateId=outerApp.CandidateId)
		and		-- FILTER: Age range 
			can.DateofBirth between
				(case @candidateAgeRange
					when -1 then @startdatetime0  
					when 1 then @startdatetime1
					when 2 then @startdatetime2
					when 3 then @startdatetime3
					when 4 then @startdatetime4
					when 5 then @startdatetime5
				end)
				and 
				(case @candidateAgeRange
					when -1 then @enddatetime0
					when 1 then @enddatetime1
					when 2 then @enddatetime2
					when 3 then @enddatetime3
					when 4 then @enddatetime4
					when 5 then @enddatetime5
				end)
		-- FILTER: area
		AND ((@type = -1) or
			((@Type = 1) AND (rla.GeographicRegionID = @RegionID)) OR
			((@Type = 2) AND (can.LocalAuthorityId = @LocalAuthority)) OR
			((@Type = 3) AND ((@cntPostcode=0) or (@cntPostcode>0 and  
				tpc.outlinePostcode is NOT Null ))))
		-- FILTER: date range
		and ah.ApplicationHistoryEventDate BETWEEN @FromDate AND @ToDate
		-- filter optin/out
		AND (@MarketMessagesOnly = 0 OR (@MarketMessagesOnly = 1 AND can.AllowMarketingMessages = 1))

	ORDER BY 
	Surname, FirstName, MiddleName, DateofBirth, Postcode, DateApplied, DateOfunsuccessfulNotification
	END
END