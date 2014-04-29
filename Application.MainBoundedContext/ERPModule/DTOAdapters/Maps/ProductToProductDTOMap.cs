
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
			

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Map for product to product dto
    /// </summary>
    public class ProductToProductDTOMap
        : TypeMapConfigurationBase<Product, ProductDTO>
    {
        protected override void BeforeMap(ref Product source)
        {
            Mapper.CreateMap<Product, ProductDTO>();
        }

        protected override void AfterMap(ref ProductDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override ProductDTO Map(Product source)
        {
            return Mapper.Map<Product, ProductDTO>(source);
        }
    }
}
