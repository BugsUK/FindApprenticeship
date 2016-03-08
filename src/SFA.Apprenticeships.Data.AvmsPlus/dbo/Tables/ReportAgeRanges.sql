CREATE TABLE [dbo].[ReportAgeRanges] (
    [ReportAgeRangeID]    INT           NOT NULL,
    [ReportAgeRangeLabel] NVARCHAR (10) NOT NULL,
    [MinYears]            INT           NOT NULL,
    [MaxYears]            INT           NOT NULL,
    CONSTRAINT [PK_ReportAgeRanges] PRIMARY KEY CLUSTERED ([ReportAgeRangeID] ASC),
    CONSTRAINT [UQ_ReportAgeRange] UNIQUE NONCLUSTERED ([ReportAgeRangeLabel] ASC),
    CONSTRAINT [UQ_ReportAgeRangeYears] UNIQUE NONCLUSTERED ([MinYears] ASC, [MaxYears] ASC)
);

