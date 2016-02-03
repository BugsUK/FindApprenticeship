create procedure [sp_MSdel_dboVacancySearchAudit]
		@pkc1 int
as
begin  
	delete [dbo].[VacancySearchAudit]
where [VacancySearchAuditId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end