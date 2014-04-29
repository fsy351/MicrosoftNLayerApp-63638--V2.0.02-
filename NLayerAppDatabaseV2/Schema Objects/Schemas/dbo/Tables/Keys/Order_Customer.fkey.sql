ALTER TABLE [dbo].[Orders]
    ADD CONSTRAINT [Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

