CREATE VIEW [MI_Views].[ReportAgeRanges_Vw]
AS
     SELECT ReportAgeRangeID,
            ReportAgeRangeLabel,
            MinYears,
            MaxYears
     FROM dbo.ReportAgeRanges;