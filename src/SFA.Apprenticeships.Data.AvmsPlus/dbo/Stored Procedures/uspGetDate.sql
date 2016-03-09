CREATE PROCEDURE [dbo].[uspGetDate]
AS
Begin
	SELECT getdate() as currentDateTime
End