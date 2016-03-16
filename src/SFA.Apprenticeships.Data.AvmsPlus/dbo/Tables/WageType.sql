CREATE TABLE [dbo].[WageType] (
    [WageTypeId]			  INT           NOT NULL IDENTITY (-1, -1),
    [CodeName]                NVARCHAR (3)  NOT NULL,
    [ShortName]               NVARCHAR (10) NOT NULL,
    [FullName]                NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_WageType] PRIMARY KEY CLUSTERED ([WageTypeId] ASC),
	CONSTRAINT [uq_idx_WageType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

