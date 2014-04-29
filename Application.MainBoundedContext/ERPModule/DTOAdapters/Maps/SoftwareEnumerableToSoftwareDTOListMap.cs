

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using AutoMapper;

    /// <summary>
    /// Map for enumerable software to list sofware DTO
    /// </summary>
    public class SoftwareEnumerableToSoftwareDTOListMap
         : TypeMapConfigurationBase<IEnumerable<Software>, List<SoftwareDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<Software> source)
        {
            Mapper.CreateMap<Software, SoftwareDTO>();
        }

        protected override void AfterMap(ref List<SoftwareDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<SoftwareDTO> Map(IEnumerable<Software> source)
        {
            return Mapper.Map<IEnumerable<Software>, List<SoftwareDTO>>(source);
        }
    }
}
