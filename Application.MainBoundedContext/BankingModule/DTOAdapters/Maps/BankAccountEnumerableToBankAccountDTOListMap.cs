//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================
			

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using AutoMapper;

    /// <summary>
    /// The enumerable bank account to list bank account dto map
    /// </summary>
    class BankAccountEnumerableToBankAccountDTOListMap
        : TypeMapConfigurationBase<IEnumerable<BankAccount>, List<BankAccountDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<BankAccount> source)
        {
            var mappingExpression = Mapper.CreateMap<BankAccount, BankAccountDTO>();

            mappingExpression.ForMember(dto => dto.BankAccountNumber, opt => opt.MapFrom(e => e.Iban));
        }

        protected override void AfterMap(ref List<BankAccountDTO> target, params object[] moreSources)
        {
            //Don't need
        }

        protected override List<BankAccountDTO> Map(IEnumerable<BankAccount> source)
        {
            return Mapper.Map<IEnumerable<BankAccount>, List<BankAccountDTO>>(source);
        }
    }
}
