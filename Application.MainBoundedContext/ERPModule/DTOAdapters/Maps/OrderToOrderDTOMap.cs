

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    
    /// <summary>
    /// The order to order dto map specification
    /// </summary>
    public class OrderToOrderDTOMap
        :TypeMapConfigurationBase<Order,OrderDTO>
    {
        protected override void BeforeMap(ref Order source)
        {
            Mapper.CreateMap<OrderLine, OrderLineDTO>();
            var mappingExpression = Mapper.CreateMap<Order, OrderDTO>();

            mappingExpression.ForMember(dto => dto.ShippingAddress, (map) => map.MapFrom(o => o.ShippingInformation.ShippingAddress));
            mappingExpression.ForMember(dto => dto.ShippingCity, (map) => map.MapFrom(o => o.ShippingInformation.ShippingCity));
            mappingExpression.ForMember(dto => dto.ShippingName, (map) => map.MapFrom(o => o.ShippingInformation.ShippingName));
            mappingExpression.ForMember(dto => dto.ShippingZipCode, (map) => map.MapFrom(o => o.ShippingInformation.ShippingZipCode));
        }

        protected override void AfterMap(ref OrderDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override OrderDTO Map(Order source)
        {
            return Mapper.Map<Order, OrderDTO>(source);
        }
    }
}
