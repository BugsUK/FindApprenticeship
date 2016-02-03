CREATE PROCEDURE [dbo].[uspBackgroundSearchLogInsert] 
	@Date DateTime,
@NumberOfVacancies int,
@NumberOfCandidatesProcessed int,
@NumberOfFailures int

AS
BEGIN
	
Insert into dbo.BackgroundSearchLog
(
Date,
NumberOfVacancies,
NumberOfCandidatesProcessed,
NumberOfFailures

)

Values
(
@Date ,
@NumberOfVacancies ,
@NumberOfCandidatesProcessed ,
@NumberOfFailures 
)


END