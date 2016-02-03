create procedure [sp_MSdel_dboReportDefinitions]
		@pkc1 int,
		@pkc2 nvarchar(100)
as
begin  
	delete [dbo].[ReportDefinitions]
where [RoleTypeID] = @pkc1
  and [DisplayName] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end