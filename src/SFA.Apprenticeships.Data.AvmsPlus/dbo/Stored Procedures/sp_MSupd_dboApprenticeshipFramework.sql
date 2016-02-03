create procedure [sp_MSupd_dboApprenticeshipFramework]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(3) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 nvarchar(200) = NULL,
		@c6 int = NULL,
		@c7 datetime = NULL,
		@c8 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ApprenticeshipFramework] set
		[ApprenticeshipOccupationId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ApprenticeshipOccupationId] end,
		[CodeName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [FullName] end,
		[ApprenticeshipFrameworkStatusTypeId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [ApprenticeshipFrameworkStatusTypeId] end,
		[ClosedDate] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [ClosedDate] end,
		[PreviousApprenticeshipOccupationId] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [PreviousApprenticeshipOccupationId] end
where [ApprenticeshipFrameworkId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end