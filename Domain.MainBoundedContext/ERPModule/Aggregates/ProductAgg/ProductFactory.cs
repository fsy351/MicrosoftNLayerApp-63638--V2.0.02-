//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg
{
    using System;

    /// <summary>
    /// This is the factory for Product creation, which means that the main purpose
    /// is to encapsulate the creation knowledge.
    /// What is created is a transient entity instance, with nothing being said about persistence as yet
    /// </summary>
    public static class ProductFactory
    {
        /// <summary>
        /// Create a new Software
        /// </summary>
        /// <returns>Valid software</returns>
        public static TEntity CreateProduct<TEntity>(string title,string description,decimal unitPrice,int amount)
            where TEntity:Product,new()
        {
            //create the instance
            return new TEntity()
            {
                Title = title,
                Description = description,
                UnitPrice = unitPrice,
                AmountInStock = amount
            };
        }

    }
}
