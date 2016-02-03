create procedure [sp_MSupd_dboCandidatePreferences]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@c6 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[CandidatePreferences] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[FirstFrameworkId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [FirstFrameworkId] end,
		[FirstOccupationId] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FirstOccupationId] end,
		[SecondFrameworkId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [SecondFrameworkId] end,
		[SecondOccupationId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [SecondOccupationId] end
where [CandidatePreferenceId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end