create procedure [sp_MSins_dboAlertPreference]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 bit,
    @c5 bit
as
begin  
	insert into [dbo].[AlertPreference](
		[AlertPreferenceId],
		[CandidateId],
		[AlertTypeId],
		[SMSAlert],
		[EmailAlert]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end