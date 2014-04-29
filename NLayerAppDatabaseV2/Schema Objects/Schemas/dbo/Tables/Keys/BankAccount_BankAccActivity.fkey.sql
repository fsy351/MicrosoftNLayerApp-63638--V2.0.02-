ALTER TABLE [dbo].[BankAccountActivities]
    ADD CONSTRAINT [BankAccount_BankAccActivity] FOREIGN KEY ([BankAccountId]) REFERENCES [dbo].[BankAccounts] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;

