create procedure [sp_MSupd_dboApplication]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@c6 int = NULL,
		@c7 nvarchar(100) = NULL,
		@c8 int = NULL,
		@c9 nvarchar(100) = NULL,
		@c10 nvarchar(200) = NULL,
		@c11 int = NULL,
		@c12 nvarchar(50) = NULL,
		@c13 datetime = NULL,
		@c14 bit = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[Application] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[VacancyId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [VacancyId] end,
		[ApplicationStatusTypeId] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ApplicationStatusTypeId] end,
		[WithdrawnOrDeclinedReasonId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [WithdrawnOrDeclinedReasonId] end,
		[UnsuccessfulReasonId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [UnsuccessfulReasonId] end,
		[OutcomeReasonOther] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [OutcomeReasonOther] end,
		[NextActionId] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [NextActionId] end,
		[NextActionOther] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [NextActionOther] end,
		[AllocatedTo] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [AllocatedTo] end,
		[CVAttachmentId] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [CVAttachmentId] end,
		[BeingSupportedBy] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [BeingSupportedBy] end,
		[LockedForSupportUntil] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [LockedForSupportUntil] end,
		[WithdrawalAcknowledged] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [WithdrawalAcknowledged] end
where [ApplicationId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end