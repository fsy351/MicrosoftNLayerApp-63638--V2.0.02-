//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters
{
    using System.Collections.Generic;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters.Maps;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Register types maps
    /// </summary>
    public class ERPModuleRegisterTypesMap
        :RegisterTypesMap
    {
        #region Constructor

        /// <summary>
        /// Create a new instace of ERPModuleRegisterTypesMap.
        /// This class is not intended to be used directly. The TypeAdapter
        /// infrastructure loads it automatically and recover information of mapping
        /// </summary>
        public ERPModuleRegisterTypesMap()
        {
            SetMaps();
        }

        #endregion

        #region Set Maps 

        void SetMaps()
        {
            //Customers
            RegisterMap<Customer, CustomerDTO>(new CustomerToCustomerDTOMap());
            RegisterMap<IEnumerable<Customer>, List<CustomerListDTO>>(new CustomerEnumerableToCustomerListDTOListMap());

            //Countries
            RegisterMap<Country, CountryDTO>(new CountryToCountryDTOMap());
            RegisterMap<IEnumerable<Country>,List<CountryDTO>>(new CountryEnumerableToCountryDTOListMap());

            //Orders
            RegisterMap<Order, OrderDTO>(new OrderToOrderDTOMap());
            RegisterMap<IEnumerable<Order>, List<OrderListDTO>>(new OrderEnumerableToOrderListDTOListMap());
            RegisterMap<OrderLine, OrderLineDTO>(new OrderLineToOrderLineDTOMap());
            RegisterMap<IEnumerable<OrderLine>, List<OrderLineDTO>>(new OrderLineEnumerableToOrderLineDTOListMap());

            //Product
            RegisterMap<Product, ProductDTO>(new ProductToProductDTOMap());
            RegisterMap<IEnumerable<Product>, List<ProductDTO>>(new ProductEnumerableToProductDTOListMap());
            RegisterMap<Software, SoftwareDTO>(new SoftwareToSoftwareDTOMap());
            RegisterMap<IEnumerable<Software>, List<SoftwareDTO>>(new SoftwareEnumerableToSoftwareDTOListMap());
            RegisterMap<Book, BookDTO>(new BookToBookDTOMap());
            RegisterMap<IEnumerable<Book>, List<BookDTO>>(new BookEnumerableToBookDTOListMap());
        }

        #endregion
    }
}
