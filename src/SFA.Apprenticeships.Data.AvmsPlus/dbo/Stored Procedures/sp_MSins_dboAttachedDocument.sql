create procedure [sp_MSins_dboAttachedDocument]
    @c1 int,
    @c2 nvarchar(50),
    @c3 varbinary(max),
    @c4 int
as
begin  
	insert into [dbo].[AttachedDocument](
		[AttachedDocumentId],
		[Title],
		[Attachment],
		[MIMEType]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end