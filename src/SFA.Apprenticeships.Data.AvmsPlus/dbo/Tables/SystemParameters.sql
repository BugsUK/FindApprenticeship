CREATE TABLE [dbo].[SystemParameters] (
    [SystemParametersId] INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ParameterName]      NVARCHAR (100) NOT NULL,
    [ParameterType]      NVARCHAR (100) NOT NULL,
    [ParameterValue]     NVARCHAR (300) NOT NULL,
    [Editable]           BIT            NULL,
    [LowerLimit]         INT            NULL,
    [UpperLimit]         INT            NULL,
    [Description]        NVARCHAR (600) NULL,
    CONSTRAINT [PK_SystemParameters] PRIMARY KEY CLUSTERED ([SystemParametersId] ASC),
    CONSTRAINT [uq_idx_SystemParameters] UNIQUE NONCLUSTERED ([ParameterName] ASC)
);

