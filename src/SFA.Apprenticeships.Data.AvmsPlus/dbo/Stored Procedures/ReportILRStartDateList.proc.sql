/*----------------------------------------------------------------------                               
Name  :                  
Description :  

                
History:                  
--------                  
Date			Version		Author		Comment
20-Aug-2008		1.0			Ian Emery	first version
04-Nov-2008		2.0			D Winter	Modified variables
13-Nov-2008		2.01		Ian Emery	added missing joins
17-Nov-2008		2.02		Femma Ashraf	Added @toDate parameter to fnReportGetAgeRangeDates
04-Dec-2008		2.03		Femma Ashraf	Changed the logic for In/Out region so it comes off the Vacancy table.
18 Jan 2010		2.04		John Hope		To stop nulled 1st name nullifying whole candidate name 
---------------------------------------------------------------------- */                 

CREATE PROCEDURE [dbo].[ReportILRStartDateList](
	
	@Type				int,
	@ManagedBy		varchar(10),
	@LSCRegion			int, 
	@LocalAuthority		int,
	@Postcode			varchar(8000),
	@ProviderSiteID	int,
	@LSCInOut			int,
	@StartDatePresent	int, 
	@AgeRange			int,
	@FromDate			datetime,
	@ToDate				DATETIME,
	@RecAgentID			int = -1,
	@Rowcount			int = 0

) AS




