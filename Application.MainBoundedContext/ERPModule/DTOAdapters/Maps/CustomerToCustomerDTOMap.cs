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


namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps
{
    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Map for Customer to CustomerDTO
    /// </summary>
    public class CustomerToCustomerDTOMap
        : TypeMapConfigurationBase<Customer,CustomerDTO>
    {
        protected override void BeforeMap(ref Customer source)
        {
            Mapper.CreateMap<Customer, CustomerDTO>();
        }

        protected override void AfterMap(ref CustomerDTO target, params object[] moreSources)
        {
            //don't need this
        }

        protected override CustomerDTO Map(Customer source)
        {
            return Mapper.Map<Customer, CustomerDTO>(source);
        }
    }
}
