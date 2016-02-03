create procedure [sp_MSupd_dboExternalApplication]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 datetime = NULL,
		@c5 uniqueidentifier = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ExternalApplication] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[VacancyId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [VacancyId] end,
		[ClickthroughDate] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ClickthroughDate] end,
		[ExternalTrackingId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ExternalTrackingId] end
where [ExternalApplicationId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end