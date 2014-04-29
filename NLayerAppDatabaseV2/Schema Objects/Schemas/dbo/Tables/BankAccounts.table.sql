CREATE TABLE [dbo].[BankAccounts] (
    [Id]                                 UNIQUEIDENTIFIER NOT NULL,
    [BankAccountNumber_OfficeNumber]     NVARCHAR (MAX)   NULL,
    [BankAccountNumber_NationalBankCode] NVARCHAR (MAX)   NULL,
    [BankAccountNumber_AccountNumber]    NVARCHAR (MAX)   NULL,
    [BankAccountNumber_CheckDigits]      NVARCHAR (MAX)   NULL,
    [Iban]                               NVARCHAR (MAX)   NULL,
    [Balance]                            DECIMAL (14, 2)  NOT NULL,
    [Locked]                             BIT              NOT NULL,
    [CustomerId]                         UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);



