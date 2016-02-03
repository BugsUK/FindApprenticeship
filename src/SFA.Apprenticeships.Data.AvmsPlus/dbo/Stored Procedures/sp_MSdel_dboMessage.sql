create procedure [sp_MSdel_dboMessage]
		@pkc1 int
as
begin  
	delete [dbo].[Message]
where [MessageId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end