create procedure [sp_MSupd_dboWorkExperience]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 datetime = NULL,
		@c5 datetime = NULL,
		@c6 nvarchar(200) = NULL,
		@c7 bit = NULL,
		@c8 bit = NULL,
		@c9 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[WorkExperience] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[Employer] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Employer] end,
		[FromDate] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FromDate] end,
		[ToDate] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ToDate] end,
		[TypeOfWork] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [TypeOfWork] end,
		[PartialCompletion] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [PartialCompletion] end,
		[VoluntaryExperience] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [VoluntaryExperience] end,
		[ApplicationId] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [ApplicationId] end
where [WorkExperienceId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end