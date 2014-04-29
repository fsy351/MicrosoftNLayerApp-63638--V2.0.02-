CREATE TABLE [dbo].[Orders] (
    [Id]                                  UNIQUEIDENTIFIER NOT NULL,
    [OrderDate]                           DATETIME         NOT NULL,
    [DeliveryDate]                        DATETIME         NULL,
    [IsDelivered]                         BIT              NOT NULL,
    [CustomerId]                          UNIQUEIDENTIFIER NOT NULL,
    [ShippingInformation_ShippingName]    NVARCHAR (MAX)   NULL,
    [ShippingInformation_ShippingAddress] NVARCHAR (MAX)   NULL,
    [ShippingInformation_ShippingCity]    NVARCHAR (MAX)   NULL,
    [ShippingInformation_ShippingZipCode] NVARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

