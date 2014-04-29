CREATE TABLE [dbo].[BankAccountActivities] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [BankAccountId]       UNIQUEIDENTIFIER NOT NULL,
    [Date]                DATETIME         NOT NULL,
    [Amount]              DECIMAL (18, 2)  NOT NULL,
    [ActivityDescription] NVARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

