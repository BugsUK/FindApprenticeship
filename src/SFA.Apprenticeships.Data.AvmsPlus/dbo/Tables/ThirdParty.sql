CREATE TABLE [dbo].[ThirdParty] (
    [ID]              INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [EDSURN]          INT            NOT NULL,
    [ThirdPartyName]  NVARCHAR (510) NULL,
    [AddressLine1]    NVARCHAR (100) NULL,
    [AddressLine2]    NVARCHAR (100) NULL,
    [AddressLine3]    NVARCHAR (100) NULL,
    [AddressLine4]    NVARCHAR (100) NULL,
    [AddressLine5]    NVARCHAR (100) NULL,
    [Town]            NVARCHAR (100) NULL,
    [CountyId]        INT            NULL,
    [PostCode]        NVARCHAR (16)  NULL,
    [Longitude]       DECIMAL (18)   NULL,
    [Latitude]        DECIMAL (18)   NULL,
    [GeocodeEasting]  INT            NULL,
    [GeocodeNorthing] INT            NULL,
    CONSTRAINT [PK_ThirdParty] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [uq_idx_thirdparty] UNIQUE NONCLUSTERED ([EDSURN] ASC)
);

