create procedure [sp_MSupd_dboEmployerHistory]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 datetime = NULL,
		@c5 int = NULL,
		@c6 varchar(4000) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[EmployerHistory] set
		[EmployerId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [EmployerId] end,
		[UserName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [UserName] end,
		[Date] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Date] end,
		[Event] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [Event] end,
		[Comment] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Comment] end
where [EmployerHistoryId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end