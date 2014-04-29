

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters.Maps
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using AutoMapper;

    /// <summary>
    /// The enumerable bank account activity to bank account activity list map
    /// </summary>
    class BankActivityEnumerableToBankActivityDTOListMap
        : TypeMapConfigurationBase<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<BankAccountActivity> source)
        {
            Mapper.CreateMap<BankAccountActivity,BankActivityDTO>();
        }

        protected override void AfterMap(ref List<BankActivityDTO> target, params object[] moreSources)
        {
            //don't need
        }

        protected override List<BankActivityDTO> Map(IEnumerable<BankAccountActivity> source)
        {
            return Mapper.Map<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>(source);
        }
    }
}
