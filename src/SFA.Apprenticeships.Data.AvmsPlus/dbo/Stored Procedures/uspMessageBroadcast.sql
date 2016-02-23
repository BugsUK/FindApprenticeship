Create Procedure [dbo].[uspMessageBroadcast]
	@sender int,
	@senderType int,
	@recipientType int,
	@messageEventId int,
	@text nvarchar(1000),
	@title nvarchar(1000)
As
Begin

	BEGIN TRY

		BEGIN TRAN
		-- Use 'Case' if more conditions required
		-- Candidate
		If @recipientType = (Select UserTypeId from UserType Where CodeName = 'CAN')
		Begin
			Insert Into Message(Sender, SenderType, Recipient, RecipientType, MessageDate, 
								MessageEventId, Text, Title, IsRead, IsDeleted)
				Values (@sender, @senderType, 0, @recipientType, GetDate(), 
						@messageEventId, @text, @title, 0, 0)
				
		End
		COMMIT TRAN
	END TRY

	BEGIN CATCH  
		ROLLBACK TRAN
		EXEC RethrowError;  
	END CATCH  		
	
End