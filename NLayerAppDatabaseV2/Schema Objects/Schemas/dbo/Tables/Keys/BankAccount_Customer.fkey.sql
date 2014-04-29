ALTER TABLE [dbo].[BankAccounts]
    ADD CONSTRAINT [BankAccount_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

