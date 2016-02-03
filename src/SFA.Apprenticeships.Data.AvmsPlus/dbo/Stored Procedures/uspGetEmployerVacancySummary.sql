/*----------------------------------------------------------------------                               
Name  : uspGetEmployerVacancySummary                  
Description :  returns employer vacancy summary 

                
History:                  
--------                  
Date			Version		Author		Comment
21/02/2011		1.0			D. Kraevoy	first version
---------------------------------------------------------------------- */				

create procedure [dbo].[uspGetEmployerVacancySummary] (
	@vacancyId int
)
as
begin

SET NOCOUNT ON  

declare @offline bit;
declare @vacancyReference int;
declare @vacancyTitle nvarchar( 100 );
declare @vacancyStatus nvarchar( 3 );
declare @employerName nvarchar( 255 );
declare @employerDisabledPositive bit;
declare @providerName nvarchar( 255 );

-- get vacancy details
select 
 	@offline = v.ApplyOutsideNAVMS
,   @vacancyReference = v.VacancyReferenceNumber 	
,	@vacancyTitle = v.Title
,	@employerName = e.FullName
,   @vacancyStatus = vst.CodeName 
,	@employerDisabledPositive = e.DisableAllowed
,	@providerName = tp.FullName
from dbo.Vacancy v 
join dbo.[VacancyOwnerRelationship] vpr on vpr.[VacancyOwnerRelationshipId] = v.[VacancyOwnerRelationshipId]
join dbo.VacancyStatusType vst on vst.VacancyStatusTypeId = v.VacancyStatusId
join dbo.[ProviderSite] tp on tp.ProviderSiteID = vpr.[ProviderSiteID]
join dbo.Employer e on e.EmployerId = vpr.EmployerId
where v.VacancyId = @vacancyId;

-- get candidates who applied for a vacancy
declare @candidates table (
	CandidateId int not null primary key
,	Gender int not null	
,   EthnicOrigin int not null
,	Disability int not null
,   Age int not null
,   AgeGroup int 	
);

if ( 1 = @offline )
begin
	insert into @candidates( CandidateId
						    ,Gender
							,EthnicOrigin
							,Disability
							,Age )
	select 
		ea.CandidateId 
	,	c.Gender	
	,   c.EthnicOrigin
	,	c.Disability
	,   datediff( year, c.DateofBirth, GETDATE() )	
	from dbo.ExternalApplication ea
	join dbo.Candidate c on c.CandidateId = ea.CandidateId
	where ea.VacancyId = @vacancyId
end
else
begin
	insert into @candidates( CandidateId
						    ,Gender
							,EthnicOrigin
							,Disability
							,Age )
	select 
		a.CandidateId 
	,	c.Gender	
	,   c.EthnicOrigin
	,	c.Disability	
	,   datediff( year, c.DateofBirth, GETDATE() )	
	from dbo.Application a
	join dbo.Candidate c on c.CandidateId = a.CandidateId
	join dbo.ApplicationStatusType ast on ast.ApplicationStatusTypeId = a.ApplicationStatusTypeId
	where a.VacancyId = @vacancyId 
	and 'DRF' != ast.CodeName 
end;

declare @numberOfCandidates int;
declare @numberOfDisabledCandidates int;

select @numberOfCandidates = COUNT(*) 
from @candidates;

select @numberOfDisabledCandidates = COUNT(tmp.CandidateId) 
from @candidates tmp
left outer join dbo.CandidateDisability cd on cd.CandidateDisabilityId = tmp.Disability 
where isnull( tmp.Disability, 0 ) != 0 
and cd.FullName != 'Prefer not to say';

-- return summary information
select 
	@offline as 'Offline'
,   @vacancyReference as 'VacancyReference' 
,	@vacancyTitle as 'VacancyTitle'
,	@vacancyStatus as 'VacancyStatus'
,	@employerName as 'EmployerName'
,	@employerDisabledPositive as 'EmployerDisabledPositive'
,   @providerName as 'ProviderName'
,	@numberOfCandidates as 'NumberOfCandidates'
,	@numberOfDisabledCandidates as 'NumberOfDisabledCandidates';

-- group by gender
-- do not filter out 'Please Select...' as database may contain some invalid records	
select 
	cg.CandidateGenderId
,	cg.CodeName
,	cg.ShortName
,	cg.FullName
,	tmp.Count
from dbo.CandidateGender cg
left outer join (
	select 
		Gender as CandidateGenderId
	  , COUNT( * ) as 'Count' 
	from @candidates 
	group by Gender
) tmp on tmp.CandidateGenderId = cg.CandidateGenderId

-- group by ethnicity
-- do not filter out 'Please Select...' as database may contain some invalid records
select 
	eo.CandidateEthnicOriginId
,	eo.CodeName
,	eo.ShortName
,   eo.FullName
,   tmp.Count
from dbo.CandidateEthnicOrigin eo
full outer join (
	select 
		EthnicOrigin as CandidateEthnicOriginId
	  , COUNT( * ) as 'Count'
	from @candidates 
	group by EthnicOrigin
) tmp on tmp.CandidateEthnicOriginId = eo.CandidateEthnicOriginId

-- group by disability
-- do not filter out 'Please Select...' as database may contain some invalid records
select 
	cd.CandidateDisabilityId
,	cd.CodeName
,	cd.ShortName
,   cd.FullName
,   tmp.Count
from dbo.CandidateDisability cd
full outer join (
	select 
		Disability as CandidateDisabilityId
	  , COUNT( * ) as 'Count'
	from @candidates 
	group by Disability
) tmp on tmp.CandidateDisabilityId = cd.CandidateDisabilityId

-- group by age
declare @ageGroups table (
	Id int not null primary key,
	AgeGroup nvarchar( 20 ) not null
);
insert into @ageGroups( Id, AgeGroup )
values 
	( 1, 'Under 16' ),
	( 2, '16-18' ),
	( 3, '19-24' ),
	( 4, '25+' );
	
update @candidates 
set AgeGroup = 
	case 
		when Age < 16 then 1
		when Age >= 16 and Age < 19 then 2
		when Age >= 19 and Age < 25 then 3
		when Age >= 25 then 4 
	end 
from @candidates 

select 
	ag.Id
,	ag.AgeGroup
,   tmp.Count
from @ageGroups ag
left outer join (
	select 
		AgeGroup
	,   COUNT( * ) as 'Count'
	from @candidates c
	group by AgeGroup
) tmp on tmp.AgeGroup = ag.Id;	

end