ALTER TABLE [dbo].[OrderLines]
    ADD CONSTRAINT [Order_OrderLines] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;

