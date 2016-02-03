create procedure [sp_MSupd_dboApprenticeshipOccupation]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 int = NULL,
		@c6 datetime = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ApprenticeshipOccupation] set
		[Codename] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Codename] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[ApprenticeshipOccupationStatusTypeId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ApprenticeshipOccupationStatusTypeId] end,
		[ClosedDate] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [ClosedDate] end
where [ApprenticeshipOccupationId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end