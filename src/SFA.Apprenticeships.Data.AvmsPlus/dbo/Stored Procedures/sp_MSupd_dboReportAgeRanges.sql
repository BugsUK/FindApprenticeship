create procedure [sp_MSupd_dboReportAgeRanges]
		@c1 int = NULL,
		@c2 nvarchar(10) = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1)
begin 
update [dbo].[ReportAgeRanges] set
		[ReportAgeRangeID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [ReportAgeRangeID] end,
		[ReportAgeRangeLabel] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ReportAgeRangeLabel] end,
		[MinYears] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [MinYears] end,
		[MaxYears] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [MaxYears] end
where [ReportAgeRangeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[ReportAgeRanges] set
		[ReportAgeRangeLabel] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ReportAgeRangeLabel] end,
		[MinYears] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [MinYears] end,
		[MaxYears] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [MaxYears] end
where [ReportAgeRangeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end