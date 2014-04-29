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
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// The bank activity to bank activity dto map
    /// </summary>
    public class BankActivityToBankActivityDTOMap
        : TypeMapConfigurationBase<BankAccountActivity, BankActivityDTO>
    {
        protected override void BeforeMap(ref BankAccountActivity source)
        {
            Mapper.CreateMap<BankAccountActivity, BankActivityDTO>();
        }

        protected override void AfterMap(ref BankActivityDTO target, params object[] moreSources)
        {
            //don't need
        }

        protected override BankActivityDTO Map(BankAccountActivity source)
        {
            return Mapper.Map<BankAccountActivity, BankActivityDTO>(source);
        }
    }
}
