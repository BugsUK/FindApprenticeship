CREATE PROC [dbo].[uspMessageGetMessageById]
    @messageId      INT
AS
BEGIN
SET NOCOUNT ON
    SELECT
        m.MessageId,
        m.MessageDate,
        m.[Text],
        m.Title,

        m.DeletedBy,
        m.DeletedDate,
        m.IsDeleted,

        m.ReadByFirst,
        m.ReadDate,
        m.IsRead,

        m.Recipient,
        m.RecipientType,
        rut.CodeName AS 'RecipientCodeName',
        rut.ShortName AS 'RecipientShortName',
        rut.FullName AS 'RecipientFullName',

        m.Sender,
        m.SenderType,
        sut.CodeName AS 'SenderCodeName',
        sut.ShortName AS 'SenderShortName',
        sut.FullName AS 'SenderFullName',

        m.MessageCategoryId,
        mc.CodeName AS 'CategoryCodeName',
        mc.ShortName AS 'CategoryShortName',
        mc.FullName AS 'CategoryFullName',
        
        m.MessageEventId,
        me.CodeName AS 'EventCodeName',
        me.ShortName AS 'EventShortName',
        me.FullName AS 'EventFullName'
    FROM
        [Message] m LEFT OUTER JOIN MessageEvent me ON m.MessageEventId = me.MessageEventId
            LEFT OUTER JOIN MessageCategory mc on m.MessageCategoryId = mc.MessageCategoryId
            LEFT OUTER JOIN UserType sut ON m.SenderType = sut.UserTypeId
            LEFT OUTER JOIN UserType rut ON m.RecipientType = rut.UserTypeId
    WHERE
        m.MessageId = @messageId

END