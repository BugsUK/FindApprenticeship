CREATE PROCEDURE [dbo].[uspBackgroundSearchLogSelectLastDate]
AS
BEGIN
	
	SET NOCOUNT ON;

   
	SELECT Top 1 Date from BackgroundSearchLog order by Date desc

END