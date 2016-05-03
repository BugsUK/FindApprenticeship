/*----------------------------------------------------------------------                  
Name  : fnReportGetAgeRangeDates                  
Description :  Returns ID for age ranges 
ID			Description
1			<16
2			16-18
3			Up to 20
4			19-24
5			25+ 

                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
03-Sep-2008		1.01		Ian Emery		Added all option
09-Oct-2008		1.02		Juandre Germishuys Added "up to 20" as value
29-Oct-2008		1.03		Ian Emery		Rewritten to give output date ranges. The view is no longer needed.
11-Nov-2008		1.04		Ian Emery		changed the date to be 1-sept-09 
17-Nov-2008		1.05		Femma Ashraf	Changed to add another parameter from ToDate and ammended age ranges
18-Nov-2008		1.06		Ian Emery   	corrected age ranges and added 23:59:59 to date
---------------------------------------------------------------------- */   

create function [dbo].[fnReportGetAgeRangeDates]
(@datetype	INT, @reportToDate DATETIME)
   RETURNS @DateRange TABLE (mindate DATETIME, maxdate	DATETIME)
as

BEGIN  
   
--1			<16
--2			16-18
--3			Up to 20
--4			19-24
--5			25+ 

--SET DATEFORMAT ymd

DECLARE	@mindate	DATETIME
DECLARE	@maxdate	DATETIME
DECLARE @septDate  DATETIME
DECLARE @AugDate Datetime
DECLARE @minage    INT
DECLARE @maxage    INT

SET @septDate = convert(datetime,convert(char(4),DATEPART(yy,@reportToDate))+'-09-01 00:00:00',120)
SET @AugDate = convert(datetime,convert(char(4),DATEPART(yy,@reportToDate))+'-08-31 23:59:59',120)

		--check the current date is past sept-1 if so add 1 year to september date
		IF (DATEPART(mm,@reportToDate)>=9)
		BEGIN
			SET @septDate= DATEADD(yy,1,@septDate)
			SET @AugDate= DATEADD(yy,1,@AugDate)
		END

	SELECT @minage = MinYears, @maxage = MaxYears
	FROM dbo.ReportAgeRanges
	WHERE ReportAgeRangeID = @datetype
   
	     
-- work out min date
SET @mindate= DATEADD(yy,-@maxage-1,@septDate)
SET @maxdate= DATEADD(yy,-@minage,@AugDate)

 INSERT INTO @DateRange 
         VALUES ( @mindate,@maxdate)
      
      RETURN  
END