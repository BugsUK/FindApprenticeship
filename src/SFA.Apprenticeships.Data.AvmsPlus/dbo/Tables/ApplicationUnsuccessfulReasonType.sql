CREATE TABLE [dbo].[ApplicationUnsuccessfulReasonType] (
    [ApplicationUnsuccessfulReasonTypeId] INT            IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [CodeName]                            NVARCHAR (3)   NOT NULL,
    [ShortName]                           NVARCHAR (10)  NOT NULL,
    [FullName]                            NVARCHAR (100) NOT NULL,
    [ReferralPoints]                      INT            NOT NULL,
    [CandidateDisplayText]                NVARCHAR (900) NOT NULL,
    [CandidateFullName]                   NVARCHAR (100) NULL,
    [Withdrawn]                           BIT            NULL,
    CONSTRAINT [PK_ApplicationUnsuccessfulReasonType] PRIMARY KEY CLUSTERED ([ApplicationUnsuccessfulReasonTypeId] ASC),
    CONSTRAINT [uq_idx_ApplicationUnsuccessfulReasonType] UNIQUE NONCLUSTERED ([FullName] ASC)
);

