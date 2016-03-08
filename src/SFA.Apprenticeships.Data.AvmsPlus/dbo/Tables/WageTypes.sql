CREATE TABLE [dbo].[WageTypes] (
    [WageTypeID]   INT           NOT NULL,
    [WageTypeName] NVARCHAR (20) NOT NULL,
    CONSTRAINT [PK_WageTypes] PRIMARY KEY CLUSTERED ([WageTypeID] ASC)
);

