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
    using System.Collections.Generic;

    using AutoMapper;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// The enumerable customer to list customer dto map specification
    /// </summary>
    public class CustomerEnumerableToCustomerListDTOListMap
         : TypeMapConfigurationBase<IEnumerable<Customer>, List<CustomerListDTO>>
    {
        protected override void BeforeMap(ref IEnumerable<Customer> source)
        {
            //with automapper you only need this
            Mapper.CreateMap<Customer, CustomerListDTO>();
        }

        protected override void AfterMap(ref List<CustomerListDTO> target, params object[] moreSources)
        {
            //don't need
        }

        protected override List<CustomerListDTO> Map(IEnumerable<Customer> source)
        {
            return Mapper.Map<IEnumerable<Customer>, List<CustomerListDTO>>(source);
        }
    }
}
