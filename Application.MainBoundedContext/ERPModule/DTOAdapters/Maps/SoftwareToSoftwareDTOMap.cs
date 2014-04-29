

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using AutoMapper;

    /// <summary>
    /// Map for software to software dto
    /// </summary>
    public class SoftwareToSoftwareDTOMap
        : TypeMapConfigurationBase<Software,SoftwareDTO>
    {
        protected override void BeforeMap(ref Software source)
        {
            Mapper.CreateMap<Software, SoftwareDTO>();
        }

        protected override void AfterMap(ref SoftwareDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override SoftwareDTO Map(Software source)
        {
            return Mapper.Map<Software, SoftwareDTO>(source);
        }
    }
}
