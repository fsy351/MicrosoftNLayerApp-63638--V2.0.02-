
namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System.Collections.Generic;

    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;


    /// <summary>
    /// the enumerable order line to list order line dto
    /// </summary>
    public class OrderLineEnumerableToOrderLineDTOListMap
          : TypeMapConfigurationBase<IEnumerable<OrderLine>, List<OrderLineDTO>>
    {

        protected override void BeforeMap(ref IEnumerable<OrderLine> source)
        {
            Mapper.CreateMap<OrderLine, OrderLineDTO>();   
        }

        protected override void AfterMap(ref List<OrderLineDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<OrderLineDTO> Map(IEnumerable<OrderLine> source)
        {
            return Mapper.Map<IEnumerable<OrderLine>, List<OrderLineDTO>>(source);
        }
    }
}
