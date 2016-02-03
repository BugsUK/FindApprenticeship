create procedure [sp_MSdel_dboSystemParameters]
		@pkc1 int
as
begin  
	delete [dbo].[SystemParameters]
where [SystemParametersId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end