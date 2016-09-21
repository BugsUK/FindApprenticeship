/*----------------------------------------------------------------------                               
Name  : ReportApplicationsReceivedList
Description :  Returns full data for report
        
History:                  
--------                  
Date			Version		Author			Comment
09-Sep-2008		1.0			Stuart Edwards	First version
12-Nov-2008		1.01		Ian Emery		corrected postcode join
13-Nov-2008		1.02		Ian Emery		added town and postcode to school, training provider and employer
17-Nov-2008		1.03		Femma Ashraf	Added @toDate parameter to fnReportGetAgeRangeDates
25-Nov-2008		1.04		Femma Ashraf	Added the fnDisplayEthnicity around the ethnicity column so that 
											'Please Select' etc are displayed as unspecified
03-Dec-2008		1.05		Femma Ashraf	Changed the Filters for LSC Region, Local Authority and Postcode
											so that they come off the Vacancy table. Removed the ManagerIsEmployer logic check
19-Sep-2016		1.06		Shoma Gujjar	Capture CandidateId and CandidateGuid
---------------------------------------------------------------------- */                 

CREATE PROCEDURE [dbo].[ReportApplicationsReceivedList](

	@ManagedBy          NVARCHAR(3),
	@GeoRegion			int, 
	@LocalAuthority		int,
	@Postcode			nvarchar(4000),
	@FromDate			date,
	@ToDate				date,
	@Sector		  		int,
	@Framework			int,
    @ProviderSIteID		int,
	@AgeRange			int,
	@Gender				int,
	--@EthnicOrigin		int,
	@Ethnicity			int,
	@ApplicationStatus	int,
	@InOrOutOfRegion	int,
	@RecAgentID			int= -1,
	@rowcount			int = 0
	
)
AS

---- *************************************** DEBUG CODE START ***************************************************************
--DECLARE
--@ID					INT,
--@TypeID				INT,
--@LSCRegion			int, 
--@LocalAuthority		int,
--@TrainingProvider	int,
--@Gender				int,
--@Postcode			nvarchar(4000),
--@FromDate			datetime,
--@ToDate				datetime,
--@Occupation  		int,
--@Framework			int,
--@AgeRange			int,
--@Status				int,
--@DisplayResults		INT,
--@Ethnicity			nvarchar(800),
--@InOrOutOfRegion	int
--
--SET  @ID			= 1				
--SET	 @TypeID		= 2	
--SET	 @LSCRegion		= -1
--SET	 @LocalAuthority= -1	
--set @Gender				= -1
--set @TrainingProvider = -1
--SET	 @Postcode		= 'N/A'--
--SET	 @Postcode		= 'ln6,pe30'
--SET	 @FromDate		= '2008-01-01'
--SET	 @ToDate		= '2009-01-01'	
--SET	 @Occupation  	= -1
--SET	 @Framework		= -1
--SET	 @AgeRange		= -1
--SET	 @Status		= -1	
--SET @DisplayResults	= 1
--set @Ethnicity		= ''
--set @InOrOutOfRegion = -1
--
--DROP TABLE #tmpPostcode
--drop table #ethnicity
--
---- **************************************** DEBUG CODE START ***************************************************************

BEGIN
	SET NOCOUNT ON  
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 



	--get the age range dates
	DECLARE @MINDT DATETIME
	DECLARE	@MAXDT DATETIME

	IF @AgeRange <> -1 
	BEGIN
		select @MINDT=mindate,  @MAXDT=maxdate from dbo.fnReportGetAgeRangeDates (@AgeRange, @ToDate)
	END

	--Define new applicaton
	declare @newApp int
	SELECT @newApp = ast.ApplicationStatusTypeId FROM dbo.ApplicationStatusType ast WHERE codename='NEW'
	
	--Define Status change
	DECLARE @aheStatusChange AS INT
	SELECT @aheStatusChange = ApplicationHistoryEventID FROM dbo.ApplicationHistoryEvent WHERE codename='STC'

	--create temp table for postcodes
	create table #tmpPostcode(outlinePostcode varchar(20) PRIMARY KEY)
																
	--populate the temp postcode table
	insert into #tmpPostcode(outlinePostcode) select ID from dbo.fnReportSplitListToTable (@Postcode)
	
	--ethnicity
	create table #ethnicity(id INT PRIMARY Key)
																 
	--populate the temp postcode table
	insert into #ethnicity(id) select ID from DBO.[fnPopulateEthnicTempTable](@Ethnicity)

