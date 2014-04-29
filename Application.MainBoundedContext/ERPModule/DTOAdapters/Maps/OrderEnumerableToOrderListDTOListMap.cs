

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// The enumerable order to list order dto map
    /// </summary>
    public class OrderEnumerableToOrderListDTOListMap
         :TypeMapConfigurationBase<IEnumerable<Order>,List<OrderListDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<Order> source)
        {
            var mappingExpression = Mapper.CreateMap<Order, OrderListDTO>();

            //shipping
            mappingExpression.ForMember(dto => dto.ShippingAddress, (map) => map.MapFrom(o => o.ShippingInformation.ShippingAddress));
            mappingExpression.ForMember(dto => dto.ShippingCity, (map) => map.MapFrom(o => o.ShippingInformation.ShippingCity));
            mappingExpression.ForMember(dto => dto.ShippingName, (map) => map.MapFrom(o => o.ShippingInformation.ShippingName));
            mappingExpression.ForMember(dto => dto.ShippingZipCode, (map) => map.MapFrom(o => o.ShippingInformation.ShippingZipCode));

            //total order
            mappingExpression.ForMember(dto => dto.TotalOrder, (map) => map.MapFrom(o => o.GetOrderTotal()));
        }

        protected override void AfterMap(ref List<OrderListDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<OrderListDTO> Map(IEnumerable<Order> source)
        {
            return Mapper.Map<IEnumerable<Order>, List<OrderListDTO>>(source);
        }
    }
}
