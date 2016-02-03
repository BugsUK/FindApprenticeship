create procedure [sp_MSdel_dboCAFFieldsFieldType]
		@pkc1 smallint
as
begin  
	delete [dbo].[CAFFieldsFieldType]
where [CAFFieldsFieldTypeId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end