ALTER TABLE [dbo].[OrderLines]
    ADD CONSTRAINT [OrderLine_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

