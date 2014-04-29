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
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// The country to country dto map specification
    /// </summary>
    public class CountryToCountryDTOMap
          : TypeMapConfigurationBase<Country,CountryDTO>
    {
        protected override void BeforeMap(ref Country source)
        {
            //using property names conventions
            Mapper.CreateMap<Country, CountryDTO>();
        }

        protected override void AfterMap(ref CountryDTO target, params object[] moreSources)
        {
            //Don't need
        }

        protected override CountryDTO Map(Country source)
        {
            return Mapper.Map<Country, CountryDTO>(source);
        }
    }
}