SET NOCOUNT ON  
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 


	DECLARE @MaxNumberOfPostcodes INT

	-- set the max number of postcodes
	SELECT @MaxNumberOfPostcodes = ParameterValue FROM SystemParameters 
		WHERE ParameterName='ReportMaxNumberOfPostcodes'

	SELECT @FromDate = dbo.fngetStartOfDay(@FromDate)
	SELECT @ToDate = dbo.fngetendOfDay(@ToDate)

    --create temp table for postcodes
	CREATE TABLE #tmpPostcode(outlinePostcode VARCHAR(20))	
															 
    --populate the temp postcode table
    INSERT INTO #tmpPostcode(outlinePostcode) SELECT ID FROM dbo.fnReportSplitListToTable (@Postcode)

	--check the max number of postcodes has not been exceeded
	DECLARE @cntPostcode AS INT

	SELECT @cntPostcode= COUNT(*) FROM #tmpPostcode

	DECLARE @statusChange int
	DECLARE @succesfulApp int
	
	SELECT @statusChange = ApplicationHistoryEventId FROM ApplicationHistoryEvent WHERE CodeName = 'STC'
	SELECT @succesfulApp = ApplicationStatusTypeId FROM ApplicationStatusType WHERE CodeName = 'SUC'

	DECLARE @aheStatusChange AS INT
	SELECT @aheStatusChange = ApplicationHistoryEventID FROM dbo.ApplicationHistoryEvent WHERE codename='STC'

	DECLARE @SuccessfulApp AS INT
	SELECT @SuccessfulApp = ast.ApplicationStatusTypeId FROM dbo.ApplicationStatusType ast WHERE codename='SUC'


	--get the age range dates
	DECLARE @MINDT DATETIME
	DECLARE	@MAXDT DATETIME

	IF @AgeRange <> -1 
	BEGIN
		select @MINDT=mindate,  @MAXDT=maxdate from dbo.fnReportGetAgeRangeDates (@AgeRange, @ToDate)
	END

	if ((@Type = 3) and (@cntPostcode<= @MaxNumberOfPostcodes)) or (@Type <> 3)

	BEGIN
		
		SET ROWCOUNT @Rowcount;

		SELECT 
			isnull(p.FirstName + ' ','') + p.Surname as Name,
			
			case when c.Gender = 0 then 'Unspecified' else CG.FullName end as Gender,
			c.Postcode,
			
			tp.TradingName + ' (' + tp.Town + ', ' + tp.PostCode +')'  LearningProvider,
			dbo.fnReportGenerateVacancyRefNo( v.VacancyReferenceNumber ) as VacancyReferenceNumber,
			v.Title VacancyTitle,
			at.FullName VacancyType,
			(AO.ShortName + ' ' + AO.FullName) Sector,
			(AF.CodeName + ' ' + AF.FullName) Framework,
			AFST.Fullname as FrameworkStatus,
		
			e.TradingName + ' (' + e.Town + ', ' + e.PostCode +')' Employer,
			CONVERT(varchar, (select min(AH.ApplicationHistoryEventDate) from ApplicationHistory AH
								where AH.ApplicationId = a.ApplicationId
								and AH.ApplicationHistoryEventTypeId = @aheStatusChange
								and AH.ApplicationHistoryEventSubTypeId = @SuccessfulApp
																		), 103) SuccessfulAppDate,
			CONVERT(varchar, sv.startdate, 103) ILRStartDate,
			sv.ILRNumber ILRReference


		FROM Application a
		INNER JOIN ApplicationHistory ah ON ah.ApplicationId = a.ApplicationId
			AND ah.ApplicationHistoryId = (SELECT MIN(ApplicationHistoryId)
											FROM ApplicationHistory ah1
											WHERE ah1.applicationid = a.applicationid
											AND ah1.ApplicationHistoryEventTypeId = @statusChange
											AND ah1.ApplicationHistoryEventSubTypeId = @succesfulApp)
		INNER JOIN Candidate c ON c.CandidateId = a.CandidateId
		INNER JOIN dbo.vwRegionsAndLocalAuthority CandidateRegion
		ON c.LocalAuthorityId = CandidateRegion.LocalAuthorityId

		INNER JOIN CandidateGender cg ON c.Gender = cg.CandidateGenderId
		INNER JOIN Person p ON c.PersonId = p.PersonId
		INNER JOIN Vacancy v ON v.VacancyId = a.VacancyId
		INNER JOIN ApprenticeshipType at ON v.apprenticeshiptype = at.apprenticeshiptypeid 
		INNER JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId
		INNER JOIN ApprenticeshipOccupation ao ON ao.ApprenticeshipOccupationId = af.ApprenticeshipOccupationId
		inner join ApprenticeshipFrameworkStatusType AFST on AF.ApprenticeshipFrameworkStatusTypeId = AFST.ApprenticeshipFrameworkStatusTypeId
		INNER JOIN VacancyOwnerRelationship vpr ON vpr.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
		INNER JOIN ProviderSIte tp ON tp.ProviderSiteID = vpr.ProviderSIteID
		INNER JOIN Employer e ON e.EmployerId = vpr.EmployerId
		JOIN dbo.LocalAuthority la
				on la.LocalAuthorityId = v.LocalAuthorityId
		INNER JOIN dbo.vwRegionsAndLocalAuthority VacancyRegion
		ON v.LocalAuthorityId = VacancyRegion.LocalAuthorityId

		LEFT OUTER JOIN SubVacancy sv ON sv.AllocatedApplicationId = a.ApplicationId
			AND sv.vacancyid = v.vacancyid
		left JOIN #tmpPostcode tpc 
			on left(c.postcode,len(tpc.outlinePostcode)+1) = tpc.outlinePostcode

		WHERE ah.ApplicationHistoryEventDate BETWEEN @FromDate AND  @ToDate
		AND ((@type = -1) 
			OR (@Type = 1  AND candidateRegion.GeographicRegionID = @LSCRegion)
			OR (@Type = 2 AND c.LocalAuthorityID = @LocalAuthority)
			OR ((@Type = 3) AND ((@cntPostcode=0) or (@cntPostcode>0 and tpc.outlinePostcode IS NOT NULL))))
		AND ((@StartDatePresent = -1)
			OR (@StartDatePresent = 1 AND sv.StartDate IS NOT NULL)
			OR (@StartDatePresent = 2 AND sv.StartDate IS NULL))
		AND ((@LSCInOut = -1) or
		    (@LSCInOut = 1 --In Region
			and (VacancyRegion.GeographicRegionID = candidateregion.GeographicRegionID))
			or (@LSCInOut = 2 --Out of Region
			and (VacancyRegion.GeographicRegionID != candidateregion.GeographicRegionID)))

		AND ((@AgeRange = -1 ) or (@AgeRange <> -1 and c.DateofBirth between @MINDT and @MAXDT))
		and (@ProviderSiteID = -1 or TP.ProviderSiteID = @ProviderSiteID)
		AND (@ManagedBy = '-1' OR TP.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))
		and a.applicationStatusTypeId = @succesfulApp
		AND (@RecAgentID = -1 or v.vacancymanagerid = @RecAgentID)
	END				
