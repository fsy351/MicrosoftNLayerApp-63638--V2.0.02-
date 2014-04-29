

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// The orderline to orderlinedto map specification
    /// </summary>
    public class OrderLineToOrderLineDTOMap
        : TypeMapConfigurationBase<OrderLine,OrderLineDTO>
    {

        protected override void BeforeMap(ref OrderLine source)
        {
            //this map use property names conventions 
            Mapper.CreateMap<OrderLine, OrderLineDTO>();
        }

        protected override void AfterMap(ref OrderLineDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override OrderLineDTO Map(OrderLine source)
        {
            return Mapper.Map<OrderLine, OrderLineDTO>(source);
        }
    }
}
