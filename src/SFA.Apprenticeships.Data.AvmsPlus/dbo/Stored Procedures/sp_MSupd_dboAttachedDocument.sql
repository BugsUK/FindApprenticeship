create procedure [sp_MSupd_dboAttachedDocument]
		@c1 int = NULL,
		@c2 nvarchar(50) = NULL,
		@c3 varbinary(max) = NULL,
		@c4 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[AttachedDocument] set
		[Title] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Title] end,
		[Attachment] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [Attachment] end,
		[MIMEType] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [MIMEType] end
where [AttachedDocumentId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end