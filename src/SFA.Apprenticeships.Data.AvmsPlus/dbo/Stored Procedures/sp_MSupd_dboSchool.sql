create procedure [sp_MSupd_dboSchool]
		@c1 int = NULL,
		@c2 nvarchar(100) = NULL,
		@c3 nvarchar(120) = NULL,
		@c4 nvarchar(2000) = NULL,
		@c5 nvarchar(100) = NULL,
		@c6 nvarchar(100) = NULL,
		@c7 nvarchar(100) = NULL,
		@c8 nvarchar(100) = NULL,
		@c9 nvarchar(100) = NULL,
		@c10 nvarchar(10) = NULL,
		@c11 nvarchar(120) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[School] set
		[URN] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [URN] end,
		[SchoolName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [SchoolName] end,
		[Address] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Address] end,
		[Address1] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [Address1] end,
		[Address2] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Address2] end,
		[Area] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [Area] end,
		[Town] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Town] end,
		[County] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [County] end,
		[Postcode] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [Postcode] end,
		[SchoolNameForSearch] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [SchoolNameForSearch] end
where [SchoolId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end