BEGIN TRY

	select @FromDate = dbo.fngetStartOfDay(@FromDate)
	select @ToDate = dbo.fngetendOfDay(@ToDate)

	--check the max number of postcodes has not been exceeded
	declare @cntPostcode as int
	select @cntPostcode= count (*) from #tmpPostcode
		Declare @MaxNumberOfPostcodes int

	-- set the max number of postcodes
	select @MaxNumberOfPostcodes = ParameterValue from SystemParameters 
		where ParameterName='ReportMaxNumberOfPostcodes'

	if ((@Postcode <> '-1') and (@cntPostcode<= @MaxNumberOfPostcodes)) or (@Postcode = '-1')
	BEGIN
	SET ROWCOUNT @rowcount;  
	select	isnull(P.FirstName + ' ','') + P.Surname as CandidateName,
			C.CandidateId as CandidateId,
			C.CandidateGuid as CandidateGuid,
			P.Email,
			C.AddressLine1 AS AddressLine1,
			C.AddressLine2 AS AddressLine2,
			C.AddressLine3 AS AddressLine3,
			C.AddressLine4 AS AddressLine4,
			C.AddressLine5 AS AddressLine5,
			C.Town AS Town,
			County.FullName AS County,
			C.Postcode AS Postcode,
			C.Town + ', ' + C.Postcode AS ShortAddress,
			P.LandlineNumber AS CandidateTelephone,
		    isnull(s.SchoolName + ' (' + s.Address + ')',sa.OtherSchoolName + ' (' + sa.OtherSchoolTown + ')') as School,
			C.DateOfBirth,
			dbo.fnDisplayEthnicity(CEO.FullName)as 'EthnicOrigin',
			
			case when c.Gender = 0 then 'Unspecified' else CG.FullName end as Gender,
			(AO.ShortName + ' ' + AO.FullName) as Sector,
			(AF.CodeName + ' ' + AF.FullName) as Framework,
			AFST.Fullname as FrameworkStatus,
			
			e.TradingName + ' (' + e.Town + ', ' + e.PostCode +')' as Employer,
			V.Postcode as VacancyPostcode,
			
			tp.TradingName + ' (' + tp.Town + ', ' + tp.PostCode +')' as TrainingProvider,
			V.ApplicationClosingDate as ApplicationClosingDate,
			(select min(AH.ApplicationHistoryEventDate) from ApplicationHistory AH
				where AH.ApplicationId = A.ApplicationId
				and AH.ApplicationHistoryEventTypeId = (select AHE.ApplicationHistoryEventId
													from ApplicationHistoryEvent AHE 
													where AHE.ApplicationHistoryEventID = @aheStatusChange)
				and AH.ApplicationHistoryEventSubTypeId = (select AST2.ApplicationStatusTypeId
													from ApplicationStatusType AST2 
													where AST2.ApplicationStatusTypeID = @newApp))
			as ApplicationDate,

			AST.FullName as ApplicationStatus,
			A.AllocatedTo,
			V.VacancyID
	from
		Application A
		inner join ApplicationStatusType AST on AST.ApplicationStatusTypeId = A.ApplicationStatusTypeId
		inner join Candidate C on C.CandidateId = A.CandidateId
		inner join CandidateEthnicOrigin CEO on CEO.CandidateEthnicOriginId = C.EthnicOrigin
		inner join CandidateGender CG on CG.CandidateGenderId = C.Gender
		inner join Person P on P.PersonId = C.PersonId
		left join  SchoolAttended SA on SA.CandidateId = C.CandidateId and a.applicationid = sa.applicationid
		left join  School S on S.SchoolId = SA.SchoolId
		inner join Vacancy V on V.VacancyId = A.VacancyId
		inner join VacancyOwnerRelationship VPR on VPR.VacancyOwnerRelationshipId = V.VacancyOwnerRelationshipId
		join ProviderSite TP on TP.ProviderSiteId = VPR.ProviderSiteId -- and vpr.ManagerIsEmployer = 0
		join Employer E on E.EmployerId = VPR.EmployerId  --and vpr.ManagerIsEmployer = 1
		inner join ApprenticeshipFramework AF on AF.ApprenticeshipFrameworkId = V.ApprenticeshipFrameworkId
		inner join ApprenticeshipOccupation AO on AO.ApprenticeshipOccupationId = AF.ApprenticeshipOccupationId
		inner join ApprenticeshipFrameworkStatusType AFST on AF.ApprenticeshipFrameworkStatusTypeId = AFST.ApprenticeshipFrameworkStatusTypeId
		inner join LocalAuthority LA on LA.LocalAuthorityId = V.LocalAuthorityId
		INNER JOIN vwRegionsAndLocalAuthority RLA
			ON LA.LocalAuthorityID = RLA.LocalAuthorityID
		INNER JOIN dbo.vwRegionsAndLocalAuthority CandidateRegion
			ON C.LocalAuthorityId = CandidateRegion.LocalAuthorityId
		 LEFT JOIN county ON C.CountyId = dbo.County.CountyId
		AND C.CountyId <> 0
		LEFT JOIN #ethnicity eth
			ON eth.id = C.EthnicOrigin
		left JOIN #tmpPostcode tpc 
			ON LEFT(V.postcode,LEN(tpc.outlinePostcode)+1) = tpc.outlinePostcode


	where 
		(@GeoRegion= -1 or rla.GeographicRegionID = @GEORegion) 
			and (@LocalAuthority = -1 or V.LocalAuthorityId = @LocalAuthority)

		and (@Postcode = '-1' or @Postcode <> '-1' AND tpc.outlinePostcode IS NOT null)
		AND (@ManagedBy = '-1' OR TP.ManagingAreaID IN (SELECT LocalAuthorityGroupID FROM dbo.ReportGetChildManagingAreas(@ManagedBy)))
		and (@Sector = -1 or AO.ApprenticeshipOccupationId = @Sector) 
		and (@Framework = -1 or AF.ApprenticeshipFrameworkId = @Framework)
		and (@ProviderSiteID = -1 or TP.ProviderSiteID = @ProviderSIteID)
		and (@Gender = -1 or C.Gender = @Gender)
		--and (@EthnicOrigin = -1 or C.EthnicOrigin = @EthnicOrigin)
		AND ((@Ethnicity = '-1') OR (@Ethnicity <> '' AND eth.id IS NOT null))
		and (@ApplicationStatus = -1 or A.ApplicationStatusTypeId = @ApplicationStatus)
		AND ((@AgeRange = -1 ) or (@AgeRange <> -1 and c.DateofBirth between @MINDT and @MAXDT))
		AND (@InOrOutOfRegion = -1 
			or (@InOrOutOfRegion = 1 --In Region
				and (rla.GeographicRegionID = CandidateRegion.GeographicRegionID))
			or (@InOrOutOfRegion = 2 --Out of Region
				and (rla.GeographicRegionID <> CandidateRegion.GeographicRegionID)))
		and (select min(AH.ApplicationHistoryEventDate) from ApplicationHistory AH
				where AH.ApplicationId = A.ApplicationId
				and AH.ApplicationHistoryEventTypeId = (select AHE.ApplicationHistoryEventId
													from ApplicationHistoryEvent AHE 
													--where AHE.FullName = 'Status Change')
													where AHE.ApplicationHistoryEventID = @aheStatusChange)
				and AH.ApplicationHistoryEventSubTypeId = (select AST3.ApplicationStatusTypeId
													from ApplicationStatusType AST3
													--where AST3.FullName = 'New')) 
													where AST3.ApplicationStatusTypeID = @newApp))

			between @FromDate and @ToDate
			AND (@RecAgentID = -1 or V.vacancymanagerid = @RecAgentID)
	order by AO.FullName, AF.FullName, (isnull(P.FirstName + ' ','') + P.Surname)
	end
END TRY  
	BEGIN CATCH  
		EXEC RethrowError
	END CATCH  
      
    SET NOCOUNT OFF  
END