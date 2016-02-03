create procedure [sp_MSdel_dboEmployerContact]
		@pkc1 int
as
begin  
	delete [dbo].[EmployerContact]
where [EmployerContactId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end