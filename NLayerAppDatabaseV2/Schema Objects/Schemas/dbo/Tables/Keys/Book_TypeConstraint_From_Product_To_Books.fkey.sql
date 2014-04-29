ALTER TABLE [dbo].[Books]
    ADD CONSTRAINT [Book_TypeConstraint_From_Product_To_Books] FOREIGN KEY ([Id]) REFERENCES [dbo].[Products] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

