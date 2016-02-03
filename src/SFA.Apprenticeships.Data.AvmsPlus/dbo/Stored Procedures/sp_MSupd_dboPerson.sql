create procedure [sp_MSupd_dboPerson]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(10) = NULL,
		@c4 nvarchar(35) = NULL,
		@c5 nvarchar(35) = NULL,
		@c6 nvarchar(35) = NULL,
		@c7 nvarchar(16) = NULL,
		@c8 nvarchar(16) = NULL,
		@c9 nvarchar(100) = NULL,
		@c10 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[Person] set
		[Title] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Title] end,
		[OtherTitle] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [OtherTitle] end,
		[FirstName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FirstName] end,
		[MiddleNames] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [MiddleNames] end,
		[Surname] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Surname] end,
		[LandlineNumber] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [LandlineNumber] end,
		[MobileNumber] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [MobileNumber] end,
		[Email] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [Email] end,
		[PersonTypeId] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [PersonTypeId] end
where [PersonId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end