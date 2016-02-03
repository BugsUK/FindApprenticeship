create procedure [sp_MSdel_dboMIMEType]
		@pkc1 int
as
begin  
	delete [dbo].[MIMEType]
where [MIMETypeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end