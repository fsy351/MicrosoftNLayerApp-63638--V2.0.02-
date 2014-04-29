CREATE TABLE [dbo].[Products] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Description]   NVARCHAR (MAX)   NULL,
    [Title]         NVARCHAR (MAX)   NOT NULL,
    [UnitPrice]     DECIMAL (10, 2)  NOT NULL,
    [AmountInStock] INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

