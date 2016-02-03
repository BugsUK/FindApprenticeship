create procedure [sp_MSupd_dboReportDefinitions]
		@c1 int = NULL,
		@c2 nvarchar(100) = NULL,
		@c3 nvarchar(100) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 nvarchar(100) = NULL,
		@c6 nvarchar(max) = NULL,
		@c7 nvarchar(100) = NULL,
		@c8 nvarchar(100) = NULL,
		@c9 nvarchar(100) = NULL,
		@c10 nvarchar(255) = NULL,
		@pkc1 int = NULL,
		@pkc2 nvarchar(100) = NULL,
		@bitmap binary(2)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1) or
 (substring(@bitmap,1,1) & 2 = 2)
begin 
update [dbo].[ReportDefinitions] set
		[RoleTypeID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [RoleTypeID] end,
		[DisplayName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [DisplayName] end,
		[HTMLVersion] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [HTMLVersion] end,
		[CSVVersion] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [CSVVersion] end,
		[SummaryVersion] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [SummaryVersion] end,
		[Description] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Description] end,
		[GeographicSectionName] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [GeographicSectionName] end,
		[DateSectionName] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [DateSectionName] end,
		[ApplicationSectionName] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [ApplicationSectionName] end,
		[Flags] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [Flags] end
where [RoleTypeID] = @pkc1
  and [DisplayName] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[ReportDefinitions] set
		[HTMLVersion] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [HTMLVersion] end,
		[CSVVersion] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [CSVVersion] end,
		[SummaryVersion] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [SummaryVersion] end,
		[Description] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Description] end,
		[GeographicSectionName] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [GeographicSectionName] end,
		[DateSectionName] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [DateSectionName] end,
		[ApplicationSectionName] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [ApplicationSectionName] end,
		[Flags] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [Flags] end
where [RoleTypeID] = @pkc1
  and [DisplayName] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end