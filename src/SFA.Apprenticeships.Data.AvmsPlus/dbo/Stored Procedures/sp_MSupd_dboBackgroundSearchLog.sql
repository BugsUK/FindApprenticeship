create procedure [sp_MSupd_dboBackgroundSearchLog]
		@c1 int = NULL,
		@c2 datetime = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[BackgroundSearchLog] set
		[Date] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Date] end,
		[NumberOfVacancies] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [NumberOfVacancies] end,
		[NumberOfCandidatesProcessed] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [NumberOfCandidatesProcessed] end,
		[NumberOfFailures] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [NumberOfFailures] end
where [BackgroundSearchLogId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end