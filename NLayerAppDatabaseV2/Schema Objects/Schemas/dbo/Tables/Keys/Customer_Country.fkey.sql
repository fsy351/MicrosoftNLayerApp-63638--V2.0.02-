ALTER TABLE [dbo].[Customers]
    ADD CONSTRAINT [Customer_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

