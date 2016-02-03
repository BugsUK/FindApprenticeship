create procedure [sp_MSupd_dboEducationResult]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 int = NULL,
		@c5 nvarchar(100) = NULL,
		@c6 nvarchar(20) = NULL,
		@c7 datetime = NULL,
		@c8 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[EducationResult] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[Subject] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Subject] end,
		[Level] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Level] end,
		[LevelOther] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [LevelOther] end,
		[Grade] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Grade] end,
		[DateAchieved] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [DateAchieved] end,
		[ApplicationId] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [ApplicationId] end
where [EducationResultId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end