CREATE procedure [dbo].[uspCandidateDeRegistrationControlInsert]

as

begin

declare @AwaitingActivationPeriod int,
		@InactivePeriod int
		

declare @temp table
(candidateid int,
 roleId int,
 isHardDelete bit
)
	-- RoleId = 1 --Candidate
	-- RoleId = 2 --Stakeholder
	
	-- isHardDelete = 1 --Pre-Registered
	-- Pre-Registered =0 -- All others 
	
select @AwaitingActivationPeriod = (select cast(parametervalue as int)
									from SystemParameters
									where ParameterName = 'AwaitingActivationPeriod')


select @InactivePeriod = (select cast(parametervalue as int)
									from SystemParameters
									where ParameterName = 'InactivePeriod')

								

-- Get those candidates where the candidate hasnt activated after receiving 
-- the activation email more than @AwaitingActivationPeriod (intially 30) days ago
		insert into @temp (candidateid,roleId,isHardDelete)
		
		select c.candidateid,1,1
		from candidate c
		-- Where the candidate is currently Pre-Registered
		where (c.candidatestatustypeid = (select CandidateStatusId 
										from CandidateStatus 
										where fullname = 'Pre-Registered'))
		
		and (c.LastAccessedDate < dateadd(d, -@AwaitingActivationPeriod, getdate()))
UNION
-- Get those active candidates where the candidate hasnt accessed the 
-- system for more than @InactivePeriod (intially 80) days
		select c.candidateid,1,1
		from candidate c
		-- Where the candidate is currently Activated
		where c.candidatestatustypeid = (select CandidateStatusId 
										from CandidateStatus 
										where fullname = 'Activated')
		-- the date they last accessed the system is more than 180 days ago
		and c.LastAccessedDate < dateadd(d, -@InactivePeriod, getdate())
UNION
		-- Get those candidates who have elected to de-register "today"
		-- by clicking on the button
		select c.candidateid,1,1
		from candidate c
		where candidatestatustypeid = (select CandidateStatusId 
									from CandidateStatus 
									where fullname = 'Pending Delete')

UNION

		select s.StakeHolderID,2,1
		from stakeholder s
		where StakeHolderStatusId = (select StakeHolderStatusId 
									from stakeholderStatus 
									where fullname = 'Pending Delete')

	
	Insert into CandidateDeRegistrationControl
	(
		CandidateId,
		roleId,
		isHardDelete
	)

	Select
		candidateid, roleId,isHardDelete
	from @temp temp
	where temp.candidateid not in (select temp2.candidateid from CandidateDeRegistrationControl temp2
									where roleId = 1)
	and roleId = 1 --Candidate
	UNION
	Select
		candidateid, roleId,isHardDelete
	from @temp temp
	where temp.candidateid not in (select temp2.candidateid from CandidateDeRegistrationControl temp2
									where roleId = 2)
	and roleId = 2 --Stakeholder

end