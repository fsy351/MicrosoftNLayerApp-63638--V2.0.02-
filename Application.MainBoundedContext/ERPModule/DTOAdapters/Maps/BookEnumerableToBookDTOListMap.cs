

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Map for book enumerable to book list dto
    /// </summary>
    public class BookEnumerableToBookDTOListMap
        : TypeMapConfigurationBase<IEnumerable<Book>, List<BookDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<Book> source)
        {
            Mapper.CreateMap<Book, BookDTO>();
        }

        protected override void AfterMap(ref List<BookDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<BookDTO> Map(IEnumerable<Book> source)
        {
            return Mapper.Map<IEnumerable<Book>, List<BookDTO>>(source);
        }
    }
}
