CREATE TABLE [dbo].[EmployerSICCodes] (
    [EmployerSICCodes] INT IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [EmployerId]       INT NOT NULL,
    [SICId]            INT NOT NULL,
    CONSTRAINT [PK_EmployerSICCodes] PRIMARY KEY CLUSTERED ([EmployerSICCodes] ASC),
    CONSTRAINT [FK_EmployerSICCodes_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [dbo].[Employer] ([EmployerId]),
    CONSTRAINT [FK_EmployerSICCodes_SICCode] FOREIGN KEY ([SICId]) REFERENCES [dbo].[SICCode] ([SICCodeId]),
    CONSTRAINT [uq_idx_employerSICCodes] UNIQUE NONCLUSTERED ([EmployerId] ASC, [SICId] ASC)
);

