

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;


    /// <summary>
    /// Map for book to book dto
    /// </summary>
    class BookToBookDTOMap
         : TypeMapConfigurationBase<Book,BookDTO>
    {
        protected override void BeforeMap(ref Book source)
        {
            Mapper.CreateMap<Book, BookDTO>();
        }

        protected override void AfterMap(ref BookDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override BookDTO Map(Book source)
        {
            return Mapper.Map<Book, BookDTO>(source);
        }
    }
}
