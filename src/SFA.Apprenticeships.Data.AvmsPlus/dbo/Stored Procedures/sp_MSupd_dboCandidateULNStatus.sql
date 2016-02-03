create procedure [sp_MSupd_dboCandidateULNStatus]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(10) = NULL,
		@c4 nvarchar(100) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[CandidateULNStatus] set
		[Codename] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Codename] end,
		[Shortname] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Shortname] end,
		[Fullname] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Fullname] end
where [CandidateULNStatusId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end