CREATE TABLE [dbo].[Customers] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [FirstName]            NVARCHAR (50)    NOT NULL,
    [LastName]             NVARCHAR (100)   NOT NULL,
    [FullName]             NVARCHAR (MAX)   NULL,
	[Telephone]            NVARCHAR (25)    NULL,
	[Company]              NVARCHAR (200)   NULL,
    [CreditLimit]          DECIMAL (18, 2)  NOT NULL,
    [IsEnabled]            BIT              NOT NULL,
    [CountryId]            UNIQUEIDENTIFIER NOT NULL,
    [Address_City]         NVARCHAR (MAX)   NULL,
    [Address_ZipCode]      NVARCHAR (MAX)   NULL,
    [Address_AddressLine1] NVARCHAR (MAX)   NULL,
    [Address_AddressLine2] NVARCHAR (MAX)   NULL,
    [RawPhoto]             VARBINARY (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

