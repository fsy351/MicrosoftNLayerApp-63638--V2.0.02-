

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Map for enumerable product to list product dto
    /// </summary>
    public class ProductEnumerableToProductDTOListMap
        : TypeMapConfigurationBase<IEnumerable<Product>, List<ProductDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<Product> source)
        {
            Mapper.CreateMap<Product, ProductDTO>();
        }

        protected override void AfterMap(ref List<ProductDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<ProductDTO> Map(IEnumerable<Product> source)
        {
            return Mapper.Map<IEnumerable<Product>, List<ProductDTO>>(source);
        }
    }
}
