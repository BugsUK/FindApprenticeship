CREATE TABLE [dbo].[WageUnit] (
    [WageUnitId]			  INT           NOT NULL IDENTITY,
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_WageUnit] PRIMARY KEY CLUSTERED ([WageUnitId] ASC),
	CONSTRAINT [uq_idx_WageUnit] UNIQUE NONCLUSTERED ([FullName] ASC)
);

