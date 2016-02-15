CREATE TABLE [dbo].[WageUnit] (
    [WageUnitId]   INT           NOT NULL IDENTITY,
    [WageUnitName] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_WageUnit] PRIMARY KEY CLUSTERED ([WageUnitId] ASC)
);

