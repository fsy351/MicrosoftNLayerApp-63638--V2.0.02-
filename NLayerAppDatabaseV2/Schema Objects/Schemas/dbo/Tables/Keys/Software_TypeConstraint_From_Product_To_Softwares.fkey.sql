ALTER TABLE [dbo].[Softwares]
    ADD CONSTRAINT [Software_TypeConstraint_From_Product_To_Softwares] FOREIGN KEY ([Id]) REFERENCES [dbo].[Products] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

