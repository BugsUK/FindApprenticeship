create procedure [sp_MSupd_dboSchoolAttended]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 nvarchar(120) = NULL,
		@c5 nvarchar(120) = NULL,
		@c6 datetime = NULL,
		@c7 datetime = NULL,
		@c8 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[SchoolAttended] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[SchoolId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [SchoolId] end,
		[OtherSchoolName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [OtherSchoolName] end,
		[OtherSchoolTown] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [OtherSchoolTown] end,
		[StartDate] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [StartDate] end,
		[EndDate] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [EndDate] end,
		[ApplicationId] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [ApplicationId] end
where [SchoolAttendedId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end