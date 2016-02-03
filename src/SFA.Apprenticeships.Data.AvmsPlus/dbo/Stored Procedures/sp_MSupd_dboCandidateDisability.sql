create procedure [sp_MSupd_dboCandidateDisability]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 nvarchar(100) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[CandidateDisability] set
		[Codename] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Codename] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end
where [CandidateDisabilityId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end